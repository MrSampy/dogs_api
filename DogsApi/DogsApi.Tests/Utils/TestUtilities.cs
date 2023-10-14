using Data.Data;
using Data.Entities;
using Data.Interfaces;
using AutoMapper;
using Business.Validation;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Moq;

namespace DogsApi.Tests.Utils
{
    public class TestUtilities
    {
        public static async Task SeedDataAsync(DogAPIDBContext context) 
        {
            var dogs = CreateDogs();
            await context.Dogs.AddRangeAsync(dogs);
            await context.SaveChangesAsync();
        }

        public static void SeedData(DogAPIDBContext context)
        {
            var dogs = CreateDogs();
            context.Dogs.AddRange(dogs);
            context.SaveChanges();
        }


        public static List<Dog> CreateDogs()
        {
            var dogs = new List<Dog>();
            for (int index = 1; index <= 20; ++index)
            {
                var dog = new Dog
                {
                    Name = $"Dog {index}",
                    Color = $"Color {index}",
                    TailLength = index + 5,
                    Weight = index + 20,
                };
                dogs.Add(dog);
            }
            return dogs;
        }


        public static IUnitOfWork CreateUnitOfWork(DogAPIDBContext context)
        {
            var unitOfWork = new UnitOfWork(context);
            return unitOfWork;
        }
        public static IMapper CreateMapper()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        public static IValidator<DogModel> CreateValidator(IUnitOfWork unitOfWork)
        {
            var validator = new DogValidator(unitOfWork);
            return validator;
        }

        public static ICacheService CreateCacheService()
        {
            var cacheServiceMock = new Mock<ICacheService>();

            cacheServiceMock.Setup(cache => cache.Get<IEnumerable<DogModel>>("GetAllAsyncAttribute: , Order: , PageNumber: , PageSize: ")).Returns((IEnumerable<DogModel>)null);
            cacheServiceMock.Setup(cache => cache.Get<IEnumerable<DogModel>>("GetAllAsyncAttribute: weight, Order: desc, PageNumber: , PageSize: ")).Returns((IEnumerable<DogModel>)null);
            cacheServiceMock.Setup(cache => cache.Get<IEnumerable<DogModel>>("GetAllAsyncAttribute: weightd, Order: desc, PageNumber: , PageSize: ")).Returns((IEnumerable<DogModel>)null);
            cacheServiceMock.Setup(cache => cache.Get<IEnumerable<DogModel>>("GetAllAsyncAttribute: weight, Order: descd, PageNumber: , PageSize: ")).Returns((IEnumerable<DogModel>)null);
            cacheServiceMock.Setup(cache => cache.Get<IEnumerable<DogModel>>("GetAllAsyncAttribute: , Order: , PageNumber: 2, PageSize: 5")).Returns((IEnumerable<DogModel>)null);
            cacheServiceMock.Setup(cache => cache.Get<IEnumerable<DogModel>>("GetAllAsyncAttribute: , Order: , PageNumber: 2, PageSize: -5")).Returns((IEnumerable<DogModel>)null);
            cacheServiceMock.Setup(cache => cache.Get<IEnumerable<DogModel>>("GetAllAsyncAttribute: tail_length, Order: desc, PageNumber: 2, PageSize: 5")).Returns((IEnumerable<DogModel>)null);

            return cacheServiceMock.Object;
        }
    }
}
