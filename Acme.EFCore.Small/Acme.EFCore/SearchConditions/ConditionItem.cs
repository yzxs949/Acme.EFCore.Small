namespace Acme.EFCore.SearchConditions;

/// <summary>
/// 组合查询明细
/// </summary>
public class ConditionItem
{
    /// <summary>
    /// 字段名称
    /// </summary>
    public string ItemName { get; set; }
    /// <summary>
    /// 字段值
    /// </summary>
    public string ItemValue { get; set; }
    /// <summary>
    /// 比较符
    /// </summary>
    public RelationEnum Relation { get; set; }

    /// <summary>
    /// 构造函数,
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="itemValue"></param>
    /// <param name="relation"></param>
    public ConditionItem(string itemName, string itemValue, RelationEnum relation)
    {
        this.ItemName = itemName;
        this.ItemValue = itemValue;
        this.Relation = relation;
    }
}
