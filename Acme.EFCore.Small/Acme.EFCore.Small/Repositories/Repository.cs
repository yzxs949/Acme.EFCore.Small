using Acme.EFCore.Small.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.EFCore.Small.Repositories
{
    /// <summary>
    /// 仓储通用类
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class Repository<TDbContext, TEntity> : IRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly IUnitOfWork<TDbContext> _unitOfWork;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// 初始化 Repository 实例
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public Repository(IUnitOfWork<TDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dbSet = unitOfWork.DbContext.Set<TEntity>();
        }

        #region 新增

        /// <summary>
        /// 新增实体到上下文（需调用 Submit 提交到数据库）
        /// </summary>
        /// <param name="entity">要新增的实体</param>
        public void Add(TEntity entity) => _dbSet.Add(entity);

        /// <summary>
        /// 异步新增实体到上下文
        /// </summary>
        /// <param name="entity">要新增的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
            => await _dbSet.AddAsync(entity, cancellationToken);

        /// <summary>
        /// 新增实体并立即提交到数据库，返回受影响的行数
        /// </summary>
        /// <param name="entity">要新增的实体</param>
        /// <returns>数据库受影响的行数</returns>
        public int AddNowSave(TEntity entity)
        {
            _dbSet.Add(entity);
            return _unitOfWork.Submit();
        }

        /// <summary>
        /// 异步新增实体并立即提交到数据库，返回受影响的行数
        /// </summary>
        /// <param name="entity">要新增的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>数据库受影响的行数</returns>
        public async Task<int> AddNowSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return await _unitOfWork.SubmitAsync(cancellationToken);
        }

        /// <summary>
        /// 批量新增实体到上下文
        /// </summary>
        /// <param name="entities">要新增的实体集合</param>
        public void AddRange(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

        /// <summary>
        /// 异步批量新增实体到上下文
        /// </summary>
        /// <param name="entities">要新增的实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => await _dbSet.AddRangeAsync(entities, cancellationToken);

        /// <summary>
        /// 批量新增（已过期，请使用 <see cref="AddRange(IEnumerable{TEntity})"/>）
        /// </summary>
        [Obsolete("请改用 AddRange")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddMany(List<TEntity> list) => AddRange(list);

        /// <summary>
        /// 异步批量新增（已过期，请使用 <see cref="AddRangeAsync(IEnumerable{TEntity}, CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 AddRangeAsync")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task AddManyAsync(List<TEntity> list, CancellationToken cancellationToken = default)
            => await AddRangeAsync(list, cancellationToken);

        #endregion

        #region 删除

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        public void Delete(TEntity entity) => _dbSet.Remove(entity);

        /// <summary>
        /// 删除实体并立即提交，返回是否删除成功
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns>是否成功删除（影响行数 &gt; 0）</returns>
        public bool DeleteNowSave(TEntity entity)
        {
            _dbSet.Remove(entity);
            return _unitOfWork.Submit() > 0;
        }

        /// <summary>
        /// 异步删除实体并立即提交，返回是否删除成功
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功删除</returns>
        public async Task<bool> DeleteNowSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return await _unitOfWork.SubmitAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">要删除的实体集合</param>
        public void DeleteRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

        /// <summary>
        /// 批量删除（已过期，请使用 <see cref="DeleteRange(IEnumerable{TEntity})"/>）
        /// </summary>
        [Obsolete("请改用 DeleteRange")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void DelMany(List<TEntity> list) => DeleteRange(list);

        #endregion

        #region 修改

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        public void Update(TEntity entity) => _dbSet.Update(entity);

        /// <summary>
        /// 修改实体并立即提交，返回是否修改成功
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns>是否成功修改（影响行数 &gt; 0）</returns>
        public bool UpdateNowSave(TEntity entity)
        {
            _dbSet.Update(entity);
            return _unitOfWork.Submit() > 0;
        }

        /// <summary>
        /// 异步修改实体并立即提交，返回是否修改成功
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功修改</returns>
        public async Task<bool> UpdateSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return await _unitOfWork.SubmitAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// 批量修改实体
        /// </summary>
        /// <param name="entities">要修改的实体集合</param>
        public void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

        #endregion

        #region 是否存在

        /// <summary>
        /// 判断是否存在满足条件的记录
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>是否存在</returns>
        public bool Any(Expression<Func<TEntity, bool>> predicate) => _dbSet.Any(predicate);

        /// <summary>
        /// 异步判断是否存在满足条件的记录
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否存在</returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(predicate, cancellationToken);

        #endregion

        #region 获取Queryable

        /// <summary>
        /// 获取 IQueryable 查询入口，可用于 LINQ 链式查询
        /// </summary>
        /// <returns>IQueryable 查询对象</returns>
        public IQueryable<TEntity> Queryable() => _dbSet;

        /// <summary>
        /// 按条件获取 IQueryable 查询入口
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>IQueryable 查询对象</returns>
        public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate)
            => _dbSet.Where(predicate);

        #endregion

        #region 获取单条数据

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="keyValues">主键值数组</param>
        /// <returns>实体对象，未找到返回 null</returns>
        public TEntity FindById(params object[] keyValues) => _dbSet.Find(keyValues);

        /// <summary>
        /// 异步根据主键查找实体
        /// </summary>
        /// <param name="keyValues">主键值数组</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象，未找到返回 null</returns>
        public ValueTask<TEntity> FindByIdAsync(object[] keyValues, CancellationToken cancellationToken = default)
            => _dbSet.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// 获取第一条满足条件的记录，不存在返回 null
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体对象，未找到返回 null</returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
            => _dbSet.FirstOrDefault(predicate);

        /// <summary>
        /// 异步获取第一条满足条件的记录，不存在返回 null
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象，未找到返回 null</returns>
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        /// <summary>
        /// 获取单条数据（已过期，请使用 <see cref="FirstOrDefault(Expression{Func{TEntity, bool}})"/>）
        /// </summary>
        [Obsolete("请改用 FirstOrDefault")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TEntity GetInfo(Expression<Func<TEntity, bool>> predicate) => FirstOrDefault(predicate);

        /// <summary>
        /// 异步获取单条数据（已过期，请使用 <see cref="FirstOrDefaultAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 FirstOrDefaultAsync")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<TEntity> GetInfoAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await FirstOrDefaultAsync(predicate, cancellationToken);

        /// <summary>
        /// 根据主键获取单条数据（已过期，请使用 <see cref="FindById(object[])"/>）
        /// </summary>
        [Obsolete("请改用 FindById")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TEntity GetInfoById<TKey>(TKey id) => FindById(id);

        /// <summary>
        /// 异步根据主键获取单条数据（已过期，请使用 <see cref="FindByIdAsync(object[], CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 FindByIdAsync")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<TEntity> GetInfoByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
            => await FindByIdAsync(new object[] { id }, cancellationToken);

        #endregion

        #region 获取条数

        /// <summary>
        /// 获取满足条件的记录数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>记录数</returns>
        public int Count(Expression<Func<TEntity, bool>> predicate) => _dbSet.Count(predicate);

        /// <summary>
        /// 异步获取满足条件的记录数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>记录数</returns>
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _dbSet.CountAsync(predicate, cancellationToken);

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns>总记录数</returns>
        public int Count() => _dbSet.Count();

        /// <summary>
        /// 异步获取总记录数
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>总记录数</returns>
        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
            => await _dbSet.CountAsync(cancellationToken);

        #endregion

        #region 获取集合数据

        /// <summary>
        /// 获取所有记录列表
        /// </summary>
        /// <returns>实体列表</returns>
        public List<TEntity> ToList() => Queryable().ToList();

        /// <summary>
        /// 异步获取所有记录列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体列表</returns>
        public async Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
            => await Queryable().ToListAsync(cancellationToken);

        /// <summary>
        /// 按条件获取记录列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        public List<TEntity> ToList(Expression<Func<TEntity, bool>> predicate)
            => Queryable(predicate).ToList();

        /// <summary>
        /// 异步按条件获取记录列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体列表</returns>
        public async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await Queryable(predicate).ToListAsync(cancellationToken);

        /// <summary>
        /// 获取所有记录（已过期，请使用 <see cref="ToList()"/>）
        /// </summary>
        [Obsolete("请改用 ToList")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<TEntity> GetList() => ToList();

        /// <summary>
        /// 异步获取所有记录（已过期，请使用 <see cref="ToListAsync(CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 ToListAsync")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
            => await ToListAsync(cancellationToken);

        /// <summary>
        /// 按条件获取记录列表（已过期，请使用 <see cref="ToList(Expression{Func{TEntity, bool}})"/>）
        /// </summary>
        [Obsolete("请改用 ToList(Expression)")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate) => ToList(predicate);

        /// <summary>
        /// 异步按条件获取记录列表（已过期，请使用 <see cref="ToListAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>）
        /// </summary>
        [Obsolete("请改用 ToListAsync(Expression)")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await ToListAsync(predicate, cancellationToken);

        #endregion
    }

    /// <summary>
    /// 单库仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class Repository<TEntity> : Repository<DbContext, TEntity>, IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 初始化单库仓储实例
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
