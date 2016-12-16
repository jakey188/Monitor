using System.Web.Mvc;

namespace Monitor.Web.Core
{
    /// <summary>
    /// HtmlHelper拓展
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 获取当前Action
        /// </summary>
        /// <param name="htmlHelper">当前HtmlHelper</param>
        /// <returns>返回Action字符串</returns>
        public static string GetAction(this HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.RouteData.Values["action"].ToString();
        }

        /// <summary>
        /// 获取当前Controller
        /// </summary>
        /// <param name="htmlHelper">当前HtmlHelper</param>
        /// <returns>返回Controller字符串</returns>
        public static string GetController(this HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.RouteData.Values["controller"].ToString();
        }
    }
}
