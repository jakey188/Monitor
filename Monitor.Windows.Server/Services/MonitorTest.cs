using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitor.Core.SystemInfos;
using Monitor.Data;
using NLog;

namespace Monitor.Windows.Server.Services
{
    public class MonitorTest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static bool Run(SystemInfo os)
        {
            DbTest();
            return CounterTest(os);
        }

        /// <summary>
        /// 计数器测试
        /// </summary>
        private static bool CounterTest(SystemInfo os)
        {
            var counterList = CounterProvider.Load();
            var result = true;
            foreach (var counter in counterList)
            {
                logger.Info(@"创建性能监视器: {0}\{1}\{2}\{3}\{4}",counter.CollectName,os.MachineName ?? ".",counter.CategoryName,
                                    counter.CounterName,counter.InstanceName);

                PerformanceCounterCategory category = new PerformanceCounterCategory(counter.CategoryName);
                if (string.IsNullOrEmpty(counter.InstanceName) && category.CategoryType == PerformanceCounterCategoryType.MultiInstance)
                {
                    var categoryInterface = category.GetInstanceNames();
                    logger.Error("存在多个实例,无法创建" + counter.CollectName);
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 数据库测试
        /// </summary>
        /// <returns></returns>
        private static bool DbTest()
        {
            try
            {
                var db = new MongoDbContext();
                var database = db.Database;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Mongodb配置错误：" + ex.Message);
                return false;
            }
        }
    }
}
