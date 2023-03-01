using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> setView(int id)
        {
            HttpClient cliente;
            cliente = new HttpClient();
            cliente.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlService"]);

            string result = "";
            HttpResponseMessage response = await cliente.GetAsync($"Notificacion/SetView/{id}/{((Usuario)Session["UsuarioAutenticado"]).Id}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<string>();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}