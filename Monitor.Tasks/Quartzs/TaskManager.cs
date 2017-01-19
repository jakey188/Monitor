using System;
using System.Collections;
using System.Collections.Generic;
using Monitor.Core;
using Monitor.Models.Entites;
using Monitor.Services;

namespace Monitor.Tasks.Quartzs
{
    /// <summary>
    /// 任务管理类
    /// </summary>
    public class TaskManager:Singleton<TaskManager>
    {
        /// <summary>
        /// TaskServices
        /// </summary>
        private readonly TaskServices _taskServices = new TaskServices();

        /// <summary>
        /// 添加或者修改任务
        /// </summary>
        /// <param name="model"></param>
        /// <param name="action">edit为修改</param>
        /// <returns></returns>
        public OperationResult AddOrEditTask(Models.Entites.Tasks model, string action = "")
        {
            var result = new OperationResult(OperationResultType.Success);

            if (model.Status != (int) EnmTaskStatus.Run) return result;

            var jobResult = QuartzManager.ScheduleJob(model, true);

            if (jobResult.ResultType != OperationResultType.Success)
            {
                result.Message = jobResult.Message;
                return result;
            }
            if (action == "edit")
            {
                _taskServices.Update(model);
            }
            else
            {
                _taskServices.AddTask(model);
            }

            return result;
        }

        /// <summary>
        /// 删除指定id任务
        /// </summary>
        /// <param name="taskId">任务id</param>
        public OperationResult DeleteById(string taskId)
        {
            var result = QuartzManager.DeleteJob(taskId);
            if (result.ResultType == OperationResultType.Success)
                _taskServices.Delete(taskId);

            return result;
        }

        /// <summary>
        /// 更新任务运行状态
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="status">任务状态</param>
        public OperationResult UpdateTaskStatus(string taskId, EnmTaskStatus status)
        {
            var result = status == EnmTaskStatus.Run ? QuartzManager.ResumeJob(taskId) : QuartzManager.PauseJob(taskId);
            if (result.ResultType == OperationResultType.Success)
                _taskServices.UpdateTaskStatus(taskId, status);
            return result;
        }

        /// <summary>
        /// 更新任务下次运行时间
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="nextFireTime">下次运行时间</param>
        public void UpdateNextFireTime(string taskId,DateTime nextFireTime)
        {
            _taskServices.UpdateNextFireTime(taskId,nextFireTime);
        }

        /// <summary>
        /// 任务完成后，更新上次执行时间
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="recentRunTime">上次执行时间</param>
        public void UpdateRecentRunTime(string taskId,DateTime recentRunTime)
        {
            _taskServices.UpdateRecentRunTime(taskId,recentRunTime);
        }

        /// <summary>
        /// 从数据库中读取全部任务列表
        /// </summary>
        /// <returns></returns>
        private IList<Monitor.Models.Entites.Tasks> GetAllTaskList()
        {
            return _taskServices.GetTaskList();
        }

        /// <summary>
        /// 获取所有启用的任务
        /// </summary>
        /// <returns>所有启用的任务</returns>
        public IList<Monitor.Models.Entites.Tasks> GetTaskRunList()
        {
            return _taskServices.GetAllTaskRunList();
        }

        /// <summary>
        /// 获取所有启用的任务
        /// </summary>
        /// <returns>所有启用的任务</returns>
        public Monitor.Models.Entites.Tasks GetTaskDetail(string taskId)
        {
            return _taskServices.GetTaskDetail(taskId);
        }
    }
}
