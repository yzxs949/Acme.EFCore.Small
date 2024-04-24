using Acme.EFCore.Small.Entitys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.EFCore.Small.BaseServices
{
    /// <summary>
    /// EFCore实现接口服务
    /// </summary>
    public interface IBaseService<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public TDbContext dbContext { get; set; }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add<T>(T entity) where T : class;

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool AddMany<T>(List<T> list) where T : class;

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> AddManyAsync<T>(List<T> list) where T : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete<T>(T entity) where T : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DelMany<T>(List<T> list) where T : class;

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> DelManyAsync<T>(List<T> list) where T : class;

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update<T>(T entity) where T : class;

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool UpdateMany<T>(List<T> list) where T : class;

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> UpdateManyAsync<T>(List<T> list) where T : class;

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda"></param>
        /// <returns></returns>
        bool Any<T>(Expression<Func<T, bool>> anyLambda) where T : class;

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda"></param>
        /// <returns></returns>
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> anyLambda) where T : class;

        /// <summary>
        /// 获取Queryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> GetQueryable<T>() where T : class;

        /// <summary>
        /// 按条件获取Queryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        T GetInfo<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<T> GetInfoAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        List<T> GetList<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        List<T> GetList<T>() where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>() where T : class;

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        T GetInfoDefault<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<T> GetInfoDefaultAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        int Count<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<int> CountAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        int Count<T>() where T : class;

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> CountAsync<T>() where T : class;

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
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        void DisposeTransaction();

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        Task DisposeTransactionAsync();

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        List<T> GetListNoTracking<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        List<T> GetListNoTracking<T>() where T : class;

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        Task<List<T>> GetListNoTrackingAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        Task<List<T>> GetListNoTrackingAsync<T>() where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        List<T> GetListTake<T>(Expression<Func<T, bool>> whereLamdba, int strip) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        Task<List<T>> GetListTakeAsync<T>(Expression<Func<T, bool>> whereLamdba, int strip) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        List<T> GetListTake<T>(int strip) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        Task<List<T>> GetListTakeAsync<T>(int strip) where T : class;

        /// <summary>
        /// 从数据库中删除具有指定键类型的实体。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <typeparam name="TKey">实体的键类型。</typeparam>
        /// <returns>删除操作的结果。</returns>
        bool Delete<T, TKey>(TKey id)
            where T : BaseEntityWithId<TKey>
            where TKey : struct;

        /// <summary>
        /// 从数据库中删除具有指定键类型的实体。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <typeparam name="TKey">实体的键类型。</typeparam>
        /// <returns>删除操作的结果。</returns>
        Task<bool> DeleteAsync<T, TKey>(TKey id)
            where T : BaseEntityWithId<TKey>
            where TKey : struct;
    }

    /// <summary>
    /// EFCore实现接口服务
    /// </summary>
    public interface IBaseService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add<T>(T entity) where T : class;

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool AddMany<T>(List<T> list) where T : class;

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> AddManyAsync<T>(List<T> list) where T : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete<T>(T entity) where T : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DelMany<T>(List<T> list) where T : class;

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> DelManyAsync<T>(List<T> list) where T : class;

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update<T>(T entity) where T : class;

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool UpdateMany<T>(List<T> list) where T : class;

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> UpdateManyAsync<T>(List<T> list) where T : class;

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda"></param>
        /// <returns></returns>
        bool Any<T>(Expression<Func<T, bool>> anyLambda) where T : class;

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda"></param>
        /// <returns></returns>
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> anyLambda) where T : class;

        /// <summary>
        /// 获取Queryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> GetQueryable<T>() where T : class;

        /// <summary>
        /// 按条件获取Queryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        T GetInfo<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<T> GetInfoAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        List<T> GetList<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        List<T> GetList<T>() where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>() where T : class;

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        T GetInfoDefault<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<T> GetInfoDefaultAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        int Count<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<int> CountAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        int Count<T>() where T : class;

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> CountAsync<T>() where T : class;

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
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        void DisposeTransaction();

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        Task DisposeTransactionAsync();

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        List<T> GetListNoTracking<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        List<T> GetListNoTracking<T>() where T : class;

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        Task<List<T>> GetListNoTrackingAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <returns>符合条件的实体列表。</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
        /// </remarks>
        Task<List<T>> GetListNoTrackingAsync<T>() where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        List<T> GetListTake<T>(Expression<Func<T, bool>> whereLamdba, int strip) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamdba"></param>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        Task<List<T>> GetListTakeAsync<T>(Expression<Func<T, bool>> whereLamdba, int strip) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        List<T> GetListTake<T>(int strip) where T : class;

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        Task<List<T>> GetListTakeAsync<T>(int strip) where T : class;

        /// <summary>
        /// 从数据库中删除具有指定键类型的实体。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <typeparam name="TKey">实体的键类型。</typeparam>
        /// <returns>删除操作的结果。</returns>
        bool Delete<T, TKey>(TKey id)
            where T : BaseEntityWithId<TKey>
            where TKey : struct;

        /// <summary>
        /// 从数据库中删除具有指定键类型的实体。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <typeparam name="TKey">实体的键类型。</typeparam>
        /// <returns>删除操作的结果。</returns>
        Task<bool> DeleteAsync<T, TKey>(TKey id)
            where T : BaseEntityWithId<TKey>
            where TKey : struct;
    }
}