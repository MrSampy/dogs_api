using Data.Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DogsApi.Tests.Utils
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var context = new DogAPIDBContext(new DbContextOptionsBuilder<DogAPIDBContext>()
               .EnableSensitiveDataLogging()
               .UseInMemoryDatabase(databaseName: "Test_Database").Options, ensureDeleted: true);
            TestUtilities.SeedData(context);
            var unitOfWork = new UnitOfWork(context);

            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IUnitOfWork>(x => unitOfWork);
                services.AddDbContext<DogAPIDBContext>(options =>
                {
                    options.UseInMemoryDatabase("Test_Database");
                });
            });
        }
    }
}
