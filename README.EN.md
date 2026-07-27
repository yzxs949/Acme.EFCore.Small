# Acme.EFCore.Small

## 1. Project Overview
Acme.EFCore.Small is a lightweight Entity Framework Core general-purpose library designed to interact with databases using Entity Framework Core (EFCore). It serves as the fundamental component for handling various database operations.
- Version: v2.0.0.2
- Release Notes:
  - Fix known bugs...

## 2. Getting Started
### 1. Install Acme.EFCore.Small
Create Project -> Click on References -> Right click -> Manage NuGet Packages -> Search Acme.EFCore.Small and select version 2.0.0.2 or above. Install the appropriate version for your .NET framework.

### 2. Install the corresponding database package
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

### 3. Create database context class
```csharp
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initialize database context
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options) :
       base(options)
    {
    }
    
    // Add your DbSet properties here
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}

// Second database context for multi-database scenario
public class OtherDbContext : DbContext
{
    /// <summary>
    /// Initialize database context
    /// </summary>
    /// <param name="options"></param>
    public OtherDbContext(DbContextOptions<OtherDbContext> options) :
       base(options)
    {
    }
    
    // Add your DbSet properties here
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
```

### 4. Configure connection string
```json
{
    "ConnectionStrings":{
        "DefaultConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=Database Name;User ID=user;Password=password;Connect Timeout=120;Encrypt=False;",
        "OtherConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=Other Database Name;User ID=user;Password=password;Connect Timeout=120;Encrypt=False;"
    }
}
```

### 5. Dependency Injection
#### 5.1. Single Database Mode Configuration
```csharp
// Configure in Startup.cs or Program.cs
// Register database context
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// Register single database mode repository (includes UnitOfWork)
services.AddRepositorys<AppDbContext>();
```

#### 5.2. Multi-database Mode Configuration
```csharp
// Configure in Startup.cs or Program.cs
// Register first database context
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// Register second database context
services.AddDbContext<OtherDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("OtherConnection")));

// Register multi-database mode repository (includes UnitOfWork)
services.AddRepositorys();
```

## 3. Core Features

### 3.1. Base Classes

#### Entity Base Class
`BaseEntity<TKey>` provides basic entity properties, suitable for most entity types. The `TKey` represents the primary key type (must be a value type).

```csharp
// Inherit from BaseEntity<TKey> to get basic entity properties
public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
}
```

#### Aggregate Root Base Class
`BaseAggregateRoot<TKey>` is suitable for entities that serve as aggregate roots, usually containing child entity collections.

```csharp
// Inherit from BaseAggregateRoot<TKey> for aggregate root entities
public class Order : BaseAggregateRoot<int>
{
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}

// Child entity
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

#### Value Object Base Class
`BaseValueObject` is suitable for value objects, usually used to represent concepts without unique identifiers.

```csharp
// Inherit from BaseValueObject for value objects
public class Address : BaseValueObject
{
    public string Street { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
}

// Using value object in entity
public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public Address HomeAddress { get; set; }
    public Address WorkAddress { get; set; }
}
```

### 3.2. Repository Pattern

#### Single Database Mode
Single database mode is suitable for scenarios where only one database is used in the project, using the `IRepository<TEntity>` interface.

```csharp
// Inject single database repository
private readonly IRepository<User> _userRepository;
private readonly IRepository<Product> _productRepository;

public UserService(IRepository<User> userRepository, IRepository<Product> productRepository)
{
    _userRepository = userRepository;
    _productRepository = productRepository;
}

// 1. Add new user
public User AddUser(User user)
{
    return _userRepository.AddNowSave(user);
}

// 2. Get user by condition
public User GetUserById(int id)
{
    return _userRepository.GetInfo(u => u.Id == id);
}

// 3. Update user
public bool UpdateUser(User user)
{
    return _userRepository.UpdateNowSave(user);
}

// 4. Delete user
public bool DeleteUser(User user)
{
    return _userRepository.DeleteNowSave(user);
}

// 5. Get user list with condition
public List<User> GetUsers(string name)
{
    return _userRepository.GetList(u => u.Name.Contains(name));
}

// 6. Batch add users
public bool AddUsers(List<User> users)
{
    return _userRepository.AddManyNowSave(users);
}

// 7. Check if user exists
public bool UserExists(string email)
{
    return _userRepository.Any(u => u.Email == email);
}

// 8. Get user count
public int GetUserCount(bool isActive)
{
    return _userRepository.Count(u => u.IsActive == isActive);
}
```

#### Multi-database Mode
Multi-database mode is suitable for scenarios where multiple databases are used in the project, using the `IRepository<TDbContext, TEntity>` interface, which requires specifying the specific database context type.

```csharp
// Inject multi-database repository
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

// Get data from different databases
public async Task<(User, Order)> GetUserAndOrder(int userId, int orderId)
{
    var user = await _userRepository.GetInfoAsync(u => u.Id == userId);
    var order = await _orderRepository.GetInfoAsync(o => o.Id == orderId);
    return (user, order);
}

// Operate between multiple databases
public async Task<bool> CreateOrderWithUser(int userId, Order order)
{
    try
    {
        // Verify if user exists
        var userExists = await _userRepository.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return false;
        }
        
        // Create order
        await _orderRepository.AddNowSaveAsync(order);
        
        // Create order items
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

#### Async Operations
```csharp
// Async add
public async Task<User> AddUserAsync(User user)
{
    return await _userRepository.AddNowSaveAsync(user);
}

// Async get
public async Task<User> GetUserByIdAsync(int id)
{
    return await _userRepository.GetInfoAsync(u => u.Id == id);
}

// Async batch add
public async Task<bool> AddUsersAsync(List<User> users)
{
    return await _userRepository.AddManyNowSaveAsync(users);
}

// Async get list
public async Task<List<User>> GetUsersAsync(string name)
{
    return await _userRepository.GetListAsync(u => u.Name.Contains(name));
}

// Async check if exists
public async Task<bool> UserExistsAsync(string email)
{
    return await _userRepository.AnyAsync(u => u.Email == email);
}
```

### 3.3. UnitOfWork Pattern

The UnitOfWork pattern is used to manage transactions and submit operations, treating data changes as a single atomic unit.

#### Injecting UnitOfWork

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

#### Basic Transaction Example

```csharp
// Using UnitOfWork transaction
public void ProcessOrder(Order order)
{
    try
    {
        // Begin transaction
        _unitOfWork.BeginTransaction();
        
        // Perform operations
        _orderRepository.Add(order);
        
        // Submit changes
        _unitOfWork.Submit();
        
        // Commit transaction
        _unitOfWork.CommitTransaction();
    }
    catch (Exception)
    {
        // Rollback transaction on error
        _unitOfWork.RollbackTransaction();
        throw;
    }
}
```

#### Async Transaction Example

```csharp
// Async transaction
public async Task<bool> ProcessOrderAsync(Order order)
{
    try
    {
        // Begin transaction
        await _unitOfWork.BeginTransactionAsync();
        
        // Perform operations
        await _orderRepository.AddAsync(order);
        
        // Submit changes
        await _unitOfWork.SubmitAsync();
        
        // Commit transaction
        await _unitOfWork.CommitTransactionAsync();
        return true;
    }
    catch (Exception)
    {
        // Rollback transaction on error
        await _unitOfWork.RollbackTransactionAsync();
        return false;
    }
}
```

### 3.4. Pagination
Pagination is used to handle large amounts of data, improving query performance and user experience.

#### Basic Pagination
```csharp
// Using pagination
public PageList<User> GetUsersPaged(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return query.ToPageList(pageIndex, pageSize);
}
```

#### Pagination with Sorting
Sorting uses the `Sorting` object with `AddSorting` extension method.

```csharp
// Pagination with sorting
public PageList<User> GetUsersPagedWithSorting(int pageIndex, int pageSize, string name, string sortField, bool isAscending)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    
    // Build sorting object
    var sorting = new Sorting
    {
        SortField = sortField,
        SortingType = isAscending ? SortingType.ASC : SortingType.DESC
    };
    query = query.AddSorting(sorting);
    
    return query.ToPageList(pageIndex, pageSize);
}
```

#### Using Pagination Results
`PageList<T>` is the pagination result containing total count and current page data.

```csharp
// Call pagination method
var pageResult = userService.GetUsersPaged(1, 10, "Zhang");

// Pagination result contains the following information
int total = pageResult.Total;          // Total record count
List<User> users = pageResult.Items;   // Current page data
```

## 4. Advanced Features

### 4.1. Query Extensions

#### Dynamic Sorting
The `Sorting` object contains the sort field and sort type, using the `SortingType` enum (ASC/DESC).

```csharp
// Using AddSorting extension method for dynamic sorting
public List<User> GetUsersWithDynamicSorting(string name, string sortField, bool isAscending)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    
    // Build sorting object
    var sorting = new Sorting
    {
        SortField = sortField,
        SortingType = isAscending ? SortingType.ASC : SortingType.DESC
    };
    query = query.AddSorting(sorting);
    
    return query.ToList();
}
```

#### Complex Condition Query
The `WhereIf` extension method allows dynamically adding query filters based on conditions.

```csharp
// Complex condition query using WhereIf
public List<User> GetUsersWithComplexConditions(string name, int? age, bool? isActive)
{
    var query = _userRepository.Queryable();
    
    // Dynamically build query conditions using WhereIf
    query = query.WhereIf(!string.IsNullOrEmpty(name), u => u.Name.Contains(name));
    query = query.WhereIf(age.HasValue, u => u.Age == age.Value);
    query = query.WhereIf(isActive.HasValue, u => u.IsActive == isActive.Value);
    
    return query.ToList();
}
```

### 4.2. No-Tracking Queries
No-tracking queries are suitable for read-only operations and can improve query performance.

```csharp
// Using no-tracking queries for read-only operations
public List<User> GetUsersReadOnly()
{
    return _userRepository.GetListNoTracking();
}

// No-tracking query with condition
public User GetUserByIdReadOnly(int id)
{
    return _userRepository.GetInfoNoTracking(u => u.Id == id);
}

// Async no-tracking list query
public async Task<List<User>> GetUsersReadOnlyAsync(string name)
{
    return await _userRepository.GetListNoTrackingAsync(u => u.Name.Contains(name));
}

// Async no-tracking single item query
public async Task<User> GetUserByIdNoTrackingAsync(int id)
{
    return await _userRepository.GetInfoNoTrackingAsync(u => u.Id == id);
}
```

### 4.3. Batch Operations

#### Batch Delete
```csharp
// Batch delete
public bool DeleteInactiveUsers()
{
    var inactiveUsers = _userRepository.GetList(u => !u.IsActive);
    if (inactiveUsers.Count == 0)
    {
        return true;
    }
    return _userRepository.DelManyNowSave(inactiveUsers);
}

// Async batch delete
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

#### Batch Update
```csharp
// Batch update
public bool UpdateUserStatus(bool isActive, List<int> userIds)
{
    var users = _userRepository.GetList(u => userIds.Contains(u.Id));
    foreach (var user in users)
    {
        user.IsActive = isActive;
    }
    return _userRepository.UpdateManyNowSave(users);
}

// Async batch update
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

## 5. Complete Usage Examples

### 5.1. Single Database Mode Complete Example

#### 1. Entity Definition
```csharp
// User entity
public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsActive { get; set; }
}

// Product entity
public class Product : BaseEntity<int>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
}
```

#### 2. Database Context
```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}
```

#### 3. Service Layer
```csharp
public class UserService
{
    private readonly IRepository<User> _userRepository;
    
    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    
    // Create user
    public async Task<User> CreateUserAsync(User user)
    {
        // Check if email already exists
        if (await _userRepository.AnyAsync(u => u.Email == user.Email))
        {
            throw new Exception("Email already registered");
        }
        
        return await _userRepository.AddNowSaveAsync(user);
    }
    
    // Get user list
    public async Task<PageList<User>> GetUsersAsync(int pageIndex, int pageSize, string name = null)
    {
        var query = _userRepository.Queryable();
        
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(u => u.Name.Contains(name));
        }
        
        return query.ToPageList(pageIndex, pageSize);
    }
    
    // Update user information
    public async Task<bool> UpdateUserAsync(User user)
    {
        // Check if user exists
        var existingUser = await _userRepository.GetInfoAsync(u => u.Id == user.Id);
        if (existingUser == null)
        {
            return false;
        }
        
        // Check if email is used by other users
        if (await _userRepository.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
        {
            throw new Exception("Email already used by other users");
        }
        
        return await _userRepository.UpdateSaveAsync(user);
    }
    
    // Delete user
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

#### 4. Controller
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

### 5.2. Multi-database Mode Complete Example

#### 1. Database Context
```csharp
// Main database context
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}

// Order database context
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {}
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
```

#### 2. Service Layer
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
    
    // Create order
    public async Task<Order> CreateOrderAsync(int userId, List<OrderItemDto> items)
    {
        // Verify if user exists
        var user = await _userRepository.GetInfoAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new Exception("User does not exist");
        }
        
        // Create order
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Status = "Pending",
            TotalAmount = items.Sum(item => item.Quantity * item.UnitPrice)
        };
        
        // Begin transaction
        await _orderRepository.BeginTransactionAsync();
        
        try
        {
            // Save order
            await _orderRepository.AddAsync(order);
            await _orderRepository.SubmitAsync();
            
            // Save order items
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
            throw new Exception("Failed to create order: " + ex.Message);
        }
        finally
        {
            await _orderRepository.DisposeTransactionAsync();
        }
    }
    
    // Get user's order list
    public async Task<List<Order>> GetUserOrdersAsync(int userId)
    {
        // Verify if user exists
        var userExists = await _userRepository.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new Exception("User does not exist");
        }
        
        // Get user orders
        return await _orderRepository.GetListAsync(o => o.UserId == userId);
    }
    
    // Get order details
    public async Task<Order> GetOrderDetailsAsync(int orderId)
    {
        var order = await _orderRepository.GetInfoAsync(o => o.Id == orderId);
        if (order == null)
        {
            throw new Exception("Order does not exist");
        }
        
        // Get order items
        order.Items = await _orderItemRepository.GetListAsync(item => item.OrderId == orderId);
        
        return order;
    }
}
```

## 6. Supported .NET Versions
- netcoreapp3.1
- net5
- net6
- net7
- net8
- net9
- net10.0

## 7. NuGet Package Information
- Package ID: Acme.EFCore.Small
- Authors: yzxs
- Description: Lightweight EFCore operation library
- Project URL: https://www.nuget.org/packages/Acme.EFCore.Small/2.0.0.2#readme-body-tab
- gitee URL:<https://gitee.com/yzxs949/acme.-efcore.-small/tree/%E6%A0%87%E5%87%86%E7%89%88/>
- github URL:<https://github.com/yzxs949/Acme.EFCore.Small/tree/%E6%A0%87%E5%87%86%E7%89%88>
- Copyright: yzxs

## 8. License
MIT License

## 9. Contributing
Contributions are welcome! Please feel free to submit a Pull Request.

## 10. Contact
For any questions or suggestions, please contact the author.
- Email: yzxs949@163.com
- NuGet: https://www.nuget.org/packages/Acme.EFCore.Small/
- gitee URL:<https://gitee.com/yzxs949/acme.-efcore.-small/tree/%E6%A0%87%E5%87%86%E7%89%88/>
- github URL:<https://github.com/yzxs949/Acme.EFCore.Small/tree/%E6%A0%87%E5%87%86%E7%89%88>
![微信公众号](https://github.com/yzxs949/FilePath/blob/main/Image/%E4%BA%8C%E7%BB%B4%E7%A0%81.jpg?raw=true)