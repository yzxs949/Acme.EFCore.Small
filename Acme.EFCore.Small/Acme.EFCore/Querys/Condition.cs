using Acme.EFCore.Enums;

namespace Acme.EFCore.Querys;

/// <summary>
/// 查询条件
/// </summary>
/// <param name="Field">字段</param>
/// <param name="Value">值</param>
/// <param name="Symbol">运算符号</param>
public record Condition(string Field, string Value, Symbol Symbol);

/// <summary>
/// 排序条件
/// </summary>
/// <param name="SortField">排序字段</param>
/// <param name="SortingType">排序类型</param>
public record Sorting(string SortField, SortingType SortingType);

/// <summary>
/// 关键词条件
/// </summary>
/// <param name="Fields">字段名称数组</param>
/// <param name="Value">值</param>
public record Keywords(string[] Fields, string Value);
