using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monitor.Models.Entites;
using Monitor.Services;

namespace Monitor.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserSerivice _userSerivice;

        public UserController()
        {
            _userSerivice = new UserSerivice();
        }
        public ActionResult List()
        {
            return View();
        }

        public ActionResult UserInfo()
        {
            return PartialView("_UserInfo");
        }

        [Route("~/api/GetUserList")]
        public JsonResult GetUserList(int pageIndex = 1,int pageSize = 10)
        {
            int total;
            var data = _userSerivice.GetUserList(pageIndex,pageSize,out total);
            return Success(data,pageIndex,pageSize,total);
        }

        [Route("~/api/user/delete")]
        public JsonResult DeleteUser(string id)
        {
            _userSerivice.Delete(id);
            return Success();
        }

        [Route("~/api/user/update"),HttpPost]
        public JsonResult Update(User user)
        {
            _userSerivice.Update(user);
            return Success();
        }
    }
}