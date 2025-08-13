using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Acme.EFCore.Small.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Acme.EFCore.Small.Repositorys
{
    /// <summary>
    /// 仓储通用类
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文</typeparam>
    /// <typeparam name="TEntity">实体</typeparam>
    public class Repository<TDbContext, TEntity> : IRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class, new()
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public TDbContext DbContext { get; }

        /// <summary>
        /// 构造函数，初始化仓储实例
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(TDbContext dbContext)
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
        public async Task<int> SubmitAsync()
            => await DbContext.SaveChangesAsync();
        #endregion

        #region 新增 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <returns>已添加的实体对象</returns>
        public TEntity Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
            return entity;
        }

        /// <summary>
        /// 新增立即保存
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <returns>已添加的实体对象</returns>
        public TEntity AddNowSave(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
            Submit();
            return entity;
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <returns>已添加的实体对象</returns>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// 异步新增立即提交
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <returns>已添加的实体对象</returns>
        public async Task<TEntity> AddNowSaveAsync(TEntity entity)
        {
            await AddAsync(entity);
            await SubmitAsync();
            return entity;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <returns>已新增的实体集合</returns>
        public List<TEntity> AddMany(List<TEntity> list)
        {
            DbContext.Set<TEntity>().AddRange(list);
            return list;
        }

        /// <summary>
        /// 批量新增立即提交
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <returns>是否成功</returns>
        public bool AddManyNowSave(List<TEntity> list)
        {
            AddMany(list);
            return Submit() > 0;
        }

        /// <summary>
        /// 异步批量新增
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <returns>已新增的实体集合</returns>
        public async Task<List<TEntity>> AddManyAsync(List<TEntity> list)
        {
            await DbContext.Set<TEntity>().AddRangeAsync(list);
            return list;
        }

        /// <summary>
        /// 异步批量新增立即提交
        /// </summary>
        /// <param name="list">要新增的实体集合</param>
        /// <returns>是否成功</returns>
        public async Task<bool> AddManyNowSaveAsync(List<TEntity> list)
        {
            await AddManyAsync(list);
            return await SubmitAsync() > 0;
        }
        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns></returns>
        public void Delete(TEntity entity)
            => DbContext.Set<TEntity>().Remove(entity);

        /// <summary>
        /// 删除立即保存
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns>是否成功</returns>
        public bool DeleteNowSave(TEntity entity)
        {
            Delete(entity);
            return Submit() > 0;
        }

        /// <summary>
        /// 异步删除立即提交
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns></returns>
        public async Task<bool> DeleteNowSaveAsync(TEntity entity)
        {
            Delete(entity);
            return await SubmitAsync() > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list">要删除的实体集合</param>
        /// <returns></returns>
        public void DelMany(List<TEntity> list)
            => DbContext.Set<TEntity>().RemoveRange(list);

        /// <summary>
        /// 批量删除立即提交
        /// </summary>
        /// <param name="list">要删除的实体集合</param>
        /// <returns></returns>
        public bool DelManyNowSave(List<TEntity> list)
        {
            DelMany(list);
            return Submit() > 0;
        }

        /// <summary>
        /// 异步批量删除立即提交
        /// </summary>
        /// <param name="list">要删除的实体集合</param>
        /// <returns></returns>
        public async Task<bool> DelManyNowSaveAsync(List<TEntity> list)
        {
            DelMany(list);
            return await SubmitAsync() > 0;
        }
        #endregion

        #region 修改

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns></returns>
        public void Update(TEntity entity)
            => DbContext.Update(entity);

        /// <summary>
        /// 修改立即提交
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns>是否修改成功</returns>
        public bool UpdateNowSave(TEntity entity)
        {
            Update(entity);
            return Submit() > 0;
        }

        /// <summary>
        /// 异步修改立即提交
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns>是否修改成功</returns>
        public async Task<bool> UpdateSaveAsync(TEntity entity)
        {
            Update(entity);
            return await SubmitAsync() > 0;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list">要修改的实体集合</param>
        /// <returns></returns>
        public void UpdateMany(List<TEntity> list)
            => DbContext.UpdateRange(list);

        /// <summary>
        /// 批量修改立即提交
        /// </summary>
        /// <param name="list">要修改的实体集合</param>
        /// <returns></returns>
        public bool UpdateManyNowSave(List<TEntity> list)
        {
            UpdateMany(list);
            return Submit() > 0;
        }

        /// <summary>
        /// 异步批量修改立即提交
        /// </summary>
        /// <param name="list">要修改的实体集合</param>
        /// <returns></returns>
        public async Task<bool> UpdateManyNowSaveAsync(List<TEntity> list)
        {
            UpdateMany(list);
            return await SubmitAsync() > 0;
        }
        #endregion

        #region 是否存在
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="anyLambda">Linq语句</param>
        /// <returns>是否存在</returns>
        public bool Any(Expression<Func<TEntity, bool>> anyLambda)
            => DbContext.Set<TEntity>().Any(anyLambda);

        /// <summary>
        /// 异步判断是否存在
        /// </summary>
        /// <param name="anyLambda">Linq语句</param>
        /// <returns>是否存在</returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> anyLambda)
            => await DbContext.Set<TEntity>().AnyAsync(anyLambda);
        #endregion

        #region 获取Queryable
        /// <summary>
        /// TEntity类型的IQueryable
        /// </summary>
        /// <returns>IQueryable</returns>
        public IQueryable<TEntity> Queryable()
            => DbContext.Set<TEntity>();

        /// <summary>
        /// 按条件返回TEntity类型的IQueryable
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns>IQueryable</returns>
        public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> whereLamdba)
            => DbContext.Where(whereLamdba);
        #endregion

        #region 获取单条数据

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <returns>实体对象</returns>
        public TEntity GetInfo(Expression<Func<TEntity, bool>> whereLamdba)
            => DbContext.Find<TEntity>(whereLamdba);

        /// <summary>
        /// 异步获取单条数据
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <returns>实体对象</returns>
        public async Task<TEntity> GetInfoAsync(Expression<Func<TEntity, bool>> whereLamdba)
            => await DbContext.FindAsync<TEntity>(whereLamdba);

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
        /// <returns>实体对象</returns>
        public async Task<TEntity> GetInfoNoTrackingAsync(Expression<Func<TEntity, bool>> whereLamdba)
            => await Queryable(whereLamdba).AsNoTracking().FirstOrDefaultAsync(whereLamdba);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns>实体对象</returns>
        public TEntity GetInfoDefault(Expression<Func<TEntity, bool>> whereLamdba)
            => DbContext.Set<TEntity>().FirstOrDefault(whereLamdba);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns>实体对象</returns>
        public async Task<TEntity> GetInfoDefaultAsync(Expression<Func<TEntity, bool>> whereLamdba)
            => await DbContext.Set<TEntity>().FirstOrDefaultAsync(whereLamdba);
        #endregion

        #region 获取条数
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <returns>条数</returns>
        public int Count(Expression<Func<TEntity, bool>> whereLamdba)
            => DbContext.Set<TEntity>().Count(whereLamdba);

        /// <summary>
        /// 异步获取条数
        /// </summary>
        /// <param name="whereLamdba">linq语句</param>
        /// <returns>条数</returns>
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereLamdba)
            => await DbContext.Set<TEntity>().CountAsync(whereLamdba);

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns>条数</returns>
        public int Count()
            => DbContext.Set<TEntity>().Count();

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns>条数</returns>
        public async Task<int> CountAsync()
            => await DbContext.Set<TEntity>().CountAsync();
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
        /// <returns>实体集合</returns>
        public async Task<List<TEntity>> GetListAsync()
        {
            return await Queryable().ToListAsync();
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
        /// <returns>实体集合</returns>
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereLamdba)
        {
            return await Queryable(whereLamdba).ToListAsync();
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
        /// <returns>实体集合</returns>
        public async Task<List<TEntity>> GetListTakeAsync(Expression<Func<TEntity, bool>> whereLamdba, int strip)
             => await Queryable(whereLamdba).Take(strip).ToListAsync();

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
        /// <returns>实体集合</returns>
        public async Task<List<TEntity>> GetListTakeAsync(int strip)
            => await Queryable().Take(strip).ToListAsync();

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
        /// 根据指定条件获取不跟踪的实体列表
        /// </summary>
        /// <returns>符合条件的实体列表</returns>
        /// <remarks>
        /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
        /// </remarks>
        public List<TEntity> GetListNoTracking()
            => Queryable().AsNoTracking().ToList();
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
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransactionAsync()
        {
            if (_contextTransaction != null)
                throw new InvalidOperationException("已有未完成的事务存在，请先提交或回滚当前事务");
            _contextTransaction = await DbContext.Database.BeginTransactionAsync();
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
        /// 提交事务
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            EnsureTransactionExists();
            try
            {
                await _contextTransaction.CommitAsync();
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
        /// 回滚事务
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            EnsureTransactionExists();
            try
            {
                await _contextTransaction.RollbackAsync();
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        public void DisposeTransaction()
        {
            if (_contextTransaction == null) return;
            _contextTransaction.Dispose();
            _contextTransaction = null;
        }

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        public async Task DisposeTransactionAsync()
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
    /// 单例仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : Repository<DbContext, TEntity>, IRepository<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}