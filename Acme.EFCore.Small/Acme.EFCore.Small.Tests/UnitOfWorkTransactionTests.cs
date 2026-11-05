using Acme.EFCore.Small.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace Acme.EFCore.Small.Tests;

/// <summary>
/// 工作单元事务行为测试
/// 覆盖 UnitOfWork 自身实现的事务状态机：
/// 重复开启拦截、未开启时提交/回滚拦截、提交/回滚后释放并可复用、事务内提交
/// 说明：EF Core InMemory 提供程序的事务为 no-op，数据层面的真实回滚语义由 EF Core 负责，
/// 需 SQLite / 真实数据库才能验证；此处验证的是本类库自行实现的事务门禁逻辑。
/// </summary>
public class UnitOfWorkTransactionTests
{
    private class TxItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    private class TxDbContext : DbContext
    {
        public TxDbContext(DbContextOptions<TxDbContext> options) : base(options) { }

        public DbSet<TxItem> Items { get; set; } = null!;
    }

    private static UnitOfWork<TxDbContext> CreateUow()
    {
        var options = new DbContextOptionsBuilder<TxDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            // InMemory 不支持真实事务（默认会把警告升级为异常），此处抑制以验证 UnitOfWork 自身的事务门禁逻辑
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new UnitOfWork<TxDbContext>(new TxDbContext(options));
    }

    #region 同步事务

    [Fact]
    public void BeginTransaction_when_already_started_throws()
    {
        var uow = CreateUow();
        uow.BeginTransaction();

        var ex = Assert.Throws<InvalidOperationException>(() => uow.BeginTransaction());

        Assert.Contains("已有未完成的事务", ex.Message);
    }

    [Fact]
    public void CommitTransaction_without_begin_throws()
    {
        var uow = CreateUow();

        var ex = Assert.Throws<InvalidOperationException>(() => uow.CommitTransaction());

        Assert.Contains("未开启事务", ex.Message);
    }

    [Fact]
    public void RollbackTransaction_without_begin_throws()
    {
        var uow = CreateUow();

        var ex = Assert.Throws<InvalidOperationException>(() => uow.RollbackTransaction());

        Assert.Contains("未开启事务", ex.Message);
    }

    [Fact]
    public void Commit_releases_transaction_so_it_can_be_reused()
    {
        var uow = CreateUow();

        uow.BeginTransaction();
        uow.CommitTransaction();

        // 提交后事务已释放，应能再次开启
        uow.BeginTransaction();
        uow.CommitTransaction();
    }

    [Fact]
    public void Rollback_releases_transaction_so_it_can_be_reused()
    {
        var uow = CreateUow();

        uow.BeginTransaction();
        uow.RollbackTransaction();

        // 回滚后事务已释放，应能再次开启
        uow.BeginTransaction();
        uow.RollbackTransaction();
    }

    [Fact]
    public void Submit_inside_transaction_returns_affected_rows()
    {
        var uow = CreateUow();
        uow.BeginTransaction();

        uow.DbContext.Items.Add(new TxItem { Id = 1, Name = "A" });
        var rows = uow.Submit();

        uow.CommitTransaction();

        Assert.Equal(1, rows);
        Assert.Equal(1, uow.DbContext.Items.Count());
    }

    #endregion

    #region 异步事务

    [Fact]
    public async Task Async_begin_commit_releases_transaction()
    {
        var uow = CreateUow();

        await uow.BeginTransactionAsync();
        await uow.CommitTransactionAsync();

        await uow.BeginTransactionAsync();
        await uow.CommitTransactionAsync();
    }

    [Fact]
    public async Task Async_begin_rollback_releases_transaction()
    {
        var uow = CreateUow();

        await uow.BeginTransactionAsync();
        await uow.RollbackTransactionAsync();

        await uow.BeginTransactionAsync();
        await uow.RollbackTransactionAsync();
    }

    [Fact]
    public async Task Async_beginTransaction_when_already_started_throws()
    {
        var uow = CreateUow();
        await uow.BeginTransactionAsync();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => uow.BeginTransactionAsync());

        Assert.Contains("已有未完成的事务", ex.Message);
    }

    [Fact]
    public async Task Async_commit_without_begin_throws()
    {
        var uow = CreateUow();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => uow.CommitTransactionAsync());

        Assert.Contains("未开启事务", ex.Message);
    }

    [Fact]
    public async Task Async_rollback_without_begin_throws()
    {
        var uow = CreateUow();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => uow.RollbackTransactionAsync());

        Assert.Contains("未开启事务", ex.Message);
    }

    [Fact]
    public async Task Async_submit_inside_transaction_persists_data()
    {
        var uow = CreateUow();
        await uow.BeginTransactionAsync();

        uow.DbContext.Items.Add(new TxItem { Id = 1, Name = "A" });
        var rows = await uow.SubmitAsync();

        await uow.CommitTransactionAsync();

        Assert.Equal(1, rows);
        Assert.Equal(1, await uow.DbContext.Items.CountAsync());
    }

    #endregion
}
