using System.Web.Mvc;
using System.Web.Routing;

namespace SilverzoneERP.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

          routes.MapRoute(
              name: "DefaultOverride",
              url: "{*.}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "SilverzoneERP.Web.Controllers" }
          );

            // default URL which contain angular UI view & then redirect to a new URL by angular ROUTING
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new string[] { "SilverzoneERP.Web.Controllers" }
            );

        }
    }
}
