﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monitor.Core;
using Monitor.Models.Entites;
using Monitor.Services;
using Monitor.Tasks.Quartzs;

namespace Monitor.Web.Controllers
{
    public class TaskController:BaseController
    {
        private readonly TaskServices _taskSerivice;
        private readonly TaskLogsService _taskLogsService;

        public TaskController()
        {
            _taskSerivice = new TaskServices();
            _taskLogsService = new TaskLogsService();
        }

        /// <summary>
        /// 任务管理视图
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskList()
        {
            return View();
        }

        /// <summary>
        /// 任务日志视图
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskLogs()
        {
            return View();
        }

        /// <summary>
        /// 任务添加局部视图
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ActionResult TaskDetails(string taskId)
        {
            var task = new Models.Entites.Tasks();
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
        /// 获取任务日志
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("~/api/tasklog/list")]
        public JsonResult GetTaskLogList(string taskId, string startDate,string endDate,int pageIndex = 1,int pageSize = 10)
        {
            int total;
            DateTime? start = string.IsNullOrEmpty(startDate) ? null : (DateTime?)DateTime.Parse(startDate);
            DateTime? end = string.IsNullOrEmpty(endDate) ? null : (DateTime?)DateTime.Parse(endDate);
            var data = _taskLogsService.GetTaskLogList(start,end, taskId,pageIndex,pageSize,out total);
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
        public JsonResult AddTask(string taskId, string taskName, string taskParam, int rdoTypes, string remoteUrl,
            string cronExpressionString, string cronRemark, int rdoStatus)
        {
            if (!QuartzManager.ValidExpression(cronExpressionString))
                return Fail("Quartz Cron 表达式错误");

            OperationResult result;

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
                result = TaskManager.Instance.AddOrEditTask(task, "edit");
            }
            else
            {
                var task = new Models.Entites.Tasks
                {
                    TaskId = Guid.NewGuid(),
                    TaskName = taskName,
                    TaskParam = taskParam,
                    Types = rdoTypes,
                    RemoteUrl = remoteUrl,
                    CronExpressionString = cronExpressionString,
                    CronRemark = cronRemark,
                    Status = rdoStatus,
                    ModifyTime = DateTime.Now
                };

                result = TaskManager.Instance.AddOrEditTask(task);
            }
            return result.ResultType != OperationResultType.Success
                ? Fail(result.Message)
                : Success(string.IsNullOrEmpty(result.Message) ? "操作成功" : result.Message);
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
            var result = TaskManager.Instance.UpdateTaskStatus(taskId,(EnmTaskStatus)status);
            if (result.ResultType != OperationResultType.Success)
                return Fail(result.Message);
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
            var result = TaskManager.Instance.DeleteById(taskId);
            if (result.ResultType != OperationResultType.Success)
                return Fail(result.Message);
            return Success();
        }
    }
}