using System.Web.Mvc;

namespace Monitor.Web.Core
{
    /// <summary>
    /// 控制器拓展
    /// </summary>
    public static class ContollerExtensions
    {
        /// <summary>
        /// 重写JsonResult返回
        /// </summary>
        /// <param name="controller">当前控制器</param>
        /// <param name="data">输出对象</param>
        /// <param name="jsonRequestBehavior">运行访问方式</param>
        /// <returns>返回JsonNetResult</returns>
        public static JsonNetResult JsonNet(this Controller controller, object data, JsonRequestBehavior jsonRequestBehavior)
        {
            var result = new JsonNetResult()
            {
                Data = data,
                JsonRequestBehavior = jsonRequestBehavior
            };

            return result;
        }
    }
}
