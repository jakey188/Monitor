using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Monitor.Web.Core.Jobs;
using Quartz;
using Quartz.Impl;

namespace Monitor.Web.Core
{
    public class JobScheduler
    {
        private IScheduler _scheduler;
        public void Start()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            _scheduler = sf.GetScheduler();

            //var performanceCounterJob = PerformanceCounterJob();

            //_scheduler.ScheduleJob(performanceCounterJob.Keys.FirstOrDefault(),performanceCounterJob.Values.FirstOrDefault());

            var job = JobBuilder.Create<TestJob>().WithIdentity("job1").Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("job1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(60).WithRepeatCount(1)
                    .RepeatForever())
                .Build();


            var trigger2 = TriggerBuilder.Create()
                .WithIdentity("job2")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30).WithRepeatCount(1)
                    .RepeatForever())
                .Build();

            // 创建一个触发器组对象
            Quartz.Collection.ISet<ITrigger> triggers = new Quartz.Collection.HashSet<ITrigger>();
            triggers.Add(trigger);
            triggers.Add(trigger2);
            _scheduler.ScheduleJob(job,triggers,false);


            _scheduler.Start();

            //Thread.Sleep(TimeSpan.FromSeconds(5));

            //_scheduler.Shutdown(true);
        }

        public void Stop()
        {
            if (_scheduler != null)
            {
                _scheduler.Clear();
                _scheduler.Shutdown(true);
            }
        }


        private Dictionary<IJobDetail,ITrigger> PerformanceCounterJob()
        {
            string name = EnmJob.PerformanceCounterJob.ToString();
            IJobDetail job = JobBuilder
                .Create<PerformanceCounterJob>()
                .WithIdentity(name)
                .Build();


            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(name)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(60).WithRepeatCount(1)
                    .RepeatForever())
                .Build();

            var dic = new Dictionary<IJobDetail,ITrigger>();
            dic.Add(job,trigger);

            return dic;
        }
    }
}