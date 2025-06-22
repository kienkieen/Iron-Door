using System.Web;
using System.Web.Mvc;

namespace Nhom12_Website_Ban_Cua
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
