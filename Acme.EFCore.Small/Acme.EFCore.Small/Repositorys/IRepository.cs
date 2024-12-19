using Acme.EFCore.Small.AggregateRoots;

namespace Acme.EFCore.Small.Repositorys;

/// <summary>
/// 仓储接口
/// </summary>
/// <typeparam name="TEntity">实体</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : IdAggregateRoot<TKey>, new()
    where TKey : struct
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public DbContext DbContext { get; init; }

    #region 提交
    /// <summary>
    /// 提交
    /// </summary>
    int Submit();

    /// <summary>
    /// 异步提交
    /// </summary>
    /// <returns></returns>
    Task<int> SubmitAsync();
    #endregion

    #region 新增
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity">要添加的实体对象</param>
    /// <returns>已添加的实体对象</returns>
    TEntity Add(TEntity entity);

    /// <summary>
    /// 新增立即保存
    /// </summary>
    /// <param name="entity">要添加的实体对象</param>
    /// <returns>已添加的实体对象</returns>
    TEntity AddNowSave(TEntity entity);

    /// <summary>
    /// 异步新增
    /// </summary>
    /// <param name="entity">要添加的实体对象</param>
    /// <returns>已添加的实体对象</returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// 异步新增立即提交
    /// </summary>
    /// <param name="entity">要添加的实体对象</param>
    /// <returns>已添加的实体对象</returns>
    Task<TEntity> AddNowSaveAsync(TEntity entity);

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="list">要新增的实体集合</param>
    /// <returns>已添加的实体集合</returns>
    List<TEntity> AddMany(List<TEntity> list);

    /// <summary>
    /// 批量新增立即提交
    /// </summary>
    /// <param name="list">要新增的实体集合</param>
    /// <returns>是否成功</returns>
    bool AddManyNowSave(List<TEntity> list);

    /// <summary>
    /// 异步批量新增
    /// </summary>
    /// <param name="list">要新增的实体集合</param>
    /// <returns>已新增的实体集合</returns>
    Task<List<TEntity>> AddManyAsync(List<TEntity> list);

    /// <summary>
    /// 异步批量新增立即提交
    /// </summary>
    /// <param name="list">要新增的实体集合</param>
    /// <returns>是否成功</returns>
    Task<bool> AddManyNowSaveAsync(List<TEntity> list);
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
    /// 删除立即保存
    /// </summary>
    /// <param name="id">主键Id</param>
    /// <returns>是否成功</returns>
    bool DeleteNowSave(TKey id);

    /// <summary>
    /// 异步删除立即提交
    /// </summary>
    /// <param name="id">主键Id</param>
    /// <returns></returns>
    Task<bool> DeleteNowSaveAsync(TKey id);

    /// <summary>
    /// 异步删除立即提交
    /// </summary>
    /// <param name="entity">要删除的实体</param>
    /// <returns></returns>
    Task<bool> DeleteNowSaveAsync(TEntity entity);

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
    /// <returns></returns>
    Task<bool> DelManyNowSaveAsync(List<TEntity> list);

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
    /// <returns>是否修改成功</returns>
    Task<bool> UpdateSaveAsync(TEntity entity);

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
    /// <returns></returns>
    Task<bool> UpdateManyNowSaveAsync(List<TEntity> list);
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
    /// <returns>是否存在</returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> anyLambda);

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
    /// <param name="whereLamdba">linq语句</param>
    /// <returns>实体对象</returns>
    TEntity GetInfo(Expression<Func<TEntity, bool>> whereLamdba);

    /// <summary>
    /// 异步获取单条数据
    /// </summary>
    /// <param name="whereLamdba">linq语句</param>
    /// <returns>实体对象</returns>
    Task<TEntity> GetInfoAsync(Expression<Func<TEntity, bool>> whereLamdba);

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="id">主键Id</param>
    /// <returns>实体对象</returns>
    TEntity GetInfo(TKey id);

    /// <summary>
    /// 异步获取单条数据
    /// </summary>
    /// <param name="id">主键Id</param>
    /// <returns>实体对象</returns>
    Task<TEntity> GetInfoAsync(TKey id);

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
    /// <returns>实体对象</returns>
    Task<TEntity> GetInfoNoTrackingAsync(Expression<Func<TEntity, bool>> whereLamdba);

    /// <summary>
    /// 获取单条数据不追踪
    /// </summary>
    /// <param name="id">主键Id</param>
    /// <returns>实体对象</returns>
    TEntity GetInfoNoTracking(TKey id);

    /// <summary>
    /// 异步获取单条数据不追踪
    /// </summary>
    /// <param name="id">主键Id</param>
    /// <returns>实体对象</returns>
    Task<TEntity> GetInfoNoTrackingAsync(TKey id);

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
    /// <returns>实体对象</returns>
    Task<TEntity> GetInfoDefaultAsync(Expression<Func<TEntity, bool>> whereLamdba);

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <param name="id">主键Id</param>
    /// <returns>Linq语句</returns>
    TEntity GetInfoDefault(TKey id);

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <param name="id">主键id</param>
    /// <returns>实体对象</returns>
    Task<TEntity> GetInfoDefaultAsync(TKey id);
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
    /// <returns>条数</returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> whereLamdba);

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <returns>条数</returns>
    int Count();

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <returns>条数</returns>
    Task<int> CountAsync();
    #endregion

    #region 获取集合数据

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba">Linq语句</param>
    /// <returns>实体集合</returns>
    List<TEntity> GetList(Expression<Func<TEntity, bool>> whereLamdba);

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
    /// <returns>实体集合</returns>
    Task<List<TEntity>> GetListTakeAsync(Expression<Func<TEntity, bool>> whereLamdba, int strip);

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
    /// <returns>实体集合</returns>
    Task<List<TEntity>> GetListTakeAsync(int strip);

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
    /// 根据指定条件获取不跟踪的实体列表
    /// </summary>
    /// <returns>符合条件的实体列表</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景
    /// </remarks>
    List<TEntity> GetListNoTracking();

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <returns>实体集合</returns>
    List<TEntity> GetList();

    #endregion

    #region 分页获取数据

    /// <summary>
    /// 获取分页列表
    /// </summary>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <returns>分页列表</returns>
    IPageList GetPageList(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, TKey>> keySelector);

    /// <summary>
    /// 获取分页列表
    /// </summary>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <param name="whereLamdba">Linq查询语句</param>
    /// <returns>分页列表</returns>
    IPageList GetPageList(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, TKey>> keySelector,
        Expression<Func<TEntity, bool>> whereLamdba);

    /// <summary>
    /// 异步获取分页列表
    /// </summary>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <returns>分页列表</returns>
    Task<IPageList> GetPageListAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, TKey>> keySelector);

    /// <summary>
    /// 异步获取分页列表
    /// </summary>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="keySelector">排序键选择器</param>
    /// <param name="whereLamdba">查询条件</param>
    /// <returns>分页列表</returns>
    Task<IPageList> GetPageListAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, TKey>> keySelector,
        Expression<Func<TEntity, bool>> whereLamdba);
    #endregion

    #region 事务

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
    #endregion
}