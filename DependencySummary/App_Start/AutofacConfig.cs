using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace DependencySummary
{
    public class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register MVC controllers.
            builder.RegisterControllers(typeof (WebApiApplication).Assembly);

            builder.RegisterType<Services.ComponentService>().As<Services.IComponentService>();
            builder.RegisterType<Services.PackageService>().As<Services.IPackageService>();
            builder.RegisterType<Services.SummaryService>().As<Services.ISummaryService>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();

            // Web API dependency resolver
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // MVC dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}