using System.Web;
using System.Web.Mvc;
using Monitor.Models.Entites;
using Monitor.Web.Response;

namespace Monitor.Web.Core.Filter
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    public class AuthRoleAttribute:AuthorizeAttribute
    {
        public EnmUserRole AuthUserRole { get; set; }

        public AuthRoleAttribute(EnmUserRole authUserRole)
        {
            AuthUserRole = authUserRole;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = UserContext.GetLoginInfo();
            string url = HttpContext.Current.Request.Url.ToString();

            if (!string.IsNullOrEmpty(url))
            {
                url = HttpUtility.UrlEncode(url);
            }

            var loginUrl = new UrlHelper(filterContext.RequestContext).Action("index","login");
            if (user != null)
            {
                if (user.Role != (int)AuthUserRole)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.ContentType = "application/json; charset=utf-8";
                        filterContext.HttpContext.Response.Write(new AjaxResponse { S = (int)CodeEnmStruct.权限不够,Msg = "无权访问" }.ToJson());
                        filterContext.HttpContext.Response.End();
                        filterContext.Result = new EmptyResult();
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(loginUrl + "?returnURL=" + url);
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectResult(loginUrl + "?returnURL=" + url);
            }
        }
    }
}
