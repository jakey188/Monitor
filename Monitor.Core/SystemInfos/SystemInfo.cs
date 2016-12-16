using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Core.SystemInfos
{
    public class SystemInfo
    {
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
        /// 可用物理内存
        /// </summary>
        public string FreePhysicalMemory { get; set; }
        /// <summary>
        /// /总的虚拟内存
        /// </summary>
        public string TotalVirtualMemorySize { get; set; }
        /// <summary>
        /// 可用虚拟内存
        /// </summary>
        public string FreeVirtualMemory { get; set;  }
        /// <summary>
        /// CPU名称
        /// </summary>
        public string CPUName { get; set; }
        /// <summary>
        /// CPU个数
        /// </summary>
        public int CPUCount { get; set; }
        

        public SystemInfo()
        {
            GetSystemInfo();
        }

        private void GetSystemInfo()
        {
            OSVersion = Environment.OSVersion.ToString();
            MachineName = Environment.MachineName; 
            MachineIp = Dns.GetHostAddresses(Dns.GetHostName()).Where(a => a.AddressFamily == AddressFamily.InterNetwork).Select(add => add.ToString()).FirstOrDefault();
            Country = System.Globalization.CultureInfo.CurrentCulture.DisplayName; 
            TimeZone = System.TimeZone.CurrentTimeZone.StandardName; 
            CPUCount = Environment.ProcessorCount; 

            var operatingSystemManagementClass = new ManagementClass("Win32_OperatingSystem");

            var operatingSystemCollection = operatingSystemManagementClass.GetInstances();

            foreach (ManagementObject mObject in operatingSystemCollection)
            {
                OSName = mObject["Caption"].ToString(); 
                MachineName = mObject["CSName"].ToString(); 
                TotalVisibleMemorySize = ((ulong)mObject["TotalVisibleMemorySize"] / 1024.0 / 1024).ToString("#0.00") + "G";  //获取总的物理内存
                FreePhysicalMemory = ((ulong)mObject["FreePhysicalMemory"] / 1024.0 / 1024).ToString("#0.00") + "G";  //获取可用物理内存
                TotalVirtualMemorySize = ((ulong)mObject["TotalVirtualMemorySize"] / 1024.0 / 1024).ToString("#0.00") + "G";   //获取总的虚拟内存
                FreeVirtualMemory = ((ulong)mObject["FreeVirtualMemory"] / 1024.0 / 1024).ToString("#0.00") + "G";  //获取可用虚拟内存
            }

            var processorManagementClass = new ManagementClass("Win32_Processor");
            var processorCollection = processorManagementClass.GetInstances();

            foreach (ManagementObject mObject in processorCollection)
            {
                CPUName = mObject["Name"].ToString();
            }
        }
    }
}
