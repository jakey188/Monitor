using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Monitor.Tasks.Quartzs;
using Monitor.Web.Core;
using Monitor.Web.Core.Filter;
using Monitor.Web.Core.Jobs;

namespace Monitor.Web
{
    public class MvcApplication:System.Web.HttpApplication
    {
        JobScheduler _scheduler = new JobScheduler();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalFilters.Filters.Add(new HandleErrorLogAttribute());
            //_scheduler.Start();
            //QuartzManager.StartScheduler();
        }

        protected void Application_End()
        {
            //_scheduler.Stop();
            QuartzManager.StopSchedule();
        }
    }
}
