using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SemaphoreApp
{
    class HttpRequests
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static async Task brutForceAsync(string webUrl)
        {

            try
            {
                WebRequest webRequest = WebRequest.Create(webUrl);
                webRequest.Method = HttpMethod.Head.ToString();
                //you can configure it if need corrected timeout
               // webRequest.Timeout = (int)TimeSpan.FromSeconds(2).TotalMilliseconds;

                HttpWebResponse webresponse = (await webRequest.GetResponseAsync()) as HttpWebResponse;

                logger.Info($"ok| {webresponse.StatusCode:D}|{webUrl}");
            }
            catch (Exception e)
            {
                logger.Error(e, $"{e.Message}|{webUrl}");
            }
        }
    }
}
