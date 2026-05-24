using Acme.EFCore.Small.Enums;

namespace Acme.EFCore.Small.Querys
{
    /// <summary>
    /// 排序
    /// </summary> 
    public class Sorting
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Sorting()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortingType">排序类型</param>
        public Sorting(string sortField, SortingType sortingType)
        {
            SortField = sortField;
            SortingType = sortingType;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public SortingType SortingType { get; set; }
    }
}
