using Acme.EFCore.Small.Enums;
using Acme.EFCore.Small.Extensions;
using Acme.EFCore.Small.Querys;
using Xunit;

namespace Acme.EFCore.Small.Tests;

/// <summary>
/// 查询扩展真实行为测试
/// 覆盖：AddSorting / AddSortingIf / AddConditionsContains / AddConditions 的 Symbol 全部 14 种运算符
/// </summary>
public class QueryExtensionTests
{
    private class Item
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? City { get; set; }

        public int Age { get; set; }

        public bool IsActive { get; set; }
    }

    private static List<Item> Data() => new()
    {
        new Item { Id = 1, Name = "张三", City = "北京", Age = 30, IsActive = true },
        new Item { Id = 2, Name = "李四", City = "上海", Age = 25, IsActive = false },
        new Item { Id = 3, Name = "王五", City = null, Age = 40, IsActive = true },
        new Item { Id = 4, Name = "赵六", City = "北京", Age = 35, IsActive = false },
    };

    private static IQueryable<Item> Query() => Data().AsQueryable();

    private static List<int> Ids(IEnumerable<Item> items) => items.Select(x => x.Id).OrderBy(x => x).ToList();

    #region AddSorting

    [Fact]
    public void AddSorting_ASC_orders_ascending()
    {
        var result = Query().AddSorting(new Sorting("Age", SortingType.ASC)).ToList();

        Assert.Equal(new[] { 25, 30, 35, 40 }, result.Select(x => x.Age).ToArray());
    }

    [Fact]
    public void AddSorting_DESC_orders_descending()
    {
        var result = Query().AddSorting(new Sorting("Age", SortingType.DESC)).ToList();

        Assert.Equal(new[] { 40, 35, 30, 25 }, result.Select(x => x.Age).ToArray());
    }

    [Fact]
    public void AddSorting_null_falls_back_to_Id_ascending()
    {
        var result = Query().AddSorting(null!).ToList();

        Assert.Equal(new[] { 1, 2, 3, 4 }, result.Select(x => x.Id).ToArray());
    }

    [Fact]
    public void AddSorting_unknown_field_throws_ArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(
            () => Query().AddSorting(new Sorting("NotExists", SortingType.ASC)).ToList());

        Assert.Contains("NotExists", ex.Message);
    }

    #endregion

    #region AddSortingIf

    [Fact]
    public void AddSortingIf_true_applies_sorting()
    {
        var result = Query().AddSortingIf(true, new Sorting("Age", SortingType.DESC)).ToList();

        Assert.Equal(40, result[0].Age);
        Assert.Equal(25, result[^1].Age);
    }

    [Fact]
    public void AddSortingIf_false_keeps_original_order()
    {
        var result = Query().AddSortingIf(false, new Sorting("Age", SortingType.DESC)).ToList();

        Assert.Equal(new[] { 1, 2, 3, 4 }, result.Select(x => x.Id).ToArray());
    }

    #endregion

    #region AddConditionsContains

    [Fact]
    public void AddConditionsContains_matches_keyword_in_any_field()
    {
        var result = Query()
            .AddConditionsContains(new Keywords(new[] { "Name", "City" }, "北京"))
            .ToList();

        Assert.Equal(new[] { 1, 4 }, Ids(result).ToArray());
    }

    [Fact]
    public void AddConditionsContains_single_field_only_matches_that_field()
    {
        var result = Query()
            .AddConditionsContains(new Keywords(new[] { "Name" }, "张三"))
            .ToList();

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public void AddConditionsContains_null_keywords_returns_all()
    {
        var result = Query().AddConditionsContains(null!).ToList();

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void AddConditionsContains_empty_value_returns_all()
    {
        var result = Query().AddConditionsContains(new Keywords(new[] { "Name" }, "")).ToList();

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void AddConditionsContains_empty_fields_returns_all()
    {
        var result = Query().AddConditionsContains(new Keywords(Array.Empty<string>(), "北京")).ToList();

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void AddConditionsContains_with_isAdd_false_skips_filter()
    {
        var result = Query()
            .AddConditionsContains(false, new Keywords(new[] { "Name", "City" }, "北京"))
            .ToList();

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void AddConditionsContains_with_isAdd_true_applies_filter()
    {
        var result = Query()
            .AddConditionsContains(true, new Keywords(new[] { "Name", "City" }, "北京"))
            .ToList();

        Assert.Equal(2, result.Count);
    }

    #endregion

    #region AddConditions - Symbol 全部 14 种运算符

    private static List<Item> Apply(string field, string? value, Symbol symbol)
        => Query().AddConditions(new List<Condition> { new(field, value!, symbol) }).ToList();

    [Fact]
    public void Symbol_Equal_matches_exact_value()
    {
        var result = Apply("Age", "30", Symbol.Equal);

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public void Symbol_NotEqual_excludes_value()
    {
        var result = Apply("Age", "30", Symbol.NotEqual);

        Assert.Equal(new[] { 2, 3, 4 }, Ids(result).ToArray());
    }

    [Fact]
    public void Symbol_GreaterThan_takes_larger_values()
    {
        var result = Apply("Age", "30", Symbol.GreaterThan);

        Assert.Equal(new[] { 3, 4 }, Ids(result).ToArray());
    }

    [Fact]
    public void Symbol_LessThan_takes_smaller_values()
    {
        var result = Apply("Age", "30", Symbol.LessThan);

        Assert.Equal(new[] { 2 }, Ids(result).ToArray());
    }

    [Fact]
    public void Symbol_GreaterThanOrEqual_includes_boundary()
    {
        var result = Apply("Age", "35", Symbol.GreaterThanOrEqual);

        Assert.Equal(new[] { 3, 4 }, Ids(result).ToArray());
    }

    [Fact]
    public void Symbol_LessThanOrEqual_includes_boundary()
    {
        var result = Apply("Age", "25", Symbol.LessThanOrEqual);

        Assert.Equal(new[] { 2 }, Ids(result).ToArray());
    }

    [Fact]
    public void Symbol_Contains_matches_substring()
    {
        var result = Apply("Name", "张", Symbol.Contains);

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public void Symbol_NotContains_excludes_substring()
    {
        var result = Apply("Name", "张", Symbol.NotContains);

        Assert.Equal(new[] { 2, 3, 4 }, Ids(result).ToArray());
    }

    [Fact]
    public void Symbol_StartsWith_matches_prefix()
    {
        var result = Apply("Name", "张", Symbol.StartsWith);

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public void Symbol_EndsWith_matches_suffix()
    {
        var result = Apply("Name", "六", Symbol.EndsWith);

        Assert.Single(result);
        Assert.Equal(4, result[0].Id);
    }

    [Fact]
    public void Symbol_In_matches_any_value_in_list()
    {
        var result = Apply("Id", "1,3", Symbol.In);

        Assert.Equal(new[] { 1, 3 }, Ids(result).ToArray());
    }

    [Fact]
    public void Symbol_NotIn_excludes_values_in_list()
    {
        var result = Apply("Id", "1,3", Symbol.NotIn);

        Assert.Equal(new[] { 2, 4 }, Ids(result).ToArray());
    }

    [Fact]
    public void Symbol_IsNull_matches_null_field()
    {
        var result = Apply("City", null, Symbol.IsNull);

        Assert.Single(result);
        Assert.Equal(3, result[0].Id);
    }

    [Fact]
    public void Symbol_IsNotNull_matches_non_null_field()
    {
        var result = Apply("City", null, Symbol.IsNotNull);

        Assert.Equal(new[] { 1, 2, 4 }, Ids(result).ToArray());
    }

    #endregion

    #region AddConditionsIf

    [Fact]
    public void AddConditionsIf_true_applies_conditions()
    {
        var result = Query()
            .AddConditionsIf(true, new List<Condition> { new("Age", "30", Symbol.Equal) })
            .ToList();

        Assert.Single(result);
    }

    [Fact]
    public void AddConditionsIf_false_skips_conditions()
    {
        var result = Query()
            .AddConditionsIf(false, new List<Condition> { new("Age", "30", Symbol.Equal) })
            .ToList();

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void AddConditions_multiple_conditions_are_combined_with_AND()
    {
        var result = Query()
            .AddConditions(new List<Condition>
            {
                new("Age", "30", Symbol.GreaterThanOrEqual),
                new("City", "北京", Symbol.Equal),
            })
            .ToList();

        Assert.Equal(new[] { 1, 4 }, Ids(result).ToArray());
    }

    #endregion
}
