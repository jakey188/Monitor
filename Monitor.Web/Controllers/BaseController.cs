using System.IO;
using System.Web.Mvc;
using Monitor.Web.Response;
using Monitor.Web.Core;

namespace Monitor.Web.Controllers
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 空数组
        /// </summary>
        public object EmptyArray
        {
            get { return new string[] { }; }
        }

        /// <summary>
        /// 运行访问方式
        /// </summary>
        public JsonRequestBehavior AllowGet
        {
            get { return JsonRequestBehavior.AllowGet; }
        }

        #region 序列化JsonTip

        /// <summary>
        /// 序列化Json
        /// </summary>
        /// <param name="s">状态码</param>
        /// <param name="msg">提示消息</param>
        /// <param name="pgIndex">当前页面</param>
        /// <param name="pgSize">每页大小</param>
        /// <param name="count">总数</param>
        /// <param name="obj">返回数据</param>
        /// <returns>AjaxPageResponse</returns>
        private AjaxPageResponse Serialize(CodeEnmStruct s,string msg,int pgIndex,int pgSize,int count,object obj)
        {
            return new AjaxPageResponse
            {
                S = (int)s,
                Msg = msg,
                Count = count,
                Data = obj,
                PgIndex = pgIndex,
                PgSize = pgSize
            };
        }

        /// <summary>
        /// 序列化Json
        /// </summary>
        /// <param name="s">状态码</param>
        /// <param name="msg">提示消息</param>
        /// <param name="obj">返回数据</param>
        /// <returns>AjaxResponse</returns>
        private AjaxResponse Serialize(CodeEnmStruct s,string msg,object obj)
        {
            return new AjaxResponse { S = (int)s,Msg = msg,Data = obj };
        }

        /// <summary>
        /// 序列化Json
        /// </summary>
        /// <param name="s">状态码</param>
        /// <param name="msg">提示消息</param>
        /// <returns>AjaxResponse</returns>
        public AjaxResponse Serialize(CodeEnmStruct s,string msg)
        {
            return new AjaxResponse { S = (int)s,Msg = msg,Data = null };
        }

        #endregion

        /// <summary>
        /// 未登录或登录超时
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <returns>JsonResult</returns>
        public JsonResult NotLogin(string msg = "")
        {
            var data = this.Serialize(CodeEnmStruct.登录超时,!string.IsNullOrEmpty(msg) ? msg : CodeTip.LoginOvertime,this.EmptyArray);
            return this.JsonNet(data,this.AllowGet);
        }

        /// <summary>
        /// 无数据提示
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <returns>JsonResult</returns>
        public JsonResult NoRecord(string msg = "")
        {
            var data = this.Serialize(CodeEnmStruct.无数据, !string.IsNullOrEmpty(msg) ? msg : CodeTip.NoRecord, this.EmptyArray);
            return this.JsonNet(data,this.AllowGet);
        }

        /// <summary>
        /// 成功提示
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <returns>JsonResult</returns>
        public JsonResult Success(string msg = "")
        {
            var data = this.Serialize(CodeEnmStruct.成功, !string.IsNullOrEmpty(msg) ? msg : CodeTip.SaveSuccess, this.EmptyArray);
            return this.JsonNet(data, this.AllowGet);
        }

        /// <summary>
        /// 成功提示
        /// </summary>
        /// <param name="obj">返回对象</param>
        /// <param name="msg">提示消息</param>
        /// <returns>JsonResult</returns>
        public JsonResult Success(object obj, string msg = "")
        {
            var data = this.Serialize(CodeEnmStruct.成功, !string.IsNullOrEmpty(msg) ? msg : CodeTip.SaveSuccess, obj);
            return this.JsonNet(data, this.AllowGet);
        }

        /// <summary>
        /// 成功提示
        /// </summary>
        /// <param name="obj">返回对象</param>
        /// <param name="pgIndex">当前页</param>
        /// <param name="pgSize">每页大小</param>
        /// <param name="count">总数</param>
        /// <param name="msg">提示消息</param>
        /// <returns>JsonResult</returns>
        public JsonResult Success(object obj, int pgIndex, int pgSize, int count, string msg = "")
        {
            var data = this.Serialize(CodeEnmStruct.成功,!string.IsNullOrEmpty(msg) ? msg : CodeTip.SaveSuccess, pgIndex, pgSize, count, obj);
            return this.JsonNet(data, this.AllowGet);
        }

        /// <summary>
        /// 失败提示
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <returns>JsonResult</returns>
        public JsonResult Fail(string msg = "")
        {
            var data = this.Serialize(CodeEnmStruct.失败,!string.IsNullOrEmpty(msg) ? msg : CodeTip.SaveFail, this.EmptyArray);
            return this.JsonNet(data, this.AllowGet);
        }
    }
}