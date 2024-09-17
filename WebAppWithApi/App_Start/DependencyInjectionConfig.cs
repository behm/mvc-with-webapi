using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using WebAppWithApi.Data;

namespace WebAppWithApi.App_Start
{
    public static class DependencyInjectionConfig
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

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
    }
}