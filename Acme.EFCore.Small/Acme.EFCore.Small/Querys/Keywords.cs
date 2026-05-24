namespace Acme.EFCore.Small.Querys
{
    /// <summary>
    /// 关键字搜索
    /// </summary>
    public class Keywords
    {
        /// <summary>
        /// 构造函数，初始化 Keywords 默认实例
        /// </summary>
        /// <param name="fields">字段名称数组</param>
        /// <param name="value">值</param>
        public Keywords(string[] fields, string value)
        {
            Fields = fields;
            Value = value;
        }

        /// <summary>
        /// 构造函数，初始化 Keywords 默认实例
        /// </summary>
        public Keywords()
        {
        }

        /// <summary>
        /// 字段名称数组
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
