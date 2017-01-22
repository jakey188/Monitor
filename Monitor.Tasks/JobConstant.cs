using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Monitor.Models.Entites;

namespace Monitor.Tasks
{
    public class JobConstant
    {
        public static Models.Entites.Tasks PerformanceCounterJob
        {

            get
            {
                var task = new Models.Entites.Tasks
                {
                    TaskId = new Guid("0216967d-d631-4777-8a94-fa856ed17dae"),
                    Status = (int) EnmTaskStatus.Run,
                    CreatedTime = DateTime.Now,
                    CronExpressionString = "0 0 0/1 * * ? *",
                    CronRemark = "每小时运行一次",
                    TaskName = "性能计数器统计",
                    Types = (int) EnmTaskTypes.System
                };
                return task;
            }
        }

        public static Models.Entites.Tasks WarningJob
        {

            get
            {
                var task = new Models.Entites.Tasks
                {
                    TaskId = new Guid("684bce1e-16f7-408c-a724-c57551fae020"),
                    Status = (int)EnmTaskStatus.Run,
                    CreatedTime = DateTime.Now,
                    CronExpressionString = "0/1 * * * * ? *",
                    CronRemark = "每秒运行一次",
                    TaskName = "警报任务通知",
                    Types = (int)EnmTaskTypes.System
                };
                return task;
            }
        }
    }
}