using Acme.EFCore.Small.Repositorys;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Acme.EFCore.Small.Tests;

public class RepositoryTests
{
    [Fact]
    public void IRepository_ShouldHaveExpectedMethods()
    {
        // 验证 IRepository 接口包含预期的方法
        var repositoryType = typeof(IRepository<,>);
        var methods = repositoryType.GetMethods();

        // 验证一些关键方法是否存在
        Assert.Contains(methods, m => m.Name == "Add");
        Assert.Contains(methods, m => m.Name == "Delete");
        Assert.Contains(methods, m => m.Name == "Update");
        Assert.Contains(methods, m => m.Name == "GetInfo");
        Assert.Contains(methods, m => m.Name == "GetList");
        Assert.Contains(methods, m => m.Name == "Submit");
    }

    [Fact]
    public void IRepository_ShouldHaveExpectedProperties()
    {
        // 验证 IRepository 接口包含预期的属性
        var repositoryType = typeof(IRepository<,>);
        var properties = repositoryType.GetProperties();

        // 验证 DbContext 属性是否存在
        Assert.Contains(properties, p => p.Name == "DbContext");
    }

    [Fact]
    public void IRepository_TEntity_ShouldInheritFromIRepository()
    {
        // 验证 IRepository<TEntity> 继承自 IRepository<DbContext, TEntity>
        var repositoryType = typeof(IRepository<>);
        var baseInterfaces = repositoryType.GetInterfaces();

        // 验证是否继承自 IRepository<DbContext, TEntity>
        Assert.Contains(baseInterfaces, i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<,>));
    }

    // 注意：由于 Repository 类依赖于 DbContext，完整的功能测试需要使用模拟框架
    // 这里我们只测试接口的结构，实际的功能测试可以在集成测试中进行
}
