using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CorParques.Presentacion.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        { 
             Exception exc = Server.GetLastError();

            //Guadar Log

            string strRuta = Server.MapPath("~/Logs/");
            string strNombre = DateTime.Now.ToString("dd_MM_yyyy") + ".txt";

            if (!System.IO.Directory.Exists(strRuta))
                System.IO.Directory.CreateDirectory(strRuta);

            using (var sw = new System.IO.StreamWriter(System.IO.Path.Combine(strRuta,strNombre), true))
            {
                sw.WriteLine("*********************************************************");
                sw.WriteLine("Error :" + exc.Message);
                sw.WriteLine("Hora :" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sw.WriteLine("Traza: " + exc.StackTrace);
                sw.Close();
            }

            // Handle HTTP errors
            //if (exc.GetType() != typeof(HttpException))
            //    Server.Transfer("~/Errores/Err500");
            
        }
    }
}
