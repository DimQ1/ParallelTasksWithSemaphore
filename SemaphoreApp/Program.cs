using System;

namespace SemaphoreApp
{
    class Program
    {
        

        static void Main(string[] args)
        {
            SemaphoreTasks semaphoreTasks = new SemaphoreTasks();

            semaphoreTasks.StartTasks();
        }
    }
}
