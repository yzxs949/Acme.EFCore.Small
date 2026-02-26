# Acme.EFCore.Small

## 1、项目概述
Acme.EFCore.Small 是一个轻量级的 Entity Framework Core 通用库，用于使用 Entity Framework Core (EFCore) 与数据库进行交互。它是处理各种数据库操作的基础组件。
- 版本：v1.3.6.4  
- 发布说明：
  - 更新 .NET 10 依赖包版本，Microsoft.EntityFrameworkCore 版本从 10.0.2 更新到 10.0.3。
  - 修复已知 bug...

## 2、入门指南
### 1. 安装 Acme.EFCore.Small
创建项目 -> 点击引用 -> 右键 -> 管理 NuGet 包 -> 搜索 Acme.EFCore.Small 并选择 1.3.6.4 或更高版本。根据您的 .NET 框架安装适当的版本。

### 2. 安装对应的数据库包
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

### 3. 创建数据库上下文类
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
    
    // 在此添加您的 DbSet 属性
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}
```

### 4. 配置连接字符串
```json
{
    "ConnectionStrings":{
        "DefaultConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=数据库名称;User ID=用户名;Password=密码;Connect Timeout=120;Encrypt=False;"
    }
}
```

### 5. 依赖注入
#### 5.1. 基本配置
```csharp
// 调用数据库配置信息
services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

//单库模式注入
services.AddRepositorys<AppDbContext>();

// 多库模式注入
services.AddRepositorys();
```

## 3、核心功能

### 3.1. 基础类

#### 实体基础类
```csharp
// 继承 BaseEntity 以获取基本实体属性
public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

#### 聚合根基础类
```csharp
// 继承 BaseAggregateRoot 用于聚合根实体
public class Order : BaseAggregateRoot
{
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItem> Items { get; set; }
}
```

#### 值对象基础类
```csharp
// 继承 BaseValueObject 用于值对象
public class Address : BaseValueObject
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
}
```

### 3.2. 仓储模式

#### 单库模式
单库模式适用于项目中只使用一个数据库的场景，使用 `IRepository<TEntity>` 接口。

```csharp
// 注入单库仓储
private readonly IRepository<User> _userRepository;

public UserService(IRepository<User> userRepository)
{
    _userRepository = userRepository;
}

// 添加新用户
public User AddUser(User user)
{
    return _userRepository.AddNowSave(user);
}

// 根据条件获取用户
public User GetUserById(int id)
{
    return _userRepository.GetInfo(u => u.Id == id);
}
```

#### 多库模式
多库模式适用于项目中使用多个数据库的场景，使用 `IRepository<TDbContext, TEntity>` 接口，需要指定具体的数据库上下文类型。

```csharp
// 注入多库仓储
private readonly IRepository<AppDbContext, User> _userRepository;
private readonly IRepository<OtherDbContext, Product> _productRepository;

public UserService(IRepository<AppDbContext, User> userRepository, 
                   IRepository<OtherDbContext, Product> productRepository)
{
    _userRepository = userRepository;
    _productRepository = productRepository;
}

// 从不同数据库获取数据
public async Task<(User, Product)> GetUserAndProduct(int userId, int productId)
{
    var user = await _userRepository.GetInfoAsync(u => u.Id == userId);
    var product = await _productRepository.GetInfoAsync(p => p.Id == productId);
    return (user, product);
}
```

#### 异步操作
```csharp
// 异步添加
public async Task<User> AddUserAsync(User user)
{
    return await _userRepository.AddNowSaveAsync(user);
}

// 异步获取
public async Task<User> GetUserByIdAsync(int id)
{
    return await _userRepository.GetInfoAsync(u => u.Id == id);
}
```

### 3.3. 事务管理
```csharp
// 使用事务
public void ProcessOrder(Order order)
{
    try
    {
        // 开始事务
        _orderRepository.BeginTransaction();
        
        // 执行操作
        _orderRepository.Add(order);
        
        foreach (var item in order.Items)
        {
            _orderItemRepository.Add(item);
        }
        
        // 提交事务
        _orderRepository.CommitTransaction();
    }
    catch (Exception ex)
    {
        // 出错时回滚事务
        _orderRepository.RollbackTransaction();
        throw;
    }
    finally
    {
        // 释放事务
        _orderRepository.DisposeTransaction();
    }
}
```

### 3.4. 分页功能
```csharp
// 使用分页
public PageList<User> GetUsersPaged(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return query.ToPageList(pageIndex, pageSize);
}
```

## 4、高级功能

### 4.1. 查询扩展
```csharp
// 使用查询扩展方法
public List<User> GetUsersWithSorting(string name, string sortField, bool isAscending)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    
    if (isAscending)
    {
        query = query.OrderBy(sortField);
    }
    else
    {
        query = query.OrderByDescending(sortField);
    }
    
    return query.ToList();
}
```

### 4.2. 无跟踪查询
```csharp
// 使用无跟踪查询进行只读操作
public List<User> GetUsersReadOnly()
{
    return _userRepository.GetListNoTracking();
}
```

## 5、支持的 .NET 版本
- netcoreapp3.1
- net5
- net6
- net7
- net8
- net9
- net10.0

## 6、NuGet 包信息
- 包 ID: Acme.EFCore.Small
- 作者: yzxs
- 描述: 轻量级 EFCore 操作类库
- 项目 URL: https://www.nuget.org/packages/Acme.EFCore.Small/

## 7、许可证
MIT 许可证

## 8、贡献
欢迎贡献！请随时提交 Pull Request。

## 9、联系
如有任何问题或问题，请联系作者。