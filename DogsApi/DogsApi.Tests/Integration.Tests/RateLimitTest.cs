using DogsApi.Tests.Utils;
using System.Net;

namespace DogsApi.Tests.Integration.Tests
{
    [TestClass]
    public class RateLimitTest
    {
        private readonly CustomWebApplicationFactory<API.Startup> _factory;
        private HttpClient _client;
        private int rateLimitThreshold = 10;
        public RateLimitTest()
        {
            _factory = new CustomWebApplicationFactory<API.Startup>();
            _client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task RateLimitExceedsLimit_Returns429StatusCode()
        {
            var responses = new List<HttpResponseMessage>();
            for (int i = 0; i < rateLimitThreshold + 1; i++)
            {
                var response = await _client.GetAsync("/dogs");
                responses.Add(response);
            }

            for (int i = 0; i < rateLimitThreshold + 1; i++) 
            {
                if (i < rateLimitThreshold)
                {
                    Assert.AreEqual(HttpStatusCode.OK, responses[i].StatusCode);
                }
                else 
                {
                    Assert.AreEqual(HttpStatusCode.TooManyRequests, responses[i].StatusCode);
                }
            }

        }
    }
}
