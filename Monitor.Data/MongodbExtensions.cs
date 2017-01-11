using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Monitor.Data
{
    public static class MongodbExtensions
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
        public static List<T> ToPageList<T>(this IFindFluent<T,T> find,int pgIndex,int pgSize) where T : class
        {
            if (pgSize > 0)
            {
                if (pgIndex < 1)
                    pgIndex = 1;
                return find.Skip((pgIndex - 1) * pgSize).Limit(pgSize).ToList();
            }
            return find.ToList();
        }
    }
}
