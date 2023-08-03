using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace DNyC.Controllers
{
    public class RSSController : Controller
    {
        // GET: RSS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgenteTop()
        {
            // ====== Para insertar un custom feed

            //Response.ContentType = "application/atom+xml";
            //PartialViewResult result = PartialView(model: ConfigurationManager.AppSettings["RSSid"]);

            ////string filePath = Path.Combine(Server.MapPath("~"), "Content", "rss", "feed_" + DateTime.Now.ToString("yyyy-MM-dd") + ".atom");
            //string filePath = Path.Combine(Server.MapPath("~"), "Content", "rss", "feed.atom");

            //if (System.IO.File.Exists(filePath))
            //    return Content(System.IO.File.ReadAllText(filePath));

            //using (StreamWriter writer = System.IO.File.CreateText(filePath))
            //{
            //    var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "AgenteTop");
            //    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer);
            //    viewResult.View.Render(viewContext, writer);
            //    viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
            //}
            //return Content(System.IO.File.ReadAllText(filePath));



            // Solo lee los feeds y los regresa en esta ruta.
            //Response.ContentType = "application/atom+xml";
            Response.ContentType = "application/rss+xml";
            var rssFeed = new Uri("https://www.AgenteTop.com/blog/index.php/feed/");
            var request = (HttpWebRequest)WebRequest.Create(rssFeed);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var feedContents = reader.ReadToEnd();
                return Content(feedContents);

                // Lee los feeds del blog.
                //var document = XDocument.Parse(feedContents);
                //var posts = (from p in document.Descendants("item")
                //             select new
                //             {
                //                 Title = p.Element("title").Value,
                //                 Link = p.Element("link").Value,
                //                 Comments = p.Element("comments").Value,
                //                 PubDate = DateTime.Parse(p.Element("pubDate").Value)
                //             }).ToList();
            }

            //return View();
        }
    }
}