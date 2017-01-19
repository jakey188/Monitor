using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Reflection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Monitor.Core;
using Quartz.Impl.Triggers;
using Monitor.Models.Entites;
using Monitor.Tasks.Jobs;
using Quartz.Impl.Matchers;

namespace Monitor.Tasks.Quartzs
{
    /// <summary>
    /// 任务处理帮助类
    /// </summary>
    public class QuartzManager
    {
        private static IScheduler scheduler = null;

        /// <summary>
        /// 初始化 远程Quartz服务器中的，各个Scheduler实例。
        /// 提供给远程管理端的后台，用户获取Scheduler实例的信息。
        /// </summary>
        public static void InitRemoteScheduler()
        {
            try
            {
                NameValueCollection properties = new NameValueCollection();

                properties["quartz.scheduler.instanceName"] = "ExampleQuartzScheduler";

                properties["quartz.scheduler.proxy"] = "true";

                //properties["quartz.scheduler.proxy.address"] = string.Format("{0}://{1}:{2}/QuartzScheduler",scheme,server,port);

                ISchedulerFactory sf = new StdSchedulerFactory(properties);

                scheduler = sf.GetScheduler();
            }
            catch (Exception ex)
            {
                Logger.Error("初始化远程任务管理器失败！",ex);
            }
        }

        /// <summary>
        /// 启用任务调度
        /// 启动调度时会把任务表中状态为“执行中”的任务加入到任务调度队列中
        /// </summary>
        public static void StartScheduler()
        {
            try
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                scheduler = schedulerFactory.GetScheduler();
                Quartz.Collection.ISet<ITrigger> triggers = new Quartz.Collection.HashSet<ITrigger>();
                if (!scheduler.IsStarted)
                {
                    scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener(),GroupMatcher<TriggerKey>.AnyGroup());
                    var type = typeof(HttpJob);
                    
                    var listTask = TaskManager.Instance.GetTaskRunList();

                    if (listTask != null && listTask.Count > 0)
                    {
                        foreach (var task in listTask)
                        {
                            if (ValidExpression(task.CronExpressionString))
                            {
                                var job = new JobDetailImpl(task.TaskId.ToString(),type);

                                //CronTriggerImpl trigger1 = new CronTriggerImpl
                                //{
                                //    CronExpressionString = task.CronExpressionString,
                                //    Name = task.TaskId.ToString(),
                                //    Description = task.TaskName
                                //};

                                //withMisfireHandlingInstructionDoNothing 不触发立即执行,等待下次Cron触发频率到达时刻开始按照Cron频率依次执行
                                //withMisfireHandlingInstructionIgnoreMisfires 以错过的第一个频率时间立刻开始执行——重做错过的所有频率周期后——当下一次触发频率发生时间大于当前时间后，再按照正常的Cron频率依次执行
                                //withMisfireHandlingInstructionFireAndProceed以当前时间为触发频率立刻触发一次执行,然后按照Cron频率依次执行


                                var trigger = TriggerBuilder
                                    .Create()
                                    .WithDescription(task.TaskName)
                                    .WithIdentity(task.TaskId.ToString())
                                    .WithCronSchedule(task.CronExpressionString,
                                        x => x.WithMisfireHandlingInstructionFireAndProceed()
                                        )
                                        .StartNow()
                                    .Build();
                                //triggers.Add(trigger);
                                scheduler.ScheduleJob(job,trigger);
                            }
                        }
                        //scheduler.ScheduleJob(job,triggers,false);
                        scheduler.Start();
                    }
                    Logger.Info("任务调度启动成功！");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("任务调度启动失败！",ex);
            }
        }

        /// <summary>
        /// 删除现有任务
        /// </summary>
        /// <param name="jobKey"></param>
        public static OperationResult DeleteJob(string jobKey)
        {
            if (scheduler == null)
                return new OperationResult(OperationResultType.Error,"作业scheduler不存在,任务无法删除");

            JobKey jk = new JobKey(jobKey);
            if (scheduler.CheckExists(jk))
            {
                scheduler.DeleteJob(jk);
                Logger.Info(string.Format("任务“{0}”已经删除",jobKey));
                return new OperationResult(OperationResultType.Success);
            }
            return new OperationResult(OperationResultType.Error,"任务不存在");
        }

        /// <summary>
        /// 启用任务
        /// <param name="task">任务信息</param>
        /// <param name="isDeleteOldTask">是否删除原有任务</param>
        /// <returns>返回任务trigger</returns>
        /// </summary>
        public static OperationResult ScheduleJob(Models.Entites.Tasks task,bool isDeleteOldTask = false)
        {
            if (scheduler == null)
                return new OperationResult(OperationResultType.Error,"作业scheduler不存在,任务无法启动");

            JobKey jk = new JobKey(task.TaskId.ToString());

            if (!scheduler.CheckExists(jk))
                return new OperationResult(OperationResultType.Error,"任务不存在");

            if (isDeleteOldTask)
                DeleteJob(task.TaskId.ToString());

            if (!ValidExpression(task.CronExpressionString))
                return new OperationResult(OperationResultType.Error,task.CronExpressionString + "不是正确的Cron表达式,无法启动该任务!");

            IJobDetail job = new JobDetailImpl(task.TaskId.ToString(),typeof(HttpJob));
            CronTriggerImpl trigger = new CronTriggerImpl
            {
                CronExpressionString = task.CronExpressionString,
                Name = task.TaskId.ToString(),
                Description = task.TaskName
            };
            scheduler.ScheduleJob(job,trigger);
            if (task.Status == (int)EnmTaskStatus.Stop)
            {
                scheduler.PauseJob(jk);
            }
            else
            {
                Logger.Info(string.Format("任务“{0}”启动成功,未来5次运行时间如下:",task.TaskName));
                List<DateTime> list = GetNextFireTime(task.CronExpressionString,5);
                foreach (var time in list)
                {
                    Logger.Info(time.ToString());
                }
            }

            return new OperationResult(OperationResultType.Success);
        }


        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="jobKey"></param>
        public static OperationResult PauseJob(string jobKey)
        {
            if (scheduler == null)
                return new OperationResult(OperationResultType.Error,"作业scheduler不存在,任务无法停止");

            JobKey jk = new JobKey(jobKey);

            if (!scheduler.CheckExists(jk))
                return new OperationResult(OperationResultType.Error,"任务不存在");

            scheduler.PauseJob(jk);
            Logger.Info(string.Format("任务“{0}”已经暂停",jobKey));
            return new OperationResult(OperationResultType.Success);
        }

        /// <summary>
        /// 恢复运行暂停的任务
        /// </summary>
        /// <param name="jobKey">任务key</param>
        public static OperationResult ResumeJob(string jobKey)
        {
            if (scheduler == null)
                return new OperationResult(OperationResultType.Error,"作业scheduler不存在,任务无法启动");

            JobKey job = new JobKey(jobKey);

            if (!scheduler.CheckExists(job))
                return new OperationResult(OperationResultType.Error,"任务不存在");

            scheduler.ResumeJob(job);
            Logger.Info(string.Format("任务“{0}”恢复运行",jobKey));
            return new OperationResult(OperationResultType.Success);
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        public static void StopSchedule()
        {
            try
            {
                if (scheduler == null)
                    return;
                //判断调度是否已经关闭
                if (!scheduler.IsShutdown)
                {
                    //等待任务运行完成
                    scheduler.Shutdown(true);
                    Logger.Info("任务调度停止！");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("任务调度停止失败！", ex);
            }
        }

        /// <summary>
        /// 校验字符串是否为正确的Cron表达式
        /// </summary>
        /// <param name="cronExpression">带校验表达式</param>
        /// <returns></returns>
        public static bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }

        /// <summary>
        /// 获取任务在未来周期内哪些时间会运行
        /// </summary>
        /// <param name="CronExpressionString">Cron表达式</param>
        /// <param name="numTimes">运行次数</param>
        /// <returns>运行时间段</returns>
        public static List<DateTime> GetNextFireTime(string CronExpressionString, int numTimes)
        {
            if (numTimes < 0)
            {
                throw new Exception("参数numTimes值大于等于0");
            }
            //时间表达式
            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(CronExpressionString).Build();
            IList<DateTimeOffset> dates = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, numTimes);
            List<DateTime> list = new List<DateTime>();
            foreach (DateTimeOffset dtf in dates)
            {
                list.Add(TimeZoneInfo.ConvertTimeFromUtc(dtf.DateTime, TimeZoneInfo.Local));
            }
            return list;
        }
    }
}