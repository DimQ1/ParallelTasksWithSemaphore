using System;

namespace SemaphoreApp
{
    class Program
    {
        

        static void Main(string[] args)
        {

            LoggerConfigure.ConfigureLogger();

            var urls = UrlsReader.GetUrls($"{System.IO.Directory.GetCurrentDirectory()}\\Hosts.txt");

            SemaphoreTasks semaphoreTasks = new SemaphoreTasks(urls, 200);

            semaphoreTasks.StartTasks();
        }
    }
}
