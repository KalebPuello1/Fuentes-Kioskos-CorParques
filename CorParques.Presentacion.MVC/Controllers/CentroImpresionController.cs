using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class CentroImpresionController : ControladorBase
    {
        // GET: CentroImpresion
        public async  Task<ActionResult> Index()
        {
            var rta = await GetAsync<IEnumerable<SolicitudBoleteria>>("CentroImpresion/ObtenerTodasSolicitudesBoleteria");
            if(rta != null)
                return View(rta.ToList());
            else
                return View();
        }

        public async Task<JsonResult> GestionarCentroImpresion(int IdSolicitud, string CodVenta, bool boletaControl, bool procesar)
        {

            try
            {
                var _modelo = new SolicitudBoleteria
                {
                    IdSolicitudBoleteria = IdSolicitud,
                    IdUsuarioModificacion = IdUsuarioLogueado,
                    EsBoletaControl = boletaControl,
                    Procesar = procesar
                };

                if (string.IsNullOrEmpty(CodVenta))
                    _modelo.IdEstado = (int)Enumerador.Estados.Inactivo;
                else
                    _modelo.IdEstado = (int)Enumerador.Estados.Activo;

                var rta = await PostAsync<SolicitudBoleteria, IEnumerable<SolicitudBoleteria>>("CentroImpresion/GestionarCentroImpresion", _modelo);
                if (rta.Elemento != null)
                {
                    var _result = rta.Elemento as List<SolicitudBoleteria>;
                    foreach (var item in _result)
                    {
                        item.FechaUsoInicialDDMMYYY = item.FechaUsoInicial.ToString("dd/MM/yyyy");
                        item.FechaUsoFinalDDMMYYY = item.FechaUsoFinal.ToString("dd/MM/yyyy");
                    }

                    rta.Elemento = _result;
                    return Json(rta, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new ArgumentException(string.Concat("Error procesando solicitud: ", IdSolicitud, " codigo venta: ", CodVenta, " es boleta control: ", boletaControl.ToString()));
                }                
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CentroImpresionController_GestionarCentroImpresion");
                return Json(new Models.RespuestaViewModel { Correcto = false, Mensaje = "" }, JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var rta = await GetAsync<IEnumerable<SolicitudBoleteria>>("CentroImpresion/ObtenerTodasSolicitudesBoleteria");
            return PartialView("_List",rta.ToList());
        }

        public async Task<ActionResult> EliminarSolicitud(int IdSolicitud, string CodVenta, bool boletaControl)
        {
            var _modelo = new SolicitudBoleteria
            {
                IdSolicitudBoleteria = IdSolicitud,
                IdUsuarioModificacion = IdUsuarioLogueado,
                EsBoletaControl = boletaControl
            };

            var rta = await PostAsync<SolicitudBoleteria,string>("CentroImpresion/EliminarSolicitud", _modelo);

            return Json(rta, JsonRequestBehavior.AllowGet);
        }
    }
}