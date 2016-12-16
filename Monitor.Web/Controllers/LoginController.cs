using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monitor.Web.Controllers
{
    /// <summary>
    /// 登录控制器
    /// </summary>
    public class LoginController : Controller
    {
        /// <summary>
        /// 登录方法
        /// </summary>
        /// <returns>返回登录视图</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}