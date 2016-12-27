using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monitor.Services;

namespace Monitor.Web.Controllers
{
    public class HttpWatchController : BaseController
    {
        private readonly ClusterHttpWatchService _clusterHttpWatchService;

        /// <summary>
        /// Initializes a new instance of the
        /// </summary>
        public HttpWatchController()
        {
            this._clusterHttpWatchService = new ClusterHttpWatchService();
        }
        /// <summary>
        /// WEB Http请求耗时视图
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>ActionResult</returns>
        public ActionResult List(int pageIndex = 1,int pageSize = 10)
        {
            return View();
        }

        /// <summary>
        /// WEB Http请求耗时视图
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>ActionResult</returns>
        public ActionResult ReportList(int pageIndex = 1,int pageSize = 10)
        {
            return View();
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
        [Route("api/httpwatch/list")]
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

            var computerInfos = _clusterHttpWatchService.GetClusterHttpWatch(watchTimeValue,url,start,end,pageIndex,pageSize,out total);
            return Success(computerInfos,pageIndex,pageSize,total);
        }

        [Route("api/httpwatch/report")]
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