using System;
using System.Collections.Generic;
using System.Linq;
using Monitor.Core;
using Monitor.Data;
using Monitor.Models;
using Monitor.Services.Dtos;

namespace Monitor.Services
{
    public class ClusterHttpWatchService
    {
        /// <summary>
        /// HTTP耗时列表
        /// </summary>
        /// <param name="watchTime"></param>
        /// <param name="url"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ClusterHttpWatch> GetClusterHttpWatch(int watchTime,string url,DateTime? startDate,DateTime? endDate,int pageIndex,int pageSize,out int total)
        {
            var db = new MongoDbContext();
            var query = db.Where<ClusterHttpWatch>();

            if (watchTime > 0)
                query = query.Where(x => x.Timevalue > watchTime);

            if (!string.IsNullOrEmpty(url))
                query = query.Where(x => x.Referer.Contains(url));

            if (startDate.HasValue && endDate.HasValue)
                query = query.Where(x => x.CreateDate < endDate.Value && x.CreateDate > startDate.Value);

            return query.OrderByDescending(x => x.Id).ToPageList(pageIndex,pageSize,out total);
        }

        /// <summary>
        /// HTTP耗时统计
        /// </summary>
        /// <param name="url"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ClusterHttpWatchTotalDto> GetClusterHttpWatchReport(string url,DateTime? startDate,DateTime? endDate,int pageIndex,int pageSize,out int total)
        {
            var db = new MongoDbContext();
            var query = db.Where<ClusterHttpWatch>();

            if (!string.IsNullOrEmpty(url))
                query = query.Where(x => x.Referer.Contains(url));

            if (startDate.HasValue && endDate.HasValue)
                query = query.Where(x => x.CreateDate < endDate.Value && x.CreateDate > startDate.Value);

            var q = query.GroupBy(x => x.Referer).Select(x => new ClusterHttpWatchTotalDto
            {
                Url = x.Key,
                CallCount = x.Count(),
                MaxTime = x.Max(m => m.Timevalue),
                MinTime = x.Min(m => m.Timevalue),
                AvgTime = x.Average(m => m.Timevalue)
            });
            return q.ToPageList(pageIndex,pageSize,out total);
        }
    }
}