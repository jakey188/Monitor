using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Monitor.Web.Core
{
    /// <summary>
    /// Json拓展类
    /// </summary>
    public static class JsonExtension
    {
        #region Json拓展

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">转换的对象</typeparam>
        /// <param name="str">当前字符串</param>
        /// <returns>返回当前对象</returns>
        public static T JsonToObject<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 序列化JSON数据
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <returns>返回JSON字符串</returns>
        public static string ToJson(this object obj)
        {
            var camelCaseFormatter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(obj, camelCaseFormatter);
        }

        #endregion
    }
}
