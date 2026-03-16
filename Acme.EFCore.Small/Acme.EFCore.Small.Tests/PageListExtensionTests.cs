using Acme.EFCore.Small.Extensions;
using Acme.EFCore.Small.Page;
using Xunit;

namespace Acme.EFCore.Small.Tests;

public class PageListExtensionTests
{
    [Fact]
    public void ToPageList_WithObjectList_ShouldReturnCorrectPageList()
    {
        // 准备测试数据
        var items = new List<object> { 1, "test", 3.14 };
        var total = 10;

        // 调用扩展方法
        var pageList = items.ToPageList(total);

        // 验证结果
        Assert.Equal(total, pageList.Total);
        Assert.Equal(items, pageList.Items);
    }

    [Fact]
    public void ToPageList_WithGenericList_ShouldReturnCorrectPageList()
    {
        // 准备测试数据
        var items = new List<string> { "item1", "item2", "item3" };
        var total = 5;

        // 调用扩展方法
        var pageList = items.ToPageList(total);

        // 验证结果
        Assert.Equal(total, pageList.Total);
        Assert.Equal(items, pageList.Items);
    }
}
