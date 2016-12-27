using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monitor.Models.Entites;

namespace Monitor.Web.Core
{
    public class UserContext
    {
        private readonly static string LoginSessionName = "LoginSessionName";
        public static void WriteUserSession(User user)
        {
            HttpContext.Current.Session[LoginSessionName] = user;
        }

        public static void ClearUserSession()
        {
            HttpContext.Current.Session[LoginSessionName] = null;
            HttpContext.Current.Session.RemoveAll();
        }

        public static User GetLoginInfo()
        {
            return HttpContext.Current.Session[LoginSessionName] as User;
        }

        public static bool IsLogin()
        {
            return HttpContext.Current.Session[LoginSessionName] != null;
        }
    }
}