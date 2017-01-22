using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Monitor.Tasks.Jobs;
using Quartz;

namespace Monitor.Tasks.Jobs
{
    public class WarningJob:JobBase, IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ExecuteJob(context,() =>
            {
            });
        }
    }
}