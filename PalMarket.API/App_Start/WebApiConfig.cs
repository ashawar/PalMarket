using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace PalMarket.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // JSON.net serializer settings
            config.Formatters.JsonFormatter.SerializerSettings =
            new JsonSerializerSettings
            {
                //DateFormatHandling = DateFormatHandling.IsoDateFormat,
                //DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Include
            };

            // ISO 8601 format
            IsoDateTimeConverter converter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                Culture = System.Globalization.CultureInfo.InvariantCulture
            };
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(converter);
        }
    }
}