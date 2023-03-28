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
    public class ReporteFANVendidasController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            var modelo = new ReporteVentasPorProductoFiltros();
            //var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            //IEnumerable<TipoGeneral> Productos = await GetAsync<IEnumerable<TipoGeneral>>("ReporteCostoProductoController/getProd");
            //IEnumerable<TipoGeneral> CentroBeneficio = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");

            //ViewBag.Puntos = _listPuntos;
            //ViewBag.Productos = Productos;
            //ViewBag.CentroBeneficio = CentroBeneficio;
            //ViewBag.Filtros = null;

            return View(modelo);
        }

        [HttpGet]
        public async Task<string> ObtenerDatosReporte(ReporteVentasPorProductoFiltros modelo)
        {
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();
            try
            {                
                modelo.FechaInicial = modelo.FechaInicial == null ? "null" : modelo.FechaInicial.ToString().Replace(@"/", "-");
                var VentasFan =                    
                    await GetAsync<IEnumerable<ReporteFANVendidas>>($"ReporteFANVendidas/ObtenerReporte/{modelo.FechaInicial}"); 

                if (VentasFan != null)
                {
                    if (VentasFan.Count() > 0)
                        strRetorno = objReportes.GenerarReporteFanVendidas(VentasFan);                    
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