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
        public bool AddOrEditTask(Models.Entites.Tasks model,string action="")
        {
            if (action == "edit")
            {
                _taskServices.Update(model);
            }
            else
            {
                _taskServices.AddTask(model);
            }
            if (model.Status == (int)EnmTaskStatus.Run)
            {
                QuartzManager.ScheduleJob(model,true);
            }
            return true;
        }

        /// <summary>
        /// 删除指定id任务
        /// </summary>
        /// <param name="TaskID">任务id</param>
        public void DeleteById(string taskId)
        {
            QuartzManager.DeleteJob(taskId);

            _taskServices.Delete(taskId);
        }

        /// <summary>
        /// 更新任务运行状态
        /// </summary>
        /// <param name="TaskID">任务id</param>
        /// <param name="Status">任务状态</param>
        public void UpdateTaskStatus(string taskId,EnmTaskStatus status)
        {
            if (status == EnmTaskStatus.Run)
            {
                QuartzManager.ResumeJob(taskId);
            }
            else
            {
                QuartzManager.PauseJob(taskId);
            }

            _taskServices.UpdateTaskStatus(taskId,status);
        }

        /// <summary>
        /// 更新任务下次运行时间
        /// </summary>
        /// <param name="TaskID">任务id</param>
        /// <param name="NextFireTime">下次运行时间</param>
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
