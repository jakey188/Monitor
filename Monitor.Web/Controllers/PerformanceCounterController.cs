using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Monitor.Services;

namespace Monitor.Web.Controllers
{
    public class PerformanceCounterController :BaseController
    {
        private readonly ClusterPerformanceCounterSnapshotService _clusterPerformanceCounterSnapshotService;
        private readonly ClusterComputerInfoServices _clusterComputerInfoServices;
        /// <summary>
        /// Initializes a new instance of the
        /// </summary>
        public PerformanceCounterController()
        {
            this._clusterPerformanceCounterSnapshotService = new ClusterPerformanceCounterSnapshotService();
            this._clusterComputerInfoServices = new ClusterComputerInfoServices();
        }


        /// <summary>
        /// SERVER性能监视器快照视图
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回视图页面</returns>
        public ActionResult SnapshotList(int pageIndex = 1,int pageSize = 10)
        {
            ViewBag.ComputerInfoList = _clusterComputerInfoServices.GetClusterComputerInfo();
            return View();
        }

        /// <summary>
        /// 性能监视器实时视图
        /// </summary>
        /// <returns>返回视图图表</returns>
        public ActionResult Charts()
        {
            ViewBag.ComputerInfoList = _clusterComputerInfoServices.GetClusterComputerInfo();
            return View();
        }

        /// <summary>
        /// 性能监视器快照图
        /// </summary>
        /// <returns></returns>
        public ActionResult ChartsSnapshotReport()
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
        [Route("api/performance/list")]
        public JsonResult GetSnapshotList(string ip,string type,string collectTypeValue,string startDate,string endDate,int pageIndex = 1,int pageSize = 10)
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

            var computerInfos = _clusterPerformanceCounterSnapshotService.ClusterPerformanceCounterSnapshotPage(ip,counterId,value,start,end,pageIndex,pageSize,out total);
            return Success(computerInfos,pageIndex,pageSize,total);
        }

        /// <summary>
        /// 获取性能监视器快照数据
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="type">监视类型</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回列表数据</returns>
        [Route("api/performance/realtime/charts")]
        public JsonResult GetSnapshotCharts(string ip)
        {

            var list = _clusterPerformanceCounterSnapshotService.ClusterPerformanceCounterSnapshot(ip,null,null,20);
            var data = list
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
        /// 获取性能监视器快照数据
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="type">监视类型</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回列表数据</returns>
        [Route("api/performance/snapshot/charts/report")]
        public JsonResult GetSnapshotChartsReport(string ip,int type=1)
        {
            var list = _clusterPerformanceCounterSnapshotService.ClusterPerformanceCounterSnapshotReport(ip,type);
            var data = list.Select(x => new
            {
                date = x.Date,
                iis = x.IIS,
                cpu = x.CPU,
                memory = x.Memory
            });
            return Json(data,JsonRequestBehavior.AllowGet);
        }
    }
}