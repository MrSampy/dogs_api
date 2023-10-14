using Business.Models;
using DogsApi.Tests.Utils;

namespace DogsApi.Tests.Integration.Tests
{
    [TestClass]
    public class DogControllerTest
    {
        private readonly APIBuilder _apiBuilder;

        public DogControllerTest()
        {
            _apiBuilder = new APIBuilder();
        }

        [TestMethod]
        public async Task DogController_Add_Dog_Fails_All_Properties_Null()
        {
            // Arrange
            var dog = new DogModel
            {
            };
            const string expectedExceptionMessage = "Validation failed: Name is required!\nColor is required!\nTail length must be greater than 0!\nWeight must be greater than 0!";
            // Act
            var actualExceptionMessage = await _apiBuilder.PostRequest("/dog", dog);
            // Assert
            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage);
        }

        [TestMethod]
        public async Task DogController_Add_New_Dog_Fail_Dog_Already_Exists()
        {
            // Arrange
            var dog = new DogModel
            {
                Name = "Dog 20",
                Color = "Color 21",
                TailLength = 26,
                Weight = 41
            };
            const string expectedExceptionMessage = "Validation failed: Dog with this name already exists!";
            // Act
            var actualExceptionMessage = await _apiBuilder.PostRequest("/dog", dog);
            // Assert
            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage);
        }

        [TestMethod]
        public async Task DogController_Add_New_Dog_Then_Get_All_Dogs_Dog_Created()
        {
            // Arrange
            var expectedDog = new DogModel 
            {
                Name = "Dog 21",
                Color = "Color 21",
                TailLength = 26,
                Weight = 41
            };
            // Act
            await _apiBuilder.PostRequest("/dog", expectedDog);
            var actualDog = (await _apiBuilder.GetRequest<IEnumerable<DogModel>>("/dogs")).Last();
            // Assert
            Assert.AreEqual(expectedDog, actualDog);
        }


        [TestMethod]
        public async Task DogController_Get_All_Dogs_Without_Filters()
        {
            // Arrange
            var mapper = TestUtilities.CreateMapper();
            var expectedDogs = mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs());
            // Act
            var actualDogs = await _apiBuilder.GetRequest<IEnumerable<DogModel>>("/dogs");
            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogController_Get_All_Dogs_Order_Weight_By_Desc()
        {
            // Arrange
            const string attribute = "Weight";
            const string order = "desc";
            var mapper = TestUtilities.CreateMapper();
            var expectedDogs = mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs()).OrderByDescending(x => x.Weight);
            // Act
            var actualDogs = await _apiBuilder.GetRequest<IEnumerable<DogModel>>($"/dogs?attribute={attribute}&order={order}");
            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogController_Get_All_Dogs_Wrong_Attribute_Name_Returns_Same_Collection()
        {
            // Arrange
            const string attribute = "Wedight";
            const string order = "desc";
            var mapper = TestUtilities.CreateMapper();
            var expectedDogs = mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs());
            // Act
            var actualDogs = await _apiBuilder.GetRequest<IEnumerable<DogModel>>($"/dogs?attribute={attribute}&order={order}");
            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogController_Get_All_Dogs_Wrong_Order_Returns_Same_Collection()
        {
            // Arrange
            const string attribute = "Weight";
            const string order = "descd";
            var mapper = TestUtilities.CreateMapper();
            var expectedDogs = mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs());
            // Act
            var actualDogs = await _apiBuilder.GetRequest<IEnumerable<DogModel>>($"/dogs?attribute={attribute}&order={order}");
            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogController_Get_All_Dogs_Apply_Pagination_Order_Tail_Length_By_Desc()
        {
            // Arrange
            const string attribute = "WeIGht";
            const string order = "desc";
            const int pageNumber = 2;
            const int pageSize = 5;
            var mapper = TestUtilities.CreateMapper();
            var expectedDogs = mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs()).Skip(5).Take(5).OrderByDescending(x => x.TailLength);
            // Act
            var actualDogs = await _apiBuilder.GetRequest<IEnumerable<DogModel>>($"/dogs?attribute={attribute}&order={order}&pageNumber={pageNumber}&pageSize={pageSize}");
            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogController_Get_All_Dogs_Apply_Pagination_Negative_Page_Size()
        {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = -5;
            var mapper = TestUtilities.CreateMapper();
            var expectedDogs = mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs());
            // Act
            var actualDogs = await _apiBuilder.GetRequest<IEnumerable<DogModel>>($"/dogs?pageNumber={pageNumber}&pageSize={pageSize}");
            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogController_Get_All_Dogs_Apply_Pagination()
        {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 5;
            var mapper = TestUtilities.CreateMapper();
            var expectedDogs = mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs()).Skip(5).Take(5);
            // Act
            var actualDogs = await _apiBuilder.GetRequest<IEnumerable<DogModel>>($"/dogs?pageNumber={pageNumber}&pageSize={pageSize}");
            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }
    }
}
