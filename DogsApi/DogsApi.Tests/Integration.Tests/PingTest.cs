using DogsApi.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsApi.Tests.Integration.Tests
{
    [TestClass]
    public class PingTest
    {
        private readonly APIBuilder _apiBuilder;

        public PingTest()
        {
            _apiBuilder = new APIBuilder();
        }

        [TestMethod]
        public async Task PingTestAsync()
        {
            // Arrange
            const string expectedPong = "Dogshouseservice.Version1.0.1";
            // Act
            var actualPong = await _apiBuilder.GetRequest<string>("/ping");
            // Assert
            Assert.AreEqual(expectedPong, actualPong);
        }
    }
}
