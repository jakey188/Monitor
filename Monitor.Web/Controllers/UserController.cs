using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Monitor.Models.Entites;
using Monitor.Services;
using Monitor.Web.Core.Filter;

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

        public ActionResult UserInfo(string id)
        {
            var user = new User();
            if (!string.IsNullOrEmpty(id))
                user = _userSerivice.Get(id);
            return PartialView("_UserInfo",user);
        }

        [Route("~/api/GetUserList")]
        public JsonResult GetUserList(string userName,int pageIndex = 1,int pageSize = 10)
        {
            int total;
            var data = _userSerivice.GetUserList(userName,pageIndex,pageSize,out total);
            return Success(data,pageIndex,pageSize,total);
        }

        [AuthRole(EnmUserRole.管理员),Route("~/api/user/delete")]
        public JsonResult DeleteUser(string id)
        {
            _userSerivice.Delete(id);
            return Success();
        }

        [AuthRole(EnmUserRole.管理员),Route("~/api/user/add"), HttpPost]
        public JsonResult Add(string id,string userName,string trueName,string role,string mobile,string email,string password)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var u = _userSerivice.Get(id);
                if (u != null)
                {
                    u.TrueName = trueName;
                    u.Role = Convert.ToInt32(role);
                    u.Mobile = mobile;
                    u.Email = email;
                    u.Password = password;
                }
                _userSerivice.Update(u);
            }
            else
            {
                if (_userSerivice.IsExitUser(userName))
                    return Fail("用户已存在");

                var user = new User
                {
                    TrueName = trueName,
                    Role = Convert.ToInt32(role),
                    Mobile = mobile,
                    Email = email,
                    UserName = userName,
                    Password = password
                };
                _userSerivice.Add(user);
            }
            return Success();
        }
    }
}