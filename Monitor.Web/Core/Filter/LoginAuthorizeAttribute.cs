using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;
using Monitor.Web.Response;

namespace Monitor.Web.Core.Filter
{
    public class LoginAuthorizeAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute),inherit: true)
    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute),inherit: true);

            if (skipAuthorization)
            {
                return;
            }

            if (!UserContext.IsLogin())
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var camelCaseFormatter = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                    var data = new AjaxResponse { S = (int)CodeEnmStruct.登录超时,Msg = "未登录或登录超时" };
                    filterContext.HttpContext.Response.Write(JsonConvert.SerializeObject(data,camelCaseFormatter));

                    filterContext.HttpContext.Response.End();
                    filterContext.Result = new EmptyResult();
                }
                else
                {
                    string url = HttpContext.Current.Request.Url.ToString();
                    if (!string.IsNullOrEmpty(url))
                    {
                        url = HttpUtility.UrlEncode(url);
                    }
                    var loginUrl = new UrlHelper(filterContext.RequestContext).Action("index","login");
                    filterContext.Result = new RedirectResult(loginUrl + "?returnURL=" + url);
                }
            }
        }
    }
}
