﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using DogsApi.Tests.Utils;
using System.Collections;

namespace DogsApi.Tests.Data.Tests
{
    [TestClass]
    public class DogRepositoryTest
    {
        private async Task<DogRepository> CreateRepositoryAsync()
        {
            var context = new DogAPIDBContext(new DbContextOptionsBuilder<DogAPIDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options, ensureDeleted: true);
            await DataSeeder.SeedData(context);
            return new DogRepository(context);
        }

        [TestMethod]
        public async Task DogRepository_GetAllAsync() 
        {
            // Arrange
            var repository = await CreateRepositoryAsync();
            var expectedDogs = new List<Dog>();
            for (int index = 1; index <= 20; ++index)
            {
                var dog = new Dog
                {
                    Name = $"Dog {index}",
                    Color = $"Color {index}",
                    TailLength = index + 5,
                    Weight = index + 20,
                };
                expectedDogs.Add(dog);
            }
            // Act
            var actualDogs = await repository.GetAllAsync();

            // Assert
            Assert.IsTrue(expectedDogs.SequenceEqual(actualDogs));

        }

        [TestMethod]
        public async Task DogRepository_AddAsync()
        {
            // Arrange
            var repository = await CreateRepositoryAsync();
            var expectedDog = new Dog
            {
                Name = $"Dog 21",
                Color = $"Color 21",
                TailLength = 21 + 5,
                Weight = 21 + 20,
            };
            // Act
            await repository.AddAsync(expectedDog);
            var actualDogs = await repository.GetAllAsync();

            // Assert
            Assert.IsTrue(actualDogs.Contains(expectedDog));
        }

    }
}
