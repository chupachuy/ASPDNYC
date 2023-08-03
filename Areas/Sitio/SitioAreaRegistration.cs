using System.Web.Mvc;

namespace DNyC.Areas.Sitio
{
    public class SitioAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Sitio";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Sitio_default",
                "Sitio/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}