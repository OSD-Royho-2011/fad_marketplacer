using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCB.Core.Filters;
using NCB.Core.Filters.CustomFilterFactory;
using NCB.Core.Models;
using NCB.EventBus.Abstractions;
using NCB.EventBus.EventBusRabbitMQ;
using NCB.EventBus.Models;
using NCB.EventBus.SubscriptionsManager;
using NCB.Store.Api.DataAccess;
using NCB.Store.Api.DataAccess.BaseRepository;
using NCB.Store.Api.DataAccess.BaseUnitOfWork;
using NCB.Store.Api.GraphQL.GraphQLSchema;
using NCB.Store.Api.IntegrationEnvents.EventHandlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NCB.Store.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Graphql
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<IDocumentWriter, DocumentWriter>();
            services.AddScoped<ISchema, AppSchema>();

            services.AddGraphQL(o => { o.ExposeExceptions = false; })
                .AddGraphTypes(ServiceLifetime.Scoped);

            services.AddControllers(options =>
            {
                //options.Filters.Add(typeof(ValidateModelAttribute));
            }).AddNewtonsoftJson(opt =>
            {
                //opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
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

            //dbcontext
            var connection = Configuration.GetConnectionString("DevelopmentConnectionString");
            services.AddDbContext<DataDbContext>(options =>
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
            //services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            // rabbitmq connection
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();

            //event bus
            services.AddSingleton<IEventBus, EventBusRabbitMQ>();

            //event bus manager
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            //handle event
            services.AddTransient<CitySaledEventHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // adding our schema to the request’s pipeline as well as the Playground UI tool which we are just about to use.
            app.UseGraphQL<AppSchema>();

            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());

            RunMigration(app);
            Init(context);
            ConfigureEventBus(app);
        }

        //private
        private void RunMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DataDbContext>().Database.Migrate();
            }
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<CitySaledEventHandler>("city_saled");
        }

        private void Init(DataDbContext context)
        {
            new DataDbSeed(context).SeedAsync().Wait();
        }
    }
}
