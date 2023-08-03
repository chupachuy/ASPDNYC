using DNyC.Areas.Sitio.Models;
using DNyC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DNyC.Areas.Sitio.Controllers
{
    public class HomeController : Controller
    {
        // GET: Sitio/Home
        public ActionResult Index()
        {
            cHome oObje = new cHome { Section = "" };
            return View(oObje);
        }

        // GET: Sitio/Contacto
        public ActionResult Contacto()
        {
            ViewBag.Recetas = "";
            cHome oObje = new cHome { Section = "" };
            return View(oObje);
        }

        // GET: Sitio/Contacto
        public ActionResult Resetas(string id)
        {
            ViewBag.Recetas = "";

            cHome oObje = new cHome { Section = "" };
            return View(oObje);
        }

        // GET: Sitio/Contacto
        public ActionResult Resetas_Contacto()
        {
            ViewBag.Recetas = "cuestionario";

            cHome oObje = new cHome { Section = "" };
            return View(oObje);
        }

        // GET: Sitio/Contacto
        public ActionResult Video(string id)
        {
            ViewBag.Recetas = "";

            cHome oObje = new cHome { Section = "" };
            return View(oObje);
        }

        // GET: Sitio/Contacto
        public ActionResult Video_Contacto(string id)
        {
            ViewBag.Recetas = "cuestionario";

            cHome oObje = new cHome { Section = "" };
            return View(oObje);
        }

        // GET: Sitio/Contacto
        public ActionResult Obtener_Resetas()
        {
            cHome oObje = new cHome { Section = "" };

            PDF_Recetas ob = new PDF_Recetas();
            String path = System.IO.Path.Combine(Server.MapPath("~"), "Documentos", "Recetas", "PDF.json");
            using (StreamReader r = new StreamReader(path, Encoding.Default))
            {
                var json2 = r.ReadToEnd();
                ob = JsonConvert.DeserializeObject<PDF_Recetas>(json2);
            }
            oObje.Recetas = ob.RECETAS_PDF;

            return View(oObje);
        }

        // GET: Sitio/Contacto
        public ActionResult Obtener_Videos()
        {
            cHome oObje = new cHome { Section = "" };

            IGTV_Recetas ob = new IGTV_Recetas();
            String path = System.IO.Path.Combine(Server.MapPath("~"), "Documentos", "Recetas", "Videos.json");
            using (StreamReader r = new StreamReader(path, Encoding.Default))
            {
                var json2 = r.ReadToEnd();
                ob = JsonConvert.DeserializeObject<IGTV_Recetas>(json2);
            }
            oObje.Videos = ob.RECETAS_VIDEOS;

            return View(oObje);
        }

        [HttpPost]
        // GET: Envio de Correo hubspot
        public ActionResult FormComentario(String pJSON)
        {

            String msj = "";
            HubSpot hs = new HubSpot();
            Boolean _EsPro = cGeneral.EsProduccion();

            String txtNombre = JObject.Parse(pJSON)["txtNombre"].ToString();
            //String txtApellido = JObject.Parse(pJSON)["txtApellido"].ToString();
            String txtCorreo = JObject.Parse(pJSON)["txtCorreo"].ToString();
            String txtEdad = JObject.Parse(pJSON)["txtEdad"].ToString();
            String txtEstado = JObject.Parse(pJSON)["txtEstado"].ToString();
            String txtPreguntas = JObject.Parse(pJSON)["txtPreguntas"].ToString();
            String txtPagina = JObject.Parse(pJSON)["txtPagina"].ToString();

            if (_EsPro == false)
            {
                txtNombre = "PRUEBA " + txtNombre;
            }

            if (hs.EnviarConacto(txtNombre, "", txtCorreo, txtEdad, txtEstado, txtPreguntas, txtPagina))
            {
                String textoCorreo = "";
                String pAsun = "";
                if (txtPagina == "Pagina de Contactanos")
                {
                    pAsun = "Te contactaremos pronto.";
                    textoCorreo = System.IO.File.ReadAllText(System.IO.Path.Combine(Server.MapPath("~"), "Documentos//Correos//formulario_contacto.html"));
                }
                else if (txtPagina == "Pagina de Recetario")
                {
                    pAsun = "¡Tu recetario está listo!";
                    textoCorreo = System.IO.File.ReadAllText(System.IO.Path.Combine(Server.MapPath("~"), "Documentos//Correos//formulario_recetario.html"));
                }
                else
                {
                    pAsun = "¡Nuestras recetas en video!";
                    textoCorreo = System.IO.File.ReadAllText(System.IO.Path.Combine(Server.MapPath("~"), "Documentos//Correos//formulario_video.html"));
                }


                msj = "Gracias por contactarnos.";
                Task mytask = Task.Run(() =>
                {
                    cGeneral.fEnviaCorreo(txtCorreo, "", "", pAsun, textoCorreo, null);
                });
            }
            else
                msj = "Error de envio";

            return Json(new { success = true, message = msj }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        // GET: Envio de Correo 52 Recetas
        public ActionResult FormEnviarCorreo(String pJSON)
        {
            String msj = "";
            Boolean success = false;
            HubSpot hs = new HubSpot();
            Boolean _EsPro = cGeneral.EsProduccion();

            String txtCorreo = JObject.Parse(pJSON)["txtCorreo"].ToString();

            String textoCorreo = System.IO.File.ReadAllText(System.IO.Path.Combine(Server.MapPath("~"), "Documentos//Correos//formulario_recetario.html"));
            String pAsun = "¡Tu recetario está listo!";

            List<System.Net.Mail.Attachment> pArchivos = new List<System.Net.Mail.Attachment>();
            String path;
            string uploadPath = System.IO.Path.Combine("Documentos", "Recetas", "Pdf");
            path = System.IO.Path.Combine(Server.MapPath("~"), uploadPath);

            Attachment archivo = new Attachment(path + "\\52Rcts_CarozosCalif_2021_sRGB.pdf");
            archivo.ContentDisposition.FileName = "52Rcts_CarozosCalif_2021_sRGB.pdf";
            pArchivos.Add(archivo);

            Task mytask = Task.Run(() =>
            {
                cGeneral.fEnviaCorreo(txtCorreo, "", "", pAsun, textoCorreo, null, pArchivos);
            });

            //msj = "Tenemos un problema en la salida de correos";
            //success = false;
            //if (cGeneral.fEnviaCorreo(txtCorreo, "", "", pAsun, textoCorreo, null, pArchivos))
            //{
            //    msj = "Gracias por contactarnos.";
            //    success = true;
            //}

            return Json(new { success = success, message = msj }, JsonRequestBehavior.AllowGet);
        }
    }
}