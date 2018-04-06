using Kebue.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Kebue.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {  
            //跨域配置
            config.EnableCors(new EnableCorsAttribute(WebSettingsConfig.Cors_origins, WebSettingsConfig.Cors_headers, WebSettingsConfig.Cors_methods));
            // Web API 配置和服务
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new ApiSecurityFilter());
            // Web API 路由

            //WebApi areas 中api路径
            config.Routes.MapHttpRoute(
               name: "DefaultApi2",
               routeTemplate: "api/{area}/{controller}/{action}/{id}",
               defaults: new { action = "Index", id = RouteParameter.Optional }
           );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
