using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace CorParques.Presentacion.MVC.Controllers
{
    public class AdminMensajesVisualController : ControladorBase
    {
        // GET: AdminMensajesVisual
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<MensajesVisual>>("Cortesia/ListarMensajesVisual");
            return View(lista);
        }
        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<MensajesVisual>>("Cortesia/ListarMensajesVisual");
            return PartialView("_List", lista);
        }

        public ActionResult GetPartial()
        {
            return PartialView("_Create");
        }

        public async Task<ActionResult> GetById(string Codigo)
        {
            var ListPantalla2 = await GetAsync<IEnumerable<MensajesVisual>>($"Cortesia/ObtenerMensajesVisualXCod/{Codigo}");

            var ListPantalla = new MensajesVisual();
            if (ListPantalla2 != null)
            {
                ListPantalla = ListPantalla2.FirstOrDefault();
            }
            return PartialView("_Edit", ListPantalla);
        }
        public async Task<ActionResult> Insert(MensajesVisual modelo)
        {
            modelo.UsuarioModificacion = ((Usuario)Session["UsuarioAutenticado"]).NombreUsuario;
            if (await PostAsync<MensajesVisual, string>("Cortesia/AgregarMensajesVisual", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el parámetro. Por favor inténtelo de nuevo" });
        }

        public async Task<ActionResult> Update(MensajesVisual modelo)
        {
            modelo.UsuarioModificacion = ((Usuario)Session["UsuarioAutenticado"]).NombreUsuario;
            if (await PostAsync<MensajesVisual, string>("Cortesia/ActualizarMensajesVisual", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el parámetro. Por favor inténtelo de nuevo" });
        }

        public async Task<ActionResult> Delete(string Codigo)
        {
            if (await GetAsync<string>($"Cortesia/EliminarMensajesVisual/{Codigo}") != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error eliminando el parámetro. Por favor inténtelo de nuevo" });
        }
    }
}
