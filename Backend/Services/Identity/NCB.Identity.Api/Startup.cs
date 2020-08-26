using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCB.Core.Extensions;
using NCB.Core.Filters;
using NCB.Core.Filters.CustomFilterFactory;
using NCB.Core.Models;
using NCB.EventBus.Abstractions;
using NCB.EventBus.EventBusRabbitMQ;
using NCB.EventBus.Models;
using NCB.EventBus.SubscriptionsManager;
using NCB.Identity.Api.DataAccess;
using NCB.Identity.Api.DataAccess.BaseRepository;
using NCB.Identity.Api.DataAccess.BaseUnitOfWork;
using NCB.Identity.Api.DataAccess.Entities;
using NCB.Identity.Api.Infrastructure.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace NCB.Identity.Api
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelAttribute));
                options.Filters.Add(typeof(GlobalExceptionFilter));
            }).AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });

            //cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                  builder => builder
                    .SetIsOriginAllowed(host => true)
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            });

            //swagger
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AccessTokenHeaderParameterOperationFilter>();
                c.DescribeAllParametersInCamelCase();
            });

            //db
            var connection = Configuration.GetConnectionString("DevelopmentConnectionString");
            services.AddDbContextPool<IdentityDbContext>(options =>
                options.UseSqlServer(connection)
            );

            //option
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<RabbitMQAppsettings>(Configuration.GetSection("RabbitMQ"));

            //services
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //authorize filter
            services.AddScoped<CustomAuthorizeFilter>();

            //rabbitmq connection
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();

            //event bus
            services.AddSingleton<IEventBus, EventBusRabbitMQ>();

            //event bus manager
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            RunMigration(app);
            Init(context);
            ConfigureEventBus(app);
        }

        //auto run migration
        private void RunMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<IdentityDbContext>().Database.Migrate();
            }
        }

        private void Init(IdentityDbContext context)
        {
            new IdentityDbSeed(context).SeedAsync().Wait();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {

            app.ApplicationServices.GetRequiredService<IEventBus>();
        }
    }
}
