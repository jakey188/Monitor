using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using Monitor.Services;
using Monitor.Web.Core;

namespace Monitor.Web.Controllers
{
    /// <summary>
    /// 集群控制器
    /// </summary>
    public class ClusterController : BaseController
    {
        private readonly ClusterComputerInfoServices _clusterComputerInfoServices;
        private readonly ClusterHttpWatchService _clusterHttpWatchService;
        private readonly ClusterPerformanceCounterSnapshotService _clusterPerformanceCounterSnapshotService;

        /// <summary>
        /// Initializes a new instance of the
        /// </summary>
        public ClusterController()
        {
            this._clusterComputerInfoServices = new ClusterComputerInfoServices();
            this._clusterHttpWatchService = new ClusterHttpWatchService();
            this._clusterPerformanceCounterSnapshotService = new ClusterPerformanceCounterSnapshotService();
        }

        /// <summary>
        /// WEB集群服务器列表
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ComputerList()
        {
            return View();
        }

        /// <summary>
        /// 获取WEB集群服务器列表
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        [Route("api/computer/list")]
        public JsonResult GetComputerList(int pageIndex = 1,int pageSize = 10)
        {
            int total = 0;
            var computerInfos = this._clusterComputerInfoServices.GetClusterComputerInfo(pageIndex,pageSize,out total);
            return Success(computerInfos,pageIndex,pageSize,total);
        }

        /// <summary>
        /// 删除WEB站点服务器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/computer/delete")]
        public JsonResult DeleteComputer(string id)
        {
            var objectId = new ObjectId(id);
            this._clusterComputerInfoServices.Delete(objectId);
            return Success();
        }

        
        
    }
}