using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Monitor.Services;
using Monitor.Tasks.Jobs;
using Quartz;

namespace Monitor.Tasks.Jobs
{
    public class PerformanceCounterJob: JobBase, IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ExecuteJob(context, () =>
            {
                var clusterComputerInfoServices = new ClusterComputerInfoServices();
                var clusterPerformanceCounterSnapshotDayHistoryTotalService = new ClusterPerformanceCounterSnapshotDayHistoryTotalService();
                var computerList = clusterComputerInfoServices.GetClusterComputerInfo();

                foreach (var computer in computerList)
                {
                    clusterPerformanceCounterSnapshotDayHistoryTotalService.AddOrUpdate(computer.MachineIp);
                }
            });
        }
    }
}