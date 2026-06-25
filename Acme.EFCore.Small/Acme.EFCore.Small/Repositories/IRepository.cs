using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Acme.EFCore.Small.Repositories
{
    /// <summary>
    /// 仓储接口，定义实体持久化操作
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        #region 新增

        /// <summary>
        /// 新增实体到上下文（需自行调用 Submit 提交到数据库）
        /// </summary>
        /// <param name="entity">要新增的实体</param>
        void Add(TEntity entity);

        /// <summary>
        /// 异步新增实体到上下文
        /// </summary>
        /// <param name="entity">要新增的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 新增并立即提交到数据库，返回受影响行数
        /// </summary>
        /// <param name="entity">要新增的实体</param>
        /// <returns>数据库受影响的行数</returns>
        int AddNowSave(TEntity entity);

        /// <summary>
        /// 异步新增并立即提交到数据库，返回受影响行数
        /// </summary>
        /// <param name="entity">要新增的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>数据库受影响的行数</returns>
        Task<int> AddNowSaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量新增实体到上下文
        /// </summary>
        /// <param name="entities">要新增的实体集合</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 异步批量新增实体到上下文
        /// </summary>
        /// <param name="entities">要新增的实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量新增（已过期，请使用 <see cref="AddRange(IEnumerable{TEntity})"/>）
        /// </summary>
        [Obsolete("请改用 AddRange")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void AddMany(List<TEntity> list);

        /// <summary>
        /// 异步批量新增（已过期，请使用 <see cref="AddRangeAsync(IEnumerable{TEntity}, CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 AddRangeAsync")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Task AddManyAsync(List<TEntity> list, CancellationToken cancellationToken = default);

        #endregion

        #region 删除

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除并立即提交，返回是否成功
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns>是否删除成功</returns>
        bool DeleteNowSave(TEntity entity);

        /// <summary>
        /// 异步删除并立即提交，返回是否成功
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteNowSaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">要删除的实体集合</param>
        void DeleteRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 批量删除（已过期，请使用 <see cref="DeleteRange(IEnumerable{TEntity})"/>）
        /// </summary>
        [Obsolete("请改用 DeleteRange")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void DelMany(List<TEntity> list);

        #endregion

        #region 修改

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        void Update(TEntity entity);

        /// <summary>
        /// 修改并立即提交，返回是否修改成功
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns>是否修改成功</returns>
        bool UpdateNowSave(TEntity entity);

        /// <summary>
        /// 异步修改并立即提交，返回是否修改成功
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否修改成功</returns>
        Task<bool> UpdateSaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量修改实体
        /// </summary>
        /// <param name="entities">要修改的实体集合</param>
        void UpdateRange(IEnumerable<TEntity> entities);

        #endregion

        #region 是否存在

        /// <summary>
        /// 判断是否存在满足条件的记录
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>是否存在</returns>
        bool Any(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步判断是否存在满足条件的记录
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否存在</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion

        #region 获取Queryable

        /// <summary>
        /// 获取 IQueryable 查询入口
        /// </summary>
        /// <returns>IQueryable 查询对象</returns>
        IQueryable<TEntity> Queryable();

        /// <summary>
        /// 按条件获取 IQueryable 查询入口
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>IQueryable 查询对象</returns>
        IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region 获取单条数据

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="keyValues">主键值数组</param>
        /// <returns>实体对象，未找到返回 null</returns>
        TEntity FindById(params object[] keyValues);

        /// <summary>
        /// 异步根据主键查找实体
        /// </summary>
        /// <param name="keyValues">主键值数组</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象，未找到返回 null</returns>
        ValueTask<TEntity> FindByIdAsync(object[] keyValues, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取第一条满足条件的记录，不存在返回 null
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体对象，未找到返回 null</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步获取第一条满足条件的记录，不存在返回 null
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象，未找到返回 null</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取单条数据（已过期，请使用 <see cref="FirstOrDefault(Expression{Func{TEntity, bool}})"/>）
        /// </summary>
        [Obsolete("请改用 FirstOrDefault")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        TEntity GetInfo(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步获取单条数据（已过期，请使用 <see cref="FirstOrDefaultAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 FirstOrDefaultAsync")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Task<TEntity> GetInfoAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据主键获取单条数据（已过期，请使用 <see cref="FindById(object[])"/>）
        /// </summary>
        [Obsolete("请改用 FindById")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        TEntity GetInfoById<TKey>(TKey id);

        /// <summary>
        /// 异步根据主键获取单条数据（已过期，请使用 <see cref="FindByIdAsync(object[], CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 FindByIdAsync")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Task<TEntity> GetInfoByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default);

        #endregion

        #region 获取条数

        /// <summary>
        /// 获取满足条件的记录数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>记录数</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步获取满足条件的记录数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>记录数</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns>总记录数</returns>
        int Count();

        /// <summary>
        /// 异步获取总记录数
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>总记录数</returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        #endregion

        #region 获取集合数据

        /// <summary>
        /// 获取所有记录列表
        /// </summary>
        /// <returns>实体列表</returns>
        List<TEntity> ToList();

        /// <summary>
        /// 异步获取所有记录列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体列表</returns>
        Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 按条件获取记录列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        List<TEntity> ToList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步按条件获取记录列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体列表</returns>
        Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取所有记录（已过期，请使用 <see cref="ToList()"/>）
        /// </summary>
        [Obsolete("请改用 ToList")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        List<TEntity> GetList();

        /// <summary>
        /// 异步获取所有记录（已过期，请使用 <see cref="ToListAsync(CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 ToListAsync")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 按条件获取记录列表（已过期，请使用 <see cref="ToList(Expression{Func{TEntity, bool}})"/>）
        /// </summary>
        [Obsolete("请改用 ToList(Expression)")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步按条件获取记录列表（已过期，请使用 <see cref="ToListAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 ToListAsync(Expression)")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion
    }

    /// <summary>
    /// 单库仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IRepository<TEntity> : IRepository<DbContext, TEntity>
        where TEntity : class
    {
    }
}
