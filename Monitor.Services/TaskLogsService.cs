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
    public class TaskLogsService
    {
        public void Add(TaskLogs log)
        {
            var db = new MongoDbContext();
            db.Add<TaskLogs>(log);
        }

        public void Add(string taskId,string remarks)
        {
            var db = new MongoDbContext();
            var taskLog = new TaskLogs
            {
                TaskId = taskId,
                Remarks = remarks,
                CreateTime = DateTime.Now
            };
            db.Add<TaskLogs>(taskLog);
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="end"></param>
        /// <param name="taskId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public IList<TaskLogs> GetTaskLogList(DateTime? start, DateTime? end, string taskId, int pageIndex,
            int pageSize, out int total)
        {
            var db = new MongoDbContext();
            var query = db.Where<TaskLogs>();
            if (!string.IsNullOrEmpty(taskId))
                query = query.Where(x => x.TaskId.Contains(taskId));

            if (start.HasValue && end.HasValue)
                query = query.Where(x => x.CreateTime > start.Value && x.CreateTime < end.Value);

            return query.OrderByDescending(x => x.Id).ToPageList(pageIndex, pageSize, out total);
        }
    }
}
