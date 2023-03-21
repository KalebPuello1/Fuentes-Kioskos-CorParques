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
    public class GestionMantenimientoController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<GestionMantenimientoControl>>("GestionMantenimientoControl/GetAll");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<GestionMantenimientoControl>>("GestionMantenimientoControl/GetAll");
            return PartialView("_List",lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new GestionMantenimientoControl();
            var lista = await GetAsync<IEnumerable<TipoGeneral>>("TipoBrazalete/ObtenerListaSimpleAtraccion");            
            modelo.Atracciones = lista;
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> GetPartialMantenimiento(int id)
        {
            var lista = await GetAsync<IEnumerable<TipoGeneral>>($"GestionMantenimiento/ObtenerxAtraccion/{id}");
            ViewBag.Mantenimientos = lista;
            return PartialView("_ListMantenimiento");
        }

        public async Task<ActionResult> Obtener(int Id)
        {
            var item = await GetAsync<GestionMantenimientoControl>($"GestionMantenimientoControl/Obtener/{Id}");
            var lista = await GetAsync<IEnumerable<GestionMantenimientoDetalle>>($"GestionMantenimientoDetalle/ObtenerListaSimple/{Id}");            
            item.MantenimientoDetalle = lista;
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> ActualizarMantenimiento(GestionMantenimientoControl modelo)
        {
            
            modelo.UsuarioModicifacion = ((Usuario)Session["UsuarioAutenticado"]).Id;
            modelo.FechaModificacion = DateTime.Now;            
            modelo.UsuarioCreacion = 1;//TODO: en actualizar solo se envia el usuario de modificacion el de creacion no se debe modificar
            modelo.FechaCreacion = DateTime.Now;            
            if (await PostAsync<GestionMantenimientoControl, string>("GestionMantenimientoControl/ActualizarMantenbimientoControl",  modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el parámetro. Por favor intentelo de nuevo" });
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (await DeleteAsync($"TipoBrazalete/block/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error eliminando el parámetro. Por favor intentelo de nuevo" });
        }

        public static bool Asociada(string strIdAtraccion, string strIdAtraccionesXBrazalete)
        {

            bool blnRetorno = false;
            string[] split;

            if (!string.IsNullOrEmpty(strIdAtraccionesXBrazalete))
            {
                split = strIdAtraccionesXBrazalete.Split(',');
                for (int i = 0; i < split.Length; i++)
                {
                    if (strIdAtraccion == split[i])
                    {
                        blnRetorno = true;
                        break;
                    }

                }
            }
            return blnRetorno;
        }
    }
}