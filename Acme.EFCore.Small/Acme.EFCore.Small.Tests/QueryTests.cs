using Acme.EFCore.Small.Enums;
using Acme.EFCore.Small.Querys;
using Xunit;

namespace Acme.EFCore.Small.Tests;

public class QueryTests
{
    [Fact]
    public void Condition_ConstructorWithParameters_ShouldInitializeWithProvidedValues()
    {
        // 准备测试数据
        var field = "Name";
        var value = "Test";
        var symbol = Symbol.Equal;

        // 创建 Condition 实例
        var condition = new Condition(field, value, symbol);

        // 验证属性值
        Assert.Equal(field, condition.Field);
        Assert.Equal(value, condition.Value);
        Assert.Equal(symbol, condition.Symbol);
    }

    [Fact]
    public void Condition_DefaultConstructor_ShouldInitializeSuccessfully()
    {
        // 创建 Condition 实例
        var condition = new Condition();

        // 验证对象创建成功
        Assert.NotNull(condition);
    }

    [Fact]
    public void Keywords_ConstructorWithParameters_ShouldInitializeWithProvidedValues()
    {
        // 准备测试数据
        var fields = new[] { "Name", "Description" };
        var value = "Test";

        // 创建 Keywords 实例
        var keywords = new Keywords(fields, value);

        // 验证属性值
        Assert.Equal(fields, keywords.Fields);
        Assert.Equal(value, keywords.Value);
    }

    [Fact]
    public void Keywords_DefaultConstructor_ShouldInitializeSuccessfully()
    {
        // 创建 Keywords 实例
        var keywords = new Keywords();

        // 验证对象创建成功
        Assert.NotNull(keywords);
    }

    [Fact]
    public void Sorting_ConstructorWithParameters_ShouldInitializeWithProvidedValues()
    {
        // 准备测试数据
        var sortField = "Name";
        var sortingType = SortingType.ASC;

        // 创建 Sorting 实例
        var sorting = new Sorting(sortField, sortingType);

        // 验证属性值
        Assert.Equal(sortField, sorting.SortField);
        Assert.Equal(sortingType, sorting.SortingType);
    }

    [Fact]
    public void Sorting_DefaultConstructor_ShouldInitializeSuccessfully()
    {
        // 创建 Sorting 实例
        var sorting = new Sorting();

        // 验证对象创建成功
        Assert.NotNull(sorting);
    }
}
