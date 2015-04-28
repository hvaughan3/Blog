using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HinesSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
