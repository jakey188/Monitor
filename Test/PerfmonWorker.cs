using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Logging;

namespace Test
{
    class PerfmonWorker: ServiceControl
    {
        private readonly LogWriter logger = HostLogger.Get<PerfmonWorker>();

        public bool Start(HostControl hostControl)
        {
            logger.Info("性能监视器开始...");

            WriteSystenInfo();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            logger.Info("性能监视器结束...");

            return true;
        }

        void WriteSystenInfo()
        {
            var info = new Monitor.Core.SystemInfos.SystemInfo();

            Console.WriteLine("OSName：" + info.OSName);
            Console.WriteLine("OSVersion：" + info.OSVersion);
            Console.WriteLine("SystemName：" + info.MachineName);
            Console.WriteLine("地区：" + info.Country);
            Console.WriteLine("CPU名称：" + info.CPUName);
            Console.WriteLine("CPU个数：" + info.CPUCount);
            Console.WriteLine("可用物理内存：" + info.FreePhysicalMemory);
            Console.WriteLine("可用虚拟内存：" + info.FreeVirtualMemory);
            Console.WriteLine("时区：" + info.TimeZone);
            Console.WriteLine("总的虚拟内存：" + info.TotalVirtualMemorySize);
            Console.WriteLine("总的物理内存：" + info.TotalVisibleMemorySize);
        }
    }
}
  
