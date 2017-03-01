using AutoMapper;
using CarRentalApi.Common;
using CarRentalApi.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CarRentalApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json" , optional : true , reloadOnChange : true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json" , optional : true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<CampContext>(options =>
                                               options.UseSqlServer(
                                                                    Configuration.GetConnectionString(
                                                                                                      "DefaultConnection")));
            services.AddAutoMapper();
            services.AddIdentity<CampUser , IdentityRole>()
                    .AddEntityFrameworkStores<CampContext>()
                    .AddDefaultTokenProviders();
            services.AddTransient<ICampRepository , CampRepository>();
            services.AddTransient<CampDbInitializer>();
            services.AddMvc().AddJsonOptions(opt =>
                                             {
                                                 opt.SerializerSettings.ReferenceLoopHandling =
                                                     ReferenceLoopHandling.Ignore;
                                             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app ,
                              IHostingEnvironment env ,
                              ILoggerFactory loggerFactory ,
                              CampDbInitializer seeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            seeder.Seed().Wait();
            app.UseStatusCodePages();
            app.UseMvc(
                       config =>
                       {
                           //config.MapRoute("MyFirstRought" , "api/{controller}/{action}");
                       });
        }
    }
}
