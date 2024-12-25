using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Acme.EFCore.SearchConditions;

/// <summary>
/// 组合查询条件
/// </summary>
public class SearchCondition
{
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页显示数量</param>
    /// <param name="orderItem">排序字段</param>
    /// <param name="isDescending">是否倒序</param>
    /// <param name="items">查询条件集合</param>
    public SearchCondition(
        int pageIndex,
        int pageSize,
        string orderItem,
        bool isDescending,
        List<ConditionItem> items = null)
    {
        PageIndex = pageIndex > 0 ? pageIndex : 1;
        PageSize = pageSize;
        OrderItem = orderItem;
        IsDescending = isDescending;
        if (items != null)
            Items = items;
    }

    /// <summary>
    /// 当前页码数
    /// </summary>
    public int PageIndex { get; init; }
    /// <summary>
    /// 每页显示数据条数（0为不分页）
    /// </summary>
    public int PageSize { get; init; }
    /// <summary>
    /// 排序字段名称（默认ID）
    /// </summary>
    public string OrderItem { get; init; }
    /// <summary>
    /// 字段信息（查询时用作条件，新增时用作传递需要写入的字段名称和值，修改时同时传递需要修改的字段值及查询条件（有比较符的为查询条件））
    /// </summary>
    public List<ConditionItem> Items { get; private set; } = new();
    /// <summary>
    /// 是否倒序
    /// </summary>
    public bool IsDescending { get; init; } = false;
    /// <summary>
    /// 添加查询条件
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="itemValue"></param>
    /// <param name="relation"></param>
    public void AddCondition(string itemName, string itemValue, RelationEnum relation)
    {
        Items.Add(new ConditionItem(itemName, itemValue, relation));
    }
    /// <summary>
    /// 生产查询条件表达式树
    /// </summary>
    /// <returns></returns>
    public Expression<Func<T, bool>> GetExpression<T>()
    {
        if (Items == null || Items.Count == 0) return null;
        else
        {
            Expression expression = null;
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            List<Expression> list = new();
            foreach (ConditionItem Item in Items)
            {
                expression = expression == null ? SearchCondition.GetCompareExpression<T>(Item, param) : Expression.AndAlso(expression, SearchCondition.GetCompareExpression<T>(Item, param));
            }
            return expression == null ? null : (Expression<Func<T, bool>>)Expression.Lambda(expression, param);
        }
    }

    private static Expression GetCompareExpression<T>(
        ConditionItem condition,
        ParameterExpression param)
    {
        Expression key;
        if (condition.ItemName.Contains('.'))
        {
            key = Expression.Property(param, condition.ItemName.Split('.')[0]);
            key = Expression.Property(key, condition.ItemName.Split('.')[1]);
        }
        else
            key = Expression.Property(param, condition.ItemName);
        PropertyInfo PropertyType = (condition.ItemName.Contains('.') ? typeof(T).GetProperty(condition.ItemName.Split('.')[0])?.PropertyType.GetProperty(condition.ItemName.Split('.')[condition.ItemName.Split('.').Length - 1]) : typeof(T).GetProperty(condition.ItemName)) ?? throw new ArgumentException($"找不到{condition.ItemName}对应的字段");
        Expression value = Expression.Constant(condition.ItemValue);
        if (PropertyType.PropertyType == typeof(Guid))
            value = Expression.Constant(new Guid(condition.ItemValue));
        else
        {
            if (PropertyType.PropertyType.IsEnum)
            {
                value = Expression.Constant(Enum.Parse(PropertyType.PropertyType, condition.ItemValue));
            }
            else
                value = Expression.Constant(Convert.ChangeType(condition.ItemValue, PropertyType.PropertyType));

        }
        Expression binaryItem;
        switch (condition.Relation)
        {
            case RelationEnum.Contains:
                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) }) ?? throw new ArgumentException("该数据类型不支持contains方法");
                binaryItem = Expression.Call(key, method, Expression.Constant(condition.ItemValue));
                break;
            case RelationEnum.Equal:
                binaryItem = Expression.Equal(key, Expression.Convert(value, key.Type));
                break;
            case RelationEnum.Greater:
                binaryItem = Expression.GreaterThan(key, Expression.Convert(value, key.Type));
                break;
            case RelationEnum.GreaterEqual:
                binaryItem = Expression.GreaterThanOrEqual(key, Expression.Convert(value, key.Type));
                break;
            case RelationEnum.Less:
                binaryItem = Expression.LessThan(key, Expression.Convert(value, key.Type));
                break;
            case RelationEnum.LessEqual:
                binaryItem = Expression.LessThanOrEqual(key, Expression.Convert(value, key.Type));
                break;
            case RelationEnum.NotEqual:
                binaryItem = Expression.NotEqual(key, Expression.Convert(value, key.Type));
                break;
            default:
                throw new NotImplementedException("不支持此操作");
        }
        return binaryItem;
    }
}
