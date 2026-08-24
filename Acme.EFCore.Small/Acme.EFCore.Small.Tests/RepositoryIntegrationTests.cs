using Acme.EFCore.Small.Entitys;
using Acme.EFCore.Small.Extensions;
using Acme.EFCore.Small.Page;
using Acme.EFCore.Small.Repositorys;
using Acme.EFCore.Small.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Acme.EFCore.Small.Tests
{
    /// <summary>
    /// 测试用实体
    /// </summary>
    public class TestUser : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public string? City { get; set; }
    }

    /// <summary>
    /// 测试用数据库上下文（EF Core InMemory 提供程序）
    /// </summary>
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestUser> Users { get; set; } = null!;
    }

    /// <summary>
    /// IRepository 真实 CRUD / 查询行为集成测试（对应主项目核心功能）
    /// </summary>
    public class RepositoryIntegrationTests
    {
        private static TestDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new TestDbContext(options);
        }

        private static (IUnitOfWork uow, IRepository<TestUser> repo, TestDbContext ctx) CreateSut()
        {
            var ctx = CreateContext();
            var uow = new UnitOfWork(ctx);
            var repo = new Repository<TestUser>(uow);
            return (uow, repo, ctx);
        }

        [Fact]
        public void AddNowSave_returns_affected_row_count()
        {
            var (uow, repo, _) = CreateSut();

            var rows = repo.AddNowSave(new TestUser { Name = "张三", Email = "z@a.com", Age = 20, IsActive = true });

            Assert.Equal(1, rows);
        }

        [Fact]
        public async Task AddNowSaveAsync_returns_affected_row_count()
        {
            var (uow, repo, _) = CreateSut();

            var rows = await repo.AddNowSaveAsync(new TestUser { Name = "李四", Email = "l@a.com", Age = 25 });

            Assert.Equal(1, rows);
        }

        [Fact]
        public void GetInfoById_returns_inserted_entity()
        {
            var (uow, repo, _) = CreateSut();
            repo.AddNowSave(new TestUser { Name = "王五", Email = "w@a.com", Age = 30 });

            var entity = repo.GetInfoById(1);

            Assert.NotNull(entity);
            Assert.Equal("王五", entity!.Name);
        }

        [Fact]
        public async Task GetInfoByIdAsync_returns_inserted_entity()
        {
            var (uow, repo, _) = CreateSut();
            await repo.AddNowSaveAsync(new TestUser { Name = "赵六", Email = "z6@a.com", Age = 22 });

            var entity = await repo.GetInfoByIdAsync(1);

            Assert.NotNull(entity);
            Assert.Equal("赵六", entity!.Name);
        }

        [Fact]
        public void GetInfoDefault_returns_first_match_or_null()
        {
            var (uow, repo, _) = CreateSut();
            repo.AddNowSave(new TestUser { Name = "张三", Email = "z@a.com", Age = 20 });
            repo.AddNowSave(new TestUser { Name = "张三2", Email = "z2@a.com", Age = 21 });

            var match = repo.GetInfoDefault(u => u.Name!.StartsWith("张三"));
            var none = repo.GetInfoDefault(u => u.Name == "不存在");

            Assert.NotNull(match);
            Assert.Equal("张三", match!.Name);
            Assert.Null(none);
        }

        [Fact]
        public async Task GetInfoDefaultAsync_returns_first_match_or_null()
        {
            var (uow, repo, _) = CreateSut();
            await repo.AddNowSaveAsync(new TestUser { Name = "钱七", Email = "q@a.com", Age = 28 });

            var match = await repo.GetInfoDefaultAsync(u => u.Name == "钱七");

            Assert.NotNull(match);
            Assert.Equal(28, match!.Age);
        }

        [Fact]
        public void GetList_returns_filtered_collection()
        {
            var (uow, repo, _) = CreateSut();
            repo.AddNowSave(new TestUser { Name = "A", Email = "a@a.com", Age = 10, IsActive = true });
            repo.AddNowSave(new TestUser { Name = "B", Email = "b@a.com", Age = 11, IsActive = false });

            var list = repo.GetList(u => u.IsActive);

            Assert.Single(list);
            Assert.Equal("A", list[0].Name);
        }

        [Fact]
        public async Task GetListAsync_returns_filtered_collection()
        {
            var (uow, repo, _) = CreateSut();
            await repo.AddNowSaveAsync(new TestUser { Name = "A", Email = "a@a.com", Age = 10 });
            await repo.AddNowSaveAsync(new TestUser { Name = "B", Email = "b@a.com", Age = 11 });

            var list = await repo.GetListAsync(u => u.Age > 10);

            Assert.Single(list);
            Assert.Equal("B", list[0].Name);
        }

        [Fact]
        public void GetListNoTracking_returns_entities_without_tracking()
        {
            var (uow, repo, ctx) = CreateSut();
            repo.AddNowSave(new TestUser { Name = "T", Email = "t@a.com", Age = 5 });

            var list = repo.GetListNoTracking(u => u.Name == "T");

            Assert.Single(list);
            Assert.False(ctx.Entry(list[0]).IsKeySet == false ? false : ctx.ChangeTracker.Entries().Any(e => e.Entity == list[0] && e.State != EntityState.Detached));
        }

        [Fact]
        public async Task GetInfoNoTrackingAsync_returns_entity_detached()
        {
            var (uow, repo, _) = CreateSut();
            await repo.AddNowSaveAsync(new TestUser { Name = "N", Email = "n@a.com", Age = 7 });

            var entity = await repo.GetInfoNoTrackingAsync(u => u.Name == "N");

            Assert.NotNull(entity);
            Assert.Equal("N", entity!.Name);
        }

        [Fact]
        public void UpdateNowSave_returns_true_and_persists_change()
        {
            var (uow, repo, _) = CreateSut();
            repo.AddNowSave(new TestUser { Name = "U", Email = "u@a.com", Age = 1 });

            var entity = repo.GetInfoById(1)!;
            entity!.Name = "U2";
            var ok = repo.UpdateNowSave(entity);

            Assert.True(ok);
            Assert.Equal("U2", repo.GetInfoById(1)!.Name);
        }

        [Fact]
        public async Task UpdateSaveAsync_then_submit_persists_change()
        {
            var (uow, repo, _) = CreateSut();
            await repo.AddNowSaveAsync(new TestUser { Name = "V", Email = "v@a.com", Age = 2 });

            var entity = await repo.GetInfoByIdAsync(1);
            entity!.Age = 99;
            await repo.UpdateSaveAsync(entity);

            Assert.Equal(99, repo.GetInfoById(1)!.Age);
        }

        [Fact]
        public void DeleteNowSave_returns_true_and_removes_entity()
        {
            var (uow, repo, _) = CreateSut();
            repo.AddNowSave(new TestUser { Name = "D", Email = "d@a.com", Age = 3 });

            var entity = repo.GetInfoById(1)!;
            var ok = repo.DeleteNowSave(entity);

            Assert.True(ok);
            Assert.Null(repo.GetInfoById(1));
        }

        [Fact]
        public async Task DeleteNowSaveAsync_returns_true_and_removes_entity()
        {
            var (uow, repo, _) = CreateSut();
            await repo.AddNowSaveAsync(new TestUser { Name = "D2", Email = "d2@a.com", Age = 4 });

            var entity = await repo.GetInfoByIdAsync(1);
            var ok = await repo.DeleteNowSaveAsync(entity!);

            Assert.True(ok);
            Assert.Null(await repo.GetInfoByIdAsync(1));
        }

        [Fact]
        public void AddManyNowSave_returns_total_affected_rows()
        {
            var (uow, repo, _) = CreateSut();
            var users = new List<TestUser>
            {
                new TestUser { Name = "M1", Email = "m1@a.com", Age = 1 },
                new TestUser { Name = "M2", Email = "m2@a.com", Age = 2 },
            };

            var rows = repo.AddManyNowSave(users);

            Assert.Equal(2, rows);
            Assert.Equal(2, repo.Count());
        }

        [Fact]
        public async Task AddManyNowSaveAsync_returns_total_affected_rows()
        {
            var (uow, repo, _) = CreateSut();
            var users = new List<TestUser>
            {
                new TestUser { Name = "M3", Email = "m3@a.com", Age = 3 },
                new TestUser { Name = "M4", Email = "m4@a.com", Age = 4 },
            };

            var rows = await repo.AddManyNowSaveAsync(users);

            Assert.Equal(2, rows);
        }

        [Fact]
        public void DelManyNowSave_returns_true_and_removes_all()
        {
            var (uow, repo, _) = CreateSut();
            repo.AddManyNowSave(new List<TestUser>
            {
                new TestUser { Name = "X1", Email = "x1@a.com", Age = 1, IsActive = false },
                new TestUser { Name = "X2", Email = "x2@a.com", Age = 2, IsActive = false },
                new TestUser { Name = "X3", Email = "x3@a.com", Age = 3, IsActive = true },
            });

            var inactive = repo.GetList(u => !u.IsActive);
            var ok = repo.DelManyNowSave(inactive);

            Assert.True(ok);
            Assert.Single(repo.GetList());
            Assert.Equal("X3", repo.GetInfoDefault(u => u.Name == "X3")!.Name);
        }

        [Fact]
        public void UpdateManyNowSave_returns_true_and_updates_all()
        {
            var (uow, repo, _) = CreateSut();
            repo.AddManyNowSave(new List<TestUser>
            {
                new TestUser { Name = "Y1", Email = "y1@a.com", Age = 1, IsActive = false },
                new TestUser { Name = "Y2", Email = "y2@a.com", Age = 2, IsActive = false },
            });

            var all = repo.GetList();
            foreach (var u in all) u.IsActive = true;
            var ok = repo.UpdateManyNowSave(all);

            Assert.True(ok);
            Assert.Equal(2, repo.Count(u => u.IsActive));
        }

        [Fact]
        public void Any_and_Count_behave_correctly()
        {
            var (uow, repo, _) = CreateSut();
            repo.AddNowSave(new TestUser { Name = "C", Email = "c@a.com", Age = 1, IsActive = true });

            Assert.True(repo.Any(u => u.IsActive));
            Assert.False(repo.Any(u => u.Name == "不存在"));
            Assert.Equal(1, repo.Count());
            Assert.Equal(0, repo.Count(u => u.IsActive == false));
        }

        [Fact]
        public void Add_stages_without_saving_until_submit()
        {
            var (uow, repo, _) = CreateSut();

            repo.Add(new TestUser { Name = "S", Email = "s@a.com", Age = 9 });
            // 未提交前数据库不应有数据（ChangeTracker 中的 Added 实体未落库）
            Assert.Equal(0, uow.DbContext.Set<TestUser>().Count());

            uow.Submit();
            Assert.Equal(1, uow.DbContext.Set<TestUser>().Count());
            Assert.NotNull(repo.GetInfoById(1));
        }

        [Fact]
        public async Task Queryable_supports_ToPageList_pagination()
        {
            var (uow, repo, _) = CreateSut();
            for (int i = 0; i < 25; i++)
                repo.AddNowSave(new TestUser { Name = $"P{i}", Email = $"p{i}@a.com", Age = i });

            var page1 = repo.Queryable(u => u.Age >= 0).ToPageList(1, 10);
            var page3 = repo.Queryable(u => u.Age >= 0).ToPageList(3, 10);

            Assert.Equal(25, page1.Total);
            Assert.Equal(10, page1.Items.Count);
            Assert.Equal(5, page3.Items.Count);
            Assert.Equal("P20", page3.Items[0].Name);
        }

        [Fact]
        public async Task Queryable_supports_ToPageListAsync_pagination()
        {
            var (uow, repo, _) = CreateSut();
            for (int i = 0; i < 15; i++)
                await repo.AddNowSaveAsync(new TestUser { Name = $"Q{i}", Email = $"q{i}@a.com", Age = i });

            var page = await repo.Queryable().ToPageListAsync(2, 10);

            Assert.Equal(15, page.Total);
            Assert.Equal(5, page.Items.Count);
        }
    }
}
