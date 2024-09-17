using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using WebAppWithApi.Data;
using WebAppWithApi.Logging;

namespace WebAppWithApi.App_Start
{
    public static class DependencyInjectionConfig
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.ConfigureLogging();

            // Register Services
            services.AddTransient<IAppInfoRepo, AppInfoRepo>();
            //...

            // Register controllers
            //services.AddTransient<HomeController>();
            //services.AddTransient<ValuesController>();
            var assembly = Assembly.GetExecutingAssembly();
            services.AddControllersAsServices(assembly);

            // Create Service Provider
            var serviceProvider = services.BuildServiceProvider();

            // Set MVC Dependency Resolver
            DependencyResolver.SetResolver(new DefaultDependencyResolver(serviceProvider));

            // Set Web API Dependency Resolver
            GlobalConfiguration.Configuration.DependencyResolver = new DefaultDependencyResolver(serviceProvider);

            return serviceProvider;
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddControllersAsServices(this IServiceCollection services, Assembly assembly)
        {
            var controllerTypes = assembly.GetExportedTypes()
                .Where(type => typeof(IController).IsAssignableFrom(type) || typeof(IHttpController).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract && type.IsPublic);

            foreach (var controllerType in controllerTypes)
            {
                services.AddTransient(controllerType);
            }
        }

        public static void ConfigureLogging(this IServiceCollection services)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ErrorLogger"]?.ConnectionString;

            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddApplicationInsights();
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    configure.AddDatabaseLogger(connectionString);  // note: this would take the place of Elmah logging
                }
            });

            var config = TelemetryConfiguration.Active;
            //config.ConnectionString = "";
            config.TelemetryInitializers.Add(new MyTelemetryInitializer());

            //services.AddSingleton<ITelemetryInitializer, MyTelemetryInitializer>();
            //services.AddSingleton<ITelemetryChannel, InMemoryChannel>();
            //services.AddSingleton<TelemetryConfiguration>(provider =>
            //{
            //    var config = TelemetryConfiguration.CreateDefault();
            //    config.ConnectionString = "";
            //    return config;
            //});
        }
    }

    public class MyTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            // Custom telemetry initialization logic

            telemetry.Context.Cloud.RoleName = "WebAppWithApi Test App";

            telemetry.Context.GlobalProperties["Environment"] = "Development";  // i.e. Staging, Production, etc.
            telemetry.Context.GlobalProperties["Version"] = "1.0.0";
        }
    }
}