using System.Collections.Generic;
using Acme.EFCore.Small.Page;

namespace Acme.EFCore.Small.Extensions
{
    /// <summary>
    /// 分页拓展类
    /// </summary>
    public static class PageListExtension
    {
        /// <summary>
        /// 将集合转换为分页实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="items">当前页数据</param>
        /// <param name="total">总记录数</param>
        /// <returns>分页结果</returns>
        public static PageList<T> ToPageList<T>(this List<T> items, int total)
        {
            return new PageList<T>(total, items);
        }
    }
}
