using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monitor.Core.SystemInfos;
using Monitor.Windows.Server.Services;
using NLog;
using Topshelf;

namespace Monitor.Windows.Server
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        static void Main(string[] args)
        {
            var os = new SystemInfo();
            if (MonitorTest.Run(os))
            {
                HostFactory.Run(hc =>
                {
                    hc.Service<PerfmonWorker>();
                    hc.RunAsLocalSystem();
                    hc.SetServiceName(typeof(PerfmonWorker).Namespace);
                    hc.SetDisplayName("Monitor.Windows.Server");
                    hc.SetDescription("服务器性能监视器,对服务器进行一些数据采集,并上报给监控中心");
                });
            }
        }
    }
}
