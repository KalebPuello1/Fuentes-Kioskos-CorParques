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
    public class ReporteCortesiasController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            var modelo = new ReporteCortesiasFiltros();            

            return View(modelo);
        }

        [HttpGet]
        public async Task<string> ObtenerDatosReporte(ReporteCortesiasFiltros modelo)
        {
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();
            try
            {                
                modelo.FechaInicial = modelo.FechaInicial == null ? "null" : modelo.FechaInicial.ToString().Replace(@"/", "-");
                modelo.FechaFinal = modelo.FechaFinal == null ? "null" : modelo.FechaFinal.ToString().Replace(@"/", "-");
                modelo.Documento = modelo.Documento == null ? "null" : modelo.Documento;
                modelo.TarjetaFan = modelo.TarjetaFan == null ? "null" : modelo.TarjetaFan;
                var Cortesias =
                    await GetAsync<IEnumerable<ReporteCortesias>>($"ReporteCortesias/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.Documento}/{modelo.TarjetaFan}");

                if (Cortesias != null)
                {
                    if (Cortesias.Count() > 0)
                        strRetorno = objReportes.GenerarReporteCortesias(Cortesias);                    
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

        
    }
}