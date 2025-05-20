# Acme.EFCore

## 1、summary
Acme.EFCore is a lightweight EFCore general-purpose library designed to interact with databases using Entity Framework Core (EFCore). It serves as the fundamental component for handling various database operations.
Version: v3.3.8-beta

## 2、 Beginner's Guide
### 2.1. Install Acme EFCore
Create Project ->Click on References ->Right click ->Manage Nuget Packages ->Search Acme EFCore selects version 3.0.0.3-alpha and above Simply install the NET version.
### 2.2 Install the corresponding database package
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

### 2.3.Create VNet database context class
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
}
```
### 2.3.Configure connection string
```json
{
    "ConnectionStrings":{
        "DefaultConnection": "Persist Security Info=False;Data Source=.;Initial Catalog=Database Name;User ID=user;Password=password;Connect Timeout=120;Encrypt=False;"
    }
}
```

### 2.5 Dependency injection
#### 2.6.1.Basic configuration
```csharp
//Call database configuration information
services.AddDbContext<AppDbContext1>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
```