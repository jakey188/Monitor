using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Monitor.Models
{
    /// <summary>
    /// 集群计算机信息
    /// </summary>
    [Table("ClusterComputerInfo")]
    public class ClusterComputerInfo
    {
        /// <summary>
        /// 主键编号
        /// </summary>
        public ObjectId Id { get; set; }
        /// <summary>
        /// OS名称
        /// </summary>
        public string OSName { get; set; }
        /// <summary>
        /// OS版本
        /// </summary>
        public string OSVersion { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        ///Ip
        /// </summary>
        public string MachineIp { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 时区
        /// </summary>
        public string TimeZone { get; set; }
        /// <summary>
        /// 总的物理内存
        /// </summary>
        public string TotalVisibleMemorySize { get; set; }
        /// <summary>
        /// 总的虚拟内存
        /// </summary>
        public string TotalVirtualMemorySize { get; set; }
        /// <summary>
        /// CPU名称
        /// </summary>
        public string CPUName { get; set; }
        /// <summary>
        /// CPU个数
        /// </summary>
        public int CPUCount { get; set; }
    }
}
