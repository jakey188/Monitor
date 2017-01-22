using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Monitor.Tasks.Quartzs;
using Monitor.Web.Core;
using Monitor.Web.Core.Filter;

namespace Monitor.Web
{
    public class MvcApplication:System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalFilters.Filters.Add(new HandleErrorLogAttribute());
            QuartzManager.InitScheduler();
            QuartzManager.InitRemoteScheduler();
        }
    }
}
