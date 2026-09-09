# Acme.EFCore.Small

[🇨🇳 中文](./README.md) | [🇬🇧 English](./README.EN.md)

![Version](https://img.shields.io/badge/version-v2.0.0.6-blue.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)
![EFCore](https://img.shields.io/badge/EFCore-2.0+-orange.svg)
![.NET](https://img.shields.io/badge/.NET-3.1%20%7C%205%20%7C%206%20%7C%207%20%7C%208%20%7C%209%20%7C%2010-9cf.svg)

`版本 v2.0.0.6`  `许可证 MIT`  `EFCore 2.0+`  `.NET 3.1/5/6/7/8/9/10`

轻量级的 Entity Framework Core 通用库，提供仓储模式、工作单元、分页、动态排序等开箱即用的数据访问能力。

---

## 📑 目录

- [1、项目概述](#1项目概述)
- [2、入门指南](#2入门指南)
  - [2.1 安装 Acme.EFCore.Small](#21-安装-acmeefcoresmall)
  - [2.2 安装对应的数据库包](#22-安装对应的数据库包)
  - [2.3 创建数据库上下文类](#23-创建数据库上下文类)
  - [2.4 配置连接字符串](#24-配置连接字符串)
  - [2.5 依赖注入](#25-依赖注入)
- [3、核心功能](#3核心功能)
  - [3.1 基础类](#31-基础类)
  - [3.2 仓储模式](#32-仓储模式)
  - [3.3 工作单元模式](#33-工作单元模式)
  - [3.4 分页功能](#34-分页功能)
- [4、高级功能](#4高级功能)
  - [4.1 查询扩展](#41-查询扩展)
  - [4.2 无跟踪查询](#42-无跟踪查询)
  - [4.3 批量操作](#43-批量操作)
- [5、完整使用案例](#5完整使用案例)
  - [5.1 单库模式完整示例](#51-单库模式完整示例)
  - [5.2 多库模式完整示例](#52-多库模式完整示例)
- [6、支持的 .NET 版本](#6支持的-net-版本)
- [7、NuGet 包信息](#7nuget-包信息)
- [8、测试](#8测试)
- [9、许可证](#9许可证)
- [10、贡献](#10贡献)
- [11、联系](#11联系)

---

## 1、项目概述

Acme.EFCore.Small 是一个轻量级的 Entity Framework Core 通用库，用于使用 Entity Framework Core (EFCore) 与数据库进行交互。它是处理各种数据库操作的基础组件。

| 项目信息 | 内容 |
| :--- | :--- |
| **版本** | v2.0.0.6 |
| **作者** | yzxs |
| **描述** | 轻量级 EFCore 操作类库 |
| **许可证** | MIT |

**发布说明（v2.0.0.6）**：

- 升级 `Microsoft.EntityFrameworkCore` 依赖包版本：net8 由 8.0.30 升级到 8.0.31，net9 由 9.0.19 升级到 9.0.20，net10.0 由 10.0.11 升级到 10.0.12。
- 同步更新中文与英文 README 文档与测试报告至 2.0.0.6。

**核心特性**：

- ✅ 单库 / 多库仓储模式
- ✅ 工作单元（事务管理，由 `IUnitOfWork` 提供）
- ✅ 分页、动态排序、条件查询扩展
- ✅ 无跟踪查询，提升只读性能
- ✅ 同步 / 异步 API 全覆盖
- ✅ 批量增删改

> ⚠️ **重要约定**：事务相关方法（`BeginTransaction` / `CommitTransaction` / `RollbackTransaction` 及其异步版本）**只在 `IUnitOfWork` 上提供，不在仓储接口上**。仓储上的 `Add`/`Update`/`Delete` 等方法只把变更挂到上下文，需调用 `IUnitOfWork.Submit()` 才会真正落库。

---

## 2、入门指南

### 2.1 安装 Acme.EFCore.Small

创建项目 → 右键引用 → 管理 NuGet 包 → 搜索 `Acme.EFCore.Small` 并选择 2.0.0.6 或更高版本。根据您的 .NET 框架安装适当的版本。

> 注意：NuGet 包名为 **`Acme.EFCore.Small`**（注意拼写，不是 AddRepositories）。

### 2.2 安装对应的数据库包

根据所使用的数据库，安装对应的 EF Core 提供程序：

| 数据库 | NuGet 包 |
| :--- | :--- |
| **SqlServer** | `Microsoft.EntityFrameworkCore.SqlServer` |
| **Sqlite** | `Microsoft.EntityFrameworkCore.Sqlite` |
| **Cosmos** | `Microsoft.EntityFrameworkCore.Cosmos` |
| **InMemory** | `Microsoft.EntityFrameworkCore.InMemory` |
| **MySql** | `Pomelo.EntityFrameworkCore.MySql` / `MySql.EntityFrameworkCore` |
| **PostgreSQL** | `Npgsql.EntityFrameworkCore.PostgreSQL` |
| **Oracle** | `Oracle.EntityFrameworkCore` |
| **Firebird** | `FirebirdSql.EntityFrameworkCore.Firebird` |
| **Dm（达梦）** | `Microsoft.EntityFrameworkCore.Dm` |

### 2.3 创建数据库上下文类

```csharp
// 主数据库上下文
public class AppDbContext : DbContext
{
    /// <summary>
    /// 初始化数据库上下文
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // 在此添加您的 DbSet 属性
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}

// 多库场景下的第二个数据库上下文
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
```

### 2.4 配置连接字符串

在 `appsettings.json` 中配置：

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=数据库名称;User ID=用户名;Password=密码;Connect Timeout=120;Encrypt=False;",
        "OrderConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=订单数据库名称;User ID=用户名;Password=密码;Connect Timeout=120;Encrypt=False;"
    }
}
```

### 2.5 依赖注入

本库通过扩展方法 `AddRepositorys`（定义在 `Acme.EFCore.Small.Extensions.CollectionExtension` 命名空间）注册服务：

- `AddRepositorys<TDbContext>()`：**单库模式**，注册非泛型的 `IUnitOfWork` / `IRepository<>` 与强类型的 `DbContext`。
- `AddRepositorys()`：**多库模式**，注册泛型的 `IUnitOfWork<>` / `IRepository<,>`。

#### 2.5.1 单库模式配置

```csharp
// 在 Program.cs / Startup.cs 中配置（注意 using Acme.EFCore.Small.Extensions;）
// 注册数据库上下文
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 注册单库模式仓储（包含工作单元）
builder.Services.AddRepositorys<AppDbContext>();
```

#### 2.5.2 多库模式配置

```csharp
// 在 Program.cs / Startup.cs 中配置（注意 using Acme.EFCore.Small.Extensions;）
// 注册第一个数据库上下文
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 注册第二个数据库上下文
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnection")));

// 注册多库模式仓储（包含工作单元）
builder.Services.AddRepositorys();
```

---

## 3、核心功能

### 3.1 基础类

所有实体基类均位于对应的命名空间下，并提供单一主键属性 `Id`（类型为泛型 `TKey`，约束为值类型）。

#### 3.1.1 实体基础类

`BaseEntity<TKey>`（`Acme.EFCore.Small.Entitys`）提供主键 `Id`，适用于大多数实体类型。

```csharp
// 继承 BaseEntity<TKey> 获取主键 Id
public class User : BaseEntity<int>
{
    // Id 由基类提供
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
}
```

#### 3.1.2 聚合根基础类

`BaseAggregateRoot<TKey>`（`Acme.EFCore.Small.AggregateRoots`）继承自 `BaseEntity<TKey>`，适用于作为聚合根的实体。

```csharp
// 继承 BaseAggregateRoot<TKey> 用于聚合根实体
public class Order : BaseAggregateRoot<int>
{
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}

// 子实体
public class OrderItem : BaseEntity<int>
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
```

#### 3.1.3 值对象基础类

`BaseValueObject`（`Acme.EFCore.Small.ValueObjects`）适用于没有唯一标识的值对象。

```csharp
// 继承 BaseValueObject 用于值对象
public class Address : BaseValueObject
{
    public string Street { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
}

// 在实体中使用值对象
public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public Address HomeAddress { get; set; }
}
```

### 3.2 仓储模式

#### 3.2.1 单库模式

单库模式使用 `IRepository<TEntity>` 接口（注入时带非泛型 `IUnitOfWork`）。

**关于返回值**：

- `AddNowSave` / `AddNowSaveAsync` 返回 **`int`**（受影响行数）。
- `Add` / `AddAsync` **不立即保存**，仅挂到上下文，需 `IUnitOfWork.Submit()` 提交。
- `DeleteNowSave` / `DeleteNowSaveAsync` / `UpdateNowSave` / `UpdateSaveAsync` / `DelManyNowSave*` / `UpdateManyNowSave*` 返回 **`bool`**（是否成功）。

```csharp
// 注入单库仓储
private readonly IRepository<User> _userRepository;
private readonly IRepository<Product> _productRepository;

public UserService(IRepository<User> userRepository, IRepository<Product> productRepository)
{
    _userRepository = userRepository;
    _productRepository = productRepository;
}

// 1. 添加新用户（返回受影响行数 int）
public int AddUser(User user)
{
    return _userRepository.AddNowSave(user);
}

// 2. 根据主键获取用户（返回实体，找不到返回 null）
public User GetUserById(int id)
{
    return _userRepository.GetInfoById(id);
}

// 3. 根据条件获取用户（返回第一条或 null，使用 GetInfoDefault）
public User GetUserByCondition(int id)
{
    return _userRepository.GetInfoDefault(u => u.Id == id);
}

// 4. 更新用户（返回 bool）
public bool UpdateUser(User user)
{
    return _userRepository.UpdateNowSave(user);
}

// 5. 删除用户（返回 bool）
public bool DeleteUser(User user)
{
    return _userRepository.DeleteNowSave(user);
}

// 6. 获取带条件的用户列表
public List<User> GetUsers(string name)
{
    return _userRepository.GetList(u => u.Name.Contains(name));
}

// 7. 批量添加用户（返回受影响行数 int）
public int AddUsers(List<User> users)
{
    return _userRepository.AddManyNowSave(users);
}

// 8. 检查用户是否存在
public bool UserExists(string email)
{
    return _userRepository.Any(u => u.Email == email);
}

// 9. 获取用户数量
public int GetUserCount(bool isActive)
{
    return _userRepository.Count(u => u.IsActive == isActive);
}
```

#### 3.2.2 多库模式

多库模式使用 `IRepository<TDbContext, TEntity>` 接口，需要指定具体的数据库上下文类型，并配合对应上下文的 `IUnitOfWork<TDbContext>`。

```csharp
// 注入多库仓储和各自上下文的工作单元
private readonly IRepository<AppDbContext, User> _userRepository;
private readonly IRepository<OrderDbContext, Order> _orderRepository;
private readonly IRepository<OrderDbContext, OrderItem> _orderItemRepository;
private readonly IUnitOfWork<OrderDbContext> _orderUnitOfWork;

public OrderService(IRepository<AppDbContext, User> userRepository,
                   IRepository<OrderDbContext, Order> orderRepository,
                   IRepository<OrderDbContext, OrderItem> orderItemRepository,
                   IUnitOfWork<OrderDbContext> orderUnitOfWork)
{
    _userRepository = userRepository;
    _orderRepository = orderRepository;
    _orderItemRepository = orderItemRepository;
    _orderUnitOfWork = orderUnitOfWork;
}

// 从不同数据库获取数据
public async Task<(User, Order)> GetUserAndOrder(int userId, int orderId)
{
    var user = await _userRepository.GetInfoDefaultAsync(u => u.Id == userId);
    var order = await _orderRepository.GetInfoDefaultAsync(o => o.Id == orderId);
    return (user, order);
}

// 在单个数据库（OrderDbContext）内操作，配合其工作单元落库
public async Task<bool> CreateOrder(Order order)
{
    _orderRepository.Add(order);                       // 仅挂到上下文
    foreach (var item in order.Items)
    {
        item.OrderId = order.Id;
        _orderItemRepository.Add(item);
    }
    return await _orderUnitOfWork.SubmitAsync() > 0;    // 统一提交
}
```

#### 3.2.3 异步操作

> 注意：按条件获取单条数据应使用 `GetInfoDefaultAsync`（非 `GetInfoAsync`）；按主键获取使用 `GetInfoByIdAsync`。

```csharp
// 异步添加（返回受影响行数 int）
public async Task<int> AddUserAsync(User user)
{
    return await _userRepository.AddNowSaveAsync(user);
}

// 异步按条件获取单条（返回实体或 null）
public async Task<User> GetUserByConditionAsync(int id)
{
    return await _userRepository.GetInfoDefaultAsync(u => u.Id == id);
}

// 异步按主键获取（返回实体或 null）
public async Task<User> GetUserByIdAsync(int id)
{
    return await _userRepository.GetInfoByIdAsync(id);
}

// 异步批量添加（返回受影响行数 int）
public async Task<int> AddUsersAsync(List<User> users)
{
    return await _userRepository.AddManyNowSaveAsync(users);
}

// 异步获取列表
public async Task<List<User>> GetUsersAsync(string name)
{
    return await _userRepository.GetListAsync(u => u.Name.Contains(name));
}

// 异步检查是否存在
public async Task<bool> UserExistsAsync(string email)
{
    return await _userRepository.AnyAsync(u => u.Email == email);
}
```

### 3.3 工作单元模式

工作单元（`IUnitOfWork` 单库 / `IUnitOfWork<TDbContext>` 多库）用于管理事务与提交。**事务方法只在 `IUnitOfWork` 上**，不在仓储上。

**`IUnitOfWork` / `IUnitOfWork<TDbContext>` 提供的方法**：

| 方法 | 说明 |
| :--- | :--- |
| `int Submit()` / `Task<int> SubmitAsync()` | 将挂起的变更保存到数据库，返回受影响行数 |
| `void BeginTransaction()` / `Task BeginTransactionAsync()` | 开始事务 |
| `void CommitTransaction()` / `Task CommitTransactionAsync()` | 提交事务 |
| `void RollbackTransaction()` / `Task RollbackTransactionAsync()` | 回滚事务 |
| `DbContext DbContext { get; }` | 当前关联的数据库上下文 |

#### 3.3.1 注入工作单元

```csharp
// 单库模式（非泛型 IUnitOfWork）
private readonly IUnitOfWork _unitOfWork;

// 多库模式（泛型 IUnitOfWork<TDbContext>）
private readonly IUnitOfWork<OrderDbContext> _orderUnitOfWork;
```

#### 3.3.2 基本事务示例

```csharp
private readonly IRepository<Order> _orderRepository;   // 单库模式示例
private readonly IRepository<OrderItem> _orderItemRepository;
private readonly IUnitOfWork _unitOfWork;

public OrderService(IRepository<Order> orderRepository,
                   IRepository<OrderItem> orderItemRepository,
                   IUnitOfWork unitOfWork)
{
    _orderRepository = orderRepository;
    _orderItemRepository = orderItemRepository;
    _unitOfWork = unitOfWork;
}

// 使用事务
public void ProcessOrder(Order order)
{
    try
    {
        // 开始事务（在 IUnitOfWork 上调用）
        _unitOfWork.BeginTransaction();

        // 仓储方法只挂到上下文，不立即保存
        _orderRepository.Add(order);
        foreach (var item in order.Items)
        {
            _orderItemRepository.Add(item);
        }

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

#### 3.3.3 异步事务示例

```csharp
// 异步事务（多库模式，使用 IUnitOfWork<OrderDbContext>）
public async Task<bool> ProcessOrderAsync(Order order)
{
    try
    {
        await _orderUnitOfWork.BeginTransactionAsync();

        _orderRepository.Add(order);
        foreach (var item in order.Items)
        {
            _orderItemRepository.Add(item);
        }

        await _orderUnitOfWork.SubmitAsync();
        await _orderUnitOfWork.CommitTransactionAsync();
        return true;
    }
    catch (Exception)
    {
        await _orderUnitOfWork.RollbackTransactionAsync();
        return false;
    }
}
```

### 3.4 分页功能

分页扩展 `ToPageList` / `ToPageListAsync` 定义在 `LinqExtension` 中，作用于 `IQueryable<T>`。`pageIndex` 从 **1** 开始（`pageIndex-1` 作为 Skip 偏移）。

**`PageList<T>` 结果属性**：

- `int Total`：总记录数
- `List<T> Items`：当前页数据

#### 3.4.1 基本分页

```csharp
public PageList<User> GetUsersPaged(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return query.ToPageList(pageIndex, pageSize);
}

// 异步版本
public async Task<PageList<User>> GetUsersPagedAsync(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return await query.ToPageListAsync(pageIndex, pageSize);
}
```

#### 3.4.2 带排序的分页

```csharp
public PageList<User> GetUsersPagedWithSorting(int pageIndex, int pageSize, string name, string sortField, bool isAscending)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));

    // 构建排序对象（也可用 new Sorting(sortField, sortingType) 构造函数）
    var sorting = new Sorting
    {
        SortField = sortField,
        SortingType = isAscending ? SortingType.ASC : SortingType.DESC
    };
    query = query.AddSorting(sorting);

    return query.ToPageList(pageIndex, pageSize);
}
```

#### 3.4.3 分页结果使用

```csharp
var pageResult = userService.GetUsersPaged(1, 10, "张");

int total = pageResult.Total;          // 总记录数
List<User> items = pageResult.Items;   // 当前页数据
```

---

## 4、高级功能

### 4.1 查询扩展

查询扩展位于 `Acme.EFCore.Small.Extensions.LinqExtension`，作用于 `IQueryable<T>` 或 `IEnumerable<T>`，提供基于 Lambda 与基于对象（Condition / Keywords / Sorting）两套动态查询能力。

#### 4.1.1 条件过滤（Lambda）：WhereIf

`WhereIf(bool, lambda)` 在条件成立时才追加 `Where` 过滤（支持 `IQueryable` 与 `IEnumerable`）。

```csharp
public List<User> GetUsersWithComplexConditions(string name, int? age, bool? isActive)
{
    var query = _userRepository.Queryable();

    query = query.WhereIf(!string.IsNullOrEmpty(name), u => u.Name.Contains(name));
    query = query.WhereIf(age.HasValue, u => u.Age == age.Value);
    query = query.WhereIf(isActive.HasValue, u => u.IsActive == isActive.Value);

    return query.ToList();
}
```

#### 4.1.2 条件对象查询：Condition 与 Symbol

除了 Lambda，还可以用 **`Condition`** 对象（字段名 + 值 + 运算符）动态拼接查询条件，内部通过表达式树构建 `AND` 连接。`Condition` 提供两个构造函数：

```csharp
public Condition(string field, string value, Symbol symbol)  // 字段名、值、运算符
public Condition()                                            // 无参，属性赋值
```

`Symbol` 枚举（`Acme.EFCore.Small.Enums`）支持的运算符如下：

| 枚举值 | 含义 | 生成的比较 |
| :--- | :--- | :--- |
| `Equal` = 0 | 等于 | `x.Field == value` |
| `NotEqual` = 1 | 不等于 | `x.Field != value` |
| `GreaterThan` = 2 | 大于 | `x.Field > value` |
| `LessThan` = 3 | 小于 | `x.Field < value` |
| `GreaterThanOrEqual` = 4 | 大于等于 | `x.Field >= value` |
| `LessThanOrEqual` = 5 | 小于等于 | `x.Field <= value` |
| `Contains` = 6 | 包含（模糊） | `x.Field.Contains(value)` |
| `NotContains` = 7 | 不包含 | `!x.Field.Contains(value)` |
| `StartsWith` = 8 | 以…开头 | `x.Field.StartsWith(value)` |
| `EndsWith` = 9 | 以…结尾 | `x.Field.EndsWith(value)` |
| `In` = 10 | 包含于列表 | `list.Contains(x.Field)` |
| `NotIn` = 11 | 不包含于列表 | `!list.Contains(x.Field)` |
| `IsNull` | 为空 | `x.Field == null` |
| `IsNotNull` | 不为空 | `x.Field != null` |

> 注：`In` / `NotIn` 的 `Value` 应为逗号分隔的值列表字符串（如 `"1,2,3"`）；`IsNull` / `IsNotNull` 不需要 `Value`。所有 `Condition` 间以 **AND** 连接。

`AddConditions` 与 `AddConditionsIf`：

```csharp
// 直接应用条件集合
var conditions = new List<Condition>
{
    new Condition("IsActive", "true", Symbol.Equal),
    new Condition("Name", "张", Symbol.Contains),
    new Condition("Age", "18", Symbol.GreaterThanOrEqual),
    new Condition("Email", "a.com,b.com", Symbol.In)
};
var query = _userRepository.Queryable().AddConditions(conditions);

// 带判断：仅当 isAdd 为 true 时才应用（常用于"有筛选条件才过滤"的场景）
var enabled = true;
query = query.AddConditionsIf(enabled, conditions);
```

#### 4.1.3 关键字模糊搜索：Keywords

`Keywords`（`Acme.EFCore.Small.Querys`）对**多个字段**做"任一字段包含关键字"的 `OR` 模糊匹配。构造函数：

```csharp
public Keywords(string[] fields, string value)  // 字段数组、关键字值
public Keywords()                               // 无参，属性赋值
```

`AddConditionsContains` 提供两个重载（其一带 `bool isAdd` 判断）：

```csharp
// 多字段模糊搜索（Name 或 Email 包含 "test"）
var keywords = new Keywords(new[] { "Name", "Email" }, "test");
query = query.AddConditionsContains(keywords);

// 带判断的重载
bool hasKeyword = !string.IsNullOrEmpty(searchText);
query = query.AddConditionsContains(hasKeyword, new Keywords(new[] { "Name", "Email" }, searchText));
```

#### 4.1.4 动态排序：Sorting 与 AddSorting

`Sorting`（`Acme.EFCore.Small.Querys`）构造函数：

```csharp
public Sorting(string sortField, SortingType sortingType)  // 排序字段、排序类型
public Sorting()                                           // 无参，属性赋值（SortField / SortingType）
```

`AddSorting`：根据 `Sorting` 生成排序；当 `sorting` 为 `null` 时按实体 `Id` 升序；`SortingType` 枚举含 `ASC` / `DESC`。`AddSortingIf` 提供条件判断版本。

```csharp
// 动态排序（ascending 由参数决定）
var sorting = new Sorting("Name", isAscending ? SortingType.ASC : SortingType.DESC);
query = query.AddSorting(sorting);

// 带判断：仅当 needSort 为 true 时排序
query = query.AddSortingIf(needSort, sorting);
```

#### 4.1.5 获取字段唯一值：GetKey / GetKeyList

`GetKey` / `GetKeyList` 用于从集合中按指定字段提取去重后的唯一值列表（基于 `GroupBy`）。

```csharp
// IQueryable 场景
IEnumerable<string> cities = _userRepository.Queryable().GetKey(u => u.City);

// IEnumerable 场景
IEnumerable<int> ages = userList.GetKeyList(u => u.Age);
```

### 4.2 无跟踪查询

无跟踪查询适用于只读操作，可提高查询性能。

```csharp
// 无跟踪列表（按条件）
public List<User> GetUsersReadOnly(string name)
{
    return _userRepository.GetListNoTracking(u => u.Name.Contains(name));
}

// 无跟踪单条（按条件，返回实体或 null）
public User GetUserByIdReadOnly(int id)
{
    return _userRepository.GetInfoNoTracking(u => u.Id == id);
}

// 异步无跟踪列表
public async Task<List<User>> GetUsersReadOnlyAsync(string name)
{
    return await _userRepository.GetListNoTrackingAsync(u => u.Name.Contains(name));
}

// 异步无跟踪单条
public async Task<User> GetUserByIdNoTrackingAsync(int id)
{
    return await _userRepository.GetInfoNoTrackingAsync(u => u.Id == id);
}
```

### 4.3 批量操作

#### 4.3.1 批量删除

`DelManyNowSave` / `DelManyNowSaveAsync` 返回 **`bool`**。

```csharp
public bool DeleteInactiveUsers()
{
    var inactiveUsers = _userRepository.GetList(u => !u.IsActive);
    if (inactiveUsers.Count == 0)
        return true;
    return _userRepository.DelManyNowSave(inactiveUsers);
}

public async Task<bool> DeleteInactiveUsersAsync()
{
    var inactiveUsers = await _userRepository.GetListAsync(u => !u.IsActive);
    if (inactiveUsers.Count == 0)
        return true;
    return await _userRepository.DelManyNowSaveAsync(inactiveUsers);
}
```

#### 4.3.2 批量更新

`UpdateManyNowSave` / `UpdateManyNowSaveAsync` 返回 **`bool`**。

```csharp
public bool UpdateUserStatus(bool isActive, List<int> userIds)
{
    var users = _userRepository.GetList(u => userIds.Contains(u.Id));
    foreach (var user in users)
        user.IsActive = isActive;
    return _userRepository.UpdateManyNowSave(users);
}

public async Task<bool> UpdateUserStatusAsync(bool isActive, List<int> userIds)
{
    var users = await _userRepository.GetListAsync(u => userIds.Contains(u.Id));
    foreach (var user in users)
        user.IsActive = isActive;
    return await _userRepository.UpdateManyNowSaveAsync(users);
}
```

---

## 5、完整使用案例

### 5.1 单库模式完整示例

#### 5.1.1 实体定义

```csharp
public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsActive { get; set; }
}

public class Product : BaseEntity<int>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
}
```

#### 5.1.2 数据库上下文

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}
```

#### 5.1.3 服务层

```csharp
public class UserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IRepository<User> userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    // 创建用户（AddNowSaveAsync 返回 int）
    public async Task<int> CreateUserAsync(User user)
    {
        if (await _userRepository.AnyAsync(u => u.Email == user.Email))
            throw new Exception("邮箱已被注册");

        return await _userRepository.AddNowSaveAsync(user);
    }

    // 获取分页列表
    public async Task<PageList<User>> GetUsersAsync(int pageIndex, int pageSize, string name = null)
    {
        var query = _userRepository.Queryable();
        if (!string.IsNullOrEmpty(name))
            query = query.Where(u => u.Name.Contains(name));

        return await query.ToPageListAsync(pageIndex, pageSize);
    }

    // 更新用户信息（UpdateSaveAsync 返回 bool）
    public async Task<bool> UpdateUserAsync(User user)
    {
        var existing = await _userRepository.GetInfoDefaultAsync(u => u.Id == user.Id);
        if (existing == null)
            return false;

        if (await _userRepository.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
            throw new Exception("邮箱已被其他用户使用");

        return await _userRepository.UpdateSaveAsync(user);
    }

    // 删除用户（DeleteNowSaveAsync 返回 bool）
    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetInfoDefaultAsync(u => u.Id == userId);
        if (user == null)
            return false;

        return await _userRepository.DeleteNowSaveAsync(user);
    }
}
```

#### 5.1.4 控制器

```csharp
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        try
        {
            var affected = await _userService.CreateUserAsync(user);
            if (affected > 0)
                return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
            return BadRequest("创建用户失败");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _userService.GetUserByConditionAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string name = null)
    {
        var users = await _userService.GetUsersAsync(pageIndex, pageSize, name);
        return Ok(users);
    }

    [HttpPut]
    public async Task<IActionResult> Update(User user)
    {
        try
        {
            var result = await _userService.UpdateUserAsync(user);
            if (!result)
                return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
            return NotFound();
        return Ok();
    }
}
```

### 5.2 多库模式完整示例

> 注意：跨两个不同 `DbContext`（用户库、订单库）的事务**无法**通过单一 `IUnitOfWork` 实现，因为它们使用不同的数据库连接。下列示例在各自上下文中使用对应的 `IUnitOfWork` 提交。

#### 5.2.1 数据库上下文

```csharp
// 主数据库上下文
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}

// 订单数据库上下文
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {}

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
```

#### 5.2.2 服务层

```csharp
public class OrderService
{
    private readonly IRepository<AppDbContext, User> _userRepository;
    private readonly IRepository<OrderDbContext, Order> _orderRepository;
    private readonly IRepository<OrderDbContext, OrderItem> _orderItemRepository;
    private readonly IUnitOfWork<OrderDbContext> _orderUnitOfWork;

    public OrderService(
        IRepository<AppDbContext, User> userRepository,
        IRepository<OrderDbContext, Order> orderRepository,
        IRepository<OrderDbContext, OrderItem> orderItemRepository,
        IUnitOfWork<OrderDbContext> orderUnitOfWork)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _orderUnitOfWork = orderUnitOfWork;
    }

    // 创建订单（在同一 OrderDbContext 内使用事务）
    public async Task<Order> CreateOrderAsync(int userId, List<OrderItemDto> items)
    {
        var user = await _userRepository.GetInfoDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new Exception("用户不存在");

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Status = "待处理",
            TotalAmount = items.Sum(i => i.Quantity * i.UnitPrice)
        };

        await _orderUnitOfWork.BeginTransactionAsync();
        try
        {
            _orderRepository.Add(order);
            await _orderUnitOfWork.SubmitAsync();   // 先保存订单以拿到 Id

            foreach (var dto in items)
            {
                var item = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = dto.ProductId,
                    ProductName = dto.ProductName,
                    UnitPrice = dto.UnitPrice,
                    Quantity = dto.Quantity,
                    Subtotal = dto.Quantity * dto.UnitPrice
                };
                _orderItemRepository.Add(item);
            }

            await _orderUnitOfWork.SubmitAsync();
            await _orderUnitOfWork.CommitTransactionAsync();
            return order;
        }
        catch (Exception ex)
        {
            await _orderUnitOfWork.RollbackTransactionAsync();
            throw new Exception("创建订单失败：" + ex.Message);
        }
    }

    // 获取用户的订单列表
    public async Task<List<Order>> GetUserOrdersAsync(int userId)
    {
        if (!await _userRepository.AnyAsync(u => u.Id == userId))
            throw new Exception("用户不存在");

        return await _orderRepository.GetListAsync(o => o.UserId == userId);
    }

    // 获取订单详情
    public async Task<Order> GetOrderDetailsAsync(int orderId)
    {
        var order = await _orderRepository.GetInfoDefaultAsync(o => o.Id == orderId);
        if (order == null)
            throw new Exception("订单不存在");

        order.Items = await _orderItemRepository.GetListAsync(item => item.OrderId == orderId);
        return order;
    }
}
```

---

## 6、支持的 .NET 版本

- netcoreapp3.1
- net5
- net6
- net7
- net8
- net9
- net10.0

---

## 7、NuGet 包信息

| 项目 | 内容 |
| :--- | :--- |
| **包 ID** | Acme.EFCore.Small |
| **作者** | yzxs |
| **描述** | 轻量级 EFCore 操作类库 |
| **文档 URL** | <https://www.nuget.org/packages/Acme.EFCore.Small/2.0.0.6#readme-body-tab> |
| **Gitee** | <https://gitee.com/yzxs949/acme.-efcore.-small/tree/%E6%A0%87%E5%87%86%E7%89%88/> |
| **GitHub** | <https://github.com/yzxs949/Acme.EFCore.Small/tree/%E6%A0%87%E5%87%86%E7%89%88> |
| **版权** | yzxs |

---

## 8、测试

项目包含完整的测试套件，覆盖所有核心功能：

| 测试类 | 覆盖范围 |
| :--- | :--- |
| **EntityTests** | BaseEntity 和 BaseAggregateRoot 类 |
| **ValueObjectTests** | BaseValueObject 类 |
| **PageListTests** | PageList 类 |
| **PageListExtensionTests** | PageListExtension / LinqExtension 的分页方法 |
| **QueryTests** | Condition、Keywords 和 Sorting 类 |
| **LinqExtensionTests** | LinqExtension 类的关键方法 |
| **RepositoryTests** | IRepository 接口的结构 |
| **UnitOfWorkTests** | UnitOfWork 类和 IUnitOfWork 接口 |

运行测试：

```bash
dotnet test Acme.EFCore.Small.Tests\Acme.EFCore.Small.Tests.csproj
```

---

## 9、许可证

本项目基于 [MIT 许可证](https://opensource.org/licenses/MIT) 开源。

---

## 10、贡献

欢迎贡献代码！您可以提交 Issue 或 Pull Request 来参与项目改进。

---

## 11、联系

如有任何问题或建议，请联系作者：

- **邮箱**：<yzxs949@163.com>
- **NuGet**：<https://www.nuget.org/packages/Acme.EFCore.Small/>
- **Gitee**：<https://gitee.com/yzxs949/acme.-efcore.-small/tree/%E6%A0%87%E5%87%86%E7%89%88/>
- **GitHub**：<https://github.com/yzxs949/Acme.EFCore.Small/tree/%E6%A0%87%E5%87%86%E7%89%88>
- **公众号**：.NET修仙日记

---

![作者公众号](https://raw.githubusercontent.com/yzxs949/FilePath/refs/heads/main/Image/%E4%BA%8C%E7%BB%B4%E7%A0%81.jpg)
