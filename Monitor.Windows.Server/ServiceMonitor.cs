using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;
using Monitor.Core.SystemInfos;
using Monitor.Models;
using Monitor.Services;
using NLog.Internal;
using System.Configuration;

namespace Monitor.Windows.Server
{
    sealed class ServiceMonitor
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IList<Tuple<int,string, PerformanceCounter>> serviceCounters;
        private IList<CounterDto> serviceCounterList;

        public void Monitor(object state)
        {
            ManualResetEvent stopHandle = (ManualResetEvent)state;
            try
            {
                var os = new SystemInfo();

                string isDbSave = System.Configuration.ConfigurationManager.AppSettings["IsDbSave"].ToString();
                string performanceCollectBackgroundTaskSleepTime = System.Configuration.ConfigurationManager.AppSettings["PerformanceCollectBackgroundTaskSleepTime"].ToString();

                GlobalConfigure.PerformanceCollectBackgroundTaskSleepTime = Convert.ToInt32(performanceCollectBackgroundTaskSleepTime);
                GlobalConfigure.IsDbSave = Convert.ToBoolean(isDbSave);
                
                Initialize(os);

                if (GlobalConfigure.IsDbSave)
                {
                    AddComputerInfo(os);

                    while (!PerfmonWorker.ShouldStop)
                    {
                        Thread.Sleep(GlobalConfigure.PerformanceCollectBackgroundTaskSleepTime);

                        DateTime timeStamp = DateTime.Now;
    
                        var snapshot = new ClusterPerformanceCounterSnapshot();
                        snapshot.CreateTime = timeStamp;
                        snapshot.MachineName = os.MachineName;
                        snapshot.MachineIP = os.MachineIp;
                        for (int i = 0;i < serviceCounters.Count;i++)
                        {
                            var counter = serviceCounterList.First(x => x.Id == serviceCounters[i].Item1);
                            try
                            {
                                var value = serviceCounters[i].Item3.NextValue();
                                switch (counter.Id)
                                {
                                    case (int)EnmCounter.CPU:
                                        snapshot.CPU = value;
                                        break;
                                    case (int)EnmCounter.IIS请求:
                                        snapshot.IIS = value;
                                        break;
                                    case (int)EnmCounter.内存:
                                        snapshot.Memory = value;
                                        break;
                                    case (int)EnmCounter.物理磁盘写字节:
                                        snapshot.PhysicalDiskWrite = value;
                                        break;
                                    case (int)EnmCounter.物理磁盘读字节:
                                        snapshot.PhysicalDiskRead = value;
                                        break;
                                }
                            }
                            catch (InvalidOperationException)
                            {
                                logger.Error(string.Format("性能监视器 {0} 未获取到任何值.",GetPerfCounterPath(serviceCounters[i].Item3)));
                            }
                        }
                        new ClusterPerformanceCounterSnapshotService().Add(snapshot);
                    }
                }
                else
                {
                    ConsoleWrite(os);
                }
            }
            finally
            {
                stopHandle.Set();
            }
        }

        private void ConsoleWrite(SystemInfo os)
        {
            while (!PerfmonWorker.ShouldStop)
            {
                Thread.Sleep(GlobalConfigure.PerformanceCollectBackgroundTaskSleepTime);

                for (int i = 0;i < serviceCounters.Count;i++)
                {
                    Console.WriteLine(serviceCounters[i].Item2 + "：" + serviceCounters[i].Item3.NextValue());
                }
            }
            Console.ReadKey();
        }

        private void Initialize(SystemInfo os)
        {
            try
            {

                var counters = new List<Tuple<int,string,PerformanceCounter>>();
                serviceCounterList = CounterProvider.Load();

                foreach (var counter in serviceCounterList)
                {
                    logger.Info(string.Format(@"创建性能监视器: {0}\{1}\{2}\{3}\{4}",counter.CollectName,os.MachineName ?? ".",counter.CategoryName,
                                        counter.CounterName,counter.InstanceName));

                    var perfCounter = new PerformanceCounter(counter.CategoryName,counter.CounterName,counter.InstanceName,os.MachineName ?? ".");
                    counters.Add(new Tuple<int,string,PerformanceCounter>(counter.Id,counter.CollectName, perfCounter));

                    try
                    {
                        perfCounter.NextValue();
                    }
                    catch { }

                }


                serviceCounters = counters;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void AddComputerInfo(SystemInfo os)
        {
            var computer = new ClusterComputerInfo();
            computer.Country = os.Country;
            computer.CPUCount = os.CPUCount;
            computer.CPUName = os.CPUName;
            computer.MachineIp = os.MachineIp;
            computer.MachineName = os.MachineName;
            computer.OSName = os.OSName;
            computer.OSVersion = os.OSVersion;
            computer.TimeZone = os.TimeZone;
            computer.TotalVirtualMemorySize = os.TotalVirtualMemorySize;
            computer.TotalVisibleMemorySize = os.TotalVisibleMemorySize;
            new ClusterComputerInfoServices().AddOrUpdate(computer);
        }

        private String GetPerfCounterPath(PerformanceCounter cnt)
        {
            return String.Format(@"{0}\{1}\{2}\{3}", cnt.MachineName, cnt.CategoryName, cnt.CounterName, cnt.InstanceName);
        }
    }
}
