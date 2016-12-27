using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monitor.Services;
using Monitor.Web.Core;

namespace Monitor.Web.Controllers
{
    /// <summary>
    /// 登录控制器
    /// </summary>
    [AllowAnonymous]
    public class LoginController:BaseController
    {
        /// <summary>
        /// 登录方法
        /// </summary>
        /// <returns>返回登录视图</returns>
        public ActionResult Index(string returnURL)
        {
            ViewBag.ReturnUrl = HttpUtility.UrlDecode(returnURL);
            return View();
        }

        public ActionResult Logout(string returnURL)
        {
            UserContext.ClearUserSession();
            return RedirectToAction("index",new { returnUrl=returnURL });
        }

        public JsonResult Signin(string userName,string password,string remember)
        {
            var userService = new UserSerivice();
            var user = userService.Get(userName,password);
            if (user == null)
                return Fail("账号或密码错误");
            UserContext.WriteUserSession(user);
            return Success();
        }
    }
}