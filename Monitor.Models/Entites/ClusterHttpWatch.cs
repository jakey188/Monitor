using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monitor.Models
{
    /// <summary>
    /// http耗时记录
    /// </summary>
    [Table("ClusterHttpWatch")]
    public class ClusterHttpWatch
    {
        public ObjectId Id { get; set; }
        /// <summary>
        /// 耗时/毫秒
        /// </summary>
        [BsonRepresentation(BsonType.Double,AllowTruncation = true)]
        public float Timevalue { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Ip
        /// </summary>
        public string Ip { get; set; }
    }
}
