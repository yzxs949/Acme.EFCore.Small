using System.Collections.Generic;

namespace Acme.EFCore.Small.Page
{
    /// <summary>
    /// 分页返回结果
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class PageList<T> : IPageList
    {
        /// <summary>
        /// 构造函数, 用于返回分页数据
        /// </summary>
        /// <param name="total">条数</param>
        /// <param name="items">每页显示的数据</param>
        public PageList(int total, List<T> items)
        {
            Total = total;
            Items = items;
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 集合数据
        /// </summary>
        public List<T> Items { get; set; }
    }
}
