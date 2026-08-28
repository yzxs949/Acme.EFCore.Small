using Acme.EFCore.Small.Enums;
using Acme.EFCore.Small.Extensions;
using Acme.EFCore.Small.Querys;
using Xunit;

namespace Acme.EFCore.Small.Tests;

/// <summary>
/// AddConditions 动态条件查询测试
/// 覆盖：NotContains、In/NotIn 逗号分隔拆分、Nullable 类型、枚举类型
/// </summary>
public class AddConditionsTests
{
    private class TestItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public int? Score { get; set; }

        public TestStatus Status { get; set; }
    }

    private enum TestStatus
    {
        Pending = 0,
        Active = 1,
        Disabled = 2,
    }

    private static List<TestItem> GetTestData() => new()
    {
        new TestItem { Id = 1, Name = "张三", Age = 18, Score = 80, Status = TestStatus.Active },
        new TestItem { Id = 2, Name = "李四", Age = 25, Score = null, Status = TestStatus.Disabled },
        new TestItem { Id = 3, Name = "张伟", Age = 30, Score = 90, Status = TestStatus.Active },
        new TestItem { Id = 4, Name = "王五", Age = 40, Score = 85, Status = TestStatus.Pending },
    };

    [Fact]
    public void AddConditions_NotContains_ShouldExcludeMatchingValues()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Name", "张", Symbol.NotContains) })
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, x => Assert.DoesNotContain("张", x.Name));
    }

    [Fact]
    public void AddConditions_Contains_ShouldKeepMatchingValues()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Name", "张", Symbol.Contains) })
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, x => Assert.Contains("张", x.Name));
    }

    [Fact]
    public void AddConditions_In_WithCommaSeparatedValues_ShouldSplitToList()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Id", "1,3", Symbol.In) })
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 1, 3 }, result.Select(x => x.Id).OrderBy(x => x));
    }

    [Fact]
    public void AddConditions_In_WithSpacesAroundValues_ShouldTrim()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Id", " 1 , 3 ", Symbol.In) })
            .ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void AddConditions_In_WithEmptyValue_ShouldSkipCondition()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Id", "", Symbol.In), new("Age", "40", Symbol.Equal) })
            .ToList();

        // In 条件值为空被跳过，仅应用 Age = 40
        Assert.Single(result);
        Assert.Equal(4, result[0].Id);
    }

    [Fact]
    public void AddConditions_NotIn_WithCommaSeparatedValues_ShouldSplitToList()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Id", "1,3", Symbol.NotIn) })
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 2, 4 }, result.Select(x => x.Id).OrderBy(x => x));
    }

    [Fact]
    public void AddConditions_StringField_In_ShouldMatchExactValues()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Name", "张三,王五", Symbol.In) })
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 1, 4 }, result.Select(x => x.Id).OrderBy(x => x));
    }

    [Fact]
    public void AddConditions_NullableField_Equal_ShouldNotThrow()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Score", "80", Symbol.Equal) })
            .ToList();

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public void AddConditions_NullableField_In_ShouldMatchValues()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Score", "80,90", Symbol.In) })
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 1, 3 }, result.Select(x => x.Id).OrderBy(x => x));
    }

    [Fact]
    public void AddConditions_EnumField_Equal_ShouldMatchByName()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Status", "Active", Symbol.Equal) })
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 1, 3 }, result.Select(x => x.Id).OrderBy(x => x));
    }

    [Fact]
    public void AddConditions_EnumField_In_ShouldMatchByName()
    {
        var queryable = GetTestData().AsQueryable();

        var result = queryable
            .AddConditions(new List<Condition> { new("Status", "Active,Disabled", Symbol.In) })
            .ToList();

        Assert.Equal(3, result.Count);
    }
}
