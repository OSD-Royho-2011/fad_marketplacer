using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCB.Catalog.Api.DataAccess;
using NCB.Catalog.Api.IntegrationEvents.EventHandlers;
using NCB.Catalog.Api.IntegrationEvents.Events;
using NCB.EventBus.Abstractions;
using NCB.EventBus.EventBusRabbitMQ;
using NCB.EventBus.SubscriptionsManager;
using RabbitMQ.Client;
using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using NCB.Core.Models;
using NCB.Core.Filters;
using NCB.Catalog.Api.Infrastructure.Filters;
using NCB.Catalog.Api.Models;
using NCB.Core.Helpers;
using NCB.Core.Filters.CustomFilterFactory;
using NCB.Catalog.Api.DataAccess.BaseUnitOfWork;
using NCB.Catalog.Api.DataAccess.BaseRepository;
using NCB.EventBus.Models;
using System.Threading.Tasks;
using NCB.Catalog.Api.Services.Products.Models;
using NCB.Catalog.Api.Services.Products.Commands;
using NCB.Catalog.Api.Services.Products.Handlers;

namespace NCB.Catalog.Api
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
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

            //dbcontext
            var connection = Configuration.GetConnectionString("DevelopmentConnectionString");
            services.AddDbContext<CatalogDbContext>(options =>
                options
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlServer(connection)
            );

            //option
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<RabbitMQAppsettings>(Configuration.GetSection("RabbitMQ"));

            //service
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //authorize filter
            services.AddScoped<CustomAuthorizeFilter>();

            //mediatR
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            // rabbitmq connection
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();

            //event bus
            services.AddSingleton<IEventBus, EventBusRabbitMQ>();

            //event bus manager
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            //handle event
            services.AddTransient<ProductSaledEventHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CatalogDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

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

        //private
        private void RunMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<CatalogDbContext>().Database.Migrate();
            }
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<ProductSaledEventHandler>("product_saled");
        }

        private void Init(CatalogDbContext context)
        {
            new CatalogDbSeed(context).SeedAsync().Wait();
        }
    }
}
