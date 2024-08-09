using Acme.EFCore.Small.Extensions;
using System.Diagnostics;

namespace Acme.EFCore.Small.BaseServices;

/// <summary>
/// 实现基于 Entity Framework Core 的服务类，用于处理与数据库的交互操作。
/// </summary>
/// <typeparam name="TDbContext">DbContext 的类型参数，用于指定要使用的数据库上下文。</typeparam>
public class BaseService<TDbContext>
    : IBaseService<TDbContext>, IBaseService
    where TDbContext : DbContext
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public TDbContext dbContext { get; set; }

    /// <summary>
    /// 依赖注入数据库上下文
    /// </summary>
    /// <param name="_dbContext"></param>
    public BaseService(TDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    /// <summary>
    /// 向数据库中添加实体，并保存更改
    /// </summary>
    /// <typeparam name="T">要添加的实体类型</typeparam>
    /// <param name="entity">要添加的实体对象</param>
    /// <returns>已添加的实体对象</returns>
    public T Add<T>(T entity) where T : class
    {
        dbContext.Set<T>().Add(entity);
        dbContext.SaveChanges();
        return entity;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<T> AddAsync<T>(T entity) where T : class
    {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool AddMany<T>(List<T> list) where T : class
    {
        dbContext.Set<T>().AddRange(list);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<bool> AddManyAsync<T>(List<T> list) where T : class
    {
        await dbContext.Set<T>().AddRangeAsync(list);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Delete<T>(T entity) where T : class
    {
        dbContext.Set<T>().Remove(entity);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 从数据库中删除具有指定键类型的实体。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <typeparam name="TKey">实体的键类型。</typeparam>
    /// <returns>删除操作的结果。</returns>
    public bool Delete<T, TKey>(TKey id)
        where TKey : struct
        where T : BaseEntityWithId<TKey>
    {
        T? t = GetInfoDefault<T>(s => s.Id.Equals(id));
        if (t is null)
            throw new ArgumentException("未获取到实体信息！");
        return Delete(t);
    }

    /// <summary>
    /// 从数据库中删除具有指定键类型的实体。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <typeparam name="TKey">实体的键类型。</typeparam>
    /// <returns>删除操作的结果。</returns>
    public async Task<bool> DeleteAsync<T, TKey>(TKey id)
        where TKey : struct
        where T : BaseEntityWithId<TKey>
    {
        T? t = await GetInfoDefaultAsync<T>(s => s.Id.Equals(id));
        if (t is null)
            throw new ArgumentException("未获取到实体信息！");
        return await DeleteAsync(t);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync<T>(T entity) where T : class
    {
        dbContext.Set<T>().Remove(entity);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool DelMany<T>(List<T> list) where T : class
    {
        dbContext.Set<T>().RemoveRange(list);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<bool> DelManyAsync<T>(List<T> list) where T : class
    {
        dbContext.Set<T>().RemoveRange(list);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Update<T>(T entity) where T : class
    {
        dbContext.Update(entity);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync<T>(T entity) where T : class
    {
        dbContext.Update(entity);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 批量修改
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool UpdateMany<T>(List<T> list) where T : class
    {
        dbContext.UpdateRange(list);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 批量修改
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<bool> UpdateManyAsync<T>(List<T> list) where T : class
    {
        dbContext.UpdateRange(list);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="anyLambda"></param>
    /// <returns></returns>
    public bool Any<T>(Expression<Func<T, bool>> anyLambda) where T : class
    {
        return dbContext.Set<T>().Any(anyLambda);
    }

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="anyLambda"></param>
    /// <returns></returns>
    public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> anyLambda) where T : class
    {
        return await dbContext.Set<T>().AnyAsync(anyLambda);
    }

    /// <summary>
    /// 获取Queryable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IQueryable<T> GetQueryable<T>() where T : class
    {
        return dbContext.Set<T>();
    }

    /// <summary>
    /// 按条件获取Queryable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return dbContext.Set<T>().Where(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public T? GetInfo<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return dbContext.Find<T>(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据不追踪
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public T? GetInfoNoTracking<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return GetQueryable<T>(whereLamdba).AsNoTracking().FirstOrDefault(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据不追踪
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T?> GetInfoNoTrackingAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await GetQueryable<T>(whereLamdba).AsNoTracking().FirstOrDefaultAsync(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T?> GetInfoAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await dbContext.FindAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public List<T> GetList<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return GetQueryable<T>(whereLamdba).ToList();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    public List<T> GetListTake<T>(Expression<Func<T, bool>> whereLamdba, int strip) where T : class
    {
        return GetQueryable<T>(whereLamdba).Take(strip).ToList();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    public async Task<List<T>> GetListTakeAsync<T>(Expression<Func<T, bool>> whereLamdba, int strip)
        where T : class
    {
        return await GetQueryable<T>(whereLamdba).Take(strip).ToListAsync();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    public List<T> GetListTake<T>(int strip) where T : class
    {
        return GetQueryable<T>().Take(strip).ToList();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    public async Task<List<T>> GetListTakeAsync<T>(int strip) where T : class => await GetQueryable<T>().Take(strip).ToListAsync();

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    public List<T> GetListNoTracking<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return GetQueryable<T>(whereLamdba).AsNoTracking().ToList();
    }

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    public List<T> GetListNoTracking<T>() where T : class
    {
        return GetQueryable<T>().AsNoTracking().ToList();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>

    public List<T> GetList<T>() where T : class
    {
        return GetQueryable<T>().ToList();
    }

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <returns>分页列表</returns>
    public PageList<T> GetPageList<T, TKey>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TKey>> keySelector) where T : class
    {
        return GetQueryable<T>()
            .OrderBy(keySelector)
            .ToPageList(pageIndex, pageSize);
    }

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <param name="whereLamdba">查询条件</param>
    /// <returns>分页列表</returns>
    public PageList<T> GetPageList<T, TKey>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TKey>> keySelector,
        Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return GetQueryable<T>(whereLamdba)
            .OrderBy(keySelector)
            .ToPageList(pageIndex, pageSize);
    }

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <returns>分页列表</returns>
    public async Task<PageList<T>> GetPageListAsync<T, TKey>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TKey>> keySelector) where T : class
    {
        return await GetQueryable<T>()
            .OrderBy(keySelector)
            .ToPageListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <param name="whereLamdba">查询条件</param>
    /// <returns>分页列表</returns>
    public async Task<PageList<T>> GetPageListAsync<T, TKey>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TKey>> keySelector,
        Expression<Func<T, bool>> whereLamdba)
    where T : class
    {
        return await GetQueryable<T>(whereLamdba)
            .OrderBy(keySelector)
            .ToPageListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await GetQueryable<T>(whereLamdba).ToListAsync();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync<T>() where T : class
    {
        return await GetQueryable<T>().ToListAsync();
    }

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    public async Task<List<T>> GetListNoTrackingAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await GetQueryable<T>(whereLamdba).AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    public async Task<List<T>> GetListNoTrackingAsync<T>() where T : class
        => await GetQueryable<T>().AsNoTracking().ToListAsync();

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public T? GetInfoDefault<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return dbContext.Set<T>().FirstOrDefault(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T?> GetInfoDefaultAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await dbContext.Set<T>().FirstOrDefaultAsync(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public int Count<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return dbContext.Set<T>().Count(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<int> CountAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await dbContext.Set<T>().CountAsync(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public int Count<T>() where T : class
    {
        return dbContext.Set<T>().Count();
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<int> CountAsync<T>() where T : class
    {
        return await dbContext.Set<T>().CountAsync();
    }

    /// <summary>
    /// 事务
    /// </summary>
    private IDbContextTransaction contextTransaction { get; set; } = default!;

    /// <summary>
    /// 开启事务
    /// </summary>
    /// <returns></returns>
    public void BeginTransaction()
    {
        contextTransaction = dbContext.Database.BeginTransaction();
    }

    /// <summary>
    /// 开启事务
    /// </summary>
    /// <returns></returns>
    public async Task BeginTransactionAsync()
    {
        contextTransaction = await dbContext.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTransaction()
    {
        if (contextTransaction is not null)
            contextTransaction.Commit();
        else
            throw new Exception("您未开启事务！");
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        if (contextTransaction is not null)
            await contextTransaction.CommitAsync();
        else
            throw new Exception("您未开启事务！");
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTransaction()
    {
        if (contextTransaction is not null)
            contextTransaction.Rollback();
        else
            throw new Exception("您未开启事务！");
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        if (contextTransaction is not null)
            await contextTransaction.RollbackAsync();
        else
            throw new Exception("您未开启事务！");
    }

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    public void DisposeTransaction()
    {
        if (contextTransaction is not null)
        {
            contextTransaction.Dispose();
            dbContext.Dispose();
        }
        else
            throw new Exception("您未开启事务！");
    }

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    public async Task DisposeTransactionAsync()
    {
        if (contextTransaction is not null)
        {
            await contextTransaction.DisposeAsync();
            await dbContext.DisposeAsync();
        }
        else
            throw new Exception("您未开启事务！");
    }
}
