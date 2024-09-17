using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebAppWithApi.App_Start;

namespace WebAppWithApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configure Dependency Injection
            var serviceProvider = DependencyInjectionConfig.ConfigureServices();

            // Set custom controller factory
            ControllerBuilder.Current.SetControllerFactory(new CustomControllerFactory(serviceProvider));
        }
    }
}
