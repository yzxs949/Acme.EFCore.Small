# Acme.EFCore.Small

## 1、项目概述

Acme.EFCore.Small 是一个轻量级的 Entity Framework Core 通用库，用于使用 Entity Framework Core (EFCore) 与数据库进行交互。它是处理各种数据库操作的基础组件。

- **版本**：v2.0.0.3-alpha
- **作者**：yzxs
- **描述**：轻量级 EFCore 操作类库
- **发布说明**：
  - 1.更新.NET10依赖包版本，Microsoft.EntityFrameworkCore 版本为 Version10.0.6 到 Version10.0.8
  - 2.修复已知bug……

## 2、入门指南
### 1. 安装 Acme.EFCore.Small
创建项目 -> 点击引用 -> 右键 -> 管理 NuGet 包 -> 搜索 `Acme.EFCore.Small` 并选择 2.0.0.3-alpha 或更高版本。根据您的 .NET 框架安装适当的版本。

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
// 主数据库上下文
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}

// 多库场景下的订单数据库上下文
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
```

### 4. 配置连接字符串
```json
{
    "ConnectionStrings":{
        "DefaultConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=数据库名称;User ID=用户名;Password=密码;Connect Timeout=120;Encrypt=False;",
        "OrderConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=订单数据库名称;User ID=用户名;Password=密码;Connect Timeout=120;Encrypt=False;"
    }
}
```

### 5. 依赖注入
#### 5.1. 单库模式配置

```csharp
// 注册数据库上下文
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// 注册单库模式仓储（包含工作单元）
services.AddRepositorys<AppDbContext>();
```

#### 5.2. 多库模式配置

```csharp
// 注册第一个数据库上下文
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// 注册第二个数据库上下文
services.AddDbContext<OrderDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("OrderConnection")));

// 注册多库模式仓储（包含工作单元）
services.AddRepositorys();
```

## 3、核心功能

### 3.1. 基础类

#### 实体基础类
```csharp
// 继承 BaseEntity<TKey> 以获取基本实体属性，TKey 为主键类型（值类型）
public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

#### 聚合根基础类
```csharp
// 继承 BaseAggregateRoot<TKey> 用于聚合根实体
public class Order : BaseAggregateRoot<int>
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
public int AddUser(User user)
{
    return _userRepository.AddNowSave(user);
}

// 根据条件获取用户
public User GetUserByCondition(int id)
{
    return _userRepository.GetInfo(u => u.Id == id);
}

// 根据主键获取用户
public User GetUserById(int id)
{
    return _userRepository.GetInfoById(id);
}
```

#### 多库模式
多库模式适用于项目中使用多个数据库的场景，使用 `IRepository<TDbContext, TEntity>` 接口，需要指定具体的数据库上下文类型，并配合 `IUnitOfWork` 使用。

```csharp
// 注入多库仓储和工作单元
private readonly IRepository<AppDbContext, User> _userRepository;
private readonly IRepository<OrderDbContext, Order> _orderRepository;
private readonly IUnitOfWork<OrderDbContext> _unitOfWork;

public OrderService(IRepository<AppDbContext, User> userRepository, 
                   IRepository<OrderDbContext, Order> orderRepository,
                   IUnitOfWork<OrderDbContext> unitOfWork)
{
    _userRepository = userRepository;
    _orderRepository = orderRepository;
    _unitOfWork = unitOfWork;
}

// 从不同数据库获取数据
public async Task<(User, Order)> GetUserAndOrder(int userId, int orderId)
{
    var user = await _userRepository.GetInfoAsync(u => u.Id == userId);
    var order = await _orderRepository.GetInfoAsync(o => o.Id == orderId);
    return (user, order);
}
```

#### 异步操作
```csharp
// 异步添加
public async Task<int> AddUserAsync(User user)
{
    return await _userRepository.AddNowSaveAsync(user);
}

// 异步获取（按条件）
public async Task<User> GetUserByConditionAsync(int id)
{
    return await _userRepository.GetInfoAsync(u => u.Id == id);
}

// 异步获取（按主键）
public async Task<User> GetUserByIdAsync(int id)
{
    return await _userRepository.GetInfoByIdAsync(id);
}

// 异步检查是否存在
public async Task<bool> UserExistsAsync(string email)
{
    return await _userRepository.AnyAsync(u => u.Email == email);
}
```

### 3.3. 工作单元模式

工作单元模式用于管理事务和提交操作，将数据变更作为一个原子单元进行处理。标准版使用 `IUnitOfWork` 管理事务和提交。

#### 注入工作单元

```csharp
private readonly IUnitOfWork<OrderDbContext> _unitOfWork;

public OrderService(
    IRepository<OrderDbContext, Order> orderRepository, 
    IUnitOfWork<OrderDbContext> unitOfWork)
{
    _orderRepository = orderRepository;
    _unitOfWork = unitOfWork;
}
```

#### 基本事务示例

```csharp
// 使用事务
public void ProcessOrder(Order order)
{
    try
    {
        // 开始事务
        _unitOfWork.BeginTransaction();
        
        // 执行操作
        _orderRepository.Add(order);
        
        // 提交更改
        _unitOfWork.Submit();
        
        // 提交事务
        _unitOfWork.CommitTransaction();
    }
    catch (Exception)
    {
        // 出错时回滚事务
        _unitOfWork.RollbackTransaction();
        throw;
    }
}
```

#### 异步事务示例

```csharp
// 异步事务
public async Task<bool> ProcessOrderAsync(Order order)
{
    try
    {
        // 开始事务
        await _unitOfWork.BeginTransactionAsync();
        
        // 执行操作
        await _orderRepository.AddAsync(order);
        
        // 提交更改
        await _unitOfWork.SubmitAsync();
        
        // 提交事务
        await _unitOfWork.CommitTransactionAsync();
        return true;
    }
    catch (Exception)
    {
        // 出错时回滚事务
        await _unitOfWork.RollbackTransactionAsync();
        return false;
    }
}
```

### 3.4. 分页功能
`PageList<T>` 返回结果只包含 `Total`（总记录数）和 `Items`（当前页数据）。

```csharp
// 使用分页
public PageList<User> GetUsersPaged(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return query.ToPageList(pageIndex, pageSize);
}
```

也可以使用异步分页：

```csharp
// 异步分页
public async Task<PageList<User>> GetUsersPagedAsync(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return await query.ToPageListAsync(pageIndex, pageSize);
}
```

## 4、高级功能

### 4.1. 查询扩展
排序使用 `Sorting` 对象 + `SortingType` 枚举，通过 `AddSorting` 扩展方法实现。

```csharp
// 使用 AddSorting 进行动态排序
public List<User> GetUsersWithSorting(string name, string sortField, bool isAscending)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    
    var sorting = new Sorting
    {
        SortField = sortField,
        SortingType = isAscending ? SortingType.ASC : SortingType.DESC
    };
    query = query.AddSorting(sorting);
    
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

// 带条件的无跟踪查询
public User GetUserByIdReadOnly(int id)
{
    return _userRepository.GetInfoNoTracking(u => u.Id == id);
}

// 异步无跟踪查询
public async Task<List<User>> GetUsersReadOnlyAsync(string name)
{
    return await _userRepository.GetListNoTrackingAsync(u => u.Name.Contains(name));
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

- **包 ID**: Acme.EFCore.Small
- **作者**: yzxs
- **描述**: 轻量级 EFCore 操作类库
- **项目 URL**: <https://www.nuget.org/packages/Acme.EFCore.Small/2.0.0.3-alpha#readme-body-tab>
- **版权**: yzxs

## 7、联系

如有任何问题或建议，请联系作者

- **邮箱**: <yzxs949@163.com>
- **NuGet**: <https://www.nuget.org/packages/Acme.EFCore.Small/>