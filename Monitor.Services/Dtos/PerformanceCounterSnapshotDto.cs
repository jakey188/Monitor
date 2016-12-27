using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services.Dtos
{
    public class PerformanceCounterSnapshotDto
    {
        public int Date { get; set; }
        public float? IIS { get; set; }
        public float? CPU { get; set; }
        public float? Memory { get; set; }
    }
}
