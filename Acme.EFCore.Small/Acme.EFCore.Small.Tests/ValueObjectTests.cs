using Acme.EFCore.Small.ValueObjects;
using Xunit;

namespace Acme.EFCore.Small.Tests;

public class ValueObjectTests
{
    [Fact]
    public void BaseValueObject_DefaultConstructor_ShouldInitializeSuccessfully()
    {
        // 为测试创建一个具体的 BaseValueObject 实现
        var valueObject = new TestValueObject();

        // 验证对象创建成功
        Assert.NotNull(valueObject);
    }

    // 测试类：BaseValueObject 的具体实现
    private class TestValueObject : BaseValueObject
    {
        public TestValueObject() : base()
        {
        }
    }
}
