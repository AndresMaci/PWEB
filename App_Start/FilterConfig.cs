using System.Web;
using System.Web.Mvc;

namespace Aplicativo_Alquiler_Vehiculos
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
