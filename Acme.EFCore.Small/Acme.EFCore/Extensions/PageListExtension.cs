using Acme.EFCore.Page;
using System.Collections.Generic;

namespace Acme.EFCore.Extensions;

/// <summary>
/// 分页拓展类
/// </summary>
public static class PageListExtension
{
    /// <summary>
    /// 集合转化为分页实体
    /// </summary>
    /// <param name="items"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    public static PageList<object> ToPageList(this List<object> items, int total)
    {
        PageList<object> pageList = new(total, items);
        return pageList;
    }

    /// <summary>
    /// 实体转化为分页实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    public static PageList<T> ToPageList<T>(this List<T> items, int total)
    {
        PageList<T> pageList = new(total, items);
        return pageList;
    }
}
