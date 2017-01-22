using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
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
            var query = (IQueryable<ClusterPerformanceCounterSnapshot>)db.GetCollection<ClusterPerformanceCounterSnapshot>().AsQueryable();
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
            
            total = query.Count();

            return query.OrderByDescending(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<ClusterPerformanceCounterSnapshot> ClusterPerformanceCounterSnapshotPage(string ip,int counterId,int value,DateTime? start,DateTime? end,int pageIndex,int pageSize,out int total)
        {
            var db = new MongoDbContext();

            var collection = db.GetCollection<ClusterPerformanceCounterSnapshot>();

            var builder = Builders<ClusterPerformanceCounterSnapshot>.Filter;

            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(ip))
            {
                filter = filter & builder.Eq(x => x.MachineIP,ip);
            }
            if (counterId > 0 && value > 0)
            {
                switch (counterId)
                {
                    case (int)EnmCounter.CPU:
                        filter = filter & builder.Gte(x => x.CPU,value);
                        break;
                    case (int)EnmCounter.IIS请求:
                        filter = filter & builder.Gte(x => x.IIS,value);
                        break;
                    case (int)EnmCounter.内存:
                        filter = filter & builder.Gte(x => x.Memory,value);
                        break;
                }
            }
            if (start.HasValue && end.HasValue)
            {
                filter = filter & builder.Gt(x => x.CreateTime,start.Value);
                filter = filter & builder.Lt(x => x.CreateTime,end.Value);
            }
            total = (int)collection.Count(filter);
            
            return collection.Find(filter).SortByDescending(x => x.Id).ToPageList(pageIndex,pageSize);
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
        /// <returns></returns>
        public List<PerformanceCounterSnapshotDto> ClusterPerformanceCounterSnapshotReport(string ip,DateTime start,DateTime end)
        {
            var db = new MongoDbContext();
            

            var d = end - start;

            if (d.Days==0 && d.Hours<24)//按天
            {
                var query = db.Where<ClusterPerformanceCounterSnapshot>(x => x.MachineIP == ip);
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
                var query = db.Where<ClusterPerformanceCounterSnapshotDayHistoryTotal>(x => x.MachineIP == ip);
                query = query.Where(x => x.CreateTime < end && x.CreateTime > start);

                var q1 = query.GroupBy(x => x.CreateTime.Day).Select(x => new PerformanceCounterSnapshotDto
                {
                    Date = x.Key,
                    IIS = x.Average(m => m.IISAvg),
                    CPU = x.Average(m => m.CPUAvg),
                    Memory = x.Average(m => m.MemoryAvg),
                });

                return q1.ToList();
            }
        }
    }
}