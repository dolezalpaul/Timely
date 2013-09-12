using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MongoDB.Driver;
using Moravia.Timely.Components;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web.Mvc;

namespace Moravia.Timely
{
    public static class ContainerConfig
    {
        public static void RegisterContainer()
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

            // Register your types, for instance:
            container.RegisterPerWebRequest<MongoDatabase>(() =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["mongo"].ConnectionString;

                var dbName = MongoUrl.Create(connectionString).DatabaseName;
                var client = new MongoDB.Driver.MongoClient(connectionString);
                var mongoServer = client.GetServer();

                return mongoServer.GetDatabase(dbName);
            });

            container.RegisterManyForOpenGeneric(typeof(IRepository<>), typeof(IRepository<>).Assembly);

            container.RegisterPerWebRequest<IPrincipal>(() => 
                HttpContext.Current.User ?? WindowsPrincipal.Current);

            var componentTypes = typeof(Component).Assembly.GetExportedTypes().Where(t => t.Name.EndsWith("Component") && !t.IsInterface);
            foreach (var component in componentTypes)
            {
                container.Register(component);
            }

            // Verify the container configuration
            container.Verify();

            // Register the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            DependencyResolver.SetResolver(
                new SimpleInjectorDependencyResolver(container));
        }
    }
}