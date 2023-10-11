using Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
namespace DogsApi.Tests.Utils
{
    public class DataSeeder
    {
        public static async Task SeedData(DogAPIDBContext context) 
        {
            for (int index = 1; index <= 20; ++index)
            {
                var dog = new Dog 
                {
                    Name = $"Dog {index}",
                    Color = $"Color {index}",
                    TailLength = index + 5,
                    Weight = index + 20,
                };
                await context.Dogs.AddAsync(dog);

                await context.SaveChangesAsync();
            }
        }
    }
}
