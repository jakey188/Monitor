using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Windows.Server
{
    public class GlobalConfigure
    {
        /// <summary>
        /// 性能监视器休眠时间/毫秒
        /// </summary>
        public static int PerformanceCollectBackgroundTaskSleepTime = 1000;
        /// <summary>
        /// 是否将采集信息写入数据库
        /// </summary>
        public static bool IsDbSave = false;
        /// <summary>
        /// 服务器警报发送邮箱
        /// </summary>
        public static string LarumEmail;
    }
}
