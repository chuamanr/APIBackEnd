using EcoOplacementApi.App_Start;
using EcoOplacementApi.Filters;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EcoOplacementApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Rutas de API web
         
            config.MapHttpAttributeRoutes();

            string host = WebConfigurationManager.AppSettings["CORS_Allowed_Host"].ToString();
            EnableCorsAttribute cors = new EnableCorsAttribute(host, "*", "*");
            config.EnableCors(cors);
            config.MessageHandlers.Add(new TokenValidationHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
