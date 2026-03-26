using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Acme.EFCore.Small.Repositorys
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文</typeparam>
    /// <typeparam name="TEntity">实体</typeparam>
    public interface IRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <returns>已添加的实体对象</returns>
        void Add(TEntity entity);

        /// <summary>
        /// 新增立即保存
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <returns>已添加的实体对象</returns>
        int AddNowSave(TEntity entity);

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>已添加的实体对象</returns>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步新增立即提交
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>已添加的实体对象</returns>
        Task<int> AddNowSaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <returns>已添加的实体集合</returns>
        void AddMany(List<TEntity> list);

        /// <summary>
        /// 批量新增立即提交
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <returns>是否成功</returns>
        int AddManyNowSave(List<TEntity> list);

        /// <summary>
        /// 异步批量新增
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>已新增的实体集合</returns>
        Task AddManyAsync(List<TEntity> list, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步批量新增立即提交
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功</returns>
        Task<int> AddManyNowSaveAsync(List<TEntity> list, CancellationToken cancellationToken = default);
        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns></returns>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除立即保存
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns>是否成功</returns>
        bool DeleteNowSave(TEntity entity);

        /// <summary>
        /// 异步删除立即提交
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<bool> DeleteNowSaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list">要删除的实体集合</param>
        /// <returns></returns>
        void DelMany(List<TEntity> list);

        /// <summary>
        /// 批量删除立即提交
        /// </summary>
        /// <param name="list">要删除的实体集合</param>
        /// <returns></returns>
        bool DelManyNowSave(List<TEntity> list);

        /// <summary>
        /// 异步批量删除立即提交
        /// </summary>
        /// <param name="list">要删除的实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<bool> DelManyNowSaveAsync(List<TEntity> list, CancellationToken cancellationToken = default);

        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns></returns>
        void Update(TEntity entity);

        /// <summary>
        /// 修改立即提交
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns>是否修改成功</returns>
        bool UpdateNowSave(TEntity entity);

        /// <summary>
        /// 异步修改立即提交
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否修改成功</returns>
        Task<bool> UpdateSaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list">要修改的实体集合</param>
        /// <returns></returns>
        void UpdateMany(List<TEntity> list);

        /// <summary>
        /// 批量修改立即提交
        /// </summary>
        /// <param name="list">要修改的实体集合</param>
        /// <returns></returns>
        bool UpdateManyNowSave(List<TEntity> list);

        /// <summary>
        /// 异步批量修改立即提交
        /// </summary>
        /// <param name="list">要修改的实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<bool> UpdateManyNowSaveAsync(List<TEntity> list, CancellationToken cancellationToken = default);
        #endregion

        #region 是否存在

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="anyLambda">Linq语句</param>
        /// <returns>是否存在</returns>
        bool Any(Expression<Func<TEntity, bool>> anyLambda);

        /// <summary>
        /// 异步判断是否存在
        /// </summary>
        /// <param name="anyLambda">Linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否存在</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> anyLambda, CancellationToken cancellationToken = default);

        #endregion

        #region 获取Queryable

        /// <summary>
        /// TEntity类型的IQueryable
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<TEntity> Queryable();

        /// <summary>
        /// 按条件返回TEntity类型的IQueryable
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns>IQueryable</returns>
        IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> whereLamdba);

        #endregion

        #region 获取单条数据
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>实体对象</returns>
        TEntity GetInfoById<TKey>(TKey id) where TKey : struct;

        /// <summary>
        /// 异步获取单条数据
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象</returns>
        Task<TEntity> GetInfoByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : struct;

        /// <summary>
        /// 获取单条数据不追踪
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <returns>实体对象</returns>
        TEntity GetInfoNoTracking(Expression<Func<TEntity, bool>> whereLamdba);

        /// <summary>
        /// 异步获取单条数据不追踪
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象</returns>
        Task<TEntity> GetInfoNoTrackingAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns>实体对象</returns>
        TEntity GetInfoDefault(Expression<Func<TEntity, bool>> whereLamdba);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象</returns>
        Task<TEntity> GetInfoDefaultAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default);
        #endregion

        #region 获取条数
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <returns>条数</returns>
        int Count(Expression<Func<TEntity, bool>> whereLamdba);

        /// <summary>
        /// 异步获取条数
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>条数</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns>条数</returns>
        int Count();

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>条数</returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        #endregion

        #region 获取集合数据

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <returns>实体集合</returns>
        List<TEntity> GetList();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体集合</returns>
        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns>实体集合</returns>
        List<TEntity> GetList(Expression<Func<TEntity, bool>> whereLamdba);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体集合</returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <param name="strip">条数</param>
        /// <returns>实体集合</returns>
        List<TEntity> GetListTake(Expression<Func<TEntity, bool>> whereLamdba, int strip);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <param name="strip">条数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体集合</returns>
        Task<List<TEntity>> GetListTakeAsync(Expression<Func<TEntity, bool>> whereLamdba, int strip, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="strip">条数</param>
        /// <returns></returns>
        List<TEntity> GetListTake(int strip);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="strip">条数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体集合</returns>
        Task<List<TEntity>> GetListTakeAsync(int strip, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表
        /// </summary>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式</param>
        /// <returns>符合条件的实体列表</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
        /// </remarks>
        List<TEntity> GetListNoTracking(Expression<Func<TEntity, bool>> whereLamdba);

        /// <summary>
        /// 根据指定条件异步获取不跟踪的实体列表
        /// </summary>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>符合条件的实体列表</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
        /// </remarks>
        Task<List<TEntity>> GetListNoTrackingAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表
        /// </summary>
        /// <returns>符合条件的实体列表</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
        /// </remarks>
        List<TEntity> GetListNoTracking();

        /// <summary>
        /// 异步获取不跟踪的实体列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>符合条件的实体列表</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
        /// </remarks>
        Task<List<TEntity>> GetListNoTrackingAsync(CancellationToken cancellationToken = default);
        #endregion
    }

    /// <summary>
    /// 单库仓储
    /// </summary>
    public interface IRepository<TEntity> : IRepository<DbContext, TEntity>
        where TEntity : class
    {
    }
}