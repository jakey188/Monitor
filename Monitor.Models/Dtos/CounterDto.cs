using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Models
{
    /// <summary>
    /// 监视器对象
    /// </summary>
    public class CounterDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 监控器名称
        /// </summary>
        public string CollectName { get; set; }
        /// <summary>
        /// 性能监视器类别
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 性能监视器名称
        /// </summary>
        public string CounterName { get; set; }
        /// <summary>
        /// 性能监视器实例名
        /// </summary>
        public string InstanceName { get; set; }
    }

    public enum EnmCounter
    {
        CPU = 1,
        内存 = 2,
        物理磁盘读字节 = 3,
        物理磁盘写字节 = 4,
        IIS请求 = 5
    }
}
