using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.IntTest.Factories
{
    public class HttpClientBuilder
    {
        private string url = "http://localhost:5094";
        private HttpRequestMessage requestMessage;

        public HttpClientBuilder WithUrl(string url)
        {
            this.url = url;
            return this;
        }
        public HttpClientBuilder WithRequestBody(object body, string relativeUrl)
        {
            this.requestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    relativeUrl)
            {
                Content = JsonContent.Create(
                    body
                )
            };
            return this;
        }


        public async Task<HttpResponseMessage> Send()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            return await client.SendAsync(requestMessage);
        }

    }
}