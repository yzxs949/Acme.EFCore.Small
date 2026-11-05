using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.EFCore.Small.UnitOfWorks
{
    /// <summary>
    /// 工作单元
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public TDbContext DbContext { get; }

        /// <summary>
        /// 工作单元
        /// </summary>
        /// <param name="dbContext"></param>
        public UnitOfWork(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        #region 提交
        /// <summary>
        /// 提交
        /// </summary>
        public int Submit() => DbContext.SaveChanges();

        /// <summary>
        /// 异步提交
        /// </summary>
        public async Task<int> SubmitAsync(CancellationToken cancellationToken = default)
            => await DbContext.SaveChangesAsync(cancellationToken);
        #endregion

        #region 事务
        /// <summary>
        /// 事务
        /// </summary>
        private IDbContextTransaction _contextTransaction = null;

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public void BeginTransaction()
        {
            if (_contextTransaction != null)
                throw new InvalidOperationException("已有未完成的事务存在，请先提交或回滚当前事务");
            _contextTransaction = DbContext.Database.BeginTransaction();
        }

        /// <summary>
        /// 异步开启事务
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_contextTransaction != null)
                throw new InvalidOperationException("已有未完成的事务存在，请先提交或回滚当前事务");
            _contextTransaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            EnsureTransactionExists();
            try
            {
                _contextTransaction.Commit();
            }
            finally
            {
                DisposeTransaction();
            }
        }

        /// <summary>
        /// 异步提交事务
        /// </summary>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            EnsureTransactionExists();
            try
            {
                await _contextTransaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            EnsureTransactionExists();
            try
            {
                _contextTransaction.Rollback();
            }
            finally
            {
                DisposeTransaction();
            }
        }

        /// <summary>
        /// 异步回滚事务
        /// </summary>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            EnsureTransactionExists();
            try
            {
                await _contextTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        private void DisposeTransaction()
        {
            if (_contextTransaction == null) return;
            _contextTransaction.Dispose();
            _contextTransaction = null;
        }

        /// <summary>
        /// 异步关闭事务释放资源
        /// </summary>
        private async Task DisposeTransactionAsync()
        {
            if (_contextTransaction is null) return;
            await _contextTransaction.DisposeAsync();
            _contextTransaction = null;
        }

        /// <summary>
        /// 检查事务是否存在
        /// </summary>
        private void EnsureTransactionExists()
        {
            if (_contextTransaction == null)
                throw new InvalidOperationException("您未开启事务！");
        }
        #endregion

    }

    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : UnitOfWork<DbContext>, IUnitOfWork
    {
        /// <summary>
        /// 初始化 UnitOfWork 实例
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public UnitOfWork(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
