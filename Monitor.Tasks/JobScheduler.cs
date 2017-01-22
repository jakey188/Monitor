using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Monitor.Services;
using Monitor.Tasks.Jobs;
using Monitor.Tasks.Quartzs;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;

namespace Monitor.Tasks
{
    public class JobScheduler
    {

        public static void ScheduleJob(Models.Entites.Tasks task,IScheduler _scheduler)
        {
            if (task.TaskId == JobConstant.PerformanceCounterJob.TaskId)
            {
                var performanceCounterJob = PerformanceCounterJob();

                _scheduler.ScheduleJob(performanceCounterJob.Keys.FirstOrDefault(),performanceCounterJob.Values.FirstOrDefault());
            }
            if (task.TaskId == JobConstant.WarningJob.TaskId)
            {
                var warningJob = WarningJob();

                _scheduler.ScheduleJob(warningJob.Keys.FirstOrDefault(),warningJob.Values.FirstOrDefault());
            }
        }

        private static Dictionary<IJobDetail,ITrigger> WarningJob()
        {
            var warningJob = JobConstant.WarningJob;

            var task = new TaskServices().GetTaskDetail(warningJob.TaskId);

            var taskId = task.TaskId.ToString();

            IJobDetail job = JobBuilder
                .Create<WarningJob>()
                .WithIdentity(taskId)
                .Build();

            CronTriggerImpl trigger = new CronTriggerImpl
            {
                CronExpressionString = task.CronExpressionString,
                Name = taskId,
                Description = task.TaskName
            };

            var dic = new Dictionary<IJobDetail,ITrigger> { { job,trigger } };

            return dic;
        }

        private static Dictionary<IJobDetail, ITrigger> PerformanceCounterJob()
        {
            var performanceCounterJob = JobConstant.PerformanceCounterJob;
            
            var task = new TaskServices().GetTaskDetail(performanceCounterJob.TaskId);

            var taskId = task.TaskId.ToString();

            IJobDetail job = JobBuilder
                .Create<PerformanceCounterJob>()
                .WithIdentity(taskId)
                .Build();

            CronTriggerImpl trigger = new CronTriggerImpl
            {
                CronExpressionString = task.CronExpressionString,
                Name = taskId,
                Description = task.TaskName
            };

            var dic = new Dictionary<IJobDetail, ITrigger> {{job, trigger}};

            return dic;
        }

        public static void CreateSystemJob()
        {
            var performanceCounterJob = JobConstant.PerformanceCounterJob;
            var taskService = new TaskServices();
            var warningJob = JobConstant.WarningJob;
            if (taskService.GetTaskDetail(performanceCounterJob.TaskId) == null)
                taskService.AddTask(performanceCounterJob);
            if (taskService.GetTaskDetail(warningJob.TaskId) == null)
                taskService.AddTask(warningJob);
        }
    }
}