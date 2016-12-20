using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Monitor.Watch.Data
{
    /// <summary>
    /// HTTP请求耗时
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

        public void AddRange(List<ClusterHttpWatch> httpWatchList)
        {
            var db = new WatchDbContext();
            db.AddRange(httpWatchList);
        }
    }
}
