using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class UbicacionesController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {

            var modelo = await ObtenerDatosIniciales();
            return View(modelo);
        }
        
        public async Task<ActionResult> GetPartial()
        {
            var _UbicacionPunto = new UbicacionPunto();
            //ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Ubicaciones}");
            ViewBag.Puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/ObtenerPuntosDestrezaAtracciones");
            return PartialView("_Crear", _UbicacionPunto);
        }

        public async Task<ActionResult> GetById(int id)
        {
            var model = await GetAsync<UbicacionPunto>($"UbicacionPunto/ObtenerPorId/{id}");
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Ubicaciones}");
            ViewBag.Puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/ObtenerTodosPuntosActivos");
            return PartialView("_Editar", model);
        }        

        public async Task<ActionResult> Insert(UbicacionPunto modelo)
        {
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdEstado = Enumerador.Estados.Activo.GetHashCode();
            var rta = await PostAsync<UbicacionPunto, string>("UbicacionPunto/Insertar", modelo);
            if (string.IsNullOrEmpty(rta.Mensaje))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = rta.Mensaje });
            //return Json(rta, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetList()
        {
            var modelo = await ObtenerDatosIniciales();
            return PartialView("_List", modelo);
        }

        private async Task<IEnumerable<UbicacionPunto>> ObtenerDatosIniciales()
        {
            var modelo = await GetAsync<IEnumerable<UbicacionPunto>>("UbicacionPunto/ObtenerUbicaciones");
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Ubicaciones}");
            ViewBag.Puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/ObtenerPuntosDestrezaAtracciones");
            return modelo;
        }

        public async Task<ActionResult> Update(UbicacionPunto modelo)
        {            
            
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.FechaModificacion = DateTime.Now;
            var rta = await PutAsync<UbicacionPunto, string>("UbicacionPunto/Actualizar", modelo);
            if (rta == string.Empty)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = rta });
            
        }
                
        public async Task<ActionResult> Delete(int id)
        {
            if (await DeleteAsync($"UbicacionPunto/Delete/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error eliminando el parámetro. Por favor intentelo de nuevo" });
        }
    }
}