using Business.Interfaces;
using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services) 
        {
            string connection = Configuration.GetConnectionString("DefaultConnection")!;
            var optionsBuilder = new DbContextOptionsBuilder<DogAPIDBContext>();

            var options = optionsBuilder
                    .UseSqlServer(connection)
                    .Options;
            var context = new DogAPIDBContext(options);
            var unitOfWork = new UnitOfWork(context);

            services.AddMemoryCache();

            services.AddControllers();

            services.AddDbContext<DogAPIDBContext>(options => options.UseSqlServer(connection));

            services.AddAutoMapper(typeof(AutomapperProfile).Assembly);

            services.AddSingleton<IUnitOfWork>(x => unitOfWork);

            services.AddTransient<ICacheService, CacheService>();

            services.AddTransient<IValidator<DogModel>, DogValidator>();

            services.AddTransient<IDogService, DogService>();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}
