using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTestHttpRequest
{
    public class SampleTests
    {
        [Test]
        public async void Auth_CheckHttpRequestMessage_HttpGet()
        {
            var client = new HttpClient(new SampleRouteHandler(r =>
            {
                Assert.AreEqual(r.RequestUri.ToString(), "http://localhost:1899/v1/querystring");

                Assert.IsNotNull(r.Headers.UserAgent);
                Assert.AreEqual(r.Headers.UserAgent.ToString(), "MyAgent");

                Assert.IsNotNull(r.Headers.Authorization);
                Assert.AreEqual(r.Headers.Authorization.ToString(), "Bearer accessToken");
            }));

            Func<HttpClient> builder = () => client;

            var toolkitHttpClient = new SampleHttpClient("http://localhost:1899", "v1", "accessToken", builder);

            await toolkitHttpClient.HttpGet<object>("querystring");
        } 
    }
}
