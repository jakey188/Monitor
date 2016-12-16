using System;
using System.Collections.Generic;
using System.Linq;
namespace Monitor.Core
{
    /// <summary>
    /// IQueryable 扩展
    /// </summary>
    public static class QueryableExpressions
    {
        /// <summary>
        /// 查询分页结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pgIndex"></param>
        /// <param name="pgSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static List<T> ToPageList<T>(this IQueryable<T> source, int pgIndex, int pgSize, out int total) where T : class {
            total = 0;
            if (pgSize > 0) {
                total = source.Count();
                if (pgIndex < 1)
                    pgIndex = 1;
                source = source.Skip((pgIndex - 1) * pgSize).Take(pgSize);
            }
            return source.ToList();
        }

        /// <summary>
        /// 结果集内存分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pgIndex"></param>
        /// <param name="pgSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToPageList<T>(this IEnumerable<T> source, int pgIndex, int pgSize, out int total) where T : class {
            total = 0;
            if (pgSize > 0) {
                total = source.Count();
                if (pgIndex < 1)
                    pgIndex = 1;
                source = source.Skip((pgIndex - 1) * pgSize).Take(pgSize);
            }
            return source;
        }
    }
}
