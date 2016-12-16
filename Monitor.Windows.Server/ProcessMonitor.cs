//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Monitor.Windows.Server
//{
//    public class ProcessMonitor
//    {

//        private class ProcessInfo
//        {
//            public float CpuPercentage { get; set; }
//            public float DiskRead { get; set; }
//            public float DiskWrite { get; set; }
//            public float DiskReadWrite { get; set; }
//            public float Memory { get; set; }
//            public float MemoryMax { get; set; }
//        }

//        private const int RowsOfBuffer = 10;
//        private const float Mega = (float)1024 * 1024;

//        private string filePath;


//        private List<ProcessInfo> monitorResults;
//        private object fileLock = new object();
//        private int timerCount;
//        private PerformanceCounter cpuCounter;
//        private PerformanceCounter diskReadCounter;
//        private PerformanceCounter diskWriteCounter;
//        private PerformanceCounter diskReadWriteCounter;
//        private PerformanceCounter memoryCounter;
//        private PerformanceCounter memoryMaxCounter;

//        //processName:no path, no extension
//        //statFilePath:absolute path, extension
//        public ProcessMonitor(string processName,string statFilePath,int statInterval)
//        {
//            filePath = statFilePath;

//            InitPerformanceCounter(processName);
//            monitorResults = new List<ProcessInfo>();

//            timer = new Timer(statInterval);
//            timer.Elapsed += DoStatistics;
//        }
//        public void StartMonitor()
//        {
//            timer.Start();
//        }
//        public void StopMonitor()
//        {
//            timer.Stop();
//            AppendSave(filePath);
//        }
//        private void DoStatistics(object sender,ElapsedEventArgs elapsedEventArgs)
//        {
//            int threadsLeft, dummy;
//            ThreadPool.GetAvailableThreads(out threadsLeft,out dummy);
//            if (threadsLeft < 2)
//                return;

//            ProcessInfo info = GetProcessInfo();
//            monitorResults.Add(info);

//            if (timerCount % RowsOfBuffer == 0)
//                AppendSave(filePath);
//        }
//        private void AppendSave(string fileName)
//        {
//            lock (fileLock)
//            {
//                using (StreamWriter sw = new StreamWriter(fileName,true))
//                {
//                    var list = monitorResults;
//                    monitorResults = new List<ProcessInfo>();
//                    foreach (ProcessInfo info in list)
//                    {
//                        sw.WriteLine("{0},{1},{2},{3},{4},{5}",info.CpuPercentage,info.Memory,info.MemoryMax,info.DiskRead,info.DiskWrite,info.DiskReadWrite);
//                    }
//                    sw.Close();
//                }
//            }
//        }
//        private ProcessInfo GetProcessInfo()
//        {
//            ProcessInfo result = new ProcessInfo();

//            ProcessCounterSamples currentSample = new ProcessCounterSamples();
//            currentSample.CpuSample = cpuCounter.NextSample();
//            currentSample.DiskReadSample = diskReadCounter.NextSample();
//            currentSample.DiskWriteSample = diskWriteCounter.NextSample();
//            currentSample.DiskReadWriteSample = diskReadWriteCounter.NextSample();

//            if (lastSample == null)
//            {
//                result.CpuPercentage = 0f;
//                result.DiskRead = 0f;
//                result.DiskWrite = 0f;
//                result.DiskReadWrite = 0f;
//            }
//            else
//            {
//                result.CpuPercentage = CounterSampleCalculator.ComputeCounterValue(lastSample.CpuSample,currentSample.CpuSample);
//                result.DiskRead = CounterSampleCalculator.ComputeCounterValue(lastSample.DiskReadSample,currentSample.DiskReadSample) / Mega;
//                result.DiskWrite = CounterSampleCalculator.ComputeCounterValue(lastSample.DiskWriteSample,currentSample.DiskWriteSample) / Mega;
//                result.DiskReadWrite = CounterSampleCalculator.ComputeCounterValue(lastSample.DiskReadWriteSample,currentSample.DiskReadWriteSample) / Mega;
//            }
//            lastSample = currentSample;

//            result.Memory = memoryCounter.NextValue() / Mega;
//            result.MemoryMax = memoryMaxCounter.NextValue() / Mega;

//            return result;
//        }

//        private void InitPerformanceCounter(string processName)
//        {
//            cpuCounter = new PerformanceCounter("Process","% Processor Time",processName);
//            diskReadCounter = new PerformanceCounter("Process","IO Read Bytes/sec",processName);
//            diskWriteCounter = new PerformanceCounter("Process","IO Write Bytes/sec",processName);
//            diskReadWriteCounter = new PerformanceCounter("Process","IO Data Bytes/sec",processName);
//            memoryCounter = new PerformanceCounter("Process","Working Set - Private",processName);//memory
//            memoryMaxCounter = new PerformanceCounter("Process","Working Set Peak",processName);//memory peak
//        }


//    }
//}
