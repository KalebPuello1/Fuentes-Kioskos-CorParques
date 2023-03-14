using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteReservaEspacioController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<TipoGeneral> tipoEspacio = await GetAsync<IEnumerable<TipoGeneral>>("ReservaEspacios/ObtenerTipoEspacios");
            IEnumerable<TipoGeneral> espacio = await GetAsync<IEnumerable<TipoGeneral>>("ReservaEspacios/obtenerEspacios");

            ViewBag.espacio = espacio;
            ViewBag.tipoEspacio = tipoEspacio;

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteReservaEspacio modelo)
        {
            string strRetorno = string.Empty;
            try
            {

                modelo.fechaInicialGet = modelo.fechaInicialGet == null ? "null" : modelo.fechaInicialGet;
                modelo.fechaFinalGet = modelo.fechaFinalGet == null ? "null" : modelo.fechaFinalGet;
                modelo.horaIniGet = modelo.horaIniGet == null ? "null" : modelo.horaIniGet;
                modelo.horaFinGet = modelo.horaFinGet == null ? "null" : modelo.horaFinGet;
                modelo.txtNPedidoGet = modelo.txtNPedidoGet == null ? "null" : modelo.txtNPedidoGet;

                IEnumerable<ReporteReservaEspacio> orden = await 
                    GetAsync<IEnumerable<ReporteReservaEspacio>>($"ReporteReservaEspacio/get/{modelo.fechaInicialGet}/{modelo.fechaFinalGet}/{modelo.horaIniGet}/{modelo.horaFinGet}/{modelo.idTipEpsGet}/{modelo.idEspGet}/{modelo.txtNPedidoGet}");

                if (orden != null)
                {
                    if (orden.Count() > 0)
                        strRetorno = new Transversales.Util.Reportes().GenerarReporteReservaEspacio(orden);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }
            return strRetorno;
        }

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try
            {
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception)
            {
                return null;
            }
            return objFileContentResult;
        }
    }
}