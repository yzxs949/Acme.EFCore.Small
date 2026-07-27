# Acme.EFCore.Small

## 1、项目概述
Acme.EFCore.Small 是一个轻量级的 Entity Framework Core 通用库，用于使用 Entity Framework Core (EFCore) 与数据库进行交互。它是处理各种数据库操作的基础组件。
- 版本：v1.3.7.1 
- 发布说明：
  - 1.更新.NET10依赖包版本，Microsoft.EntityFrameworkCore 版本从 Version 10.0.7 升级到 Version 10.0.8
  - 2.修复已知bug……

## 2、入门指南
### 1. 安装 Acme.EFCore.Small
创建项目 -> 点击引用 -> 右键 -> 管理 NuGet 包 -> 搜索 Acme.EFCore.Small 并选择 1.3.7.1 或更高版本。根据您的 .NET 框架安装适当的版本。

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

// 多库场景下的第二个数据库上下文
public class OtherDbContext : DbContext
{
    /// <summary>
    /// 初始化数据库上下文
    /// </summary>
    /// <param name="options"></param>
    public OtherDbContext(DbContextOptions<OtherDbContext> options) :
       base(options)
    {
    }
    
    // 在此添加您的 DbSet 属性
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
```

### 4. 配置连接字符串
```json
{
    "ConnectionStrings":{
        "DefaultConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=数据库名称;User ID=用户名;Password=密码;Connect Timeout=120;Encrypt=False;",
        "OtherConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=其他数据库名称;User ID=用户名;Password=密码;Connect Timeout=120;Encrypt=False;"
    }
}
```

### 5. 依赖注入
#### 5.1. 单库模式配置
```csharp
// 在 Startup.cs 或 Program.cs 中配置
// 注册数据库上下文
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// 注册单库模式仓储
services.AddRepositorys<AppDbContext>();
```

#### 5.2. 多库模式配置
```csharp
// 在 Startup.cs 或 Program.cs 中配置
// 注册第一个数据库上下文
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// 注册第二个数据库上下文
services.AddDbContext<OtherDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("OtherConnection")));

// 注册多库模式仓储
services.AddRepositorys();
```

## 3、核心功能

### 3.1. 基础类

#### 实体基础类
`BaseEntity` 提供了基本的实体属性，适用于大多数实体类型。

```csharp
// 继承 BaseEntity<TKey> 以获取基本实体属性，TKey 为主键类型（值类型）
public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
}
```

#### 聚合根基础类
`BaseAggregateRoot<TKey>` 适用于作为聚合根的实体，通常包含子实体集合。

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

#### 值对象基础类
`BaseValueObject` 适用于值对象，通常用于表示没有唯一标识的概念。

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
    public Address WorkAddress { get; set; }
}
```

### 3.2. 仓储模式

#### 单库模式
单库模式适用于项目中只使用一个数据库的场景，使用 `IRepository<TEntity>` 接口。

```csharp
// 注入单库仓储
private readonly IRepository<User> _userRepository;
private readonly IRepository<Product> _productRepository;

public UserService(IRepository<User> userRepository, IRepository<Product> productRepository)
{
    _userRepository = userRepository;
    _productRepository = productRepository;
}

// 1. 添加新用户
public User AddUser(User user)
{
    return _userRepository.AddNowSave(user);
}

// 2. 根据条件获取用户
public User GetUserById(int id)
{
    return _userRepository.GetInfo(u => u.Id == id);
}

// 3. 更新用户
public bool UpdateUser(User user)
{
    return _userRepository.UpdateNowSave(user);
}

// 4. 删除用户
public bool DeleteUser(User user)
{
    return _userRepository.DeleteNowSave(user);
}

// 5. 获取带条件的用户列表
public List<User> GetUsers(string name)
{
    return _userRepository.GetList(u => u.Name.Contains(name));
}

// 6. 批量添加用户
public bool AddUsers(List<User> users)
{
    return _userRepository.AddManyNowSave(users);
}

// 7. 检查用户是否存在
public bool UserExists(string email)
{
    return _userRepository.Any(u => u.Email == email);
}

// 8. 获取用户数量
public int GetUserCount(bool isActive)
{
    return _userRepository.Count(u => u.IsActive == isActive);
}
```

#### 多库模式
多库模式适用于项目中使用多个数据库的场景，使用 `IRepository<TDbContext, TEntity>` 接口，需要指定具体的数据库上下文类型。

```csharp
// 注入多库仓储
private readonly IRepository<AppDbContext, User> _userRepository;
private readonly IRepository<OtherDbContext, Order> _orderRepository;
private readonly IRepository<OtherDbContext, OrderItem> _orderItemRepository;

public OrderService(IRepository<AppDbContext, User> userRepository, 
                   IRepository<OtherDbContext, Order> orderRepository,
                   IRepository<OtherDbContext, OrderItem> orderItemRepository)
{
    _userRepository = userRepository;
    _orderRepository = orderRepository;
    _orderItemRepository = orderItemRepository;
}

// 从不同数据库获取数据
public async Task<(User, Order)> GetUserAndOrder(int userId, int orderId)
{
    var user = await _userRepository.GetInfoAsync(u => u.Id == userId);
    var order = await _orderRepository.GetInfoAsync(o => o.Id == orderId);
    return (user, order);
}

// 在多个数据库之间进行操作
public async Task<bool> CreateOrderWithUser(int userId, Order order)
{
    try
    {
        // 验证用户是否存在
        var userExists = await _userRepository.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return false;
        }
        
        // 创建订单
        await _orderRepository.AddNowSaveAsync(order);
        
        // 创建订单项
        foreach (var item in order.Items)
        {
            item.OrderId = order.Id;
            await _orderItemRepository.AddNowSaveAsync(item);
        }
        
        return true;
    }
    catch (Exception)
    {
        return false;
    }
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

// 异步批量添加
public async Task<bool> AddUsersAsync(List<User> users)
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

### 3.3. 事务管理
事务管理用于确保多个数据库操作的原子性，要么全部成功，要么全部失败。

#### 基本事务示例
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

#### 异步事务示例
```csharp
// 异步事务
public async Task<bool> ProcessOrderAsync(Order order)
{
    try
    {
        // 开始事务
        await _orderRepository.BeginTransactionAsync();
        
        // 执行操作
        await _orderRepository.AddAsync(order);
        
        foreach (var item in order.Items)
        {
            await _orderItemRepository.AddAsync(item);
        }
        
        // 提交事务
        await _orderRepository.CommitTransactionAsync();
        return true;
    }
    catch (Exception ex)
    {
        // 出错时回滚事务
        await _orderRepository.RollbackTransactionAsync();
        return false;
    }
    finally
    {
        // 释放事务
        await _orderRepository.DisposeTransactionAsync();
    }
}
```

### 3.4. 分页功能
分页功能用于处理大量数据，提高查询性能和用户体验。

#### 基本分页
```csharp
// 使用分页
public PageList<User> GetUsersPaged(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return query.ToPageList(pageIndex, pageSize);
}
```

#### 带排序的分页
排序使用 `Sorting` 对象结合 `AddSorting` 扩展方法。

```csharp
// 带排序的分页
public PageList<User> GetUsersPagedWithSorting(int pageIndex, int pageSize, string name, string sortField, bool isAscending)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    
    // 构建排序对象
    var sorting = new Sorting
    {
        SortField = sortField,
        SortingType = isAscending ? SortingType.ASC : SortingType.DESC
    };
    query = query.AddSorting(sorting);
    
    return query.ToPageList(pageIndex, pageSize);
}
```

#### 分页结果使用
`PageList<T>` 是分页返回结果，包含总条数和当前页数据。

```csharp
// 调用分页方法
var pageResult = userService.GetUsersPaged(1, 10, "张");

// 分页结果包含以下信息
int total = pageResult.Total;          // 总记录数
List<User> users = pageResult.Items;   // 当前页数据
```

## 4、高级功能

### 4.1. 查询扩展

#### 动态排序
`Sorting` 对象包含排序字段和排序类型，类型使用 `SortingType` 枚举（ASC/DESC）。

```csharp
// 使用 AddSorting 扩展方法进行动态排序
public List<User> GetUsersWithDynamicSorting(string name, string sortField, bool isAscending)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    
    // 构建排序对象
    var sorting = new Sorting
    {
        SortField = sortField,
        SortingType = isAscending ? SortingType.ASC : SortingType.DESC
    };
    query = query.AddSorting(sorting);
    
    return query.ToList();
}
```

#### 复杂条件查询
`WhereIf` 扩展方法允许根据条件动态添加查询过滤。

```csharp
// 复杂条件查询（使用 WhereIf 动态构建条件）
public List<User> GetUsersWithComplexConditions(string name, int? age, bool? isActive)
{
    var query = _userRepository.Queryable();
    
    // 使用 WhereIf 动态构建查询条件
    query = query.WhereIf(!string.IsNullOrEmpty(name), u => u.Name.Contains(name));
    query = query.WhereIf(age.HasValue, u => u.Age == age.Value);
    query = query.WhereIf(isActive.HasValue, u => u.IsActive == isActive.Value);
    
    return query.ToList();
}
```

### 4.2. 无跟踪查询
无跟踪查询适用于只读操作，可以提高查询性能。

```csharp
// 获取所有用户（无跟踪）
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

// 异步获取单条无跟踪数据
public async Task<User> GetUserByIdNoTrackingAsync(int id)
{
    return await _userRepository.GetInfoNoTrackingAsync(u => u.Id == id);
}
```

### 4.3. 查询条件与排序扩展

除了基础的 `WhereIf` 动态条件查询外，库还提供了 `AddConditions`、`AddSorting` 和 `AddConditionsContains` 等高级扩展方法。

#### 结构条件查询（AddConditions）
使用 `Condition` 对象构建查询条件，支持等于、不等于、大于、小于等多种运算符。

```csharp
using Acme.EFCore.Small.Enums;
using Acme.EFCore.Small.Querys;

// 使用 AddConditions 构建结构化的查询条件
public List<User> GetUsersByConditions(string name, int minAge)
{
    var query = _userRepository.Queryable();
    
    var conditions = new List<Condition>
    {
        new Condition("Name", name, Symbol.Equal),
        new Condition("Age", minAge.ToString(), Symbol.GreaterThanOrEqual)
    };
    
    return query.AddConditions(conditions).ToList();
}
```

#### 关键字模糊查询（AddConditionsContains）
支持指定多个字段进行关键字模糊匹配。

```csharp
using Acme.EFCore.Small.Querys;

// 使用 AddConditionsContains 进行多字段关键字模糊查询
public List<User> SearchUsers(string keyword)
{
    var query = _userRepository.Queryable();
    
    var keywords = new Keywords(
        fields: new[] { "Name", "Email", "Phone" },
        value: keyword
    );
    
    return query.AddConditionsContains(keywords).ToList();
}
```

### 4.4. 提交操作
`Submit()` 和 `SubmitAsync()` 用于手动提交更改到数据库。`AddNowSave` 等方法内部自动调用 Submit，而 `Add` 等方法需要手动调用 Submit。

```csharp
// 手动提交模式
public void AddUserManually(User user)
{
    _userRepository.Add(user);     // 仅添加到上下文
    _userRepository.Submit();      // 手动提交到数据库
}

// 自动提交模式
public User AddUserAuto(User user)
{
    return _userRepository.AddNowSave(user);  // 添加并立即提交
}
```

### 4.5. 获取指定条数数据

```csharp
// 获取前 N 条数据
public List<User> GetTopUsers(int count)
{
    return _userRepository.GetListTake(count);
}

// 带条件获取前 N 条数据
public List<User> GetTopActiveUsers(int count)
{
    return _userRepository.GetListTake(u => u.IsActive, count);
}
```

### 4.6. 获取默认值（无数据时不抛异常）

```csharp
// 获取单条数据，不存在时返回 null
public User GetUserOrDefault(int id)
{
    return _userRepository.GetInfoDefault(u => u.Id == id);
}
```

### 4.7. 异步分页

```csharp
// 异步分页查询
public async Task<PageList<User>> GetUsersPagedAsync(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return await query.ToPageListAsync(pageIndex, pageSize);
}
```

### 4.8. 批量操作

#### 批量删除
```csharp
// 批量删除
public bool DeleteInactiveUsers()
{
    var inactiveUsers = _userRepository.GetList(u => !u.IsActive);
    if (inactiveUsers.Count == 0)
    {
        return true;
    }
    return _userRepository.DelManyNowSave(inactiveUsers);
}

// 异步批量删除
public async Task<bool> DeleteInactiveUsersAsync()
{
    var inactiveUsers = await _userRepository.GetListAsync(u => !u.IsActive);
    if (inactiveUsers.Count == 0)
    {
        return true;
    }
    return await _userRepository.DelManyNowSaveAsync(inactiveUsers);
}
```

#### 批量更新
```csharp
// 批量更新
public bool UpdateUserStatus(bool isActive, List<int> userIds)
{
    var users = _userRepository.GetList(u => userIds.Contains(u.Id));
    foreach (var user in users)
    {
        user.IsActive = isActive;
    }
    return _userRepository.UpdateManyNowSave(users);
}

// 异步批量更新
public async Task<bool> UpdateUserStatusAsync(bool isActive, List<int> userIds)
{
    var users = await _userRepository.GetListAsync(u => userIds.Contains(u.Id));
    foreach (var user in users)
    {
        user.IsActive = isActive;
    }
    return await _userRepository.UpdateManyNowSaveAsync(users);
}
```

## 5、完整使用案例

### 5.1. 单库模式完整示例

#### 1. 实体定义
```csharp
// 用户实体
public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsActive { get; set; }
}

// 产品实体
public class Product : BaseEntity<int>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
}
```

#### 2. 数据库上下文
```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}
```

#### 3. 服务层
```csharp
public class UserService
{
    private readonly IRepository<User> _userRepository;
    
    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    
    // 创建用户
    public async Task<User> CreateUserAsync(User user)
    {
        // 检查邮箱是否已存在
        if (await _userRepository.AnyAsync(u => u.Email == user.Email))
        {
            throw new Exception("邮箱已被注册");
        }
        
        return await _userRepository.AddNowSaveAsync(user);
    }
    
    // 获取用户列表（支持按名称筛选）
    public async Task<PageList<User>> GetUsersAsync(int pageIndex, int pageSize, string name = null)
    {
        var query = _userRepository.Queryable();
        
        // 使用 WhereIf 进行条件筛选
        query = query.WhereIf(!string.IsNullOrEmpty(name), u => u.Name.Contains(name));
        
        return query.ToPageList(pageIndex, pageSize);
    }
    
    // 更新用户信息
    public async Task<bool> UpdateUserAsync(User user)
    {
        // 检查用户是否存在
        var existingUser = await _userRepository.GetInfoAsync(u => u.Id == user.Id);
        if (existingUser == null)
        {
            return false;
        }
        
        // 检查邮箱是否被其他用户使用
        if (await _userRepository.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
        {
            throw new Exception("邮箱已被其他用户使用");
        }
        
        return await _userRepository.UpdateSaveAsync(user);
    }
    
    // 删除用户
    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetInfoAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        
        return await _userRepository.DeleteNowSaveAsync(user);
    }
}
```

#### 4. 控制器
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
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
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
            {
                return NotFound();
            }
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
        {
            return NotFound();
        }
        return Ok();
    }
}
```

### 5.2. 多库模式完整示例

#### 1. 数据库上下文
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

#### 2. 服务层
```csharp
public class OrderService
{
    private readonly IRepository<AppDbContext, User> _userRepository;
    private readonly IRepository<OrderDbContext, Order> _orderRepository;
    private readonly IRepository<OrderDbContext, OrderItem> _orderItemRepository;
    
    public OrderService(IRepository<AppDbContext, User> userRepository,
                       IRepository<OrderDbContext, Order> orderRepository,
                       IRepository<OrderDbContext, OrderItem> orderItemRepository)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
    }
    
    // 创建订单
    public async Task<Order> CreateOrderAsync(int userId, List<OrderItemDto> items)
    {
        // 验证用户是否存在
        var user = await _userRepository.GetInfoAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new Exception("用户不存在");
        }
        
        // 创建订单
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Status = "待处理",
            TotalAmount = items.Sum(item => item.Quantity * item.UnitPrice)
        };
        
        // 开始事务
        await _orderRepository.BeginTransactionAsync();
        
        try
        {
            // 保存订单
            await _orderRepository.AddAsync(order);
            await _orderRepository.SubmitAsync();
            
            // 保存订单项
            foreach (var itemDto in items)
            {
                var item = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = itemDto.ProductId,
                    ProductName = itemDto.ProductName,
                    UnitPrice = itemDto.UnitPrice,
                    Quantity = itemDto.Quantity,
                    Subtotal = itemDto.Quantity * itemDto.UnitPrice
                };
                await _orderItemRepository.AddAsync(item);
            }
            
            await _orderItemRepository.SubmitAsync();
            await _orderRepository.CommitTransactionAsync();
            
            return order;
        }
        catch (Exception ex)
        {
            await _orderRepository.RollbackTransactionAsync();
            throw new Exception("创建订单失败: " + ex.Message);
        }
        finally
        {
            await _orderRepository.DisposeTransactionAsync();
        }
    }
    
    // 获取用户的订单列表
    public async Task<List<Order>> GetUserOrdersAsync(int userId)
    {
        // 验证用户是否存在
        var userExists = await _userRepository.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new Exception("用户不存在");
        }
        
        // 获取用户订单
        return await _orderRepository.GetListAsync(o => o.UserId == userId);
    }
    
    // 获取订单详情
    public async Task<Order> GetOrderDetailsAsync(int orderId)
    {
        var order = await _orderRepository.GetInfoAsync(o => o.Id == orderId);
        if (order == null)
        {
            throw new Exception("订单不存在");
        }
        
        // 获取订单项
        order.Items = await _orderItemRepository.GetListAsync(item => item.OrderId == orderId);
        
        return order;
    }
}
```

## 6、支持的 .NET 版本
- netcoreapp3.1
- net5.0
- net6.0
- net7.0
- net8.0
- net9.0
- net10.0

## 7、NuGet 包信息
- 包 ID: Acme.EFCore.Small
- 作者: yzxs
- 描述: 轻量级 EFCore 操作类库
- 项目源码: https://gitee.com/yzxs949/acme.-efcore.-small
- NuGet 页面: https://www.nuget.org/packages/Acme.EFCore.Small/

## 8、许可证
MIT 许可证

## 9、贡献
欢迎贡献！请随时提交 Pull Request。

## 10、联系
如有任何问题或建议，请联系作者
邮箱：yzxs949@163.com