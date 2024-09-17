using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebAppWithApi.App_Start
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomControllerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_serviceProvider.GetService(controllerType);
        }
    }
}