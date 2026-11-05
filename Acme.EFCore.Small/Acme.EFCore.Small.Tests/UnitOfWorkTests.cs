using Acme.EFCore.Small.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Acme.EFCore.Small.Tests;

public class UnitOfWorkTests
{
    [Fact]
    public void UnitOfWork_Constructor_ShouldInitializeWithDbContext()
    {
        // 准备测试数据
        var mockDbContext = new Mock<DbContext>();

        // 创建 UnitOfWork 实例
        var unitOfWork = new UnitOfWork(mockDbContext.Object);

        // 验证构造函数是否正确初始化
        Assert.NotNull(unitOfWork);
        Assert.Equal(mockDbContext.Object, unitOfWork.DbContext);
    }

    [Fact]
    public void UnitOfWork_Generic_Constructor_ShouldInitializeWithDbContext()
    {
        // 准备测试数据
        var mockDbContext = new Mock<TestDbContext>();

        // 创建 UnitOfWork 实例
        var unitOfWork = new UnitOfWork<TestDbContext>(mockDbContext.Object);

        // 验证构造函数是否正确初始化
        Assert.NotNull(unitOfWork);
        Assert.Equal(mockDbContext.Object, unitOfWork.DbContext);
    }

    [Fact]
    public void IUnitOfWork_ShouldHaveExpectedMethods()
    {
        // 验证 IUnitOfWork 接口包含预期的方法
        var unitOfWorkType = typeof(IUnitOfWork<>);
        var methods = unitOfWorkType.GetMethods();

        // 验证一些关键方法是否存在
        Assert.Contains(methods, m => m.Name == "Submit");
        Assert.Contains(methods, m => m.Name == "SubmitAsync");
        Assert.Contains(methods, m => m.Name == "BeginTransaction");
        Assert.Contains(methods, m => m.Name == "BeginTransactionAsync");
        Assert.Contains(methods, m => m.Name == "CommitTransaction");
        Assert.Contains(methods, m => m.Name == "CommitTransactionAsync");
        Assert.Contains(methods, m => m.Name == "RollbackTransaction");
        Assert.Contains(methods, m => m.Name == "RollbackTransactionAsync");
    }

    [Fact]
    public void IUnitOfWork_ShouldHaveExpectedProperties()
    {
        // 验证 IUnitOfWork 接口包含预期的属性
        var unitOfWorkType = typeof(IUnitOfWork<>);
        var properties = unitOfWorkType.GetProperties();

        // 验证 DbContext 属性是否存在
        Assert.Contains(properties, p => p.Name == "DbContext");
    }

    // 注意：由于事务操作需要实际的数据库连接，完整的功能测试需要集成测试
    // 这里我们只测试基本的构造函数和初始化

    // 测试用的 DbContext 类
    public class TestDbContext : DbContext
    {
        public TestDbContext() : base()
        {
        }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
    }
}
