namespace Acme.EFCore.Small.Enums
{
    /// <summary>
    /// 动态查询运算符号
    /// </summary>
    /// <summary>
    /// 运算符号
    /// </summary>
    public enum Symbol
    {
        /// <summary>
        /// 等于 =
        /// </summary>
        Equal = 0,

        /// <summary>
        /// 不等于 !=
        /// </summary>
        NotEqual = 1,

        /// <summary>
        /// 大于 &gt;
        /// </summary>
        GreaterThan = 2,

        /// <summary>
        /// 小于 &lt;
        /// </summary>
        LessThan = 3,

        /// <summary>
        /// 大于等于 &gt;=
        /// </summary>
        GreaterThanOrEqual = 4,

        /// <summary>
        /// 小于等于 &lt;=
        /// </summary>
        LessThanOrEqual = 5,

        /// <summary>
        /// 模糊包含 %value%
        /// </summary>
        Contains = 6,

        /// <summary>
        /// 不包含
        /// </summary>
        NotContains = 7,

        /// <summary>
        /// 开头匹配 value%
        /// </summary>
        StartsWith = 8,

        /// <summary>
        /// 结尾匹配 %value
        /// </summary>
        EndsWith = 9,

        /// <summary>
        /// 包含于列表 In
        /// </summary>
        In = 10,

        /// <summary>
        /// 不包含于列表 Not In
        /// </summary>
        NotIn = 11,

        /// <summary>
        /// 为 NULL
        /// </summary>
        IsNull = 12,

        /// <summary>
        /// 不为 NULL
        /// </summary>
        IsNotNull = 13
    }
}
