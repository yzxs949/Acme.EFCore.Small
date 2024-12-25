namespace Acme.EFCore.SearchConditions;

/// <summary>
/// 查询条件枚举
/// </summary>
public enum RelationEnum
{
    /// <summary>
    /// 包含
    /// </summary>
    Contains = 0,
    /// <summary>
    /// 等于
    /// </summary>
    Equal = 1,
    /// <summary>
    /// 不等于
    /// </summary>
    NotEqual = 2,
    /// <summary>
    /// 小于
    /// </summary>
    Less = 3,
    /// <summary>
    /// 小于等于
    /// </summary>
    LessEqual = 4,
    /// <summary>
    /// 大于
    /// </summary>
    Greater = 5,
    /// <summary>
    /// 大于等于
    /// </summary>
    GreaterEqual = 6,
}
