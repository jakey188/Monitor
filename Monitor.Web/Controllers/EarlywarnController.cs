using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monitor.Web.Controllers
{
    public class EarlywarnController : Controller
    {
        // GET: Earlywarn
        public ActionResult Set()
        {
            return View();
        }

        public ActionResult Logs()
        {
            return View();
        }
    }
}