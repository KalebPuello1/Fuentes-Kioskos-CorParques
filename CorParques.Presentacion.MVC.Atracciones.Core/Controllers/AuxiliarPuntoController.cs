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
    public class AuxiliarPuntoController : ControladorBase
    {
        #region Metodos
                      
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<AuxiliarPunto>>($"AuxiliarPunto/ObtenerListaAuxiliarPunto/{IdPunto}");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<AuxiliarPunto>>($"AuxiliarPunto/ObtenerListaAuxiliarPunto/{IdPunto}");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new AuxiliarPunto();
            var ListaUbicacionesPunto = await GetAsync<IEnumerable<UbicacionPunto>>($"UbicacionPunto/ObtenerListaSimplePorPunto/{IdPunto}");            
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(AuxiliarPunto modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = IdUsuarioLogueado;
            modelo.IdPunto = IdPunto;
            modelo.FechaCreacion = DateTime.Now;            
            var resultado = await PostAsync<AuxiliarPunto, string>("AuxiliarPunto/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Update(AuxiliarPunto modelo)
        {
            modelo.IdPunto = IdPunto;
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = IdUsuarioLogueado;
            var resultado = await PutAsync<AuxiliarPunto, string>("AuxiliarPunto/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> BuscarDatosAuxiliar(string Documento, bool Agregar)
        {

            EstructuraEmpleado objEstructuraEmpleado = null;
            AuxiliarPunto objAuxiliarPunto = new AuxiliarPunto();

            try
            {
                objEstructuraEmpleado = await GetAsync<EstructuraEmpleado>($"AuxiliarPunto/ObtenerInformacionAuxiliar/{IdPunto}/{Documento}");

                if (objEstructuraEmpleado != null)
                {
                    objAuxiliarPunto.IdEstructuraEmpleado = objEstructuraEmpleado.IdEstructuraEmpleado;
                    objAuxiliarPunto.TieneUbicacion = objEstructuraEmpleado.TieneUbicacion;
                    if (objAuxiliarPunto.TieneUbicacion > 0 || Agregar)
                    {                        
                        objAuxiliarPunto.NombreEmpleado = string.Join(" ", objEstructuraEmpleado.Nombres, objEstructuraEmpleado.Apellidos);
                        objAuxiliarPunto.Certificado = objEstructuraEmpleado.EsCertificado;
                        var objListaUbicaciones = await GetAsync<IEnumerable<TipoGeneral>>($"UbicacionPunto/ObtenerListaSimplePorPunto/{IdPunto}");
                        objAuxiliarPunto.ListaUbicacionesPunto = objListaUbicaciones;
                    }                    
                }          
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en: AuxiliarPuntoController_BuscarDatosAuxiliar", ex.Message);
            }
            if (Agregar)
            {
                return PartialView("_Create", objAuxiliarPunto);
            }
            else
            {
                return PartialView("_Remove", objAuxiliarPunto);
            }
            
        
        }

        [HttpGet]
        public async Task<ActionResult> ValidarEmpleado(string Documento)
        {

            EstructuraEmpleado objEstructuraEmpleado = null;
            ViewBag.EmpleadoExiste = false;

            try
            {
                objEstructuraEmpleado = await GetAsync<EstructuraEmpleado>($"AuxiliarPunto/ObtenerInformacionAuxiliar/{IdPunto}/{Documento}");

                if (objEstructuraEmpleado != null)
                {
                    ViewBag.EmpleadoExiste = true;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en: AuxiliarPuntoController_ValidarEmpleado", ex.Message);
            }

            return View();
        }


        #endregion
    }
}