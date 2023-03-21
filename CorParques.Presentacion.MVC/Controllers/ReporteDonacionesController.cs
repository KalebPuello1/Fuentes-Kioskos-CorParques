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
    public class ReporteDonacionesController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {

            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            var _listProductos = await GetAsync<IEnumerable<TipoGeneral>>("ReporteDonaciones/ObtenerProductos");

            ViewBag.Puntos = _listPuntos;
            ViewBag.Productos = _listProductos;
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
                IEnumerable<ReporteDonaciones> orden =
                await GetAsync<IEnumerable<ReporteDonaciones>>($"ReporteDonaciones/ObtenerReporte/{modelo.FechaInicial.ToString("yyyy-MM-dd")}/{modelo.FechaFinal.ToString("yyyy-MM-dd")}/{(modelo.idProducto == null ?"null":modelo.idProducto )}/{(modelo.IdPunto == null ? "null" : modelo.IdPunto.ToString())}");

                if (orden != null)
                {
                    if (orden.Count() > 0)
                        strRetorno = objReportes.GenerarReporteDonaciones(orden);
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