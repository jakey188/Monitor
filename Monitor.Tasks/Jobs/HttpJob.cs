using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Monitor.Core;
using Monitor.Services;
using Quartz;

namespace Monitor.Tasks.Jobs
{
    public class HttpJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                string taskId = context.Trigger.Key.Name;
                var _jobService = new TaskServices();
                var task = _jobService.GetTaskDetail(taskId);
                Logger.Info(task.TaskName + " 开始" + task.TaskId.ToString());

                var httpClient = new HttpClient();

                //httpClient.GetAsync("http://www.baidu.com").Result.Content.ToString();
                Debug.WriteLine(taskId + task.CronExpressionString+"："+DateTime.Now);

                Logger.Info(task.TaskName + " 结束" + task.TaskId.ToString() + "成功执行");
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                //true  是立即重新执行任务 
                e2.RefireImmediately = true;

                Logger.Error("任务错误",ex);
            }
        }
    }
}
