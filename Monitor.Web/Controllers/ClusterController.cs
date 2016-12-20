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
        /// WEB Http请求耗时视图
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>ActionResult</returns>
        public ActionResult HttpWatchList(int pageIndex = 1,int pageSize = 10)
        {
            return View();
        }

        /// <summary>
        /// WEB Http请求耗时视图
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>ActionResult</returns>
        public ActionResult HttpWatchReportList(int pageIndex = 1,int pageSize = 10)
        {
            return View();
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
        /// 获取WEB集群服务器列表
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        [Route("api/GetComputerList")]
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
        [Route("api/Computer/Delete")]
        public JsonResult DeleteComputer(string id)
        {
            var objectId = new ObjectId(id);
            this._clusterComputerInfoServices.Delete(objectId);
            return Success();
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
        public JsonResult GetPerformanceCounterSnapshotList(string ip,string type,string collectTypeValue,string startDate,string endDate,int pageIndex = 1,int pageSize = 10)
        {
            var total = 0;
            var value = -1;
            var counterId = -1;
            if (!string.IsNullOrEmpty(collectTypeValue) && !string.IsNullOrEmpty(type))
            {
                int.TryParse(collectTypeValue,out value);
                int.TryParse(type,out counterId);
            }

            DateTime? start = string.IsNullOrEmpty(startDate) ? null : (DateTime?)DateTime.Parse(startDate);
            DateTime? end = string.IsNullOrEmpty(endDate) ? null : (DateTime?)DateTime.Parse(endDate);

            var computerInfos = _clusterPerformanceCounterSnapshotService.ClusterPerformanceCounterSnapshot(ip,counterId,value,start,end,pageIndex,pageSize,out total);
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
            var computerInfos = _clusterPerformanceCounterSnapshotService.ClusterPerformanceCounterSnapshot(ip,null,null,20);
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

        /// <summary>
        /// 获取HTTP耗时列表
        /// </summary>
        /// <param name="watchTime"></param>
        /// <param name="url"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("api/GetHttpWatchList")]
        public JsonResult GetHttpWatchList(string watchTime,string url,string startDate,string endDate,int pageIndex = 1,int pageSize = 10)
        {
            int total = 0;
            var watchTimeValue = -1;
            if (!string.IsNullOrEmpty(watchTime))
            {
                int.TryParse(watchTime,out watchTimeValue);
            }

            DateTime? start = string.IsNullOrEmpty(startDate) ? null : (DateTime?)DateTime.Parse(startDate);
            DateTime? end = string.IsNullOrEmpty(endDate) ? null : (DateTime?)DateTime.Parse(endDate);

            var computerInfos = _clusterHttpWatchService.GetClusterHttpWatch(watchTimeValue,url,start,end, pageIndex,pageSize,out total);
            return Success(computerInfos,pageIndex,pageSize,total);
        }

        [Route("api/GetHttpWatchReportList")]
        public JsonResult GetHttpWatchReportList(string url,string startDate,string endDate,int pageIndex = 1,int pageSize = 10)
        {
            int total = 0;

            DateTime? start = string.IsNullOrEmpty(startDate) ? null : (DateTime?)DateTime.Parse(startDate);
            DateTime? end = string.IsNullOrEmpty(endDate) ? null : (DateTime?)DateTime.Parse(endDate);

            var computerInfos = _clusterHttpWatchService.GetClusterHttpWatchReport(url,start,end,pageIndex,pageSize,out total);
            return Success(computerInfos,pageIndex,pageSize,total);
        }
    }
}