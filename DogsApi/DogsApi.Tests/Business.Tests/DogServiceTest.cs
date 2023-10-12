using Business.Services;
using Data.Data;
using DogsApi.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Business.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DogsApi.Tests.Business.Tests
{
    [TestClass]
    public class DogServiceTest
    {
        private async Task<DogService> CreateServiceAsync()
        {
            var context = new DogAPIDBContext(new DbContextOptionsBuilder<DogAPIDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options, ensureDeleted: true);
            await TestUtilities.SeedData(context);
            var unitOfWork = TestUtilities.CreateUnitOfWork(context);
            var mapper = TestUtilities.CreateMapper();
            var cacheService = TestUtilities.CreateCacheService();
            var validator = TestUtilities.CreateValidator(unitOfWork);

            return new DogService(unitOfWork,mapper,cacheService,validator);
        }

        [TestMethod]
        public async Task DogService_AddDog_Fails_All_Properties_Null()
        {
            //Arrange
            var service = await CreateServiceAsync();
            var expectedDog = new DogModel();
            const string expectedMessage = "Validation failed: Name is required!\nColor is required!\nTail length must be greater than 0!\nWeight must be greater than 0!";
            //Act
            ValidationException ex = await Assert.ThrowsExceptionAsync<ValidationException>(() => service.AddAsync(expectedDog));
            //Assert
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [TestMethod]
        public async Task DogService_AddDog_Fails_Dog_Already_Exists()
        {
            //Arrange
            var service = await CreateServiceAsync();
            var expectedDog = new DogModel
            {
                Name = $"Dog 20",
                Color = $"Color 21",
                TailLength = 21 + 5,
                Weight = 21 + 20,
            };
            const string expectedMessage = "Validation failed: Dog with this name already exists!";
            //Act
            ValidationException ex = await Assert.ThrowsExceptionAsync<ValidationException>(() => service.AddAsync(expectedDog));
            //Assert
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [TestMethod]
        public async Task DogService_AddDog()
        {
            //Arrange
            var service = await CreateServiceAsync();
            var expectedDog = new DogModel
            {
                Name = $"Dog 21",
                Color = $"Color 21",
                TailLength = 21 + 5,
                Weight = 21 + 20,
            };
            //Act
            await service.AddAsync(expectedDog);
            var actualDogs = await service.GetAllAsync(new FilterModel());
            //Assert
            Assert.IsTrue(actualDogs.Contains(expectedDog));
        }

        [TestMethod]
        public async Task DogService_GetAllAsync_WithoutFilter()
        {
            // Arrange
            var service = await CreateServiceAsync();
            var expectedDogs = service.Mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs());
            // Act
            var actualDogs = await service.GetAllAsync(new FilterModel());

            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));

        }

        [TestMethod]
        public async Task DogService_GetAllAsync_Order_Weight_By_Desc()
        {
            // Arrange
            var service = await CreateServiceAsync();
            var expectedDogs = service.Mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs()).OrderByDescending(x=>x.Weight);
            // Act
            var actualDogs = await service.GetAllAsync(new FilterModel 
            {
                Order = "desc",
                Attribute = "weight"
            });

            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogService_GetAllAsync_Wrong_Attribute_Name_Returns_Same_Collection()
        {
            // Arrange
            var service = await CreateServiceAsync();
            var expectedDogs = service.Mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs());
            // Act
            var actualDogs = await service.GetAllAsync(new FilterModel
            {
                Order = "desc",
                Attribute = "weightd"
            });

            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogService_GetAllAsync_Wrong_Order_Returns_Same_Collection()
        {
            // Arrange
            var service = await CreateServiceAsync();
            var expectedDogs = service.Mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs());
            // Act
            var actualDogs = await service.GetAllAsync(new FilterModel
            {
                Order = "descd",
                Attribute = "weight"
            });

            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogService_GetAllAsync_Apply_Pagination_Order_Tail_Length_By_Desc()
        {
            // Arrange
            var service = await CreateServiceAsync();
            var expectedDogs = service.Mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs()).Skip(5).Take(5).OrderByDescending(x=>x.TailLength);
            // Act
            var actualDogs = await service.GetAllAsync(new FilterModel
            {
                Order = "desc",
                Attribute = "tail_length",
                PageSize = 5,
                PageNumber = 2,
            });

            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogService_GetAllAsync_Apply_Pagination_Negative_Page_Size()
        {
            // Arrange
            var service = await CreateServiceAsync();
            var expectedDogs = service.Mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs());
            // Act
            var actualDogs = await service.GetAllAsync(new FilterModel
            {
                PageSize = -5,
                PageNumber = 2,
            });

            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }

        [TestMethod]
        public async Task DogService_GetAllAsync_Apply_Pagination()
        {
            // Arrange
            var service = await CreateServiceAsync();
            var expectedDogs = service.Mapper.Map<IEnumerable<DogModel>>(TestUtilities.CreateDogs()).Skip(5).Take(5);
            // Act
            var actualDogs = await service.GetAllAsync(new FilterModel
            {
                PageSize = 5,
                PageNumber = 2,
            });

            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));
        }
    }
}
