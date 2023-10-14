using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DogsApi.Tests.Utils
{
    public class APIBuilder
    {
        private readonly CustomWebApplicationFactory<API.Startup> _factory;
        private HttpClient _client;

        public APIBuilder()
        {
            _factory = new CustomWebApplicationFactory<API.Startup>();
            _client = _factory.CreateClient();
        }
        private void AssertRequestSuccessful(HttpResponseMessage response)
        {
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.InternalServerError);
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
        public async Task<T> GetRequest<T>(string endPoint)
        {
            var response = await _client.GetAsync(endPoint);

            AssertRequestSuccessful(response);

            string res = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(res)!;
        }

        public async Task<string> PostRequest(string endPoint, object value) 
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await _client.PostAsync(endPoint, new StringContent(json, null, "application/json"));

            AssertRequestSuccessful(response);

            return await response.Content.ReadAsStringAsync();
        }

    }
}
