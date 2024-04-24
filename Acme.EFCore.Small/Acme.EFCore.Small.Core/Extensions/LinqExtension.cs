using Acme.EFCore.Small.Page;
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
        {
            return verification ? source.Where(anyLambda) : source;
        }

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
        {
            return verification ? source.Where(anyLambda) : source;
        }

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
        {
            return verification ? source.Where(anyLambda) : source;
        }

        /// <summary>
        /// 分页（排序后使用）
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="source">分页数据</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <returns></returns>
        public static PageList<T> ToPageList<T>(
           this IQueryable<T> source,
           int pageIndex,
           int pageSize)
        {
            int total = source.Count();
            var rows = new List<T>();
            if (total > 0)
                rows = source.Skip((pageIndex > 0 ? pageIndex - 1 : 0) * pageSize).Take(pageSize).ToList();
            var data = rows.ToPageList<T>(total, pageIndex, pageSize);
            return data;
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
            var data = rows.ToPageList<T>(total, pageIndex, pageSize);
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
        {
            return items.GroupBy(keySelector).Select(g => g.Key);
        }

        /// <summary>
        /// 集合中获取指定字段的唯一值列表。
        /// </summary>
        /// <typeparam name="T">集合中的元素类型。</typeparam>
        /// <typeparam name="TKey">字段的类型。</typeparam>
        /// <param name="items">要获取字段值的 IEnumerable&lt;T&gt; 集合。</param>
        /// <param name="keySelector">用于从元素中提取字段值的函数。</param>
        /// <returns>字段值的列表。</returns>
        public static IEnumerable<TKey> GetKeyList<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
        {
            return items.GroupBy(keySelector).Select(g => g.Key);
        }
    }
}
