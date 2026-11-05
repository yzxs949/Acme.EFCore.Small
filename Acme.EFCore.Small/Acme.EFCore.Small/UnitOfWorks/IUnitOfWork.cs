using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.EFCore.Small.UnitOfWorks
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public TDbContext DbContext { get; }

        #region 单元提交

        /// <summary>
        /// 提交
        /// </summary>
        int Submit();

        /// <summary>
        /// 异步提交
        /// </summary>
        Task<int> SubmitAsync(CancellationToken cancellationToken = default);
        #endregion

        #region 事务
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        void BeginTransaction();

        /// <summary>
        /// 异步开启事务
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 异步提交事务
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 异步回滚事务
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        #endregion
    }

    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IUnitOfWork<DbContext>
    {
    }
}
