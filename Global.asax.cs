using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DNyC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            try
            {
                String SessionID = Session.SessionID;
                dynamic dJSON = new System.Dynamic.ExpandoObject();
                dJSON.SessionID = Session.SessionID;
                dJSON.Tipo = "newSession";
                HttpRequest request = HttpContext.Current.Request;
                String platform = Models.cGeneral.GetUserPlatform(request);
                dJSON.Browser_Name = request.Browser.Browser;
                dJSON.Browser_Version = request.Browser.Version;
                dJSON.Browser_Platform = platform;

                System.Xml.XmlDocument oXML = Models.cGeneral.LlamaSP("bo.Gen$AgenteTopAnalytics", Newtonsoft.Json.JsonConvert.SerializeObject(dJSON));
            }
            catch { }
        }
    }
}
