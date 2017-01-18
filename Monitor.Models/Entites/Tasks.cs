using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Monitor.Models.Entites
{
    /// <summary>
    /// 任务实体
    /// </summary>
    [Table("Tasks")]
    public class Tasks
    {
        /// <summary>
        /// 主键
        /// </summary>
        public ObjectId Id { get; set; }
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 任务执行参数
        /// </summary>
        public string TaskParam { get; set; }

        /// <summary>
        /// 运行频率设置
        /// </summary>
        public string CronExpressionString { get; set; }

        /// <summary>
        /// 任务运频率中文说明
        /// </summary>
        public string CronRemark { get; set; }

        /// <summary>
        /// 任务作业接口
        /// </summary>
        /// <remarks>只限制get请求</remarks>
        public string RemoteUrl { get; set; }

        /// <summary>
        /// 任务状态,开始,停止
        /// <see cref="EnmTaskStatus"/>
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 任务类别,系统任务,外部任务
        /// <see cref="EnmTaskTypes"/>
        /// </summary>
        public int Types { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 任务修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 任务最近运行时间
        /// </summary>
        public DateTime? RecentRunTime { get; set; }

        /// <summary>
        /// 任务下次运行时间
        /// </summary>
        public DateTime? NextFireTime { get; set; }

        /// <summary>
        /// 任务备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否删除
        /// <see cref="EnmTaskDelete"/>
        /// </summary>
        public int IsDelete { get; set; }
    }

    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum EnmTaskStatus
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        Run = 1,

        /// <summary>
        /// 停止状态
        /// </summary>
        Stop = 0
    }

    /// <summary>
    /// 任务类别枚举
    /// </summary>
    public enum EnmTaskTypes
    {
        /// <summary>
        /// 系统任务
        /// </summary>
        System = 1,

        /// <summary>
        /// 外部任务
        /// </summary>
        Outside = 0
    }

    public enum EnmTaskDelete
    {
        /// <summary>
        /// 未删除
        /// </summary>
        UnDelete = 1,
        /// <summary>
        /// 已删除
        /// </summary>
        OnDelete = 0
    }
}
