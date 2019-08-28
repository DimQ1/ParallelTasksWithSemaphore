﻿using System;
using System.Collections.Generic;
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

        public void StartTasks()
        {
            semaphoreSlim = new SemaphoreSlim(5);

            Console.WriteLine($"{semaphoreSlim.CurrentCount} tasks can enter th semaphore");

            Task[] tasks = new Task[500];
            PrintCount();

            for (var i = 0; i < 500; i++)
            {
                tasks[i] = Task.Run(async () =>
               {
                   Interlocked.Add(ref countWaitingTasks, 1);
                   PrintCount();
                   //waite if all semaphores busy
                   await semaphoreSlim.WaitAsync();
                   Interlocked.Add(ref countWaitingTasks, -1);
                   Interlocked.Add(ref countWorkingTasks, 1);
                   Interlocked.Add(ref padding, 1);
                   PrintCount();
                   //your method for executing smth
                   yourMethod();
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
