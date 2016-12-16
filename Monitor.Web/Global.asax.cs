using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Monitor.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 应用程序启动
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
