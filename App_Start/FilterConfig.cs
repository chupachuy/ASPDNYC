using System.Web;
using System.Web.Mvc;
using DNyC.Filters;

namespace DNyC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RemoveHeadersAttribute());
        }
    }
}
