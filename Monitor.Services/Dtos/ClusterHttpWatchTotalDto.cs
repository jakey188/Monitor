using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services.Dtos
{
    public class ClusterHttpWatchTotalDto
    {
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 平均耗时
        /// </summary>
        public float AvgTime { get; set; }
        /// <summary>
        /// 最大耗时
        /// </summary>
        public float MaxTime { get; set; }
        /// <summary>
        /// 最小耗时
        /// </summary>
        public float MinTime { get; set; }
        /// <summary>
        /// 请求次数
        /// </summary>
        public int CallCount { get; set; }
    }
}
