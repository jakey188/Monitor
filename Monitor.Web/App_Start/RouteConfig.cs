using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Monitor.Web
{
    /// <summary>
    /// RouteConfig 路由配置
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes">路由集合</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home",action = "Index",id = UrlParameter.Optional });
        }
    }
}
