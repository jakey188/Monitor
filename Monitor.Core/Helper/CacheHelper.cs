using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace Monitor.Core
{
    public class CacheHelper
    {
        private static readonly object _syncObject = new object();

        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static object Get(string cacheKey)
        {
            var objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }


        public static T Get<T>(string key)
        {
            var objCache = HttpRuntime.Cache;
            return (T)objCache[key];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="objObject"></param>
        public static void Set(string cacheKey, object objObject)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject);
        }

        public static bool IsSet(string key)
        {
            var objCache = HttpRuntime.Cache;
            return objCache[key] != null;
        }

        /// <summary>
        /// 设置缓存（平滑过期）
        /// </summary>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="objObject">缓存value</param>
        /// <param name="timeOut">平滑过期时间：10秒平滑TimeSpan.FromSeconds(10)</param>
        public static void Set(string cacheKey, object objObject, TimeSpan timeOut)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, System.Web.Caching.Cache.NoAbsoluteExpiration, timeOut);
        }

        /// <summary>
        ///  设置缓存(绝对过期)
        /// </summary>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="objObject">缓存value</param>
        /// <param name="absoluteExpiration">绝对过期时间：1天绝对过期,DateTime.Now.AddDays(absoluteExpiration)</param>
        public static void Set(string cacheKey, object objObject, DateTime absoluteExpiration)
        {
            var objCache = HttpRuntime.Cache;

            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 设置缓存(绝对过期),默认1天
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="absoluteExpiration">缓存天数</param>
        public static void Set(string cacheKey, object objObject, int absoluteExpiration = 24)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, DateTime.Now.AddHours(absoluteExpiration),
                System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="absoluteExpiration"></param>
        public static void Refresh(string cacheKey, int absoluteExpiration = 1)
        {
            var objCache = HttpRuntime.Cache;
            object obj = Get(cacheKey);
            Remove(cacheKey);
            objCache.Insert(cacheKey, obj, null, DateTime.Now.AddDays(absoluteExpiration), TimeSpan.Zero);
        }

        /// <summary>
        /// 移除当前应用程序所有的的缓存值
        /// </summary>
        public static void Remove()
        {
            var cache = HttpRuntime.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            var al = new ArrayList();
            while (cacheEnum.MoveNext())
            {
                al.Add(cacheEnum.Key);
            }
            foreach (string key in al)
            {
                cache.Remove(key);
            }
        }

        /// <summary>
        /// 移除当前应用程序中指定的缓存值
        /// </summary>
        /// <param name="cacheKey"></param>
        public static void Remove(string cacheKey)
        {
            var cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDataSet()
        {
            var cache = HttpRuntime.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            var list = new List<string>();
            while (cacheEnum.MoveNext())
            {
                list.Add(cacheEnum.Key.ToString());
            }
            return list;
        }

        /// <summary>
        /// 设置or读取缓存
        /// </summary>
        /// <typeparam name="T">当前对象</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="acquire">结果集</param>
        /// <param name="hour">默认为24小时</param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> acquire, int hour = 24)
        {
            return Get(key, acquire, DateTime.Now.AddHours(hour));
        }

        /// <summary>
        /// 设置or读取缓存
        /// </summary>
        /// <typeparam name="T">当前对象</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="acquire">结果集</param>
        /// <param name="absoluteExpiration">缓存绝对时间</param>
        /// <remarks>如果结果集为null,直接return null,只有结果集为null时不会写入缓存</remarks>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> acquire, DateTime absoluteExpiration)
        {
            lock (_syncObject)
            {
                if (IsSet(key))
                {
                    return Get<T>(key);
                }

                var result = acquire();
                if (result != null)
                    Set(key, result, absoluteExpiration);
                return result;
            }
        }
    }
}
