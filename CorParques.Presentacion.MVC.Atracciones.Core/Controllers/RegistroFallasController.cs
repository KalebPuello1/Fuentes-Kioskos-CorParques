using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Atracciones.Core.Controllers
{
    public class RegistroFallasController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            ViewBag.Puntos = puntos;
            var areas = await GetAsync<IEnumerable<TipoGeneral>>("Area/ObtenerListaAreas");
            ViewBag.Areas = areas;
            var orden = await GetAsync<IEnumerable<Orden>>("Orden/ObtenerTodos");
            ViewBag.Orden = orden;

            return View();
        }

        public async Task<JsonResult> Salvar(RegistroFallas modelo)
        {
            Models.RespuestaViewModel resultado = new Models.RespuestaViewModel();

            try
            {
                resultado = await PostAsync<RegistroFallas, object>($"RegistroFallas/set", modelo);
            }
            catch (Exception ex)
            {
                resultado.Elemento = false;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> consultarUsuario(string nombreUsuario)
        {
            string nombre = string.Empty;

            IEnumerable<Usuario> usuarios = null;
            try
            {
                usuarios = await GetAsync<IEnumerable<Usuario>>("Usuario/GetAll");
                var datos = usuarios.Where(M => M.NombreUsuario == nombreUsuario).Select(M => new { M.Nombre, M.Apellido }).FirstOrDefault();
                nombre = datos.Nombre + " " + datos.Apellido;
            }
            catch (Exception ex)
            {
                nombre = string.Empty;
            }

            return Json(nombre, JsonRequestBehavior.AllowGet);
        }


    }
}