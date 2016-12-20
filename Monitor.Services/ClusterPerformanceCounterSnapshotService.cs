using System;
using System.Collections.Generic;
using System.Linq;
using Monitor.Core;
using Monitor.Data;
using Monitor.Models;

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

        public List<ClusterPerformanceCounterSnapshot> ClusterPerformanceCounterSnapshot(string ip,int counterId,int value,DateTime? start,DateTime? end, int pageIndex,int pageSize,out int total)
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

        public List<ClusterPerformanceCounterSnapshot> ClusterPerformanceCounterSnapshot(string ip,DateTime? startTime,DateTime? endTime, int top=20)
        {
            var db = new MongoDbContext();
            var query = db.Where<ClusterPerformanceCounterSnapshot>();
            if (!string.IsNullOrEmpty(ip))
            {
                query = query.Where(x => x.MachineIP == ip);
            }
            return query.OrderByDescending(x => x.Id).Take(top).ToList();
        }
    }
}