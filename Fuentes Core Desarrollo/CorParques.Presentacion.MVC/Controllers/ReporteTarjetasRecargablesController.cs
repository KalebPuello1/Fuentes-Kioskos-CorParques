//Cambioquitar: Este controlador usa el enumerador de perfiles.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteTarjetasRecargablesController : ControladorBase
    {
        
        public async Task<ActionResult> Index()
        {
            var _listClientes = await GetAsync<IEnumerable<TipoGeneral>>("ReporteTarjetaRecargable/ObtenerClientes");

            ViewBag.Clientes = _listClientes;
            ViewBag.Filtros = null;

            return View();
        }
        [HttpGet]
        public async Task<string> GenerarReporte(ReporteVentasFiltros modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                IEnumerable<ReporteTarjetaRecargable> orden =
                await GetAsync<IEnumerable<ReporteTarjetaRecargable>>($"ReporteTarjetaRecargable/ObtenerReporte/{modelo.FechaInicial.ToString("yyyy-MM-dd")}/{modelo.FechaFinal.ToString("yyyy-MM-dd")}/{(modelo.FechaCompra!=null? modelo.FechaCompra?.ToString("yyyy-MM-dd"):"null")}/{(modelo.FechaVencimiento != null ? modelo.FechaVencimiento?.ToString("yyyy-MM-dd") : "null")}/{modelo.idProducto}");

                if (orden != null)
                {
                    if (orden.Count() > 0)
                        strRetorno = objReportes.GenerarReporteTarjetaRecargable(orden);
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