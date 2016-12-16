using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Topshelf;

namespace Test
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            HostFactory.Run(hc =>
            {
                hc.StartAutomatically();
                hc.RunAsLocalSystem();
                hc.Service<PerfmonWorker>();
                
                hc.SetServiceName(typeof(PerfmonWorker).Namespace);
                hc.SetDisplayName("Monitor.Windows.ServerTest服务器性能监视器");
                hc.SetDescription("对服务器进行一些数据采集,并上报给监控中心");
            });
        }

    }
}
