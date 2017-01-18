using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Quartz;

namespace Monitor.Web.Core.Jobs
{
    public class TestJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string jobKey = context.Trigger.Key.Name;
            Debug.WriteLine("执行1"+ jobKey);
        }
    }
}