using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace http2client
{
    public class http2handler : WinHttpHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (Program.useHttp2 == true)
            {
                request.Version = new Version("2.0");
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
