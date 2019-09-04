using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreApp
{
    public class SemaphoreTasks
    {
        private static SemaphoreSlim semaphoreSlim;

        private static int padding;
        private static int countFinishedTasks;
        private static int countWorkingTasks;
        private static int countWaitingTasks;


        private readonly int _parralelCount;
        private readonly Stack<string> _urls;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public SemaphoreTasks(Stack<string> urls, int parralelCount = 10)
        {
            _parralelCount = parralelCount;
            _urls = urls;
            ServicePointManager.DefaultConnectionLimit = parralelCount;
        }


        public void StartTasks()
        {
            semaphoreSlim = new SemaphoreSlim(this._parralelCount);

            Console.WriteLine($"{semaphoreSlim.CurrentCount} tasks can enter th semaphore");

            Task[] tasks = new Task[_urls.Count];
            PrintCount();
            
            while (_urls.Count > 0)
            {
                var url = _urls.Pop();
                tasks[_urls.Count] = Task.Run(async () =>
               {
                   var urlLock = url;
                   Interlocked.Add(ref countWaitingTasks, 1);
                   PrintCount();
                   //waite if all semaphores busy
                   await semaphoreSlim.WaitAsync();
                   Interlocked.Add(ref countWaitingTasks, -1);
                   Interlocked.Add(ref countWorkingTasks, 1);
                   Interlocked.Add(ref padding, 1);
                   PrintCount();
                   //your method for executing smth
                   await HttpRequests.brutForceAsync(urlLock);
                   //one semaphore will be freed
                   semaphoreSlim.Release(1);
                   Interlocked.Add(ref countWorkingTasks, -1);
                   Interlocked.Add(ref countFinishedTasks, 1);
                   PrintCount();
               });

            }

            Task.WaitAll(tasks);

            Console.WriteLine("Main thread exits.");
        }

        private void PrintCount()
        {
            var consoleLine = $"\n---------------------------------" +
                $"\n{nameof(countWaitingTasks)}:{countWaitingTasks}" +
                 $"\n{nameof(countWorkingTasks)}:{countWorkingTasks}" +
                 $"\n{nameof(countFinishedTasks)}:{countFinishedTasks}";
            Console.Write(consoleLine);
        }

        private void yourMethod()
        {
            Thread.Sleep(1000 + padding);
        }
    }
}
