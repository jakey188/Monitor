using System.Collections.Generic;
using System.Linq;
using Monitor.Core;
using Monitor.Data;
using Monitor.Models;

namespace Monitor.Services
{
    public class ClusterHttpWatchService
    {
        public List<ClusterHttpWatch> GetClusterHttpWatch(int pageIndex, int pageSize, out int total)
        {
            var db = new MongoDbContext();
            return db.Where<ClusterHttpWatch>().OrderByDescending(x => x.Id).ToPageList(pageIndex, pageSize, out total);
        }
    }
}