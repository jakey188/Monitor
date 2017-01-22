using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monitor.Services;
using Monitor.Tasks.Quartzs;

namespace Monitor.Web.Controllers
{
    public class ApiController : Controller
    {
        // GET: Api
        public JsonResult Index()
        {
            var task = new Monitor.Models.Entites.Tasks();
            task.TaskId = Guid.NewGuid();
            task.Status = 1;
            task.CronExpressionString = "0/60 * * * * ?";
            new TaskServices().AddTask(task);
            return Json(1);
        }

        public JsonResult Pause(string taskId)
        {
            QuartzManager.PauseJob(taskId);
            return Json(1);
        }

        public JsonResult Start(string taskId)
        {
            QuartzManager.ResumeJob(taskId);
            return Json(1);
        }

        public JsonResult Start1(string taskId)
        {
            var model = TaskManager.Instance.GetTaskDetail(taskId);
            model.CronExpressionString = "0/10 * * * * ?";
            TaskManager.Instance.AddOrEditTask(model,"edit");
            return Json(1);
        }
    }
}