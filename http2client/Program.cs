using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Mono.Options;
using System.Reflection.Emit;

namespace http2client
{
    class Program
    {
        static public bool useHttp2 { get; set;}

        static void Main(string[] args)
        {
            PerfCounters perfCounters = new PerfCounters();

            string url = "https://localhost:44381/"; ;
            int numThreads = 10;
            useHttp2 = false;
            var shouldShowHelp = false;

            var options = new OptionSet {
                { "u|url=", "target server.", n => url=n },
                { "t|threads=", "the number of threads to spawn.", (int t) => numThreads = t },
                { "http2", "use http2", h => useHttp2 = h != null},
                { "help", "show this message and exit", h => shouldShowHelp = h != null },
            };

            List<string> extra;
            try
            {
                // parse the command line
                extra = options.Parse(args);
            }
            catch (OptionException e)
            {
                // output some error message
                Console.Write("http2client: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `http2client --help' for more information.");
                return;
            }

            Console.WriteLine("url: "+url);
            Console.WriteLine("numThreads: " + numThreads);
            Console.WriteLine("useHttp2: " + useHttp2);

            //Console.WriteLine($"PerfCounters: \n\tBytes Received - {perfCounters.GetBytesReceived()}\n\tBytes Sent - {perfCounters.GetBytesSent()}\n\tConnectionsEstablished - {perfCounters.GetConnectionsEstablished()}");

            HttpClient httpClient = new HttpClient(new http2handler());
            httpClient.BaseAddress = new Uri(url);

            Task result = SendRequests.SendRequest(0, httpClient, url);
            result.Wait();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < numThreads; i++)
            {
                Console.WriteLine(DateTime.UtcNow + ": Spawning Thread " + i);
                Task t = SendRequests.SendRequest(i, httpClient, url);
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());

            // Thread.Sleep(10000);
            //Console.WriteLine($"PerfCounters: \n\tBytes Received - {perfCounters.GetBytesReceived()}\n\tBytes Sent - {perfCounters.GetBytesSent()}\n\tConnectionsEstablished - {perfCounters.GetConnectionsEstablished()}");
        }


    }
}
