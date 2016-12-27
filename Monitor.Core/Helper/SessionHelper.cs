using System.Web;

namespace Monitor.Core
{
    public class SessionHelper  {

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        /// <param name="iExpires">调动有效期（默认20分钟）</param>
        public static void Set(string strSessionName, object strValue, int iExpires=20) {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }


        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static object Get(string strSessionName)
        {
            return HttpContext.Current.Session[strSessionName];
        }


        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void Remove(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
            HttpContext.Current.Session.Remove(strSessionName);
        }


        /// <summary>
        /// 删除所有的ession
        /// </summary>
        /// <returns></returns>
        public static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }

        /// <summary>
        /// 获得会话ID
        /// </summary>
        /// <returns></returns>
        public static string GetSessionID()
        {
            return HttpContext.Current.Session.SessionID;
        }
    }
}
