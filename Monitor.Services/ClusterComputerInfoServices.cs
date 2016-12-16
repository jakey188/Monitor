using System.Collections.Generic;
using System.Linq;
using Monitor.Core;
using Monitor.Data;
using Monitor.Models;

namespace Monitor.Services
{
    public class ClusterComputerInfoServices
    {
        public void AddOrUpdate(ClusterComputerInfo model)
        {
            var db = new MongoDbContext();
            var computerInfo = db.Where<ClusterComputerInfo>(x => x.MachineIp == model.MachineIp).FirstOrDefault();
            if (computerInfo == null)
            {
                db.Add(model);
            }
            else
            {
                model.Id = computerInfo.Id;
                db.Update(model);
            }
        }

        public List<ClusterComputerInfo> GetClusterComputerInfo(int pageIndex, int pageSize, out int total)
        {
            var db = new MongoDbContext();
            return db.Where<ClusterComputerInfo>()
                .OrderByDescending(x => x.Id)
                .ToPageList(pageIndex, pageSize, out total);
        }

        public List<ClusterComputerInfo> GetClusterComputerInfo()
        {
            var db = new MongoDbContext();
            return db.Where<ClusterComputerInfo>().OrderByDescending(x => x.Id).ToList();
        }
    }
}