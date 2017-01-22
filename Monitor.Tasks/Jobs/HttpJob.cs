using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Monitor.Core;
using Monitor.Models.Entites;
using Monitor.Services;
using Quartz;

namespace Monitor.Tasks.Jobs
{
    public class HttpJob : JobBase, IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ExecuteJob(context, () => { });
        }
    }
}
