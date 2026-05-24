using Acme.EFCore.Small.Enums;

namespace Acme.EFCore.Small.Querys
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// 构造函数，初始化 Condition 实例
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
        /// 构造函数，初始化 Condition 实例
        /// </summary>
        public Condition()
        {
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 运算符
        /// </summary>
        public Symbol Symbol { get; set; }
    }
}
