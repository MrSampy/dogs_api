using AspNetCoreRateLimit;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

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
            ConfigureDependencyInjection(services);

            services.AddControllers();
            services.AddHealthChecks();
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

            ConfigureMiddleware(app);

            app.UseEndpoints(endpoints =>
            {
                ConfigureEndpoints(endpoints);
            });
        }

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection")!;
            var optionsBuilder = new DbContextOptionsBuilder<DogAPIDBContext>();
            var options = optionsBuilder
                .UseSqlServer(connection)
                .Options;
            var context = new DogAPIDBContext(options);
            var unitOfWork = new UnitOfWork(context);

            #region RateLimiting
            services.AddMemoryCache();
            services.AddOptions();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            #endregion

            services.AddDbContext<DogAPIDBContext>(options => options.UseSqlServer(connection));
            services.AddAutoMapper(typeof(AutomapperProfile).Assembly);
            services.AddSingleton<IUnitOfWork>(x => unitOfWork);
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<IValidator<DogModel>, DogValidator>();
            services.AddTransient<IDogService, DogService>();
        }

        private void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.UseIpRateLimiting();
        }

        private void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/ping", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = report.Status == HealthStatus.Healthy
                        ? "Dogshouseservice.Version1.0.1"
                        : "Dogshouseservice.Version1.0.1:Failure";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
        }
    }
}
