using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Monitor.Core;
using Monitor.Data;
using Monitor.Models;

namespace Monitor.Services
{
    public class ClusterComputerInfoServices
    {
        /// <summary>
        /// 添加或者更新WBE集群服务器
        /// </summary>
        /// <param name="model">集群服务器</param>
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

        /// <summary>
        /// 删除WEB站点集群服务器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Delete(ObjectId id)
        {
            var db = new MongoDbContext();
            db.Delete<ClusterComputerInfo>(x => x.Id == id);
        }

        /// <summary>
        /// 获取WEB站点集群服务器
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ClusterComputerInfo> GetClusterComputerInfo(int pageIndex, int pageSize, out int total)
        {
            var db = new MongoDbContext();
            return db.Where<ClusterComputerInfo>()
                .OrderByDescending(x => x.Id)
                .ToPageList(pageIndex, pageSize, out total);
        }

        /// <summary>
        /// 获取WEB站点集群服务器
        /// </summary>
        /// <returns></returns>
        public List<ClusterComputerInfo> GetClusterComputerInfo()
        {
            var db = new MongoDbContext();
            return db.Where<ClusterComputerInfo>().OrderByDescending(x => x.Id).ToList();
        }
    }
}