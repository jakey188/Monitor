using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Monitor.Tasks.Windows.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hc =>
            {
                hc.Service<TaskManagerServer>();
                hc.RunAsLocalSystem();
                hc.SetServiceName(typeof(TaskManagerServer).Namespace);
                hc.SetDisplayName("Monitor.Tasks.Windows.Server");
                hc.SetDescription("任务调度中心执行的任务");
            });
            Console.ReadKey();
        }
    }
}
