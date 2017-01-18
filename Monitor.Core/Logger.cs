using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Monitor.Core
{
    public class Logger
    {
        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 普通的文件记录日志
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            logger.Info(info);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="info"></param>
        public static void Error(string info)
        {
            logger.Error(info);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void Error(string info,Exception ex)
        {
            logger.Error(ex,info);
        }
    }
}
