using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monitor.Services;
using Monitor.Web.Core;

namespace Monitor.Web.Controllers
{
    /// <summary>
    /// 集群控制器
    /// </summary>
    public class ClusterController : BaseController
    {
        /// <summary>
        /// 计算机信息业务
        /// </summary>
        private readonly ClusterComputerInfoServices _clusterComputerInfoServices;

        /// <summary>
        /// Http监控业务
        /// </summary>
        private readonly ClusterHttpWatchService _clusterHttpWatchService;

        /// <summary>
        /// 性能监控器业务
        /// </summary>
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
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>ActionResult</returns>
        public ActionResult ComputerList(int pageIndex=1,int pageSize=10)
        {
            var total = 0;

            var computerInfos = this._clusterComputerInfoServices.GetClusterComputerInfo(pageIndex,pageSize,out total);
            
            return View();
        }

        /// <summary>
        /// WEB Http请求耗时视图
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>ActionResult</returns>
        public ActionResult HttpWatchList(int pageIndex = 1,int pageSize = 10)
        {
            var total = 0;
            var computerInfos = _clusterHttpWatchService.GetClusterHttpWatch(pageIndex,pageSize,out total);
            return View(computerInfos.ToPagedList(total,pageIndex,pageSize));
        }

        /// <summary>
        /// SERVER性能监视器快照视图
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回视图页面</returns>
        public ActionResult PerformanceCounterSnapshotList(int pageIndex = 1,int pageSize = 10)
        {
            ViewBag.ComputerInfoList = _clusterComputerInfoServices.GetClusterComputerInfo();
            return View();
        }

        /// <summary>
        /// 图标视图
        /// </summary>
        /// <returns>返回视图图表</returns>
        public ActionResult Charts()
        {
            ViewBag.ComputerInfoList = _clusterComputerInfoServices.GetClusterComputerInfo();
            return View();
        }

        /// <summary>
        /// 获取性能监视器快照数据
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="type">监视类型</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回列表数据</returns>
        [Route("api/GetPerformanceCounterSnapshotList")]
        public JsonResult GetPerformanceCounterSnapshotList(string ip,string type, int pageIndex = 1,int pageSize = 10)
        {
            var total = 0;
            var computerInfos = _clusterPerformanceCounterSnapshotService.ClusterPerformanceCounterSnapshot(ip,string.IsNullOrWhiteSpace(type) ? 0 : Convert.ToInt32(type),pageIndex,pageSize,out total);
            return Success(computerInfos.ToPagedList(total,pageIndex,pageSize),pageIndex,pageSize,total);
        }

        /// <summary>
        /// 获取性能监视器快照数据
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="type">监视类型</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回列表数据</returns>
        [Route("api/GetPerformanceCounterSnapshotCharts")]
        public JsonResult GetPerformanceCounterSnapshotList(string ip,string type)
        {
            var computerInfos = _clusterPerformanceCounterSnapshotService.ClusterPerformanceCounterSnapshot(ip,string.IsNullOrWhiteSpace(type) ? 0 : Convert.ToInt32(type),null,null,20);
            var data = computerInfos
                .OrderBy(x => x.Id)
                .Select(x => new
                {
                    time = x.CreateTime.ToString("HH:mm:ss"),
                    cpu = x.CPU,
                    iis = x.IIS,
                    memory = x.Memory,
                    physicalDiskRead = x.PhysicalDiskRead,
                    physicalDiskWrite = x.PhysicalDiskWrite
                });
            return Json(data,JsonRequestBehavior.AllowGet);
        }
    }
}