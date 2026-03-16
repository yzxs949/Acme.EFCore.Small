using Acme.EFCore.Small.AggregateRoots;
using Acme.EFCore.Small.Entitys;
using Xunit;

namespace Acme.EFCore.Small.Tests;

public class EntityTests
{
    // 测试 BaseEntity 类
    [Fact]
    public void BaseEntity_DefaultConstructor_ShouldInitializeWithDefaultId()
    {
        // 为测试创建一个具体的 BaseEntity 实现
        var entity = new TestEntity();
        // 验证 Id 是默认值
        Assert.Equal(default(int), entity.Id);
    }

    [Fact]
    public void BaseEntity_ConstructorWithId_ShouldInitializeWithProvidedId()
    {
        // 为测试创建一个具体的 BaseEntity 实现
        var expectedId = 123;
        var entity = new TestEntity(expectedId);
        // 验证 Id 是提供的值
        Assert.Equal(expectedId, entity.Id);
    }

    // 测试 BaseAggregateRoot 类
    [Fact]
    public void BaseAggregateRoot_DefaultConstructor_ShouldInitializeWithDefaultId()
    {
        // 为测试创建一个具体的 BaseAggregateRoot 实现
        var aggregateRoot = new TestAggregateRoot();
        // 验证 Id 是默认值
        Assert.Equal(default(int), aggregateRoot.Id);
    }

    [Fact]
    public void BaseAggregateRoot_ConstructorWithId_ShouldInitializeWithProvidedId()
    {
        // 为测试创建一个具体的 BaseAggregateRoot 实现
        var expectedId = 456;
        var aggregateRoot = new TestAggregateRoot(expectedId);
        // 验证 Id 是提供的值
        Assert.Equal(expectedId, aggregateRoot.Id);
    }

    // 测试类：BaseEntity 的具体实现
    private class TestEntity : BaseEntity<int>
    {
        public TestEntity() : base()
        {
        }

        public TestEntity(int id) : base(id)
        {
        }
    }

    // 测试类：BaseAggregateRoot 的具体实现
    private class TestAggregateRoot : BaseAggregateRoot<int>
    {
        public TestAggregateRoot() : base()
        {
        }

        public TestAggregateRoot(int id) : base(id)
        {
        }
    }
}
