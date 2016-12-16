using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monitor.Web.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}