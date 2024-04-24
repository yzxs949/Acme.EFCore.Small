using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.EFCore.Small.Repositorys
{

    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class, new()
    {

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// 新增实体集合
        /// </summary>
        /// <param name="entitys">实体集合</param>
        /// <returns></returns>
        bool Add(List<T> entitys);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 新增实体集合
        /// </summary>
        /// <param name="entitys">实体集合</param>
        /// <returns></returns>
        Task<bool> AddAsync(List<T> entitys);

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        void BeginTransaction();

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除</param>
        /// <returns></returns>
        bool Delete(T entity);

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="entitys">集合</param>
        /// <returns></returns>
        bool Delete(List<T> entitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="entitys">删除</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(List<T> entitys);

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        void DisposeTransaction();

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        Task DisposeTransactionAsync();

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        T GetInfo(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<T> GetInfoAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        T GetInfoDefault(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<T> GetInfoDefaultAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <returns></returns>
        List<T> GetList();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAsync();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        List<T> GetListNoTracking(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        List<T> GetListNoTracking();

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        Task<List<T>> GetListNoTrackingAsync();

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        Task<List<T>> GetListNoTrackingAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取Queryable
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetQueryable();

        /// <summary>
        /// 按条件获取Queryable
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns></returns>
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">删除</param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 修改集合
        /// </summary>
        /// <param name="entitys">集合</param>
        /// <returns></returns>
        bool Update(List<T> entitys);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">删除</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entitys">集合</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<T> entitys);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        List<T> GetListTake(Expression<Func<T, bool>> whereLamdba, int strip);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        Task<List<T>> GetListTakeAsync(Expression<Func<T, bool>> whereLamdba, int strip);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        List<T> GetListTake(int strip);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        Task<List<T>> GetListTakeAsync(int strip);
    }
}