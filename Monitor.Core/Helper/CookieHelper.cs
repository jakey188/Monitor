using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Monitor.Core
{
    public class CookieHelper
    {
        /// <summary>
        /// 设置Cookie有效期
        /// </summary>
        /// <param name="name"></param>
        /// <param name="day"></param>
        public static void SetCookieTime(string name,int day)
        {
            var httpCookie = HttpContext.Current.Response.Cookies[name];
            if (httpCookie != null)
                httpCookie.Expires = DateTime.Now.AddDays(day);
        }

        /// <summary>
        /// 设置Cookie域名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        public static void SetCookieDomain(string name,string domain)
        {
            var httpCookie = HttpContext.Current.Response.Cookies[name];
            if (httpCookie != null)
                httpCookie.Domain = domain;
        }

        /// <summary>
        /// 设置多子集cookies
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetCookie(string key,string name,string value)
        {
            var httpCookie = HttpContext.Current.Response.Cookies[key];
            if (httpCookie != null)
                httpCookie[name] = value;
            else
                Set(key,name,value);

        }

        /// <summary>
        /// 修改Cookie的值 如果不存在键则创建
        /// </summary>
        /// <param name="cookieKey"></param>
        /// <param name="cookieValue"></param>
        /// <param name="expires">过期时间</param>
        public static void Set(string cookieKey,string cookieValue,DateTime? expires,string cookieDomain = "")
        {
            HttpCookie cookie = new HttpCookie(cookieKey,cookieValue);


            if (!string.IsNullOrEmpty(cookieDomain))
                cookie.Domain = cookieDomain;
            cookie.Path = "/";

            if (expires != null && expires.Value != DateTime.MinValue)
                cookie.Expires = expires.Value;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 修改Cookie的值 如果不存在键则创建 (不设置过期时间)
        /// </summary>
        /// <param name="cookieKey"></param>
        /// <param name="cookieValue"></param>
        public static void Set(string cookieKey,string cookieValue)
        {
            Set(cookieKey,cookieValue,DateTime.MinValue);
        }



        /// <summary>
        /// 设置Cookie (不设置过期时间)
        /// </summary>
        /// <param name="cookieKey"></param>
        /// <param name="itemKey">子项</param>
        /// <param name="cookieValue">子项的值</param>
        public static void Set(string cookieKey,string itemKey,string cookieValue)
        {
            Set(cookieKey,itemKey,cookieValue,null);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieKey">键值</param>
        /// <param name="itemKey">子项</param>
        /// <param name="cookieValue">子项的值</param>
        /// <param name="expires">过期时间</param>
        public static void Set(string cookieKey,string itemKey,string cookieValue,DateTime? expires,string cookieDomain = "")
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[cookieKey];

            cookie[itemKey] = cookieValue;

            if (!string.IsNullOrEmpty(cookieDomain))
                cookie.Domain = cookieDomain;
            cookie.Path = "/";

            if (expires != null)
                cookie.Expires = expires.Value;

        }
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieKey"></param>
        /// <returns></returns>
        public static string Get(string cookieKey)
        {
            //return HttpContext.Current.Request.Cookies[cookieKey].Value;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieKey];
            if (cookie != null)
                return cookie.Value;
            return "";
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieKey"></param>
        /// <returns></returns>
        public static string Get(string cookieKey,string itemKey)
        {
            string value = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieKey];
            if (cookie != null)
            {
                value = cookie.Values[itemKey];
            }
            return value;
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="cookieKey"></param>
        public static void Remove(string cookieKey,string cookieDomain = "")
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieKey];

            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(cookieDomain))
                    cookie.Domain = cookieDomain;

                cookie.Value = "";
                cookie.Expires = DateTime.Now.AddDays(-1);

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}
