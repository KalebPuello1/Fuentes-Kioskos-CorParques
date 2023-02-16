using CorParques.Presentacion.MVC.Redencion.Core.App_Start;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Redencion.Core
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
