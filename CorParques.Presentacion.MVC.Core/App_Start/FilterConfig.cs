using CorParques.Presentacion.MVC.Core.App_Start;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Core
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new FiltroAutorizacion());
        }
    }
}
