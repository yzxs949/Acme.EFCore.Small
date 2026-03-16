using Acme.EFCore.Small.Page;
using Xunit;

namespace Acme.EFCore.Small.Tests;

public class PageListTests
{
    [Fact]
    public void PageList_Constructor_ShouldInitializeWithProvidedValues()
    {
        // 准备测试数据
        var total = 100;
        var items = new List<int> { 1, 2, 3, 4, 5 };

        // 创建 PageList 实例
        var pageList = new PageList<int>(total, items);

        // 验证属性值
        Assert.Equal(total, pageList.Total);
        Assert.Equal(items, pageList.Items);
    }
}
