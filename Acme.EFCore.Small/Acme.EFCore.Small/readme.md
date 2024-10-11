# Acme.EFCore.Small

## 一、概述
Acme.EFCore.Small是一款轻量级的EFCore通用类库，旨在使用 Entity Framework Core（EFCore）与数据库进行交互。它作为处理各种数据库操作的基础组件。
版本：v1.2.7.1

## 二、入门指南 
### 1.安装Acme.EFCore.Small
创建项目 -> 点击引用 -> 右键 -> 管理Nuget程序包->搜索Acme.EFCore.Small选择v1.2.7及以上版本的.NET版本安装即可。
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
services.AddDbContext<AppDbContext1>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
services.AddBaseService();
```