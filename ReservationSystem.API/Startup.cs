using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using ReservationSystem.Domain.Contexts;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Services.Mappers;
using ReservationSystem.Services.Services;

namespace ReservationSystem.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(EntityToDtoProfile));
            
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IChiefService, ChiefService>();
            services.AddScoped<IClassroomService, ClassroomService>();
            services.AddScoped<IReservationRequestService, ReservationRequestService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            var connection = Configuration.GetConnectionString("PostgresConnection");
            services.AddDbContext<ReservationDbContext>(options =>
                options.UseNpgsql(connection, b => b.MigrationsAssembly("ReservationSystem.Domain")));
            
            services.AddMemoryCache();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReservationSystem.API", Version = "v1" });
            });

            services.AddStackExchangeRedisCache(options =>
            {
                var connectionRedis = Configuration.GetValue<string>("Redis:ConnectionString");
                options.Configuration = connectionRedis;
            });

        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReservationSystem.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}