using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace http2client
{
    class SendRequests
    {
        public static async Task<int> SendRequest(int taskNum, HttpClient httpClient, string url)
        {
            //httpClient.DefaultRequestHeaders.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine(DateTime.UtcNow + ": " + taskNum + " - " + "Begin calling " + url);
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url + "api/values");
                HttpResponseMessage message = await httpClient.SendAsync(request);
                if (message.IsSuccessStatusCode)
                {
                    Console.WriteLine(DateTime.UtcNow + ": " + taskNum + " - " + "End success calling " + url + " " + message.StatusCode);
                    return 1;
                }
                else
                {
                    Console.WriteLine(DateTime.UtcNow + ": " + taskNum + " - " + "End fail calling " + url + " " + message.StatusCode);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.UtcNow + ": " + taskNum + " - " + "Exception: " + ex.Message);
            }

            return -1;
        }
    }
}
