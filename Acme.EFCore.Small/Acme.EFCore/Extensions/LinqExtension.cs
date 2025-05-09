using Acme.EFCore.Enums;
using Acme.EFCore.Extensions;
using Acme.EFCore.Page;
using Acme.EFCore.Querys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.EFCore.Extensions;


/// <summary>
/// Linq拓展类
/// </summary>
public static class LinqExtension
{
    /// <summary>
    /// Linq验证查询方法拓展
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="source">IQueryable</param>
    /// <param name="verification">验证语句</param>
    /// <param name="anyLambda">Linq语句</param>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool verification,
        Expression<Func<T, bool>> anyLambda)
        => verification ? source.Where(anyLambda) : source;

    /// <summary>
    /// Linq验证查询方法拓展
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="anyLambda">Linq语句</param>
    /// <returns></returns>
    public static IQueryable<T> Where<T>(this DbContext dbContext, Expression<Func<T, bool>> anyLambda)
        where T : class
        => dbContext.Set<T>().Where(anyLambda);

    /// <summary>
    /// Linq验证查询方法拓展
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="source">IEnumerable</param>
    /// <param name="verification">验证语句</param>
    /// <param name="anyLambda">Linq语句</param>
    /// <returns></returns>
    public static IEnumerable<T> WhereIf<T>(
        this IEnumerable<T> source,
        bool verification,
        Func<T, int, bool> anyLambda)
    => verification ? source.Where(anyLambda) : source;

    /// <summary>
    /// Linq验证查询方法拓展
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="verification"></param>
    /// <param name="anyLambda"></param>
    /// <returns></returns>
    public static IEnumerable<T> WhereIf<T>(
        this IEnumerable<T> source,
        bool verification,
        Func<T, bool> anyLambda)
    => verification ? source.Where(anyLambda) : source;

    /// <summary>
    /// 分页（排序后使用）
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    /// <param name="source">分页数据</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <returns></returns>
    public static IPageList ToPageList<T>(
       this IQueryable<T> source,
       int pageIndex,
       int pageSize)
    {
        int total = source.Count();
        var rows = new List<T>();
        if (total > 0)
            rows = [.. source.Skip((pageIndex > 0 ? pageIndex - 1 : 0) * pageSize).Take(pageSize)];
        return new PageList<T>(total, rows);
    }

    /// <summary>
    /// 分页（排序后使用）
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    /// <param name="source">分页数据</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <returns></returns>
    public async static Task<IPageList> ToPageListAsync<T>(
       this IQueryable<T> source,
       int pageIndex,
       int pageSize)
    {
        int total = await source.CountAsync();
        var rows = new List<T>();
        if (total > 0)
            rows = await source.Skip((pageIndex > 0 ? pageIndex - 1 : 0) * pageSize).Take(pageSize).ToListAsync();
        var data = new PageList<T>(total, rows);
        return data;
    }

    /// <summary>
    /// 集合中获取指定字段的唯一值列表
    /// </summary>
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <typeparam name="TKey">字段的类型</typeparam>
    /// <param name="items">要获取字段值的 IQueryable&lt;T&gt; 集合</param>
    /// <param name="keySelector">用于从元素中提取字段值的函数</param>
    /// <returns>字段值的列表</returns>
    public static IEnumerable<TKey> GetKey<T, TKey>(this IQueryable<T> items, Func<T, TKey> keySelector)
        => items.GroupBy(keySelector).Select(g => g.Key);

    /// <summary>
    /// 集合中获取指定字段的唯一值列表
    /// </summary>
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <typeparam name="TKey">字段的类型</typeparam>
    /// <param name="items">要获取字段值的 IEnumerable&lt;T&gt; 集合</param>
    /// <param name="keySelector">用于从元素中提取字段值的函数</param>
    /// <returns>字段值的列表</returns>
    public static IEnumerable<TKey> GetKeyList<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
        => items.GroupBy(keySelector).Select(g => g.Key);

    /// <summary>
    /// 根据条件集合对查询进行过滤
    /// </summary>
    /// <typeparam name="T">查询的实体类型</typeparam>
    /// <param name="query">要过滤的查询</param>
    /// <param name="conditions">条件集合，每个条件包含字段名、运算符和值</param>
    /// <returns>应用了条件过滤后的查询</returns>
    /// <exception cref="ArgumentException">当条件中的运算符无效时抛出</exception>
    public static IQueryable<T> AddConditions<T>(
        this IQueryable<T> query,
        List<Condition> conditions)
    {
        if (conditions == null)
            return query;
        var parameter = Expression.Parameter(typeof(T), "x");
        foreach (var condition in conditions)
        {
            var right = Expression.Constant(Convert.ChangeType(condition.Value, Expression.Property(parameter, condition.Field).Type));
            var conditionExpression = condition.Symbol switch
            {
                Symbol.Equal => Expression.Equal(Expression.Property(parameter, condition.Field), right),
                Symbol.NotEqual => Expression.NotEqual(Expression.Property(parameter, condition.Field), right),
                Symbol.GreaterThan => Expression.GreaterThan(Expression.Property(parameter, condition.Field), right),
                Symbol.LessThan => Expression.LessThan(Expression.Property(parameter, condition.Field), right),
                Symbol.GreaterThanOrEqual => Expression.GreaterThanOrEqual(Expression.Property(parameter, condition.Field), right),
                Symbol.LessThanOrEqual => Expression.LessThanOrEqual(Expression.Property(parameter, condition.Field), right),
                _ => throw new ArgumentException("无效运算符")
            };
            var lambda = Expression.Lambda<Func<T, bool>>(conditionExpression, parameter);
            query = query.Where(lambda);
        }
        return query;
    }

    /// <summary>
    /// 根据条件判断是否对查询进行过滤
    /// </summary>
    /// <typeparam name="T">查询的实体类型</typeparam>
    /// <param name="query">要过滤的查询</param>
    /// <param name="isAdd">是否应用条件过滤</param>
    /// <param name="conditions">条件集合，每个条件包含字段名、运算符和值</param>
    /// <returns>如果 isAdd 为 true，则返回应用了条件过滤后的查询；否则返回原始查询</returns>
    public static IQueryable<T> AddConditionsIf<T>(this IQueryable<T> query, bool isAdd, List<Condition> conditions)
    {
        return isAdd ? query.AddConditions(conditions) : query;
    }

    /// <summary>
    /// 根据排序参数对查询进行排序
    /// </summary>
    /// <typeparam name="T">查询的实体类型</typeparam>
    /// <param name="query">要排序的查询</param>
    /// <param name="sorting">排序参数，包含排序字段和排序类型</param>
    /// <returns>应用了排序后的查询</returns>
    /// <exception cref="InvalidOperationException">当实体类型 T 中不存在默认排序字段 'Id' 时抛出</exception>
    /// <exception cref="ArgumentException">当实体类型 T 中不存在指定的排序字段时抛出</exception>
    public static IQueryable<T> AddSorting<T>(this IQueryable<T> query, Sorting sorting)
    {
        if (sorting == null)
        {
            var idProperty = typeof(T).GetProperty("Id")
                ?? throw new InvalidOperationException($"实体{typeof(T).Name}中不存在Id字段");
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(parameter, idProperty);
            LambdaExpression lambda = Expression.Lambda(property, parameter);
            string methodName = "OrderBy";
            var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [typeof(T), idProperty.PropertyType],
            query.Expression,
            lambda);
            return query.Provider.CreateQuery<T>(resultExpression);
        }
        else
        {
            var propertyInfo = typeof(T).GetProperty(sorting.SortField)
                ?? throw new ArgumentException($"实体{typeof(T).Name}不存在{sorting.SortField}字段");
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(parameter, propertyInfo);
            LambdaExpression lambda = Expression.Lambda(property, parameter);
            string methodName = sorting.SortingType == SortingType.ASC ? "OrderBy" : "OrderByDescending";
            MethodCallExpression resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [typeof(T), propertyInfo.PropertyType],
            query.Expression,
            lambda);
            return query.Provider.CreateQuery<T>(resultExpression);
        }
    }

    /// <summary>
    /// 根据条件判断是否对查询进行排序
    /// </summary>
    /// <typeparam name="T">查询的实体类型</typeparam>
    /// <param name="query">要排序的查询</param>
    /// <param name="isAdd">是否应用排序</param>
    /// <param name="sorting">排序参数，包含排序字段和排序类型</param>
    /// <returns>如果 isAdd 为 true，则返回应用了排序后的查询；否则返回原始查询</returns>
    public static IQueryable<T> AddSortingIf<T>(this IQueryable<T> query, bool isAdd, Sorting sorting)
    {
        return isAdd ? query.AddSorting(sorting) : query;
    }

    /// <summary>
    /// 添加关键字模糊查询条件
    /// </summary>
    /// <typeparam name="T">查询的实体类型</typeparam>
    /// <param name="query">要过滤的查询</param>
    /// <param name="keywords">关键字参数，包含字段集合和关键字值</param>
    /// <returns>应用了模糊查询条件后的查询</returns>
    public static IQueryable<T> AddConditionsContains<T>(
        this IQueryable<T> query,
        Keywords keywords)
    {
        if (keywords == null || keywords.Fields == null || keywords.Fields.Length == 0 || string.IsNullOrEmpty(keywords.Value))
            return query;
        var types = new Type[] { typeof(string) };
        var param = Expression.Parameter(typeof(T), "s");
        Expression body = null;
        var containsMethod = typeof(string).GetMethod("Contains", types);
        foreach (var key in keywords.Fields)
        {
            var property = typeof(T).GetProperty(key);
            if (property != null)
            {
                var propertyAccess = Expression.MakeMemberAccess(param, property);
                var constant = Expression.Constant(keywords.Value, typeof(string));
                var containsCall = Expression.Call(propertyAccess, containsMethod, constant);
                if (body is null)
                    body = containsCall;
                else
                    body = Expression.OrElse(body, containsCall);
            }
        }
        if (body is null)
            return query;
        return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
    }

    /// <summary>
    /// 根据条件判断是否对查询添加关键字模糊查询条件
    /// </summary>
    /// <typeparam name="T">查询的实体类型</typeparam>
    /// <param name="query">要过滤的查询</param>
    /// <param name="isAdd">是否应用模糊查询条件</param>
    /// <param name="keywords">关键字参数，包含字段集合和关键字值</param>
    /// <returns>如果 isAdd 为 true，则返回应用了模糊查询条件后的查询；否则返回原始查询</returns>
    public static IQueryable<T> AddConditionsContains<T>(
        this IQueryable<T> query,
        bool isAdd,
        Keywords keywords)
    {
        return isAdd ? query.AddConditionsContains(keywords) : query;
    }
}
