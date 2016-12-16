using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Monitor.Web
{
    public class JsonNetResult : JsonResult
    {
        /// <summary>
        /// Gets or sets the javascript callback function that is
        /// to be invoked in the resulting script output.
        /// </summary>
        /// <value>The callback function name.</value>
        public string Callback { get; set; }

        /// <summary>
        /// 重写ExecuteResult
        /// </summary>
        /// <param name="context">当前ControllerContext</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }

            if (string.IsNullOrEmpty(Callback))
            {
                Callback = context.HttpContext.Request.QueryString["callback"];
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var camelCaseFormatter = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                };

                string buffer = !string.IsNullOrEmpty(Callback)
                    ? string.Format("{0}({1})", Callback, JsonConvert.SerializeObject(Data, camelCaseFormatter))
                    : JsonConvert.SerializeObject(Data, camelCaseFormatter);

                response.Write(buffer);
            }
        }
    }
}
