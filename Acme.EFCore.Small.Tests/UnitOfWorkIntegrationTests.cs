using Acme.EFCore.Small.Entitys;
using Acme.EFCore.Small.Repositorys;
using Acme.EFCore.Small.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Acme.EFCore.Small.Tests
{
    /// <summary>
    /// IUnitOfWork 真实的提交 / 事务行为集成测试（对应主项目工作单元功能）
    /// </summary>
    public class UnitOfWorkIntegrationTests
    {
        private static TestDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new TestDbContext(options);
        }

        [Fact]
        public void Submit_persists_staged_changes_and_returns_affected_rows()
        {
            var ctx = CreateContext();
            var uow = new UnitOfWork(ctx);
            var repo = new Repository<TestUser>(uow);

            repo.Add(new TestUser { Name = "A", Email = "a@a.com", Age = 1 });
            repo.Add(new TestUser { Name = "B", Email = "b@a.com", Age = 2 });

            var rows = uow.Submit();

            Assert.Equal(2, rows);
            Assert.Equal(2, ctx.Users.Count());
        }

        [Fact]
        public async Task SubmitAsync_persists_staged_changes()
        {
            var ctx = CreateContext();
            var uow = new UnitOfWork(ctx);
            var repo = new Repository<TestUser>(uow);

            repo.Add(new TestUser { Name = "C", Email = "c@a.com", Age = 3 });
            var rows = await uow.SubmitAsync();

            Assert.Equal(1, rows);
            Assert.Single(ctx.Users);
        }

        [Fact]
        public void CommitTransaction_without_error_persists_data()
        {
            var ctx = CreateContext();
            var uow = new UnitOfWork(ctx);
            var repo = new Repository<TestUser>(uow);

            uow.BeginTransaction();
            repo.Add(new TestUser { Name = "T", Email = "t@a.com", Age = 9 });
            uow.Submit();
            uow.CommitTransaction();

            Assert.Single(ctx.Users);
        }

        [Fact]
        public void RollbackTransaction_discards_staged_changes()
        {
            var ctx = CreateContext();
            var uow = new UnitOfWork(ctx);
            var repo = new Repository<TestUser>(uow);

            uow.BeginTransaction();
            repo.Add(new TestUser { Name = "R", Email = "r@a.com", Age = 9 });
            uow.Submit();
            uow.RollbackTransaction();

            Assert.Equal(0, ctx.Users.Count());
        }

        [Fact]
        public async Task Async_transaction_commit_and_rollback()
        {
            var ctx = CreateContext();
            var uow = new UnitOfWork(ctx);
            var repo = new Repository<TestUser>(uow);

            // commit
            await uow.BeginTransactionAsync();
            repo.Add(new TestUser { Name = "AC", Email = "ac@a.com", Age = 1 });
            await uow.SubmitAsync();
            await uow.CommitTransactionAsync();
            Assert.Single(ctx.Users);

            // rollback
            await uow.BeginTransactionAsync();
            repo.Add(new TestUser { Name = "AR", Email = "ar@a.com", Age = 2 });
            await uow.SubmitAsync();
            await uow.RollbackTransactionAsync();
            Assert.Single(ctx.Users); // 仅保留前一次的提交
        }

        [Fact]
        public void DbContext_property_exposes_underlying_context()
        {
            var ctx = CreateContext();
            var uow = new UnitOfWork(ctx);

            Assert.Same(ctx, uow.DbContext);
        }
    }
}
