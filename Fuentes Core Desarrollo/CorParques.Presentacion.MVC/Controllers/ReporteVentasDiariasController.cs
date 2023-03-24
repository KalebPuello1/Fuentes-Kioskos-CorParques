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
    public class ReporteVentasDiariasController : ControladorBase
    {

        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public async Task<string> GenerarArchivoInventario()
        {
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();
            try
            {
                var InventarioEquipos =
                        await GetAsync<IEnumerable<ReporteInventario>>($"ReporteVentas/ObtenerReporteInventario/");

                 if (InventarioEquipos != null)
                {
                    if (InventarioEquipos.Count() > 0)
                        strRetorno = objReportes.GenerarReporteInventarioEquipos(InventarioEquipos);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }

            if (strRetorno.Contains("No hay datos"))
                strRetorno = "";
            return strRetorno;
        }

        [HttpGet]
        public async Task<string> GenerarArchivoVentasDiarias(ReporteVentasFiltros modelo)
        {
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();
            try
            {
                var ReporteDiario =
                        await GetAsync<IEnumerable<ReporteVentasDiario>>($"ReporteVentas/ObtenerReporteDiario/{modelo.FechaInicial.ToString("yyyy-MM-dd")}");

                if (ReporteDiario != null)
                {
                    if (ReporteDiario.Count() > 3)
                        strRetorno = objReportes.GenerarReporteVentasDiarias(ReporteDiario);  //ojo cambiar reporte diarios
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }

            if (strRetorno.Contains("No hay datos"))
                strRetorno = "";
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