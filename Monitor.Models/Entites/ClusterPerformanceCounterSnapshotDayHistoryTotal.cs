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
    /// 性能监视器按照天统计历史记录表//统计安防https://www.yangrunwei.com/a/44.html
    /// </summary>
    [Table("ClusterPerformanceCounterSnapshotDayHistoryTotal")]
    public class ClusterPerformanceCounterSnapshotDayHistoryTotal
    {
        /// <summary>
        /// 主键
        /// </summary>
        public ObjectId Id { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public String MachineIP { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// CPU使用率总数
        /// </summary>
        [BsonRepresentation(BsonType.Double,AllowTruncation = true)]
        public float? CPUAvg { get; set; }
        /// <summary>
        /// CPU使用率最大数
        /// </summary>
        [BsonRepresentation(BsonType.Double,AllowTruncation = true)]
        public float? CPUMax { get; set; }
        /// <summary>
        /// IIS请求数总数
        /// </summary>
        [BsonRepresentation(BsonType.Double,AllowTruncation = true)]
        public float? IISAvg { get; set; }
        /// <summary>
        /// IIS请求数最大数
        /// </summary>
        [BsonRepresentation(BsonType.Double,AllowTruncation = true)]
        public float? IISMax { get; set; }
        /// <summary>
        /// 使用内存总数
        /// </summary>
        [BsonRepresentation(BsonType.Double,AllowTruncation = true)]
        public float? MemoryAvg { get; set; }
        /// <summary>
        /// 使用内存最大数
        /// </summary>
        [BsonRepresentation(BsonType.Double,AllowTruncation = true)]
        public float? MemoryMax { get; set; }
    }
}
