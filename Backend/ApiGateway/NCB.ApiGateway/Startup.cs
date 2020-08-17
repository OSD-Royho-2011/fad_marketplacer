using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using MMLib.Ocelot.Provider.AppConfiguration;
namespace NCB.ApiGateway
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("ocelot.json", optional: true, reloadOnChange: true)
                //.AddOcelotWithSwaggerSupport(folder: "Configuration")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOcelot(Configuration);
            services.AddSwaggerForOcelot(Configuration);

            // gatteway controller
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseHttpsRedirection();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // gatteway controller
            app.UseSwagger();
            app.UseSwaggerForOcelotUI(opt =>
            {
                //opt.DownstreamSwaggerHeaders = new[]
                //{
                //        new KeyValuePair<string, string>("Key", "Value"),
                //        new KeyValuePair<string, string>("Key2", "Value2"),
                //};
                opt.PathToSwaggerGenerator = "/swagger/docs"; // microservice
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); // gatteway controller
            })
            .UseOcelot()
            .Wait();

            //// only microservice
            //app.UseSwaggerForOcelotUI(opt =>
            //{
            //    opt.PathToSwaggerGenerator = "/swagger/docs";
            //})
            //.UseOcelot()
            //.Wait();
        }
    }
}
