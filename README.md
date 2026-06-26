# Acme.EFCore.Small

## 项目简介

Acme.EFCore.Small 是一个轻量级的 Entity Framework Core 通用库，提供了实体基类、仓储模式、查询扩展等常用功能，帮助开发者快速构建基于 EF Core 的数据访问层。

- **当前版本**：v1.3.7.0
- **支持 .NET 版本**：.NET Core 3.1 / .NET 5.0 / .NET 6.0 / .NET 7.0 / .NET 8.0 / .NET 9.0 / .NET 10.0

## 功能特性

### 基础类
- **BaseEntity<TKey>**：实体基类，包含主键属性
- **BaseAggregateRoot<TKey>**：聚合根基类，适用于包含子实体集合的实体
- **BaseValueObject**：值对象基类，用于表示无唯一标识的概念

### 仓储模式
- **IRepository<TEntity>**：单库模式仓储接口
- **IRepository<TDbContext, TEntity>**：多库模式仓储接口，支持多数据库场景
- 提供同步/异步两种操作方式

### 查询扩展
- **WhereIf**：动态条件查询
- **AddConditions**：结构化条件查询
- **AddSorting**：动态排序
- **AddConditionsContains**：多字段关键字模糊查询

### 分页功能
- **PageList<T>**：分页结果类型，包含总记录数和当前页数据
- 支持同步/异步分页

## 快速开始

### 安装

```bash
dotnet add package Acme.EFCore.Small
```

### 配置依赖注入

```csharp
// 单库模式
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(connectionString));
services.AddRepositorys<AppDbContext>();

// 多库模式
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(connectionString1));
services.AddDbContext<OtherDbContext>(options => 
    options.UseSqlServer(connectionString2));
services.AddRepositorys();
```

### 定义实体

```csharp
public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### 使用仓储

```csharp
// 注入仓储
private readonly IRepository<User> _userRepository;

// 添加
await _userRepository.AddNowSaveAsync(user);

// 查询
var user = await _userRepository.GetInfoAsync(u => u.Id == id);

// 更新
await _userRepository.UpdateSaveAsync(user);

// 删除
await _userRepository.DeleteNowSaveAsync(user);

// 分页查询
var pageList = await _userRepository.Queryable()
    .WhereIf(!string.IsNullOrEmpty(name), u => u.Name.Contains(name))
    .ToPageListAsync(pageIndex, pageSize);
```

## 项目结构

```
Acme.EFCore.Small/
├── AggregateRoots/          # 聚合根基类
├── Entitys/                 # 实体基类
├── Enums/                   # 枚举定义
├── Extensions/              # 扩展方法
├── Page/                    # 分页类
├── Querys/                  # 查询条件类
├── Repositorys/            # 仓储接口和实现
└── ValueObjects/            # 值对象基类
```

## NuGet 包信息

| 属性 | 值 |
|------|-----|
| 包名 | Acme.EFCore.Small |
| 作者 | yzxs |
| 项目地址 | https://gitee.com/yzxs949/acme.-efcore.-small |
| NuGet 地址 | https://www.nuget.org/packages/Acme.EFCore.Small/ |

## 许可证

本项目基于 MIT 许可证开源。

## 贡献

欢迎提交 Issue 和 Pull Request！

## 联系方式

邮箱：yzxs949@163.com