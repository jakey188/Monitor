using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using Topshelf;
using Monitor.Tasks.Quartzs;

namespace Monitor.Tasks.Windows.Server
{
    public class TaskManagerServer : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            QuartzManager.StartScheduler();
            return true;
        }
         
        public bool Stop(HostControl hostControl)
        {
            QuartzManager.StopSchedule();
            return true;
        }
    }
}
