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
    public class ReporteRecargasController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
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
                await GetAsync<IEnumerable<ReporteDonaciones>>($"ReporteTarjetasRecargabless/ObtenerRecargas/{modelo.FechaInicial.ToString("yyyy-MM-dd")}/{modelo.FechaFinal.ToString("yyyy-MM-dd")}");

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