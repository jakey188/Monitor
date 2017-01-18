using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Monitor.Models.Entites;
using Monitor.Services;
using Monitor.Web.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Monitor.Web.Core.Filter
{
    /// <summary>
    /// 关闭customErrors mode="On" 由OnException接收错误处理
    /// </summary>
    public class HandleErrorLogAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 异常重写
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");

            if (filterContext.IsChildAction) return;

            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled) return;
            //if (filterContext.ExceptionHandled)
            //    return;

            Exception exception = filterContext.Exception.GetBaseException();

            if (exception.GetType() == typeof(TaskCanceledException))
                return;

            if (new HttpException(null, exception).GetHttpCode() != 500) return;

            if (!ExceptionType.IsInstanceOfType(exception)) return;

            WriteExceptionToDb(exception,filterContext);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                AjaxException(filterContext,exception);
                return;
            }

            var controllerName = (string) filterContext.RouteData.Values["controller"];
            var actionName = (string) filterContext.RouteData.Values["action"];

            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            filterContext.Result = new ViewResult
            {
                ViewName = View,
                MasterName = Master,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = filterContext.Controller.TempData
            };

            filterContext.ExceptionHandled = true;
            if (!filterContext.HttpContext.Response.IsRequestBeingRedirected)
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }

        private void AjaxException(ExceptionContext filterContext,Exception exception)
        {
            var camelCaseFormatter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var data = new AjaxResponse { S = (int)CodeEnmStruct.失败,Msg = exception.Message };

            filterContext.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(data,camelCaseFormatter),
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        private void WriteExceptionToDb(Exception exception,ExceptionContext filterContext)
        {
            var sbBuilder = new StringBuilder();
            sbBuilder.Append("Source         :" + exception.Source + "\r\n");
            sbBuilder.Append("GetType        :" + exception.GetType() + "\r\n");
            sbBuilder.Append("StackTrace     :" + exception.StackTrace + "\r\n");
            sbBuilder.Append("Time           :" + DateTime.Now + "\r\n");
            var context = sbBuilder.ToString();
            var log = new Logs();
            log.CreateTime = DateTime.Now;
            log.Message = exception.Message;
            log.Types = (int)EnmLogs.监控中心系统日志;
            log.Context = context;
            new LogsServices().Add(log);
        }
    }
}

