using Acme.EFCore.Enums;
using System.Collections.Generic;

namespace Acme.EFCore.Querys;

/// <summary>
/// 分页查询参数
/// </summary>
/// <param name="PageIndex">页码</param>
/// <param name="PageSize">每页显示的条数</param>
/// <param name="Conditions">查询条件</param>
/// <param name="Sorting">排序</param>
/// <param name="Keywords">关键字</param>
public record PageQueryParam(int PageIndex,
        int PageSize, List<Condition> Conditions, Sorting Sorting, Keywords Keywords);

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
