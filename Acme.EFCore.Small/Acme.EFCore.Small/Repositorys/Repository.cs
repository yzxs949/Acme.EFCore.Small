using Acme.EFCore.Small.Extensions;
using Acme.EFCore.Small.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.EFCore.Small.Repositorys
{
    /// <summary>
    /// 仓储通用类
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文</typeparam>
    /// <typeparam name="TEntity">实体</typeparam>
    public class Repository<TDbContext, TEntity> : IRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly IUnitOfWork<TDbContext> _unitOfWork;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// 构造函数，初始化 Repository 实例
        /// </summary>
        /// <param name="unitOfWork"></param>
        public Repository(IUnitOfWork<TDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = _unitOfWork.DbContext.Set<TEntity>();
        }

        #region 新增 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <returns>已添加的实体对象</returns>
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// 新增立即保存
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <returns>已添加的实体对象</returns>
        public int AddNowSave(TEntity entity)
        {
            _dbSet.Add(entity);
            return _unitOfWork.Submit();
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>已添加的实体对象</returns>
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        /// <summary>
        /// 异步新增立即提交
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>已添加的实体对象</returns>
        public async Task<int> AddNowSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await AddAsync(entity, cancellationToken);
            return await _unitOfWork.SubmitAsync(cancellationToken);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <returns>已新增的实体集合</returns>
        public void AddMany(List<TEntity> list)
        {
            _dbSet.AddRange(list);
        }

        /// <summary>
        /// 批量新增立即提交
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <returns>是否成功</returns>
        public int AddManyNowSave(List<TEntity> list)
        {
            AddMany(list);
            return _unitOfWork.Submit();
        }

        /// <summary>
        /// 异步批量新增
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>已新增的实体集合</returns>
        public async Task AddManyAsync(List<TEntity> list, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(list, cancellationToken);
        }

        /// <summary>
        /// 异步批量新增立即提交
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功</returns>
        public async Task<int> AddManyNowSaveAsync(List<TEntity> list, CancellationToken cancellationToken = default)
        {
            await AddManyAsync(list, cancellationToken);
            return await _unitOfWork.SubmitAsync(cancellationToken);
        }
        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns></returns>
        public void Delete(TEntity entity)
            => _dbSet.Remove(entity);


        /// <summary>
        /// 删除立即保存
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns>是否成功</returns>
        public bool DeleteNowSave(TEntity entity)
        {
            Delete(entity);
            return _unitOfWork.Submit() > 0;
        }

        /// <summary>
        /// 异步删除立即提交
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteNowSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Delete(entity);
            return await _unitOfWork.SubmitAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list">要删除的实体集合</param>
        /// <returns></returns>
        public void DelMany(List<TEntity> list)
            => _dbSet.RemoveRange(list);

        /// <summary>
        /// 批量删除立即提交
        /// </summary>
        /// <param name="list">要删除的实体集合</param>
        /// <returns></returns>
        public bool DelManyNowSave(List<TEntity> list)
        {
            DelMany(list);
            return _unitOfWork.Submit() > 0;
        }

        /// <summary>
        /// 异步批量删除立即提交
        /// </summary>
        /// <param name="list">要删除的实体集合</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DelManyNowSaveAsync(List<TEntity> list, CancellationToken cancellationToken = default)
        {
            DelMany(list);
            return await _unitOfWork.SubmitAsync(cancellationToken) > 0;
        }
        #endregion

        #region 修改

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns></returns>
        public void Update(TEntity entity)
            => _dbSet.Update(entity);

        /// <summary>
        /// 修改立即提交
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns>是否修改成功</returns>
        public bool UpdateNowSave(TEntity entity)
        {
            Update(entity);
            return _unitOfWork.Submit() > 0;
        }

        /// <summary>
        /// 异步修改立即提交
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否修改成功</returns>
        public async Task<bool> UpdateSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Update(entity);
            return await _unitOfWork.SubmitAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list">要修改的实体集合</param>
        /// <returns></returns>
        public void UpdateMany(List<TEntity> list)
            => _dbSet.UpdateRange(list);

        /// <summary>
        /// 批量修改立即提交
        /// </summary>
        /// <param name="list">要修改的实体集合</param>
        /// <returns></returns>
        public bool UpdateManyNowSave(List<TEntity> list)
        {
            UpdateMany(list);
            return _unitOfWork.Submit() > 0;
        }

        /// <summary>
        /// 异步批量修改立即提交
        /// </summary>
        /// <param name="list">要修改的实体集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<bool> UpdateManyNowSaveAsync(List<TEntity> list, CancellationToken cancellationToken = default)
        {
            UpdateMany(list);
            return await _unitOfWork.SubmitAsync(cancellationToken) > 0;
        }
        #endregion

        #region 是否存在
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="anyLambda">Linq语句</param>
        /// <returns>是否存在</returns>
        public bool Any(Expression<Func<TEntity, bool>> anyLambda)
            => _dbSet.Any(anyLambda);

        /// <summary>
        /// 异步判断是否存在
        /// </summary>
        /// <param name="anyLambda">Linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否存在</returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> anyLambda, CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(anyLambda, cancellationToken);
        #endregion

        #region 获取Queryable
        /// <summary>
        /// TEntity类型的IQueryable
        /// </summary>
        /// <returns>IQueryable</returns>
        public IQueryable<TEntity> Queryable()
            => _dbSet;

        /// <summary>
        /// 按条件返回TEntity类型的IQueryable
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns>IQueryable</returns>
        public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> whereLamdba)
            => _dbSet.Where(whereLamdba);
        #endregion

        #region 获取单条数据

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>实体对象</returns>
        public TEntity GetInfoById<TKey>(TKey id) where TKey : struct
            => _dbSet.Find(id);

        /// <summary>
        /// 异步获取单条数据
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象</returns>
        public async Task<TEntity> GetInfoByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : struct 
            => await _dbSet.FindAsync(new object[] { id }, cancellationToken: cancellationToken);

        /// <summary>
        /// 获取单条数据不追踪
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <returns>实体对象</returns>
        public TEntity GetInfoNoTracking(Expression<Func<TEntity, bool>> whereLamdba)
            => Queryable(whereLamdba).AsNoTracking().FirstOrDefault(whereLamdba);

        /// <summary>
        /// 异步获取单条数据不追踪
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象</returns>
        public async Task<TEntity> GetInfoNoTrackingAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default)
            => await Queryable(whereLamdba).AsNoTracking().FirstOrDefaultAsync(whereLamdba, cancellationToken);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns>实体对象</returns>
        public TEntity GetInfoDefault(Expression<Func<TEntity, bool>> whereLamdba)
            => _dbSet.FirstOrDefault(whereLamdba);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体对象</returns>
        public async Task<TEntity> GetInfoDefaultAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(whereLamdba, cancellationToken);
        #endregion

        #region 获取条数
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <returns>条数</returns>
        public int Count(Expression<Func<TEntity, bool>> whereLamdba)
            => _dbSet.Count(whereLamdba);

        /// <summary>
        /// 异步获取条数
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>条数</returns>
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default)
            => await _dbSet.CountAsync(whereLamdba, cancellationToken);

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns>条数</returns>
        public int Count()
            => _dbSet.Count();

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>条数</returns>
        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
            => await _dbSet.CountAsync(cancellationToken);
        #endregion

        #region 获取集合数据
        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <returns>实体集合</returns>
        public List<TEntity> GetList()
        {
            return Queryable().ToList();
        }

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体集合</returns>
        public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await Queryable().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns>实体集合</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> whereLamdba)
        {
            return Queryable(whereLamdba).ToList();
        }

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体集合</returns>
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default)
        {
            return await Queryable(whereLamdba).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <param name="strip">条数</param>
        /// <returns>实体集合</returns>
        public List<TEntity> GetListTake(Expression<Func<TEntity, bool>> whereLamdba, int strip)
            => Queryable(whereLamdba).Take(strip).ToList();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <param name="strip">条数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体集合</returns>
        public async Task<List<TEntity>> GetListTakeAsync(Expression<Func<TEntity, bool>> whereLamdba, int strip, CancellationToken cancellationToken = default)
             => await Queryable(whereLamdba).Take(strip).ToListAsync(cancellationToken);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="strip">条数</param>
        /// <returns>实体集合</returns>
        public List<TEntity> GetListTake(int strip)
            => Queryable().Take(strip).ToList();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="strip">条数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体集合</returns>
        public async Task<List<TEntity>> GetListTakeAsync(int strip, CancellationToken cancellationToken = default)
            => await Queryable().Take(strip).ToListAsync(cancellationToken);

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表
        /// </summary>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式</param>
        /// <returns>符合条件的实体列表</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
        /// </remarks>
        public List<TEntity> GetListNoTracking(Expression<Func<TEntity, bool>> whereLamdba)
            => Queryable(whereLamdba).AsNoTracking().ToList();

        /// <summary>
        /// 根据指定条件异步获取不跟踪的实体列表
        /// </summary>
        /// <param name="whereLamdba">筛选条件的 Lambda 表达式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>符合条件的实体列表</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
        /// </remarks>
        public async Task<List<TEntity>> GetListNoTrackingAsync(Expression<Func<TEntity, bool>> whereLamdba, CancellationToken cancellationToken = default)
            => await Queryable(whereLamdba).AsNoTracking().ToListAsync(cancellationToken);

        /// <summary>
        /// 根据指定条件获取不跟踪的实体列表
        /// </summary>
        /// <returns>符合条件的实体列表</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
        /// </remarks>
        public List<TEntity> GetListNoTracking()
            => Queryable().AsNoTracking().ToList();

        /// <summary>
        /// 异步获取不跟踪的实体列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>符合条件的实体列表</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
        /// </remarks>
        public async Task<List<TEntity>> GetListNoTrackingAsync(CancellationToken cancellationToken = default)
            => await Queryable().AsNoTracking().ToListAsync(cancellationToken);
        #endregion
    }

    /// <summary>
    /// 单例仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : Repository<DbContext, TEntity>, IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 构造函数，初始化 Repository 实例
        /// </summary>
        /// <param name="unitOfWork"></param>
        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}