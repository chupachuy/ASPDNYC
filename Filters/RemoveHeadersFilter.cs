using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DNyC.Filters
{
    public class RemoveHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.Remove("Server");
            filterContext.HttpContext.Response.Headers.Remove("X-AspNetMvc-Version");
            base.OnResultExecuted(filterContext);
        }
    }
}