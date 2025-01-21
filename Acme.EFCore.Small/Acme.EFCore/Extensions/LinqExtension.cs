using Acme.EFCore.Extensions;
using Acme.EFCore.Page;
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
    /// 条件表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="key">字段名</param>
    /// <param name="symbol">运算符号</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static IQueryable<T> AddConditions<T>(
        this IQueryable<T> query,
        string key,
        string symbol,
        string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var right = Expression.Constant(Convert.ChangeType(value, Expression.Property(parameter, key).Type));
        var conditionExpression = symbol switch
        {
            "==" => Expression.Equal(Expression.Property(parameter, key), right),
            "!=" => Expression.NotEqual(Expression.Property(parameter, key), right),
            ">" => Expression.GreaterThan(Expression.Property(parameter, key), right),
            "<" => Expression.LessThan(Expression.Property(parameter, key), right),
            ">=" => Expression.GreaterThanOrEqual(Expression.Property(parameter, key), right),
            "<=" => Expression.LessThanOrEqual(Expression.Property(parameter, key), right),
            _ => throw new ArgumentException("Unsupported symbol")
        };
        var lambda = Expression.Lambda<Func<T, bool>>(conditionExpression, parameter);
        query = query.Where(lambda);
        return query;
    }

    /// <summary>
    /// 添加关键字模糊查询条件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="keys">字段集合</param>
    /// <param name="value">关键字值</param>
    /// <returns></returns>
    public static IQueryable<T> AddConditionsContains<T>(
        this IQueryable<T> query,
        string[] keys,
        string value)
    {
        if (keys == null || keys.Length == 0 || string.IsNullOrEmpty(value))
            return query;
        var types = new Type[] { typeof(string) };
        var param = Expression.Parameter(typeof(T), "s");
        Expression body = null;
        var containsMethod = typeof(string).GetMethod("Contains", types);
        foreach (var key in keys)
        {
            var property = typeof(T).GetProperty(key);
            if (property is not null)
            {
                var propertyAccess = Expression.MakeMemberAccess(param, property);
                var constant = Expression.Constant(value, typeof(string));
                var containsCall = Expression.Call(propertyAccess, containsMethod, constant);
                body = body is null ? containsCall : Expression.OrElse(body, containsCall);
            }
        }
        if (body is null)
            return query;
        return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
    }

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
            rows = source.Skip((pageIndex > 0 ? pageIndex - 1 : 0) * pageSize).Take(pageSize).ToList();
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
    public async static Task<PageList<T>> ToPageListAsync<T>(
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
    /// 集合中获取指定字段的唯一值列表。
    /// </summary>
    /// <typeparam name="T">集合中的元素类型。</typeparam>
    /// <typeparam name="TKey">字段的类型。</typeparam>
    /// <param name="items">要获取字段值的 IQueryable&lt;T&gt; 集合。</param>
    /// <param name="keySelector">用于从元素中提取字段值的函数。</param>
    /// <returns>字段值的列表。</returns>
    public static IEnumerable<TKey> GetKey<T, TKey>(this IQueryable<T> items, Func<T, TKey> keySelector)
        => items.GroupBy(keySelector).Select(g => g.Key);

    /// <summary>
    /// 集合中获取指定字段的唯一值列表。
    /// </summary>
    /// <typeparam name="T">集合中的元素类型。</typeparam>
    /// <typeparam name="TKey">字段的类型。</typeparam>
    /// <param name="items">要获取字段值的 IEnumerable&lt;T&gt; 集合。</param>
    /// <param name="keySelector">用于从元素中提取字段值的函数。</param>
    /// <returns>字段值的列表。</returns>
    public static IEnumerable<TKey> GetKeyList<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
        => items.GroupBy(keySelector).Select(g => g.Key);
}
