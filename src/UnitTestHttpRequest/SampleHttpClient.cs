using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTestHttpRequest
{

    public class SampleHttpClient
    {
        private const string UserAgent = "MyAgent";

        private readonly string _instanceUrl;
        private readonly string _apiVersion;
        private readonly string _accessToken;

        private readonly Func<HttpClient> _builder;

        public SampleHttpClient(string instanceUrl, string apiVersion, string accessToken)
        {
            _instanceUrl = instanceUrl;
            _apiVersion = apiVersion;
            _accessToken = accessToken;

            _builder = () => new HttpClient();
        }

        public SampleHttpClient(string instanceUrl, string apiVersion, string accessToken, Func<HttpClient> builder)
        {
            _instanceUrl = instanceUrl;
            _apiVersion = apiVersion;
            _accessToken = accessToken;

            _builder = builder;
        }

        public async Task<T> HttpGet<T>(string query)
        {
            var url = FormatUrl(_instanceUrl, _apiVersion, query);

            using (var client = _builder())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };

                request.Headers.Add("Authorization", "Bearer " + _accessToken);

                var responseMessage = await client.SendAsync(request);
                var response = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jObject = JObject.Parse(response);

                    var r = JsonConvert.DeserializeObject<T>(jObject.ToString());
                    return r;
                }

                throw new Exception("Something happened!");
            }
        }

        public static string FormatUrl(string instanceUrl, string apiVersion, string query)
        {
            return string.Format("{0}/{1}/{2}", instanceUrl, apiVersion, query);
        }
    }
}
