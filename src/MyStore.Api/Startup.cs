﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyStore.Api.Framework;
using MyStore.Core.Repositories;
using MyStore.Infrastructure.Repositories;
using MyStore.Services.Products;
using Newtonsoft.Json;

namespace MyStore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
 
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(o => o.SerializerSettings.Formatting = Formatting.Indented);
            services.AddMemoryCache();
            services.AddResponseCaching();
            services.Configure<AppOptions>(Configuration.GetSection("app"));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, InMemoryProductRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole()
                .AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseResponseCaching();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMvc();
        }
    }
}
