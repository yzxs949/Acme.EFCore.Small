using Acme.EFCore.Small.Extensions;
using Xunit;

namespace Acme.EFCore.Small.Tests;

public class LinqExtensionTests
{
    [Fact]
    public void WhereIf_WithTrueVerification_ShouldApplyFilter()
    {
        // 准备测试数据
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var queryable = numbers.AsQueryable();

        // 测试当验证条件为 true 时，应该应用过滤器
        var result = queryable.WhereIf(true, x => x > 3).ToList();

        // 验证结果
        Assert.Equal(2, result.Count);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
    }

    [Fact]
    public void WhereIf_WithFalseVerification_ShouldNotApplyFilter()
    {
        // 准备测试数据
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var queryable = numbers.AsQueryable();

        // 测试当验证条件为 false 时，不应该应用过滤器
        var result = queryable.WhereIf(false, x => x > 3).ToList();

        // 验证结果
        Assert.Equal(5, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
    }

    [Fact]
    public void ToPageList_ShouldReturnCorrectPage()
    {
        // 准备测试数据
        var numbers = Enumerable.Range(1, 10).ToList();
        var queryable = numbers.AsQueryable();

        // 测试分页方法
        var pageResult = queryable.ToPageList(2, 3);

        // 验证结果
        Assert.Equal(10, pageResult.Total);
        Assert.Equal(3, pageResult.Items.Count);
        Assert.Contains(4, pageResult.Items);
        Assert.Contains(5, pageResult.Items);
        Assert.Contains(6, pageResult.Items);
    }

    [Fact]
    public void GetKey_ShouldReturnUniqueKeys()
    {
        // 准备测试数据
        var items = new List<TestItem>
        {
            new TestItem { Id = 1, Category = "A" },
            new TestItem { Id = 2, Category = "B" },
            new TestItem { Id = 3, Category = "A" },
            new TestItem { Id = 4, Category = "C" }
        };
        var queryable = items.AsQueryable();

        // 测试获取唯一键
        var keys = queryable.GetKey(x => x.Category).ToList();

        // 验证结果
        Assert.Equal(3, keys.Count);
        Assert.Contains("A", keys);
        Assert.Contains("B", keys);
        Assert.Contains("C", keys);
    }

    [Fact]
    public void GetKeyList_ShouldReturnUniqueKeys()
    {
        // 准备测试数据
        var items = new List<TestItem>
        {
            new TestItem { Id = 1, Category = "A" },
            new TestItem { Id = 2, Category = "B" },
            new TestItem { Id = 3, Category = "A" },
            new TestItem { Id = 4, Category = "C" }
        };

        // 测试获取唯一键
        var keys = items.GetKeyList(x => x.Category).ToList();

        // 验证结果
        Assert.Equal(3, keys.Count);
        Assert.Contains("A", keys);
        Assert.Contains("B", keys);
        Assert.Contains("C", keys);
    }

    // 测试类
    private class TestItem
    {
        public int Id { get; set; }
        public string Category { get; set; }
    }
}
