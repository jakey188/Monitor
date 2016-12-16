using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Monitor.Watch.Data;

namespace Monitor.Watch
{
    public class HttpRequestWatch
    {
        public static ConcurrentQueue<ClusterHttpWatch> WatchQueue = new ConcurrentQueue<ClusterHttpWatch>();
        private static int ReportQueueCount = 100;//队列上报长度

        public static void Start()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Run));
        }

        static void Run(object state)
        {
            try
            {
                var httpWatchList = new List<ClusterHttpWatch>();
                while (true)
                {
                    var httpWatch = new ClusterHttpWatch();

                    if (WatchQueue.TryDequeue(out httpWatch))
                    {
                        httpWatchList.Add(httpWatch);
                        if (httpWatchList.Count == ReportQueueCount)
                        {
                            //Trace.WriteLine("【"+httpWatchList.Count+"】"+string.Join(",",httpWatchList));
                            httpWatch.AddRange(httpWatchList);
                            httpWatchList = new List<ClusterHttpWatch>();
                        }
                    }
                    if(WatchQueue.Count < ReportQueueCount)
                        Thread.Sleep(1000);
                }
            }
            catch (Exception) {

            }
            
        }
    }
}
