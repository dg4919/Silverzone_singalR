using System.Web.Http;
using System.Web.Http.Cors;

namespace SilverzoneERP.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // enable cors to call this api by jquery/angularjs
            var cors = new EnableCorsAttribute("*", "*", "*");      // allow from all origins > means from which url this API will call
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
              name: "VersoningAPI",
              routeTemplate: "api/{namespace}/{controller}/{action}/{Id}",
              defaults: new { Id = RouteParameter.Optional }
          );

        }
    }
}
