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
    [Table("Logs")]
    public class Logs
    {
        public ObjectId Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 当前EnmLogs消息
        /// </summary>
        public int Types { get; set; }
        /// <summary>
        /// 描述当前异常的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 异常具体消息
        /// </summary>
        public string Context { get; set; }
    }

    public enum EnmLogs
    {
        监控中心系统日志 = 1
    }
}
