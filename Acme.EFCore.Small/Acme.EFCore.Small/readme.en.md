# Acme.EFCore.Small

[🇨🇳 中文](./readme.md) | [🇬🇧 English](./readme.en.md)

![Version](https://img.shields.io/badge/version-v2.0.0.4-blue.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)
![EFCore](https://img.shields.io/badge/EFCore-2.0+-orange.svg)
![.NET](https://img.shields.io/badge/.NET-3.1%20%7C%205%20%7C%206%20%7C%207%20%7C%208%20%7C%209%20%7C%2010-9cf.svg)

`Version v2.0.0.4`  `License MIT`  `EFCore 2.0+`  `.NET 3.1/5/6/7/8/9/10`

A lightweight Entity Framework Core general-purpose library that provides ready-to-use data access capabilities such as the Repository pattern, Unit of Work, pagination, and dynamic sorting.

---

## 📑 Table of Contents

- [1. Project Overview](#1-project-overview)
- [2. Getting Started](#2-getting-started)
  - [2.1 Install Acme.EFCore.Small](#21-install-acmeefcoresmall)
  - [2.2 Install the corresponding database package](#22-install-the-corresponding-database-package)
  - [2.3 Create database context class](#23-create-database-context-class)
  - [2.4 Configure connection string](#24-configure-connection-string)
  - [2.5 Dependency Injection](#25-dependency-injection)
- [3. Core Features](#3-core-features)
  - [3.1 Base Classes](#31-base-classes)
  - [3.2 Repository Pattern](#32-repository-pattern)
  - [3.3 UnitOfWork Pattern](#33-unitofwork-pattern)
  - [3.4 Pagination](#34-pagination)
- [4. Advanced Features](#4-advanced-features)
  - [4.1 Query Extensions](#41-query-extensions)
  - [4.2 No-Tracking Queries](#42-no-tracking-queries)
  - [4.3 Batch Operations](#43-batch-operations)
- [5. Complete Usage Examples](#5-complete-usage-examples)
  - [5.1 Single Database Mode Complete Example](#51-single-database-mode-complete-example)
  - [5.2 Multi-database Mode Complete Example](#52-multi-database-mode-complete-example)
- [6. Supported .NET Versions](#6-supported-net-versions)
- [7. NuGet Package Information](#7-nuget-package-information)
- [8. Testing](#8-testing)
- [9. License](#9-license)
- [10. Contributing](#10-contributing)
- [11. Contact](#11-contact)

---

## 1. Project Overview

Acme.EFCore.Small is a lightweight Entity Framework Core general-purpose library designed to interact with databases using Entity Framework Core (EFCore). It serves as the fundamental component for handling various database operations.

| Item | Detail |
| :--- | :--- |
| **Version** | v2.0.0.4 |
| **Author** | yzxs |
| **Description** | Lightweight EFCore operation library |
| **License** | MIT |

**Release Notes (v2.0.0.4)**:

- Fix known bugs...

**Core Features**:

- ✅ Single / multi-database repository pattern
- ✅ Unit of Work (transaction management, provided by `IUnitOfWork`)
- ✅ Pagination, dynamic sorting, conditional query extensions
- ✅ No-tracking queries to boost read-only performance
- ✅ Full synchronous / asynchronous API coverage
- ✅ Batch insert, update and delete

> ⚠️ **Important**: Transaction methods (`BeginTransaction` / `CommitTransaction` / `RollbackTransaction` and their async versions) are provided **only on `IUnitOfWork`, not on the repository interface**. Repository methods like `Add` / `Update` / `Delete` only stage changes on the context; you must call `IUnitOfWork.Submit()` to persist them.

---

## 2. Getting Started

### 2.1 Install Acme.EFCore.Small

Create Project → Click on References → Right click → Manage NuGet Packages → Search `Acme.EFCore.Small` and select version 2.0.0.4 or above. Install the appropriate version for your .NET framework.

> Note: the NuGet package name is **`Acme.EFCore.Small`** (mind the spelling, it is not "AddRepositories").

### 2.2 Install the corresponding database package

Install the corresponding EF Core provider based on the database you use:

| Database | NuGet Package |
| :--- | :--- |
| **SqlServer** | `Microsoft.EntityFrameworkCore.SqlServer` |
| **Sqlite** | `Microsoft.EntityFrameworkCore.Sqlite` |
| **Cosmos** | `Microsoft.EntityFrameworkCore.Cosmos` |
| **InMemory** | `Microsoft.EntityFrameworkCore.InMemory` |
| **MySql** | `Pomelo.EntityFrameworkCore.MySql` / `MySql.EntityFrameworkCore` |
| **PostgreSQL** | `Npgsql.EntityFrameworkCore.PostgreSQL` |
| **Oracle** | `Oracle.EntityFrameworkCore` |
| **Firebird** | `FirebirdSql.EntityFrameworkCore.Firebird` |
| **Dm (Dameng)** | `Microsoft.EntityFrameworkCore.Dm` |

### 2.3 Create database context class

```csharp
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initialize database context
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Add your DbSet properties here
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}

// Second database context for multi-database scenario
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
```

### 2.4 Configure connection string

Configure in `appsettings.json`:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=Database Name;User ID=user;Password=password;Connect Timeout=120;Encrypt=False;",
        "OtherConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=Other Database Name;User ID=user;Password=password;Connect Timeout=120;Encrypt=False;"
    }
}
```

### 2.5 Dependency Injection

This library registers services via the extension method `AddRepositorys` (defined in the `Acme.EFCore.Small.Extensions.CollectionExtension` namespace):

- `AddRepositorys<TDbContext>()` — **single database mode**, registers non-generic `IUnitOfWork` / `IRepository<>` and a strongly-typed `DbContext`.
- `AddRepositorys()` — **multi-database mode**, registers generic `IUnitOfWork<>` / `IRepository<,>`.

#### 2.5.1 Single Database Mode Configuration

```csharp
// In Program.cs / Startup.cs (note: using Acme.EFCore.Small.Extensions;)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register single database mode repository (includes UnitOfWork)
builder.Services.AddRepositorys<AppDbContext>();
```

#### 2.5.2 Multi-database Mode Configuration

```csharp
// In Program.cs / Startup.cs (note: using Acme.EFCore.Small.Extensions;)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OtherConnection")));

// Register multi-database mode repository (includes UnitOfWork)
builder.Services.AddRepositorys();
```

---

## 3. Core Features

### 3.1 Base Classes

All entity base classes provide a single primary-key property `Id` (generic `TKey`, constrained to value types).

#### Entity Base Class

`BaseEntity<TKey>` (`Acme.EFCore.Small.Entitys`) provides the primary key `Id`, suitable for most entity types.

```csharp
// Inherit from BaseEntity<TKey> to get the primary key Id
public class User : BaseEntity<int>
{
    // Id is provided by the base class
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
}
```

#### Aggregate Root Base Class

`BaseAggregateRoot<TKey>` (`Acme.EFCore.Small.AggregateRoots`) inherits from `BaseEntity<TKey>`, suitable for aggregate root entities.

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

`BaseValueObject` (`Acme.EFCore.Small.ValueObjects`) is for value objects without a unique identity.

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
}
```

### 3.2 Repository Pattern

#### Single Database Mode

Single database mode uses the `IRepository<TEntity>` interface (injected together with the non-generic `IUnitOfWork`).

**Return values**:

- `AddNowSave` / `AddNowSaveAsync` return **`int`** (number of affected rows).
- `Add` / `AddAsync` **do not save immediately** — they only stage the entity; call `IUnitOfWork.Submit()` to persist.
- `DeleteNowSave` / `DeleteNowSaveAsync` / `UpdateNowSave` / `UpdateSaveAsync` / `DelManyNowSave*` / `UpdateManyNowSave*` return **`bool`** (success).

```csharp
// Inject single database repository
private readonly IRepository<User> _userRepository;
private readonly IRepository<Product> _productRepository;

public UserService(IRepository<User> userRepository, IRepository<Product> productRepository)
{
    _userRepository = userRepository;
    _productRepository = productRepository;
}

// 1. Add new user (returns int: affected rows)
public int AddUser(User user)
{
    return _userRepository.AddNowSave(user);
}

// 2. Get user by primary key (returns entity or null)
public User GetUserById(int id)
{
    return _userRepository.GetInfoById(id);
}

// 3. Get user by condition (returns first match or null, use GetInfoDefault)
public User GetUserByCondition(int id)
{
    return _userRepository.GetInfoDefault(u => u.Id == id);
}

// 4. Update user (returns bool)
public bool UpdateUser(User user)
{
    return _userRepository.UpdateNowSave(user);
}

// 5. Delete user (returns bool)
public bool DeleteUser(User user)
{
    return _userRepository.DeleteNowSave(user);
}

// 6. Get user list with condition
public List<User> GetUsers(string name)
{
    return _userRepository.GetList(u => u.Name.Contains(name));
}

// 7. Batch add users (returns int: affected rows)
public int AddUsers(List<User> users)
{
    return _userRepository.AddManyNowSave(users);
}

// 8. Check if user exists
public bool UserExists(string email)
{
    return _userRepository.Any(u => u.Email == email);
}

// 9. Get user count
public int GetUserCount(bool isActive)
{
    return _userRepository.Count(u => u.IsActive == isActive);
}
```

#### Multi-database Mode

Multi-database mode uses the `IRepository<TDbContext, TEntity>` interface, specifying the concrete database context type, together with the matching `IUnitOfWork<TDbContext>`.

```csharp
// Inject multi-database repository and the matching UnitOfWork
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

// Get data from different databases
public async Task<(User, Order)> GetUserAndOrder(int userId, int orderId)
{
    var user = await _userRepository.GetInfoDefaultAsync(u => u.Id == userId);
    var order = await _orderRepository.GetInfoDefaultAsync(o => o.Id == orderId);
    return (user, order);
}

// Operate inside a single database (OrderDbContext) and persist via its UnitOfWork
public async Task<bool> CreateOrder(Order order)
{
    _orderRepository.Add(order);                       // stage only
    foreach (var item in order.Items)
    {
        item.OrderId = order.Id;
        _orderItemRepository.Add(item);
    }
    return await _orderUnitOfWork.SubmitAsync() > 0;    // persist
}
```

#### Async Operations

> Note: for fetching a single entity by condition use `GetInfoDefaultAsync` (not `GetInfoAsync`); by primary key use `GetInfoByIdAsync`.

```csharp
// Async add (returns int: affected rows)
public async Task<int> AddUserAsync(User user)
{
    return await _userRepository.AddNowSaveAsync(user);
}

// Async get by condition (returns entity or null)
public async Task<User> GetUserByConditionAsync(int id)
{
    return await _userRepository.GetInfoDefaultAsync(u => u.Id == id);
}

// Async get by primary key (returns entity or null)
public async Task<User> GetUserByIdAsync(int id)
{
    return await _userRepository.GetInfoByIdAsync(id);
}

// Async batch add (returns int: affected rows)
public async Task<int> AddUsersAsync(List<User> users)
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

### 3.3 UnitOfWork Pattern

The UnitOfWork (`IUnitOfWork` for single-db / `IUnitOfWork<TDbContext>` for multi-db) manages transactions and commits. **Transaction methods exist only on `IUnitOfWork`, not on the repository.**

**Methods provided by `IUnitOfWork` / `IUnitOfWork<TDbContext>`**:

| Method | Description |
| :--- | :--- |
| `int Submit()` / `Task<int> SubmitAsync()` | Persist staged changes to the database, returns affected rows |
| `void BeginTransaction()` / `Task BeginTransactionAsync()` | Begin a transaction |
| `void CommitTransaction()` / `Task CommitTransactionAsync()` | Commit the transaction |
| `void RollbackTransaction()` / `Task RollbackTransactionAsync()` | Rollback the transaction |
| `DbContext DbContext { get; }` | The associated database context |

#### Injecting UnitOfWork

```csharp
// Single database mode (non-generic IUnitOfWork)
private readonly IUnitOfWork _unitOfWork;

// Multi-database mode (generic IUnitOfWork<TDbContext>)
private readonly IUnitOfWork<OrderDbContext> _orderUnitOfWork;
```

#### Basic Transaction Example

```csharp
private readonly IRepository<Order> _orderRepository;   // single-db example
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

// Using UnitOfWork transaction
public void ProcessOrder(Order order)
{
    try
    {
        // Begin transaction (on IUnitOfWork)
        _unitOfWork.BeginTransaction();

        // Repository methods only stage changes
        _orderRepository.Add(order);
        foreach (var item in order.Items)
        {
            _orderItemRepository.Add(item);
        }

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
// Async transaction (multi-db mode using IUnitOfWork<OrderDbContext>)
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

### 3.4 Pagination

Pagination extensions `ToPageList` / `ToPageListAsync` are defined in `LinqExtension` and operate on `IQueryable<T>`. `pageIndex` is **1-based** (`pageIndex - 1` is used as the Skip offset).

**`PageList<T>` result properties**:

- `int Total` — total record count
- `List<T> Items` — current page data

#### Basic Pagination

```csharp
public PageList<User> GetUsersPaged(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return query.ToPageList(pageIndex, pageSize);
}

// Async version
public async Task<PageList<User>> GetUsersPagedAsync(int pageIndex, int pageSize, string name)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));
    return await query.ToPageListAsync(pageIndex, pageSize);
}
```

#### Pagination with Sorting

```csharp
public PageList<User> GetUsersPagedWithSorting(int pageIndex, int pageSize, string name, string sortField, bool isAscending)
{
    var query = _userRepository.Queryable(u => u.Name.Contains(name));

    // Build sorting object (constructor new Sorting(field, type) also works)
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

```csharp
var pageResult = userService.GetUsersPaged(1, 10, "Zhang");

int total = pageResult.Total;          // Total record count
List<User> users = pageResult.Items;   // Current page data
```

---

## 4. Advanced Features

### 4.1 Query Extensions

Query extensions live in `Acme.EFCore.Small.Extensions.LinqExtension` and operate on `IQueryable<T>` or `IEnumerable<T>`. They provide two dynamic-query approaches: Lambda-based and object-based (`Condition` / `Keywords` / `Sorting`).

#### Lambda Condition Filtering: WhereIf

`WhereIf(bool, lambda)` appends a `Where` filter only when the condition is true (available for both `IQueryable` and `IEnumerable`).

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

#### Condition Object and Symbol

Instead of Lambdas, you can build query conditions dynamically with the **`Condition`** object (field name + value + operator). Conditions are combined with `AND` via expression trees. `Condition` provides two constructors:

```csharp
public Condition(string field, string value, Symbol symbol)  // field, value, operator
public Condition()                                            // parameterless, set properties
```

The `Symbol` enum (`Acme.EFCore.Small.Enums`) supports:

| Value | Meaning | Comparison generated |
| :--- | :--- | :--- |
| `Equal` = 0 | equals | `x.Field == value` |
| `NotEqual` = 1 | not equals | `x.Field != value` |
| `GreaterThan` = 2 | greater than | `x.Field > value` |
| `LessThan` = 3 | less than | `x.Field < value` |
| `GreaterThanOrEqual` = 4 | greater than or equal | `x.Field >= value` |
| `LessThanOrEqual` = 5 | less than or equal | `x.Field <= value` |
| `Contains` = 6 | contains (fuzzy) | `x.Field.Contains(value)` |
| `NotContains` = 7 | not contains | `!x.Field.Contains(value)` |
| `StartsWith` = 8 | starts with | `x.Field.StartsWith(value)` |
| `EndsWith` = 9 | ends with | `x.Field.EndsWith(value)` |
| `In` = 10 | in list | `list.Contains(x.Field)` |
| `NotIn` = 11 | not in list | `!list.Contains(x.Field)` |
| `IsNull` | is null | `x.Field == null` |
| `IsNotNull` | is not null | `x.Field != null` |

> Note: for `In` / `NotIn`, `Value` should be a comma-separated list string (e.g. `"1,2,3"`); `IsNull` / `IsNotNull` do not need a `Value`. All `Condition`s are joined with **AND**.

`AddConditions` and `AddConditionsIf`:

```csharp
// Apply a condition list directly
var conditions = new List<Condition>
{
    new Condition("IsActive", "true", Symbol.Equal),
    new Condition("Name", "Zhang", Symbol.Contains),
    new Condition("Age", "18", Symbol.GreaterThanOrEqual),
    new Condition("Email", "a.com,b.com", Symbol.In)
};
var query = _userRepository.Queryable().AddConditions(conditions);

// Conditional: apply only when isAdd is true (useful for "filter only if provided")
bool enabled = true;
query = query.AddConditionsIf(enabled, conditions);
```

#### Keyword Fuzzy Search: Keywords

`Keywords` (`Acme.EFCore.Small.Querys`) performs an `OR` fuzzy match of "any field contains the keyword" across **multiple fields**. Constructors:

```csharp
public Keywords(string[] fields, string value)  // field array, keyword value
public Keywords()                               // parameterless, set properties
```

`AddConditionsContains` has two overloads (one with a `bool isAdd` switch):

```csharp
// Multi-field fuzzy search (Name or Email contains "test")
var keywords = new Keywords(new[] { "Name", "Email" }, "test");
query = query.AddConditionsContains(keywords);

// Overload with condition switch
bool hasKeyword = !string.IsNullOrEmpty(searchText);
query = query.AddConditionsContains(hasKeyword, new Keywords(new[] { "Name", "Email" }, searchText));
```

#### Dynamic Sorting: Sorting and AddSorting

`Sorting` (`Acme.EFCore.Small.Querys`) constructors:

```csharp
public Sorting(string sortField, SortingType sortingType)  // sort field, sort type
public Sorting()                                           // parameterless, set SortField / SortingType
```

`AddSorting` generates ordering from a `Sorting`; when `sorting` is `null` it orders by the entity `Id` ascending; `SortingType` enum has `ASC` / `DESC`. `AddSortingIf` provides a conditional version.

```csharp
// Dynamic sorting (ascending decided by parameter)
var sorting = new Sorting("Name", isAscending ? SortingType.ASC : SortingType.DESC);
query = query.AddSorting(sorting);

// Conditional: sort only when needSort is true
query = query.AddSortingIf(needSort, sorting);
```

#### Get Distinct Field Values: GetKey / GetKeyList

`GetKey` / `GetKeyList` extract distinct field values from a collection (based on `GroupBy`).

```csharp
// IQueryable scenario
IEnumerable<string> cities = _userRepository.Queryable().GetKey(u => u.City);

// IEnumerable scenario
IEnumerable<int> ages = userList.GetKeyList(u => u.Age);
```

### 4.2 No-Tracking Queries

No-tracking queries are suitable for read-only operations and improve query performance.

```csharp
// No-tracking list (by condition)
public List<User> GetUsersReadOnly(string name)
{
    return _userRepository.GetListNoTracking(u => u.Name.Contains(name));
}

// No-tracking single (by condition, returns entity or null)
public User GetUserByIdReadOnly(int id)
{
    return _userRepository.GetInfoNoTracking(u => u.Id == id);
}

// Async no-tracking list
public async Task<List<User>> GetUsersReadOnlyAsync(string name)
{
    return await _userRepository.GetListNoTrackingAsync(u => u.Name.Contains(name));
}

// Async no-tracking single
public async Task<User> GetUserByIdNoTrackingAsync(int id)
{
    return await _userRepository.GetInfoNoTrackingAsync(u => u.Id == id);
}
```

### 4.3 Batch Operations

#### Batch Delete

`DelManyNowSave` / `DelManyNowSaveAsync` return **`bool`**.

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

#### Batch Update

`UpdateManyNowSave` / `UpdateManyNowSaveAsync` return **`bool`**.

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

## 5. Complete Usage Examples

### 5.1 Single Database Mode Complete Example

#### 1. Entity Definition

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
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IRepository<User> userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    // Create user (AddNowSaveAsync returns int)
    public async Task<int> CreateUserAsync(User user)
    {
        if (await _userRepository.AnyAsync(u => u.Email == user.Email))
            throw new Exception("Email already registered");

        return await _userRepository.AddNowSaveAsync(user);
    }

    // Get paged list
    public async Task<PageList<User>> GetUsersAsync(int pageIndex, int pageSize, string name = null)
    {
        var query = _userRepository.Queryable();
        if (!string.IsNullOrEmpty(name))
            query = query.Where(u => u.Name.Contains(name));

        return await query.ToPageListAsync(pageIndex, pageSize);
    }

    // Update user (UpdateSaveAsync returns bool)
    public async Task<bool> UpdateUserAsync(User user)
    {
        var existing = await _userRepository.GetInfoDefaultAsync(u => u.Id == user.Id);
        if (existing == null)
            return false;

        if (await _userRepository.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
            throw new Exception("Email already used by other users");

        return await _userRepository.UpdateSaveAsync(user);
    }

    // Delete user (DeleteNowSaveAsync returns bool)
    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetInfoDefaultAsync(u => u.Id == userId);
        if (user == null)
            return false;

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
            var affected = await _userService.CreateUserAsync(user);
            if (affected > 0)
                return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
            return BadRequest("Failed to create user");
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

### 5.2 Multi-database Mode Complete Example

> Note: a transaction **across two different `DbContext`s** (user DB and order DB) cannot be achieved through a single `IUnitOfWork`, because they use separate database connections. The example below uses the matching `IUnitOfWork` for each context.

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

    // Create order (transaction within the same OrderDbContext)
    public async Task<Order> CreateOrderAsync(int userId, List<OrderItemDto> items)
    {
        var user = await _userRepository.GetInfoDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new Exception("User does not exist");

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Status = "Pending",
            TotalAmount = items.Sum(i => i.Quantity * i.UnitPrice)
        };

        await _orderUnitOfWork.BeginTransactionAsync();
        try
        {
            _orderRepository.Add(order);
            await _orderUnitOfWork.SubmitAsync();   // save order first to get its Id

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
            throw new Exception("Failed to create order: " + ex.Message);
        }
    }

    // Get user's order list
    public async Task<List<Order>> GetUserOrdersAsync(int userId)
    {
        if (!await _userRepository.AnyAsync(u => u.Id == userId))
            throw new Exception("User does not exist");

        return await _orderRepository.GetListAsync(o => o.UserId == userId);
    }

    // Get order details
    public async Task<Order> GetOrderDetailsAsync(int orderId)
    {
        var order = await _orderRepository.GetInfoDefaultAsync(o => o.Id == orderId);
        if (order == null)
            throw new Exception("Order does not exist");

        order.Items = await _orderItemRepository.GetListAsync(item => item.OrderId == orderId);
        return order;
    }
}
```

---

## 6. Supported .NET Versions

- netcoreapp3.1
- net5
- net6
- net7
- net8
- net9
- net10.0

---

## 7. NuGet Package Information

| Item | Detail |
| :--- | :--- |
| **Package ID** | Acme.EFCore.Small |
| **Authors** | yzxs |
| **Description** | Lightweight EFCore operation library |
| **Project URL** | https://www.nuget.org/packages/Acme.EFCore.Small/2.0.0.4#readme-body-tab |
| **Gitee** | <https://gitee.com/yzxs949/acme.-efcore.-small/tree/%E6%A0%87%E5%87%86%E7%89%88/> |
| **GitHub** | <https://github.com/yzxs949/Acme.EFCore.Small/tree/%E6%A0%87%E5%87%86%E7%89%88> |
| **Copyright** | yzxs |

---

## 8. Testing

The project includes a complete test suite covering all core features:

| Test Class | Coverage |
| :--- | :--- |
| **EntityTests** | BaseEntity and BaseAggregateRoot classes |
| **ValueObjectTests** | BaseValueObject class |
| **PageListTests** | PageList class |
| **PageListExtensionTests** | PageListExtension / pagination methods of LinqExtension |
| **QueryTests** | Condition, Keywords and Sorting classes |
| **LinqExtensionTests** | Key methods of the LinqExtension class |
| **RepositoryTests** | Structure of the IRepository interface |
| **UnitOfWorkTests** | UnitOfWork class and IUnitOfWork interface |

Run the tests:

```bash
dotnet test Acme.EFCore.Small.Tests\Acme.EFCore.Small.Tests.csproj
```

---

## 9. License

This project is open source under the [MIT License](https://opensource.org/licenses/MIT).

---

## 10. Contributing

Contributions are welcome! Feel free to submit an Issue or Pull Request to help improve the project.

---

## 11. Contact

For any questions or suggestions, please contact the author.

- **Email**: yzxs949@163.com
- **NuGet**: https://www.nuget.org/packages/Acme.EFCore.Small/
- **Gitee**: <https://gitee.com/yzxs949/acme.-efcore.-small/tree/%E6%A0%87%E5%87%86%E7%89%88/>
- **GitHub**: <https://github.com/yzxs949/Acme.EFCore.Small/tree/%E6%A0%87%E5%87%86%E7%89%88>

---

![作者公众号](https://raw.githubusercontent.com/yzxs949/FilePath/refs/heads/main/Image/%E4%BA%8C%E7%BB%B4%E7%A0%81.jpg)
