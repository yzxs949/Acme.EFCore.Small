using Acme.EFCore.Small.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.EFCore.Small.Querys
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="symbol">运算符</param>
        public Condition(string field, string value, Symbol symbol)
        {
            Field = field;
            Value = value;
            Symbol = symbol;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// 字段值
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 运算符
        /// </summary>
        public Symbol Symbol { get; }
    }

    /// <summary>
    /// 排序
    /// </summary> 
    public class Sorting
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public SortingType SortingType { get; }
    }

    /// <summary>
    /// 关键字搜索
    /// </summary>
    public class Keywords
    {
        /// <summary>
        /// 字段名称数组
        /// </summary>
        public string[] Fields { get; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; }
    }
}
