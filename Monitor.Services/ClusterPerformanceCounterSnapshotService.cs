﻿using System;
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

        public List<ClusterPerformanceCounterSnapshot> ClusterPerformanceCounterSnapshot(string ip, int type,
            int pageIndex, int pageSize, out int total)
        {
            var db = new MongoDbContext();
            var query = db.Where<ClusterPerformanceCounterSnapshot>();
            if (!string.IsNullOrEmpty(ip))
            {
                query = query.Where(x => x.MachineIP == ip);
            }
            if (type > 0)
            {
                query = query.Where(x => x.Counter.Id == type);
            }
            return query.OrderByDescending(x => x.Id).ToPageList(pageIndex, pageSize, out total);
        }

        public List<ClusterPerformanceCounterSnapshot> ClusterPerformanceCounterSnapshot(string ip,int type,DateTime? startTime,DateTime? endTime, int top=20)
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