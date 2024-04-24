using System.Collections.Generic;

namespace Acme.EFCore.Small.Page
{

    /// <summary>
    /// 分页返回结果
    /// </summary>
    public class PageList<T>
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 索引
        /// </summary>

        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>

        public int PageSize { get; set; }

        /// <summary>
        /// 集合数据
        /// </summary>

        public List<T> Items { get; set; } = new List<T>();
    }

    ///<summary>
    /// 分页返回结果
    /// </summary>
    public class PageList
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 索引
        /// </summary>

        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>

        public int PageSize { get; set; }

        /// <summary>
        /// 集合数据
        /// </summary>

        public List<object> Items { get; set; } = new List<object>();
    }
}