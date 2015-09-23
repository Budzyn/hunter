using System.Web;
using System.Web.Mvc;
using Hunter.Rest.Filters;

namespace Hunter.Rest
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new System.Web.Mvc.HandleErrorAttribute());
        }
    }
}
