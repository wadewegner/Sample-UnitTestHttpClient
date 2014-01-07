using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestHttpRequest
{
    internal class SampleRouteHandler : DelegatingHandler
    {
        Action<HttpRequestMessage> _testingAction;

        public SampleRouteHandler(Action<HttpRequestMessage> testingAction)
        {
            _testingAction = testingAction;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            _testingAction(request);
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(new
                {
                    Success = true,
                    Message = "Success"
                })
            };

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(resp);
            return tsc.Task;
        }
    }
}
