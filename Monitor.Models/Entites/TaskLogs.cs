using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Monitor.Models.Entites
{
    /// <summary>
    /// 任务日志
    /// </summary>
    [Table("TaskLogs")]
    public class TaskLogs
    {
        /// <summary>
        /// 主键
        /// </summary>
        public ObjectId Id { get; set; }
        public string TaskId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }
        public string Remarks { get; set; }
    }
}
