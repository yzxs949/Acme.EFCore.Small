using System;
using System.Collections.Generic;

namespace Acme.EFCore.Small.Page
{
    /// <summary>
    /// 分页返回结果
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class PageList<T>
    {
        /// <summary>
        /// 构造函数, 用于返回分页数据
        /// </summary>
        /// <param name="total">总记录数</param>
        /// <param name="items">当前页数据</param>
        /// <param name="pageIndex">当前页码（从1开始）</param>
        /// <param name="pageSize">每页大小</param>
        public PageList(int total, List<T> items, int pageIndex = 1, int pageSize = 10)
        {
            Total = total;
            Items = items ?? new List<T>();
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// 当前页数据
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(Total / (double)PageSize) : 0;

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
