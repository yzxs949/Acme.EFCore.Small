using Acme.EFCore.Small.Enums;
using Acme.EFCore.Small.Page;
using Acme.EFCore.Small.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.EFCore.Small.Extensions
{
    /// <summary>
    /// Linq拓展类
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        /// 根据条件决定是否应用查询过滤（IQueryable）
        /// </summary>
        public static IQueryable<TEntity> WhereIf<TEntity>(
            this IQueryable<TEntity> source,
            bool condition,
            Expression<Func<TEntity, bool>> whereLambda)
            => condition ? source.Where(whereLambda) : source;

        /// <summary>
        /// 根据条件决定是否应用查询过滤（IEnumerable）
        /// </summary>
        public static IEnumerable<TEntity> WhereIf<TEntity>(
            this IEnumerable<TEntity> source,
            bool condition,
            Func<TEntity, bool> whereLambda)
            => condition ? source.Where(whereLambda) : source;

        /// <summary>
        /// 分页（排序后使用）- 同步
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <param name="source">分页数据</param>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <returns>分页结果</returns>
        public static PageList<TEntity> ToPageList<TEntity>(
           this IQueryable<TEntity> source,
           int pageIndex,
           int pageSize)
        {
            int total = source.Count();
            var rows = new List<TEntity>();
            if (total > 0)
                rows = source.Skip((pageIndex > 0 ? pageIndex - 1 : 0) * pageSize).Take(pageSize).ToList();
            return new PageList<TEntity>(total, rows, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页（排序后使用）- 异步
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <param name="source">分页数据</param>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <returns>分页结果</returns>
        public static async Task<PageList<TEntity>> ToPageListAsync<TEntity>(
           this IQueryable<TEntity> source,
           int pageIndex,
           int pageSize)
        {
            int total = await source.CountAsync();
            var rows = new List<TEntity>();
            if (total > 0)
                rows = await source.Skip((pageIndex > 0 ? pageIndex - 1 : 0) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<TEntity>(total, rows, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取集合中指定字段的唯一值列表（IQueryable）
        /// </summary>
        public static IEnumerable<TKey> SelectDistinct<TEntity, TKey>(this IQueryable<TEntity> items, Func<TEntity, TKey> keySelector)
            => items.GroupBy(keySelector).Select(g => g.Key);

        /// <summary>
        /// 获取集合中指定字段的唯一值列表（IEnumerable）
        /// </summary>
        public static IEnumerable<TKey> SelectDistinct<TEntity, TKey>(this IEnumerable<TEntity> items, Func<TEntity, TKey> keySelector)
            => items.GroupBy(keySelector).Select(g => g.Key);

        /// <summary>
        /// 根据条件集合对查询进行过滤
        /// </summary>
        public static IQueryable<TEntity> AddConditions<TEntity>(
            this IQueryable<TEntity> query,
            List<Condition> conditions)
        {
            if (conditions == null || conditions.Count == 0)
                return query;

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            foreach (var condition in conditions)
            {
                var property = Expression.Property(parameter, condition.Field);
                var propertyType = property.Type;

                object convertedValue;
                try
                {
                    convertedValue = Convert.ChangeType(condition.Value, propertyType);
                }
                catch
                {
                    continue;
                }

                var right = Expression.Constant(convertedValue, propertyType);

                Expression conditionExpression = condition.Symbol switch
                {
                    Symbol.Equal => Expression.Equal(property, right),
                    Symbol.NotEqual => Expression.NotEqual(property, right),
                    Symbol.GreaterThan => Expression.GreaterThan(property, right),
                    Symbol.LessThan => Expression.LessThan(property, right),
                    Symbol.GreaterThanOrEqual => Expression.GreaterThanOrEqual(property, right),
                    Symbol.LessThanOrEqual => Expression.LessThanOrEqual(property, right),
                    Symbol.Contains when propertyType == typeof(string) =>
                        Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right),
                    Symbol.NotContains when propertyType == typeof(string) =>
                        Expression.Not(Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right)),
                    _ => throw new ArgumentException($"不支持的运算符或类型: {condition.Symbol}")
                };

                var lambda = Expression.Lambda<Func<TEntity, bool>>(conditionExpression, parameter);
                query = query.Where(lambda);
            }
            return query;
        }

        /// <summary>
        /// 根据条件判断是否对查询进行过滤
        /// </summary>
        public static IQueryable<TEntity> AddConditionsIf<TEntity>(this IQueryable<TEntity> query, bool isAdd, List<Condition> conditions)
            => isAdd ? query.AddConditions(conditions) : query;

        /// <summary>
        /// 根据排序参数对查询进行排序
        /// </summary>
        public static IQueryable<TEntity> AddSorting<TEntity>(this IQueryable<TEntity> query, Sorting sorting)
        {
            if (sorting == null)
            {
                var idProperty = typeof(TEntity).GetProperty("Id")
                    ?? throw new InvalidOperationException($"实体 {typeof(TEntity).Name} 中不存在 Id 字段");
                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var property = Expression.Property(parameter, idProperty);
                var lambda = Expression.Lambda(property, parameter);
                var methodName = "OrderBy";
                var resultExpression = Expression.Call(
                    typeof(Queryable), methodName,
                    new[] { typeof(TEntity), idProperty.PropertyType },
                    query.Expression, lambda);
                return query.Provider.CreateQuery<TEntity>(resultExpression);
            }
            else
            {
                var propertyInfo = typeof(TEntity).GetProperty(sorting.SortField)
                    ?? throw new ArgumentException($"实体 {typeof(TEntity).Name} 不存在 {sorting.SortField} 字段");
                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var property = Expression.Property(parameter, propertyInfo);
                var lambda = Expression.Lambda(property, parameter);
                var methodName = sorting.SortingType == SortingType.ASC ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(
                    typeof(Queryable), methodName,
                    new[] { typeof(TEntity), propertyInfo.PropertyType },
                    query.Expression, lambda);
                return query.Provider.CreateQuery<TEntity>(resultExpression);
            }
        }

        /// <summary>
        /// 根据条件判断是否对查询进行排序
        /// </summary>
        public static IQueryable<TEntity> AddSortingIf<TEntity>(this IQueryable<TEntity> query, bool isAdd, Sorting sorting)
            => isAdd ? query.AddSorting(sorting) : query;

        /// <summary>
        /// 添加关键字模糊查询条件
        /// </summary>
        public static IQueryable<TEntity> AddConditionsContains<TEntity>(
            this IQueryable<TEntity> query,
            Keywords keywords)
        {
            if (keywords == null || keywords.Fields == null || keywords.Fields.Length == 0 || string.IsNullOrEmpty(keywords.Value))
                return query;

            var param = Expression.Parameter(typeof(TEntity), "s");
            Expression body = null;
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            foreach (var key in keywords.Fields)
            {
                var property = typeof(TEntity).GetProperty(key);
                if (property != null && property.PropertyType == typeof(string))
                {
                    var propertyAccess = Expression.MakeMemberAccess(param, property);
                    var constant = Expression.Constant(keywords.Value, typeof(string));
                    var containsCall = Expression.Call(propertyAccess, containsMethod, constant);
                    if (body == null) body = containsCall;
                    else body = Expression.OrElse(body, containsCall);
                }
            }

            if (body == null)
                return query;

            return query.Where(Expression.Lambda<Func<TEntity, bool>>(body, param));
        }

        /// <summary>
        /// 根据条件判断是否对查询添加关键字模糊查询条件
        /// </summary>
        public static IQueryable<TEntity> AddConditionsContains<TEntity>(
            this IQueryable<TEntity> query,
            bool isAdd,
            Keywords keywords)
            => isAdd ? query.AddConditionsContains(keywords) : query;
    }
}
