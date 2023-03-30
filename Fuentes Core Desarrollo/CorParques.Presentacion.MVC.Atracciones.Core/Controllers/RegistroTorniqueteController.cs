using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Atracciones.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Transversales.Contratos;

namespace CorParques.Presentacion.MVC.Atracciones.Core.Controllers
{
    public class RegistroTorniqueteController : ControladorBase
    {
        #region Metodos
                      
        public async Task<ActionResult> Index()
        {

            RegistroTorniquete objRegistroTorniquete = null;
            objRegistroTorniquete = await GetAsync<RegistroTorniquete>($"RegistroTorniquete/ObtenerRegistroTorniquete/{IdPunto}");
            if (objRegistroTorniquete == null)
            {
                objRegistroTorniquete = new RegistroTorniquete();
                objRegistroTorniquete.IdRegistroTorniquete = 0;
            }
            else if (objRegistroTorniquete.IdRegistroTorniquete == 0)
            {
                objRegistroTorniquete.Inicio = objRegistroTorniquete.Fin;
                objRegistroTorniquete.Fin = 0;
            }                

            return View(objRegistroTorniquete);
        }

        //public async Task<ActionResult> GetList()
        //{
        //    var lista = await GetAsync<RegistroTorniquete>($"RegistroTorniquete/ObtenerRegistroTorniquete/{IdPunto}");
        //    return PartialView("_RegistroTorniquete", lista);
        //}

        public async Task<ActionResult> Insert(RegistroTorniquete modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = IdUsuarioLogueado;
            modelo.IdPunto = IdPunto;
            modelo.FechaCreacion = DateTime.Now;            
            var resultado = await PostAsync<RegistroTorniquete, string>("RegistroTorniquete/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Update(RegistroTorniquete modelo)
        {
            modelo.IdPunto = IdPunto;
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = IdUsuarioLogueado;
            var resultado = await PutAsync<RegistroTorniquete, string>("RegistroTorniquete/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}