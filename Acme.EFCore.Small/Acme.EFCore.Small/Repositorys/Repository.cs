namespace Acme.EFCore.Small.Repositorys;

/// <summary>
/// 仓储类
/// </summary>
/// <typeparam name="T"></typeparam>
public class Repository<T> : IRepository<T> where T : class, new()
{
    private readonly IBaseService _baseService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="baseService"></param>
    public Repository(IBaseService baseService)
    {
        _baseService = baseService;
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    public T Add(T entity)
    {
        return _baseService.Add<T>(entity);
    }

    /// <summary>
    /// 新增实体集合
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <returns></returns>
    public bool Add(List<T> entitys)
    {
        return _baseService.AddMany<T>(entitys);
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    public async Task<T> AddAsync(T entity)
    {
        return await _baseService.AddAsync<T>(entity);
    }

    /// <summary>
    /// 新增实体集合
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <returns></returns>
    public async Task<bool> AddAsync(List<T> entitys)
    {
        return await _baseService.AddManyAsync<T>(entitys);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    public bool Delete(T entity)
    {
        return _baseService.Delete<T>(entity);
    }

    /// <summary>
    /// 删除集合
    /// </summary>
    /// <param name="entitys">集合</param>
    /// <returns></returns>
    public bool Delete(List<T> entitys)
    {
        return _baseService.DelMany<T>(entitys);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(T entity)
    {
        return await _baseService.DeleteAsync<T>(entity);
    }

    /// <summary>
    /// 删除集合
    /// </summary>
    /// <param name="entitys">删除</param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(List<T> entitys)
    {
        return await _baseService.DelManyAsync<T>(entitys);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    public bool Update(T entity)
    {
        return _baseService.Update<T>(entity);
    }

    /// <summary>
    /// 修改集合
    /// </summary>
    /// <param name="entitys">集合</param>
    /// <returns></returns>
    public bool Update(List<T> entitys)
    {
        return _baseService.UpdateMany(entitys);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(T entity)
    {
        return await _baseService.UpdateAsync<T>(entity);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entitys">集合</param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(List<T> entitys)
    {
        return await _baseService.UpdateManyAsync<T>(entitys);
    }

    /// <summary>
    /// 获取Queryable
    /// </summary>
    /// <returns></returns>
    public IQueryable<T> GetQueryable()
    {
        return _baseService.GetQueryable<T>();
    }

    /// <summary>
    /// 按条件获取Queryable
    /// </summary>
    /// <param name="whereLamdba">Linq语句</param>
    /// <returns></returns>
    public IQueryable<T> GetQueryable(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetQueryable<T>(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public T? GetInfo(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetInfo<T>(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据不追踪
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public T? GetInfoNoTracking(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetInfoNoTracking<T>(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据不追踪
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T?> GetInfoNoTrackingAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.GetInfoNoTrackingAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T?> GetInfoAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.GetInfoAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public List<T> GetList(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetList<T>(whereLamdba);
    }

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    public List<T> GetListNoTracking(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetListNoTracking<T>(whereLamdba);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <returns></returns>
    public List<T> GetList()
    {
        return _baseService.GetList<T>();
    }

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    public List<T> GetListNoTracking()
    {
        return _baseService.GetListNoTracking<T>();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.GetListAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    public async Task<List<T>> GetListNoTrackingAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.GetListNoTrackingAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync()
    {
        return await _baseService.GetListAsync<T>();
    }

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    public async Task<List<T>> GetListNoTrackingAsync()
    {
        return await _baseService.GetListNoTrackingAsync<T>();
    }

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public T? GetInfoDefault(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetInfoDefault<T>(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T?> GetInfoDefaultAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.GetInfoDefaultAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public int Count(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.Count(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<int> CountAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.CountAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <returns></returns>
    public int Count()
    {
        return _baseService.Count<T>();
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <returns></returns>
    public async Task<int> CountAsync()
    {
        return await _baseService.CountAsync<T>();
    }

    /// <summary>
    /// 开启事务
    /// </summary>
    /// <returns></returns>
    public void BeginTransaction()
    {
        _baseService.BeginTransaction();
    }

    /// <summary>
    /// 开启事务
    /// </summary>
    /// <returns></returns>
    public async Task BeginTransactionAsync()
    {
        await _baseService.BeginTransactionAsync();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTransaction()
    {
        _baseService.CommitTransaction();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        await _baseService.CommitTransactionAsync();
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTransaction()
    {
        _baseService.RollbackTransaction();
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        await _baseService.RollbackTransactionAsync();
    }

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    public void DisposeTransaction()
    {
        _baseService.DisposeTransaction();
    }

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    public async Task DisposeTransactionAsync()
    {
        await _baseService.DisposeTransactionAsync();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    public List<T> GetListTake(Expression<Func<T, bool>> whereLamdba, int strip)
    {
        return _baseService.GetListTake<T>(whereLamdba, strip);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    public async Task<List<T>> GetListTakeAsync(Expression<Func<T, bool>> whereLamdba, int strip)
    {
        return await _baseService.GetListTakeAsync(whereLamdba, strip);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    public List<T> GetListTake(int strip)
    {
        return _baseService.GetListTake<T>(strip);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    public async Task<List<T>> GetListTakeAsync(int strip)
    {
        return await _baseService.GetListTakeAsync<T>(strip);
    }

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <returns>分页列表</returns>
    public PageList<T> GetPageList<TKey>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TKey>> keySelector)
    {
        return _baseService.GetPageList(pageIndex, pageSize, keySelector);
    }

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <param name="whereLamdba">查询条件</param>
    /// <returns>分页列表</returns>
    public PageList<T> GetPageList<TKey>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TKey>> keySelector,
        Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetPageList(pageIndex, pageSize, keySelector, whereLamdba);
    }

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <returns>分页列表</returns>
    public async Task<PageList<T>> GetPageListAsync<TKey>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TKey>> keySelector)
    {
        return await _baseService.GetPageListAsync(pageIndex, pageSize, keySelector);
    }

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <param name="whereLamdba">查询条件</param>
    /// <returns>分页列表</returns>
    public async Task<PageList<T>> GetPageListAsync<TKey>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, TKey>> keySelector,
        Expression<Func<T, bool>> whereLamdba) => await _baseService.GetPageListAsync(pageIndex, pageSize, keySelector, whereLamdba);
}