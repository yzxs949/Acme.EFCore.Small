using Acme.EFCore.Small.Page;
using System.Collections.Generic;

namespace Acme.EFCore.Small.Extensions
{

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
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static PageList<object> ToPageList(this List<object> items, int total, int PageIndex, int PageSize)
        {
            PageList<object> pageList = new PageList<object>()
            {
                Total = total,
                PageIndex = PageIndex,
                PageSize = PageSize,
                Items = items,
            };
            return pageList;
        }

        /// <summary>
        /// 实体转化为分页实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="total"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static PageList<T> ToPageList<T>(this List<T> items, int total, int PageIndex, int PageSize)
        {
            PageList<T> pageList = new PageList<T>()
            {
                Total = total,
                PageIndex = PageIndex,
                PageSize = PageSize,
                Items = items,
            };
            return pageList;
        }
    }
}
