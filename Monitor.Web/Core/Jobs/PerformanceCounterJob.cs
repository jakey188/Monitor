using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Monitor.Services;
using Quartz;

namespace Monitor.Web.Core.Jobs
{
    public class PerformanceCounterJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var _clusterComputerInfoServices = new ClusterComputerInfoServices();
            var _clusterPerformanceCounterSnapshotDayHistoryTotalService = new ClusterPerformanceCounterSnapshotDayHistoryTotalService();
            var computerList = _clusterComputerInfoServices.GetClusterComputerInfo();

            foreach (var computer in computerList)
            {
                _clusterPerformanceCounterSnapshotDayHistoryTotalService.AddOrUpdate(computer.MachineIp);
            }
        }
    }
}