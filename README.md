# Acme.EFCore.Small

## 一、概述
Acme.EFCore.Small是一款轻量级的EFCore通用类库，旨在使用 Entity Framework Core（EFCore）与数据库进行交互。它作为处理各种数据库操作的基础组件。

## 二、入门指南 
### 1.安装Acme.EFCore.Small
创建项目 -> 点击引用 -> 右键 -> 管理Nuget程序包->搜索Acme.EFCore.Small选择对应版本的.NET版本安装即可。
### 2.安装对应数据库包
- SqlServer: `Microsoft.EntityFrameworkCore.SqlServer`
- Sqlite: `Microsoft.EntityFrameworkCore.Sqlite`
- Cosmos: `Microsoft.EntityFrameworkCore.Cosmos`
- InMemoryDatabase: `Microsoft.EntityFrameworkCore.InMemory`
- MySql:
  - `Pomelo.EntityFrameworkCore.MySql`
  - `MySql.EntityFrameworkCore`
- PostgreSQL: `Npgsql.EntityFrameworkCore.PostgreSQL`
- Oracle: `Oracle.EntityFrameworkCore`
- Firebird: `FirebirdSql.EntityFrameworkCore.Firebird`
- Dm: `Microsoft.EntityFrameworkCore.Dm`

### 4.创建DbContext数据库上下文类
```csharp
public class AppDbContext : DbContext
{
    /// <summary>
    /// 初始化数据库上下文
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options) :
       base(options)
    {
    }
}
```
### 5.配置连接字符串
```json
{
    "ConnectionStrings":{
        "DefaultConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=数据库名;User ID=用户;Password=密码;Connect Timeout=120;Encrypt=False;"
    }
}
```

### 6.依赖注入
#### 6.1.基础配置
```csharp
//调用数据库配置信息
services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
//注入基础类IBaseService
services.AddBaseService<AppDbContext>();
//注入仓储类
services.AddRepository();
//注入静态Db类
services.AddDbExtension();
```
#### 6.2.多库配置
```csharp
//调用数据库配置信息
services.AddDbContext<AppDbContext1>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection1")));
//注入基础类IBaseService,传入数据库上下文进行注入
services.AddBaseService();
```
### 7.使用
```csharp
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IRepository<UserInfo> _userRepository;
        //构造函数注入
        public UserController(IRepository<UserInfo> userRepository)
        {
            _userRepository=userRepository;
        }

        public void Operate(UserInfo user)
        {
            //新增
            _userRepository.Add(info);
            //删除
            _userRepository.Delete(info);
            //查询
            _userRepository.GetList(info);
        }
    }
```
## 三、进阶
### 1.基础父类 BaseService
```csharp
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
    T? GetInfo<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<T?> GetInfoAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

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
    T? GetInfoDefault<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<T?> GetInfoDefaultAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

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
```
### 2.多库父类
```csharp
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
    T? GetInfo<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<T?> GetInfoAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

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
    T? GetInfoDefault<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<T?> GetInfoDefaultAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class;

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
```
### 3.仓储模式
```csharp
public interface IRepository<T> where T : class, new()
{

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    T Add(T entity);

    /// <summary>
    /// 新增实体集合
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <returns></returns>
    bool Add(List<T> entitys);

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// 新增实体集合
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <returns></returns>
    Task<bool> AddAsync(List<T> entitys);

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
    /// 获取条数
    /// </summary>
    /// <returns></returns>
    int Count();

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    int Count(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <returns></returns>
    Task<int> CountAsync();

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    bool Delete(T entity);

    /// <summary>
    /// 删除集合
    /// </summary>
    /// <param name="entitys">集合</param>
    /// <returns></returns>
    bool Delete(List<T> entitys);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(T entity);

    /// <summary>
    /// 删除集合
    /// </summary>
    /// <param name="entitys">删除</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(List<T> entitys);

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    void DisposeTransaction();

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    Task DisposeTransactionAsync();

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    T? GetInfo(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<T?> GetInfoAsync(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    T? GetInfoDefault(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<T?> GetInfoDefaultAsync(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <returns></returns>
    List<T> GetList();

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    List<T> GetList(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <returns></returns>
    Task<List<T>> GetListAsync();

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    List<T> GetListNoTracking(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    List<T> GetListNoTracking();

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    Task<List<T>> GetListNoTrackingAsync();

    /// <summary>
    /// 根据指定条件获取不跟踪的实体列表。
    /// </summary>
    /// <param name="whereLamdba">筛选条件的 Lambda 表达式。</param>
    /// <returns>符合条件的实体列表。</returns>
    /// <remarks>
    /// 此方法返回的实体列表不会被上下文跟踪，适用于只需要读取数据而不需要对实体进行更改的场景。
    /// </remarks>
    Task<List<T>> GetListNoTrackingAsync(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 获取Queryable
    /// </summary>
    /// <returns></returns>
    IQueryable<T> GetQueryable();

    /// <summary>
    /// 按条件获取Queryable
    /// </summary>
    /// <param name="whereLamdba">Linq语句</param>
    /// <returns></returns>
    IQueryable<T> GetQueryable(Expression<Func<T, bool>> whereLamdba);

    /// <summary>
    /// 回滚事务
    /// </summary>
    void RollbackTransaction();

    /// <summary>
    /// 回滚事务
    /// </summary>
    Task RollbackTransactionAsync();

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    bool Update(T entity);

    /// <summary>
    /// 修改集合
    /// </summary>
    /// <param name="entitys">集合</param>
    /// <returns></returns>
    bool Update(List<T> entitys);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    Task<bool> UpdateAsync(T entity);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entitys">集合</param>
    /// <returns></returns>
    Task<bool> UpdateAsync(List<T> entitys);

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    List<T> GetListTake(Expression<Func<T, bool>> whereLamdba, int strip);

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    Task<List<T>> GetListTakeAsync(Expression<Func<T, bool>> whereLamdba, int strip);

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    List<T> GetListTake(int strip);
    
    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="strip">条数</param>
    /// <returns></returns>
    Task<List<T>> GetListTakeAsync(int strip);
}
```
### 4.静态模式
```csharp
//单库
Db.GetRepository<T>();

//多库
Db.GetRepository<TDbContext, T>();
```
### 5.联合查询
```csharp
var userList = await (from user in _userRepository.GetQueryable(s => s.UserRole == UserRoleEnum.ZH)
                      join cqal in _cqalRepository.GetQueryable(s => s.State == CqalStateEnum.TG)
                      on user.UserId equals cqal.UserId
                      join price in _priceRepository.GetQueryable()
                      on user.UserId equals price.UserId
                      select user)
.ToListAsync();
```
### 6.事务,基础类中提供事务的开启，提交，回滚，关闭