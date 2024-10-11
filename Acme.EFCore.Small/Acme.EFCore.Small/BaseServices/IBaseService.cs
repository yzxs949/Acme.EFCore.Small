namespace Acme.EFCore.Small.BaseServices;

/// <summary>
/// 实现基于 Entity Framework Core 的服务类，用于处理与数据库的交互操作接口。
/// </summary>
/// <typeparam name="TDbContext">DbContext 的类型参数，用于指定要使用的数据库上下文。</typeparam>
public interface IBaseService<TDbContext> where TDbContext : DbContext
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    TDbContext dbContext { get; set; }

    /// <summary>
    /// 新增
    /// </summary>
    /// <typeparam name="T">要添加的实体类型</typeparam>
    /// <param name="entity">要添加的实体对象</param>
    /// <returns>已添加的实体对象</returns>
    T Add<T>(T entity) where T : class;

    /// <summary>
    /// 异步新增
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
    List<T> AddMany<T>(List<T> list) where T : class;

    /// <summary>
    /// 异步批量新增
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Task<List<T>> AddManyAsync<T>(List<T> list) where T : class;

    /// <summary>
    /// 异步批量新增立即提交
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Task<bool> AddManySaveAsync<T>(List<T> list) where T : class;

    /// <summary>
    /// 新增立即提交
    /// </summary>
    /// <typeparam name="T">要添加的实体类型</typeparam>
    /// <param name="entity">要添加的实体对象</param>
    /// <returns>已添加的实体对象</returns>
    T AddSave<T>(T entity) where T : class;

    /// <summary>
    /// 异步新增立即提交
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> AddSaveAsync<T>(T entity) where T : class;

    /// <summary>
    /// 批量新增立即提交
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    bool AddSaveMany<T>(List<T> list) where T : class;

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
    /// <returns></returns>
    void CommitTransaction();

    /// <summary>
    /// 异步提交事务
    /// </summary>
    /// <returns></returns>
    Task CommitTransactionAsync();

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
    int Count<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

    /// <summary>
    /// 异步获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<int> CountAsync<T>() where T : class;

    /// <summary>
    /// 异步获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<int> CountAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="id"></param>
    void Delete<T, TKey>(TKey id)
        where T : BaseEntityWithId<TKey>
        where TKey : struct;

    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    void Delete<T>(T entity) where T : class;

    /// <summary>
    /// 删除立即保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="id"></param>
    void DeleteSave<T, TKey>(TKey id)
        where T : BaseEntityWithId<TKey>
        where TKey : struct;

    /// <summary>
    /// 删除立即保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool DeleteSave<T>(T entity) where T : class;

    /// <summary>
    /// 异步删除立即保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteSaveAsync<T, TKey>(TKey id)
        where T : BaseEntityWithId<TKey>
        where TKey : struct;

    /// <summary>
    /// 删除立即保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> DeleteSaveAsync<T>(T entity) where T : class;

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    void DelMany<T>(List<T> list) where T : class;

    /// <summary>
    /// 批量删除立即保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    Task<bool> DelManySaveAsync<T>(List<T> list) where T : class;

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    void DisposeTransaction();

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    Task DisposeTransactionAsync();

    /// <summary>
    /// 
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
    /// 获取单条数据不追踪
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    T GetInfoNoTracking<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

    /// <summary>
    /// 获取单条数据不追踪
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<T> GetInfoNoTrackingAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

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
    List<T> GetList<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<List<T>> GetListAsync<T>() where T : class;

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

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
    List<T> GetListNoTracking<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

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
    /// <param name="strip">条数</param>
    /// <returns></returns>
    List<T> GetListTake<T>(int strip) where T : class;

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
    Task<List<T>> GetListTakeAsync<T>(int strip) where T : class;

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <returns>分页列表</returns>
    PageList<T> GetPageList<T, TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector) where T : class;

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
    PageList<T> GetPageList<T, TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> whereLamdba) where T : class;

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <returns>分页列表</returns>
    Task<PageList<T>> GetPageListAsync<T, TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector) where T : class;

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
    Task<PageList<T>> GetPageListAsync<T, TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> whereLamdba) where T : class;

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
    /// 回滚事务
    /// </summary>
    void RollbackTransaction();

    /// <summary>
    /// 回滚事务
    /// </summary>
    Task RollbackTransactionAsync();

    /// <summary>
    /// 提交
    /// </summary>
    int Submit();

    /// <summary>
    /// 异步提交
    /// </summary>
    Task<int> SubmitAsync();

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Update<T>(T entity) where T : class;

    /// <summary>
    /// 批量修改
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    void UpdateMany<T>(List<T> list) where T : class;

    /// <summary>
    /// 异步批量修改立即提交
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Task<bool> UpdateManyAsync<T>(List<T> list) where T : class;

    /// <summary>
    /// 修改立即提交
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool UpdateSave<T>(T entity) where T : class;

    /// <summary>
    /// 异步修改立即提交
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> UpdateSaveAsync<T>(T entity) where T : class;

    /// <summary>
    /// 批量修改立即提交
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    bool UpdateSaveMany<T>(List<T> list) where T : class;
}