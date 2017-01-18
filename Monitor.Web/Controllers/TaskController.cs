using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monitor.Models.Entites;
using Monitor.Services;
using Monitor.Tasks.Quartzs;

namespace Monitor.Web.Controllers
{
    public class TaskController:BaseController
    {
        private readonly TaskServices _taskSerivice;

        public TaskController()
        {
            _taskSerivice = new TaskServices();
        }

        // GET: Task
        public ActionResult TaskList()
        {
            return View();
        }

        public ActionResult TaskDetails(string taskId)
        {
            var task = new Monitor.Models.Entites.Tasks();
            if (!string.IsNullOrEmpty(taskId))
                task = _taskSerivice.GetTaskDetail(taskId);
            return PartialView("_TaskDetails",task);
        }

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("~/api/task/list")]
        public JsonResult GetTaskList(string taskName,int pageIndex = 1,int pageSize = 10)
        {
            int total;
            var data = _taskSerivice.GetTaskList(taskName,pageIndex,pageSize,out total);
            return Success(data,pageIndex,pageSize,total);
        }

        /// <summary>
        /// 添加或者修改任务
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="taskParam">任务参数</param>
        /// <param name="rdoTypes">任务类别</param>
        /// <param name="remoteUrl">远程执行API</param>
        /// <param name="cronExpressionString">Cron表达式</param>
        /// <param name="cronRemark">Cron说明</param>
        /// <param name="rdoStatus">任务状态</param>
        /// <returns></returns>
        [Route("~/api/task/add"), HttpPost]
        public JsonResult AddTask(string taskId,string taskName,string taskParam,int rdoTypes,string remoteUrl,string cronExpressionString,string cronRemark,int rdoStatus)
        {
            if (QuartzManager.ValidExpression(cronExpressionString))
                return Fail("Quartz Cron 表达式错误");

            if (!string.IsNullOrEmpty(taskId))
            {
                var task = _taskSerivice.GetTaskDetail(taskId);
                if (task != null)
                {
                    task.TaskName = taskName;
                    task.TaskParam = taskParam;
                    task.Types = rdoTypes;
                    task.RemoteUrl = remoteUrl;
                    task.CronExpressionString = cronExpressionString;
                    task.CronRemark = cronRemark;
                    task.Status = rdoStatus;
                    task.ModifyTime = DateTime.Now;
                }
                TaskManager.Instance.AddOrEditTask(task,"edit");
            }
            else
            {

                var task = new Monitor.Models.Entites.Tasks();
                task.TaskId = Guid.NewGuid();
                task.TaskName = taskName;
                task.TaskParam = taskParam;
                task.Types = rdoTypes;
                task.RemoteUrl = remoteUrl;
                task.CronExpressionString = cronExpressionString;
                task.CronRemark = cronRemark;
                task.Status = rdoStatus;
                task.ModifyTime = DateTime.Now;

                TaskManager.Instance.AddOrEditTask(task);
            }
            return Success();
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="status">任务状态1运行,0停止</param>
        /// <returns></returns>
        [Route("~/api/task/update/status")]
        public ActionResult UpdateTaskStatus(string taskId,int status)
        {
            TaskManager.Instance.UpdateTaskStatus(taskId,(EnmTaskStatus)status);

            return Success();
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <returns></returns>
        [Route("~/api/task/delete")]
        public ActionResult DeleteTask(string taskId)
        {
            TaskManager.Instance.DeleteById(taskId);

            return Success();
        }
    }
}