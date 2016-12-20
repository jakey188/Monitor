using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Diagnostics;
using Monitor.Watch.Data;
using System.Text.RegularExpressions;

namespace Monitor.Watch
{
    /// <summary>
    /// 请求执行时间处理模块
    /// </summary>
    public class HttpRequestTimerModule:IHttpModule
    {
        private Stopwatch stopwatch;

        /// <summary>
        /// IHttpModule初始化
        /// </summary>
        /// <param name="context"></param>
        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(BeginRequest);
            context.EndRequest += new EventHandler(EndRequest);
        }

        /// <summary>
        /// 请求开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginRequest(object sender,EventArgs e)
        {
            stopwatch = new Stopwatch();
            stopwatch.Restart();
        }

        /// <summary>
        /// 请求结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndRequest(object sender,EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            stopwatch.Stop();

            if (context.Response.StatusCode == 200 && IsEnableCounter(context))
            {
                var time = stopwatch.ElapsedMilliseconds;
                var watch = new ClusterHttpWatch();
                watch.CreateDate = DateTime.Now;
                watch.Ip = GetIP();
                watch.Referer = context.Request.Url.ToString().ToLower();
                watch.Timevalue = time;
                HttpRequestWatch.WatchQueue.Enqueue(watch);
            }
        }

        /// <summary>
        /// 是否启用计数器
        /// </summary>
        /// <param name="context">当前线程上下文</param>
        /// <returns></returns>
        private bool IsEnableCounter(HttpContext context)
        {
            var contentType = context.Response.ContentType;
            if (contentType.Contains("text/html") || contentType.Contains("application/json"))
                return true;
            return false;
        }

        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <returns></returns>
        private string GetIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (null == result || result == String.Empty || !IsIP(result))
            {
                return "0.0.0.0";
            }

            return result;
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private  bool IsIP(string ip)
        {
            return Regex.IsMatch(ip,@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }

        void IHttpModule.Dispose()
        {

        }
    }
}
