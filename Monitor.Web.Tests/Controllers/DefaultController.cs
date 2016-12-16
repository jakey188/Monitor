using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monitor.Watch.Tests.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index(int a=1)
        {
            return View();
        }
    }
}