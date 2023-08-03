using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DNyC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //url /contactanos
            routes.MapRoute(
              "contactanos",                                           // Route name
              "contactanos",                            // URL with parameters
              new { controller = "Home", action = "Contacto", id = UrlParameter.Optional } // Parameter defaults
           ).DataTokens.Add("area", "Sitio");

            //url /recetario
            routes.MapRoute(
              "recetario",                                           // Route name
              "recetario",                            // URL with parameters
              new { controller = "Home", action = "Resetas", id = UrlParameter.Optional } // Parameter defaults
           ).DataTokens.Add("area", "Sitio");

            //url /video-recetas
            routes.MapRoute(
              "video-recetas",                                           // Route name
              "video-recetas",                            // URL with parameters
              new { controller = "Home", action = "Video", id = UrlParameter.Optional } // Parameter defaults
           ).DataTokens.Add("area", "Sitio");

            //url /recetario
            routes.MapRoute(
              "recetario/cuestionario",                                           // Route name
              "recetario/cuestionario",                            // URL with parameters
              new { controller = "Home", action = "Resetas_Contacto", id = UrlParameter.Optional } // Parameter defaults
           ).DataTokens.Add("area", "Sitio");

            //url /video-recetas
            routes.MapRoute(
              "video-recetas/cuestionario",                                           // Route name
              "video-recetas/cuestionario",                            // URL with parameters
              new { controller = "Home", action = "Video_Contacto", id = UrlParameter.Optional } // Parameter defaults
           ).DataTokens.Add("area", "Sitio");

            //url /descargar-recetario
            routes.MapRoute(
              "descargar-recetario",                                           // Route name
              "descargar-recetario",                            // URL with parameters
              new { controller = "Home", action = "Obtener_Resetas", id = UrlParameter.Optional } // Parameter defaults
           ).DataTokens.Add("area", "Sitio");


            //url /descargar-recetario
            routes.MapRoute(
              "video-recetario",                                           // Route name
              "videos-recetario",                            // URL with parameters
              new { controller = "Home", action = "Obtener_Videos", id = UrlParameter.Optional } // Parameter defaults
           ).DataTokens.Add("area", "Sitio");

            //Url Home
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ).DataTokens.Add("area", "Sitio");



        }
    }
}
