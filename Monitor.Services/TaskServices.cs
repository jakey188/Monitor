using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitor.Core;
using Monitor.Data;
using Monitor.Models.Entites;

namespace Monitor.Services
{
    public class TaskServices
    {
        /// <summary>
        /// 添加计划任务
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Tasks task)
        {
            var db = new MongoDbContext();
            db.Add(task);
        }

        /// <summary>
        /// 修改计划任务
        /// </summary>
        /// <param name="task"></param>
        public void Update(Tasks task)
        {
            var db = new MongoDbContext();
            db.Update<Tasks>(task);
        }

        /// <summary>
        /// 删除指定id任务
        /// </summary>
        /// <param name="TaskID">任务id</param>
        public void Delete(string taskId)
        {
            var db = new MongoDbContext();
            var guid = new Guid(taskId);
            db.Delete<Tasks>(x => x.TaskId == guid);
        }

        /// <summary>
        /// 更新任务运行状态
        /// </summary>
        /// <param name="TaskID">任务id</param>
        /// <param name="Status">任务状态</param>
        public void UpdateTaskStatus(string taskId,EnmTaskStatus enmTaskStatus)
        {
            var db = new MongoDbContext();
            var guid = new Guid(taskId);
            var status = (int)enmTaskStatus;
            db.Update<Tasks>(x => x.TaskId == guid,x => new Tasks { Status = status });
        }

        /// <summary>
        /// 更新任务下次运行时间
        /// </summary>
        /// <param name="TaskID">任务id</param>
        /// <param name="NextFireTime">下次运行时间</param>
        public void UpdateNextFireTime(string taskId,DateTime nextFireTime)
        {
            var db = new MongoDbContext();
            var guid = new Guid(taskId);
            db.Update<Tasks>(x => x.TaskId == guid,x => new Tasks { NextFireTime = nextFireTime });
        }

        /// <summary>
        /// 任务完成后，更新上次执行时间
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="recentRunTime">上次执行时间</param>
        public void UpdateRecentRunTime(string taskId,DateTime recentRunTime)
        {
            var db = new MongoDbContext();
            var guid = new Guid(taskId);
            db.Update<Tasks>(x => x.TaskId == guid,x => new Tasks { RecentRunTime = recentRunTime });
        }

        public Tasks GetTaskDetail(string taskId)
        {
            var db = new MongoDbContext();
            var guid = new Guid(taskId);
            return db.Where<Tasks>(x => x.TaskId == guid).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有启用的任务
        /// </summary>
        /// <returns>所有启用的任务</returns>
        public IList<Tasks> GetAllTaskRunList()
        {
            var db = new MongoDbContext();
            var status = (int)EnmTaskStatus.Run;
            return db.Where<Tasks>(x => x.Status == status).ToList();
        }

        /// <summary>
        /// 获取所有计划任务
        /// </summary>
        /// <returns></returns>
        public IList<Tasks> GetTaskList()
        {
            var db = new MongoDbContext();
            return db.Where<Tasks>().ToList();
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IList<Tasks> GetTaskList(string taskName,int pageIndex,int pageSize,out int total)
        {
            var db = new MongoDbContext();
            var query = db.Where<Tasks>();
            if (!string.IsNullOrEmpty(taskName))
                query = query.Where(x => x.TaskName == taskName);
            return query.OrderByDescending(x => x.Id).ToPageList(pageIndex,pageSize,out total);
        }
    }
}
