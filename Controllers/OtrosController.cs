using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DNyC.Controllers
{
    public class OtrosController : Controller
    {
        // GET: Otros
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Configuracion()
        {
            return View();
        }

        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            return View();
        }
        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}