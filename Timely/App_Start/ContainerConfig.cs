using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Moravia.Timely.Business;
using Moravia.Timely.Controllers;
using Moravia.Timely.Models;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web.Mvc;

namespace Moravia.Timely
{
    public static class ContainerConfig
    {
        public static Container RegisterContainer()
        {
            // Create the container builder.
            var container = new Container();

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcAttributeFilterProvider();

            var services = GlobalConfiguration.Configuration.Services;
            var controllerTypes = services.GetHttpControllerTypeResolver()
                .GetControllerTypes(services.GetAssembliesResolver());

            // register Web API controllers (important! http://bit.ly/1aMbBW0)
            foreach (var controllerType in controllerTypes)
            {
                container.Register(controllerType);
            }
            container.RegisterInitializer<IRestController>(controller => container.InjectProperties(controller));
            container.RegisterSingleOpenGeneric(typeof(AutoRestMapper<,>), typeof(AutoRestMapper<,>));
            container.RegisterInitializer<IAutoRestMapper>(arm => container.InjectProperties(arm));

            // Register your types, for instance:
            container.RegisterPerWebRequest<DbContext>(() =>
            {
                return new TimelyContext();
            }, true);

            // Register Current User depending on the context
            container.RegisterPerWebRequest<IPrincipal>(() =>
                HttpContext.Current.User ?? WindowsPrincipal.Current);

            // Register business components            
            container.RegisterManyForOpenGeneric(
                typeof(BusinessComponent<>),
                (type, impls) => container.RegisterAll(type, impls),
                typeof(BusinessComponent<>).Assembly);

            // Register services
            container.RegisterManyForOpenGeneric(typeof(Service<>), typeof(Service<>).Assembly);
            container.RegisterSingleOpenGeneric(typeof(Service<>), typeof(Service<>));

            // Business Initializers
            container.RegisterInitializer<IBusinessComponent>(component => container.InjectProperties(component));
            container.RegisterInitializer<IService>(service => container.InjectProperties(service));
            
            // Verify the container configuration
            container.Verify();

            // Register the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            DependencyResolver.SetResolver(
                new SimpleInjectorDependencyResolver(container));

            return container;
        }
    }
}