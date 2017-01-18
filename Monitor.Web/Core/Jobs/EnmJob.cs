using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Monitor.Web.Core.Jobs
{
    public enum EnmJob
    {
        [Description("性能计数器统计任务")]
        PerformanceCounterJob
    }
}