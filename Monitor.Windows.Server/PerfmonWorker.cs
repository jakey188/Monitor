using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Logging;

namespace Monitor.Windows.Server
{
    class PerfmonWorker: ServiceControl
    {
        private readonly LogWriter logger = HostLogger.Get<PerfmonWorker>();
        public static bool ShouldStop { get; private set; }
        private ManualResetEvent stopHandle;

        public bool Start(HostControl hostControl)
        {
            logger.Info("性能监视器开始...");

            stopHandle = new ManualResetEvent(false);

            ThreadPool.QueueUserWorkItem(new ServiceMonitor().Monitor, stopHandle);

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            ShouldStop = true;
            logger.Info("性能监视器结束...");
            // wait for all threads to finish
            stopHandle.WaitOne(GlobalConfigure.PerformanceCollectBackgroundTaskSleepTime + 10);

            return true;
        }
    }
  
}
