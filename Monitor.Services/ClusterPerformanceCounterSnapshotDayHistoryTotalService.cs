using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitor.Data;
using Monitor.Models;

namespace Monitor.Services
{
    public class ClusterPerformanceCounterSnapshotDayHistoryTotalService
    {
        public void AddOrUpdate(string ip)
        {
            var db = new MongoDbContext();
            var date = DateTime.Now;

            var start = new DateTime(date.Year,date.Month,date.Day,0,0,0);
            var end = new DateTime(date.Year,date.Month,date.Day,23,59,59);
            var history = db
                .Where<ClusterPerformanceCounterSnapshotDayHistoryTotal>(x => x.MachineIP == ip && x.CreateTime < end && x.CreateTime > start)
                .FirstOrDefault();

            var dayTotal = db.Where<ClusterPerformanceCounterSnapshot>(x => x.MachineIP == ip && x.CreateTime < end && x.CreateTime > start)
                        .GroupBy(x => x.CreateTime.Day).Select(x => new
                        {
                            CPUAvg = (float?)x.Average(m => m.CPU),
                            CPUMax = (float?)x.Max(m => m.CPU),
                            IISAvg = (float?)x.Average(m => m.IIS),
                            IISMax = (float?)x.Max(m => m.IIS),
                            MemoryAvg = (float?)x.Average(m => m.Memory),
                            MemoryMax = (float?)x.Max(m => m.Memory),
                        }).FirstOrDefault();

            if (dayTotal != null)
            {
                var model = new ClusterPerformanceCounterSnapshotDayHistoryTotal();
                model.UpdateTime = DateTime.Now;
                model.CPUAvg = dayTotal.CPUAvg;
                model.CPUMax = dayTotal.CPUMax;
                model.IISAvg = dayTotal.IISAvg;
                model.IISMax = dayTotal.IISMax;
                model.MemoryAvg = dayTotal.MemoryAvg;
                model.MemoryMax = dayTotal.MemoryMax;
                if (history == null)
                {
                    model.CreateTime = DateTime.Now;
                    model.MachineIP = ip;
                    db.Add(model);
                }
                else
                {
                    model.Id = history.Id;
                    model.MachineIP = history.MachineIP;
                    model.CreateTime = history.CreateTime;
                    db.Update(model);
                }
            }
        }
    }
}
