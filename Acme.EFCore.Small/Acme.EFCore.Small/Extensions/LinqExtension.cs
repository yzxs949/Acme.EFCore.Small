using Acme.EFCore.Small.Enums;
using Acme.EFCore.Small.Page;
using Acme.EFCore.Small.Querys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.EFCore.Small.Extensions
{

    /// <summary>
    /// Linq拓展类
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        /// Linq验证查询方法拓展
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="verification">验证语句</param>
        /// <param name="anyLambda">Linq语句</param>
        /// <returns></returns>
        public static IQueryable<TEntity> WhereIf<TEntity>(
            this IQueryable<TEntity> source,
            bool verification,
            Expression<Func<TEntity, bool>> anyLambda)
            => verification ? source.Where(anyLambda) : source;

        /// <summary>
        /// Linq验证查询方法拓展
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="anyLambda">Linq语句</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Where<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> anyLambda)
            where TEntity : class
            => dbContext.Set<TEntity>().Where(anyLambda);

        /// <summary>
        /// Linq验证查询方法拓展
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="source">IEnumerable</param>
        /// <param name="verification">验证语句</param>
        /// <param name="anyLambda">Linq语句</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> WhereIf<TEntity>(
            this IEnumerable<TEntity> source,
            bool verification,
            Func<TEntity, int, bool> anyLambda)
        => verification ? source.Where(anyLambda) : source;

        /// <summary>
        /// Linq验证查询方法拓展
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="verification"></param>
        /// <param name="anyLambda"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> WhereIf<TEntity>(
            this IEnumerable<TEntity> source,
            bool verification,
            Func<TEntity, bool> anyLambda)
        => verification ? source.Where(anyLambda) : source;

        /// <summary>
        /// 分页（排序后使用）
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <param name="source">分页数据</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <returns></returns>
        public static PageList<TEntity> ToPageList<TEntity>(
           this IQueryable<TEntity> source,
           int pageIndex,
           int pageSize)
        {
            int total = source.Count();
            var rows = new List<TEntity>();
            if (total > 0)
                rows = source.Skip((pageIndex > 0 ? pageIndex - 1 : 0) * pageSize).Take(pageSize).ToList();
            return new PageList<TEntity>(total, rows);
        }

        /// <summary>
        /// 分页（排序后使用）
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <param name="source">分页数据</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async static Task<PageList<TEntity>> ToPageListAsync<TEntity>(
           this IQueryable<TEntity> source,
           int pageIndex,
           int pageSize,
           CancellationToken cancellationToken = default)
        {
            int total = await source.CountAsync(cancellationToken);
            var rows = new List<TEntity>();
            if (total > 0)
                rows = await source.Skip((pageIndex > 0 ? pageIndex - 1 : 0) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            var data = new PageList<TEntity>(total, rows);
            return data;
        }

        /// <summary>
        /// 集合中获取指定字段的唯一值列表
        /// </summary>
        /// <typeparam name="TEntity">集合中的元素类型</typeparam>
        /// <typeparam name="TKey">字段的类型</typeparam>
        /// <param name="items">要获取字段值的 IQueryable&lt;T&gt; 集合</param>
        /// <param name="keySelector">用于从元素中提取字段值的函数</param>
        /// <returns>字段值的列表</returns>
        public static IEnumerable<TKey> GetKey<TEntity, TKey>(
            this IQueryable<TEntity> items,
            Func<TEntity, TKey> keySelector)
            => items.GroupBy(keySelector).Select(g => g.Key);

        /// <summary>
        /// 集合中获取指定字段的唯一值列表
        /// </summary>
        /// <typeparam name="TEntity">集合中的元素类型</typeparam>
        /// <typeparam name="TKey">字段的类型</typeparam>
        /// <param name="items">要获取字段值的 IEnumerable&lt;T&gt; 集合</param>
        /// <param name="keySelector">用于从元素中提取字段值的函数</param>
        /// <returns>字段值的列表</returns>
        public static IEnumerable<TKey> GetKeyList<TEntity, TKey>(
            this IEnumerable<TEntity> items,
            Func<TEntity, TKey> keySelector)
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
            Expression finalExpr = null;

            foreach (var condition in conditions)
            {
                if (string.IsNullOrWhiteSpace(condition.Field))
                    continue;

                // 左值：x.Field
                MemberExpression left = Expression.Property(parameter, condition.Field);
                Expression right;

                // 处理不需要值的运算符
                if (condition.Symbol == Symbol.IsNull || condition.Symbol == Symbol.IsNotNull)
                {
                    right = Expression.Constant(null, left.Type);
                }
                else
                {
                    if (condition.Value == null)
                        continue;

                    // In/NotIn：Value 为逗号分隔的值列表字符串，如 "1,2,3"
                    if (condition.Symbol == Symbol.In || condition.Symbol == Symbol.NotIn)
                    {
                        if (string.IsNullOrWhiteSpace(condition.Value))
                            continue;

                        right = GenerateListConstant(condition.Value, left.Type);
                    }
                    else
                    {
                        right = GenerateConstant(condition.Value, left.Type);
                    }
                }

                // 生成条件表达式
                Expression conditionExpr = condition.Symbol switch
                {
                    Symbol.Equal => Expression.Equal(left, right),
                    Symbol.NotEqual => Expression.NotEqual(left, right),
                    Symbol.GreaterThan => Expression.GreaterThan(left, right),
                    Symbol.LessThan => Expression.LessThan(left, right),
                    Symbol.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
                    Symbol.LessThanOrEqual => Expression.LessThanOrEqual(left, right),

                    // 字符串模糊
                    Symbol.Contains => GenerateLikeMethod(left, "Contains", right),
                    Symbol.NotContains => Expression.Not(GenerateLikeMethod(left, "Contains", right)),
                    Symbol.StartsWith => GenerateLikeMethod(left, "StartsWith", right),
                    Symbol.EndsWith => GenerateLikeMethod(left, "EndsWith", right),

                    // 集合包含
                    Symbol.In => GenerateInMethod(left, right),
                    Symbol.NotIn => Expression.Not(GenerateInMethod(left, right)),

                    // 空判断
                    Symbol.IsNull => Expression.Equal(left, right),
                    Symbol.IsNotNull => Expression.NotEqual(left, right),

                    _ => throw new ArgumentException($"不支持的运算符: {condition.Symbol}")
                };

                // 拼接多个条件（AND）
                finalExpr = finalExpr == null
                    ? conditionExpr
                    : Expression.AndAlso(finalExpr, conditionExpr);
            }

            if (finalExpr != null)
            {
                var lambda = Expression.Lambda<Func<TEntity, bool>>(finalExpr, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        /// <summary>
        /// 生成模糊查询方法
        /// </summary>
        private static MethodCallExpression GenerateLikeMethod(
            Expression left,
            string methodName,
            Expression value)
        {
            MethodInfo method = typeof(string).GetMethod(methodName, new[] { typeof(string) })!;
            return Expression.Call(left, method, value);
        }

        /// <summary>
        /// 生成 In 查询
        /// </summary>
        private static MethodCallExpression GenerateInMethod(
            MemberExpression left,
            Expression value)
        {
            // 支持 value 是 List/数组
            var method = typeof(Enumerable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .MakeGenericMethod(left.Type);

            return Expression.Call(null, method, value, left);
        }

        /// <summary>
        /// 将字符串值转换为目标类型的常量表达式（支持 Nullable、枚举）
        /// </summary>
        /// <param name="rawValue">原始字符串值</param>
        /// <param name="targetType">目标类型</param>
        /// <returns>常量表达式</returns>
        private static Expression GenerateConstant(string rawValue, Type targetType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                // Nullable<T>：先转换到底层类型，再用 Convert 包装以匹配 left 的类型
                object converted = ConvertConditionValue(rawValue, underlyingType);
                return Expression.Convert(Expression.Constant(converted, underlyingType), targetType);
            }
            return Expression.Constant(ConvertConditionValue(rawValue, targetType), targetType);
        }

        /// <summary>
        /// 将逗号分隔的值字符串拆分为目标类型列表的常量表达式（用于 In/NotIn）
        /// </summary>
        /// <param name="rawValue">逗号分隔的值列表字符串，如 "1,2,3"</param>
        /// <param name="elementType">元素类型</param>
        /// <returns>List&lt;T&gt; 常量表达式</returns>
        private static Expression GenerateListConstant(string rawValue, Type elementType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(elementType) ?? elementType;
            Type listType = typeof(List<>).MakeGenericType(elementType);
            var list = (System.Collections.IList)Activator.CreateInstance(listType)!;

            foreach (string item in rawValue.Split(','))
            {
                string trimmed = item.Trim();
                if (trimmed.Length == 0)
                    continue;
                list.Add(ConvertConditionValue(trimmed, underlyingType));
            }

            return Expression.Constant(list, listType);
        }

        /// <summary>
        /// 将字符串值转换为目标类型（支持枚举），使用不变量区域性避免本地化差异
        /// </summary>
        /// <param name="rawValue">原始字符串值</param>
        /// <param name="targetType">目标类型</param>
        /// <returns>转换后的值</returns>
        private static object ConvertConditionValue(string rawValue, Type targetType)
        {
            if (targetType.IsEnum)
                return Enum.Parse(targetType, rawValue, ignoreCase: true);
            return Convert.ChangeType(rawValue, targetType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 根据条件判断是否对查询进行过滤
        /// </summary>
        /// <typeparam name="TEntity">查询的实体类型</typeparam>
        /// <param name="query">要过滤的查询</param>
        /// <param name="isAdd">是否应用条件过滤</param>
        /// <param name="conditions">条件集合，每个条件包含字段名、运算符和值</param>
        /// <returns>如果 isAdd 为 true，则返回应用了条件过滤后的查询；否则返回原始查询</returns>
        public static IQueryable<TEntity> AddConditionsIf<TEntity>(
            this IQueryable<TEntity> query,
            bool isAdd, List<Condition> conditions)
        {
            return isAdd ? query.AddConditions(conditions) : query;
        }

        /// <summary>
        /// 根据排序参数对查询进行排序
        /// </summary>
        /// <typeparam name="TEntity">查询的实体类型</typeparam>
        /// <param name="query">要排序的查询</param>
        /// <param name="sorting">排序参数，包含排序字段和排序类型</param>
        /// <returns>应用了排序后的查询</returns>
        /// <exception cref="InvalidOperationException">当实体类型 T 中不存在默认排序字段 'Id' 时抛出</exception>
        /// <exception cref="ArgumentException">当实体类型 T 中不存在指定的排序字段时抛出</exception>
        public static IQueryable<TEntity> AddSorting<TEntity>(
            this IQueryable<TEntity> query,
            Sorting sorting)
        {
            if (sorting == null)
            {
                var idProperty = typeof(TEntity).GetProperty("Id")
                    ?? throw new InvalidOperationException($"实体{typeof(TEntity).Name}中不存在Id字段");
                ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
                MemberExpression property = Expression.Property(parameter, idProperty);
                LambdaExpression lambda = Expression.Lambda(property, parameter);
                string methodName = "OrderBy";
                var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(TEntity), idProperty.PropertyType },
                query.Expression,
                lambda);
                return query.Provider.CreateQuery<TEntity>(resultExpression);
            }
            else
            {
                var propertyInfo = typeof(TEntity).GetProperty(sorting.SortField)
                    ?? throw new ArgumentException($"实体{typeof(TEntity).Name}不存在{sorting.SortField}字段");
                ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
                MemberExpression property = Expression.Property(parameter, propertyInfo);
                LambdaExpression lambda = Expression.Lambda(property, parameter);
                string methodName = sorting.SortingType == SortingType.ASC ? "OrderBy" : "OrderByDescending";
                MethodCallExpression resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(TEntity), propertyInfo.PropertyType },
                query.Expression,
                lambda);
                return query.Provider.CreateQuery<TEntity>(resultExpression);
            }
        }

        /// <summary>
        /// 根据条件判断是否对查询进行排序
        /// </summary>
        /// <typeparam name="TEntity">查询的实体类型</typeparam>
        /// <param name="query">要排序的查询</param>
        /// <param name="isAdd">是否应用排序</param>
        /// <param name="sorting">排序参数，包含排序字段和排序类型</param>
        /// <returns>如果 isAdd 为 true，则返回应用了排序后的查询；否则返回原始查询</returns>
        public static IQueryable<TEntity> AddSortingIf<TEntity>(
            this IQueryable<TEntity> query,
            bool isAdd,
            Sorting sorting)
        {
            return isAdd ? query.AddSorting(sorting) : query;
        }

        /// <summary>
        /// 添加关键字模糊查询条件
        /// </summary>
        /// <typeparam name="TEntity">查询的实体类型</typeparam>
        /// <param name="query">要过滤的查询</param>
        /// <param name="keywords">关键字参数，包含字段集合和关键字值</param>
        /// <returns>应用了模糊查询条件后的查询</returns>
        public static IQueryable<TEntity> AddConditionsContains<TEntity>(
            this IQueryable<TEntity> query,
            Keywords keywords)
        {
            if (keywords == null
                || keywords.Fields == null
                || keywords.Fields.Length == 0
                || string.IsNullOrEmpty(keywords.Value))
            {
                return query;
            }
            var types = new Type[] { typeof(string) };
            var param = Expression.Parameter(typeof(TEntity), "s");
            Expression body = null;
            var containsMethod = typeof(string).GetMethod("Contains", types);
            foreach (var key in keywords.Fields)
            {
                var property = typeof(TEntity).GetProperty(key);
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
            return query.Where(Expression.Lambda<Func<TEntity, bool>>(body, param));
        }

        /// <summary>
        /// 根据条件判断是否对查询添加关键字模糊查询条件
        /// </summary>
        /// <typeparam name="TEntity">查询的实体类型</typeparam>
        /// <param name="query">要过滤的查询</param>
        /// <param name="isAdd">是否应用模糊查询条件</param>
        /// <param name="keywords">关键字参数，包含字段集合和关键字值</param>
        /// <returns>如果 isAdd 为 true，则返回应用了模糊查询条件后的查询；否则返回原始查询</returns>
        public static IQueryable<TEntity> AddConditionsContains<TEntity>(
            this IQueryable<TEntity> query,
            bool isAdd,
            Keywords keywords)
        {
            return isAdd ? query.AddConditionsContains(keywords) : query;
        }
    }
}
