using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WireMock.Settings;

namespace WireMockServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
                        // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables() // <-- this is needed to to override settings via the Azure Portal App Settings
                .Build();

            // Add LoggerFactory
            var factory = new LoggerFactory();
            services.AddSingleton(factory
                .AddConsole(configuration.GetSection("Logging"))
                .AddDebug()
            );

            services.AddSingleton(factory.CreateLogger("WireMock.Net Logger"));

            // Add access to generic IConfigurationRoot
            services.AddSingleton(configuration);

            // Add access to IFluentMockServerSettings
            var settings = configuration.GetSection("FluentMockServerSettings").Get<FluentMockServerSettings>();
            services.AddSingleton<IFluentMockServerSettings>(settings);

            // Add services
            services.AddTransient<IWireMockService, WireMockService>();

            // Add app
            services.AddTransient<App>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ApplicationServices.GetService<App>().Run();
        }
    }
}
