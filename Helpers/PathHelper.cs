using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNyC.Helpers
{
    public class PathHelper
    {
        /// <summary>
        /// Regresa la ruta base del dominio donde se está ejecutando.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetCurrentDomain(HttpRequestBase request)
        {
            return string.Format("{0}{1}/", request.Url.GetLeftPart(UriPartial.Authority), request.ApplicationPath.TrimEnd('/'));
        }
    }
}