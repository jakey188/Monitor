using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Monitor.Models
{
    /// <summary>
    /// 性能监视器快照
    /// </summary>
    [Table("ClusterPerformanceCounterSnapshot")]
    public class ClusterPerformanceCounterSnapshot
    {
        /// <summary>
        /// 主键
        /// </summary>
        public ObjectId Id { get; set; }
        /// <summary>
        /// 监视器对象
        /// </summary>
        public Counter Counter { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public String MachineIP { get; set; }
        /// <summary>
        /// 服务器名称
        /// </summary>
        public String MachineName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// CPU使用率
        /// </summary>
        public float? CPU { get; set; }
        /// <summary>
        /// IIS请求数
        /// </summary>
        public float? IIS { get; set; }
        /// <summary>
        /// 使用内存
        /// </summary>
        public float? Memory { get; set; }
        /// <summary>
        /// 物理磁盘读字节/s
        /// </summary>
        public float? PhysicalDiskRead { get; set; }
        /// <summary>
        /// 物理磁盘写字节/s
        /// </summary>
        public float? PhysicalDiskWrite { get; set; }
    }
}
