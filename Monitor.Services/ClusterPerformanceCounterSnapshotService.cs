using System;
using System.Collections.Generic;
using System.Linq;
using Monitor.Core;
using Monitor.Data;
using Monitor.Models;
using Monitor.Services.Dtos;

namespace Monitor.Services
{
    public class ClusterPerformanceCounterSnapshotService
    {
        public void AddRange(List<ClusterPerformanceCounterSnapshot> list)
        {
            var db = new MongoDbContext();
            db.AddRange(list);
        }

        public void Add(ClusterPerformanceCounterSnapshot clusterPerformanceCounterSnapshot)
        {
            var db = new MongoDbContext();
            db.Add(clusterPerformanceCounterSnapshot);
        }

        public List<ClusterPerformanceCounterSnapshot> ClusterPerformanceCounterSnapshot(string ip,int counterId,int value,DateTime? start,DateTime? end,int pageIndex,int pageSize,out int total)
        {
            var db = new MongoDbContext();
            var query = db.Where<ClusterPerformanceCounterSnapshot>();
            if (!string.IsNullOrEmpty(ip))
            {
                query = query.Where(x => x.MachineIP == ip);
            }
            if (counterId > 0 && value > 0)
            {
                switch (counterId)
                {
                    case (int)EnmCounter.CPU:
                        query = query.Where(x => x.CPU > value);
                        break;
                    case (int)EnmCounter.IIS请求:
                        query = query.Where(x => x.IIS > value);
                        break;
                    case (int)EnmCounter.内存:
                        query = query.Where(x => x.Memory > value);
                        break;
                }
            }
            if (start.HasValue && end.HasValue)
            {
                query = query.Where(x => x.CreateTime < end.Value && x.CreateTime > start.Value);
            }
            return query.OrderByDescending(x => x.Id).ToPageList(pageIndex,pageSize,out total);
        }

        public List<ClusterPerformanceCounterSnapshot> ClusterPerformanceCounterSnapshot(string ip,DateTime? startTime,DateTime? endTime,int top = 20)
        {
            var db = new MongoDbContext();

            var query = db.Where<ClusterPerformanceCounterSnapshot>();

            if (!string.IsNullOrEmpty(ip))
            {
                query = query.Where(x => x.MachineIP == ip);
            }
            return query.OrderByDescending(x => x.Id).Take(top).ToList();
        }

        /// <summary>
        /// 性能监视器统计
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="dateType">1当天,2当月</param>
        /// <returns></returns>
        public List<PerformanceCounterSnapshotDto> ClusterPerformanceCounterSnapshotReport(string ip,int dateType)
        {
            var db = new MongoDbContext();
            var query = db.Where<ClusterPerformanceCounterSnapshot>(x => x.MachineIP == ip);

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            if (dateType == 1)//按天
            {
                var start = new DateTime(year,month,day,0,0,0);
                var end = new DateTime(year,month,day,23,59,59);
                query = query.Where(x => x.CreateTime < end && x.CreateTime > start);
                var q1 = query.GroupBy(x => x.CreateTime.Hour).Select(x => new PerformanceCounterSnapshotDto
                {
                    Date = x.Key,
                    IIS = x.Average(m => m.IIS),
                    CPU = x.Average(m => m.CPU),
                    Memory = x.Average(m => m.Memory),
                });
                return q1.ToList();
            }
            else
            {
                var start = new DateTime(year,month,1,0,0,0);

                var end = new DateTime(year,month,start.AddMonths(1).AddDays(-1).Day,23,59,59);

                query = query.Where(x => x.CreateTime < end && x.CreateTime > start);

                var q1 = query.GroupBy(x => x.CreateTime.Day).Select(x => new PerformanceCounterSnapshotDto
                {
                    Date = x.Key,
                    IIS = x.Average(m => m.IIS),
                    CPU = x.Average(m => m.CPU),
                    Memory = x.Average(m => m.Memory),
                });
                return q1.ToList();
            }
        }
    }

   
}