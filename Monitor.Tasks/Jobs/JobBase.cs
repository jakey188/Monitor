using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitor.Core;
using Monitor.Services;
using Quartz;

namespace Monitor.Tasks.Jobs
{
    public class JobBase
    {
        public void ExecuteJob(IJobExecutionContext context,Action action)
        {
            try
            {
                var taskId = context.Trigger.Key.Name;
                var task = new TaskServices().GetTaskDetail(taskId);

                Debug.WriteLine(taskId + task.CronExpressionString + "：" + DateTime.Now);
                action();
                new TaskLogsService().Add(taskId,"成功执行完毕");
            }
            catch (Exception ex)
            {
                new JobExecutionException(ex) { RefireImmediately = true };
                //true  是立即重新执行任务 
                Logger.Error("任务错误",ex);
            }
        }
    }
}
