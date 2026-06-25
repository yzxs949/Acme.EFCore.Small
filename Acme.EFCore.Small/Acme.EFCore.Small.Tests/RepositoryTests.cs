using Acme.EFCore.Small.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Acme.EFCore.Small.Tests;

public class RepositoryTests
{
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
