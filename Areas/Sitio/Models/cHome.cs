using DNyC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNyC.Areas.Sitio.Models
{
    public class cHome: DNyCGeneal
    {
        public String Section { set; get; }

        public List<PDF_Receta> Recetas { set; get; }

        public List<IGTV_Receta> Videos { set; get; }

    }

    public class PDF_Recetas
    {
        public List<PDF_Receta> RECETAS_PDF { set; get; }
    }

    public class PDF_Receta {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string NombrePDF { get; set; }
        public string UrlPDF { get; set; }
        public string Titulo { get; set; }
        public string Fecha { get; set; }
    }

    public class IGTV_Recetas
    {
        public List<IGTV_Receta> RECETAS_VIDEOS { set; get; }
    }

    public class IGTV_Receta
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Url { get; set; }
        public string Titulo { get; set; }
        public string Fecha { get; set; }
    }

}