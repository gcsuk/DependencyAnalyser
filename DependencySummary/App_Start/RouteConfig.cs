using System.Web.Mvc;
using System.Web.Routing;

namespace DependencySummary
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ComponentDetails",
                url: "ComponentDetails/{componentId}",
                defaults: new { controller = "Home", action = "ComponentDetails" }
            );

            routes.MapRoute(
                name: "ComponentDetailsAPI",
                url: "api/Summary/{componentId}",
                defaults: new { controller = "Summary", action = "Get" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
