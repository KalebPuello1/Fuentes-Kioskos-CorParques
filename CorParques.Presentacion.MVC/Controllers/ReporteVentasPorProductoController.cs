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
    public class ReporteVentasPorProductoController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            var modelo = new ReporteVentasPorProductoFiltros();
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            IEnumerable<TipoGeneral> Productos = await GetAsync<IEnumerable<TipoGeneral>>("ReporteCostoProductoController/getProd");
            IEnumerable<TipoGeneral> CentroBeneficio = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");

            ViewBag.Puntos = _listPuntos;
            ViewBag.Productos = Productos;
            ViewBag.CentroBeneficio = CentroBeneficio;
            ViewBag.Filtros = null;

            return View(modelo);
        }

        [HttpGet]
        public async Task<string> ObtenerDatosReporte(ReporteVentasPorProductoFiltros modelo)
        {
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            IEnumerable<TipoGeneral> Productos = await GetAsync<IEnumerable<TipoGeneral>>("ReporteCostoProductoController/getProd");
            IEnumerable<TipoGeneral> CentroBeneficio = await GetAsync<IEnumerable<TipoGeneral>>("TipoPunto/ObtenerListaSimple");
            ViewBag.Puntos = _listPuntos;
            ViewBag.Productos = Productos;
            ViewBag.CentroBeneficio = CentroBeneficio;
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();

            modelo.FechaInicial = modelo.FechaInicial == null ? "null" : modelo.FechaInicial.ToString().Replace(@"/", "");
            modelo.FechaFinal = modelo.FechaFinal == null ? "null" : modelo.FechaFinal.ToString().Replace(@"/", "");
            modelo.CodigoProducto = modelo.CodigoProducto == null ? "null" : modelo.CodigoProducto;
            modelo.CodigoPunto = modelo.CodigoPunto == null ? "null" : modelo.CodigoPunto;
            modelo.CentroBeneficio = modelo.CentroBeneficio == null ? "null" : modelo.CentroBeneficio;
            try
            {
                var objReporteVentasPorProducto = await GetAsync<IEnumerable<ReporteVentasPorProducto>>($"ReporteVentasPorProducto/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.CodigoProducto}/{modelo.CodigoPunto}/{modelo.CentroBeneficio}");

                if (objReporteVentasPorProducto != null)
                {
                    if (objReporteVentasPorProducto.Count() > 0)
                        strRetorno = objReportes.GenerarReporteVentasPorProducto(objReporteVentasPorProducto);
                }
                
                //modelo.objReporteVentasPorProducto = objReporteVentasPorProducto;
                //var objReporte = new List<ReporteVentasPorProducto>();

                //if (objReporteVentasPorProducto != null)
                //{
                //    foreach (var item in objReporteVentasPorProducto[0])
                //    {
                //        var reporte = new ReporteVentasPorProducto();
                //        reporte.IdPunto = item.IdPunto;
                //        reporte.Punto = item.Punto;
                //        reporte.CodigoProducto = item.CodigoProducto;
                //        reporte.NombreProducto = item.NombreProducto;
                //        reporte.Cantidad = item.Cantidad;
                //        reporte.ValorSinImpuesto = item.ValorSinImpuesto;
                //        reporte.ValorConImpuesto = item.ValorConImpuesto;
                //        objReporte.Add(reporte);
                //    }
                //    modelo.objReporteVentasPorProducto = objReporte;
                //}
                //else
                //{
                //    modelo = null;
                //}

                //return await Index(modelo);
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }
            return strRetorno;
        }

        [HttpPost]
        public async Task<ActionResult> Index(ReporteVentasPorProductoFiltros modelo)
        {
            ViewBag.Filtros = modelo.FechaInicial + "|" + modelo.FechaFinal + "|" + modelo.CodigoProducto + "|" + modelo.CodigoPunto + "|" + modelo.CentroBeneficio;
            return View("Index", modelo);
        }

        public async Task<string> ExportarExcel(string Filtros)
        {
            ReporteVentasPorProductoFiltros modelo = new ReporteVentasPorProductoFiltros();
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();
            try
            {
                var filtros = Filtros.Split('|');
                if (filtros.Count() > 0)
                {
                    modelo.FechaInicial = filtros[0];
                    modelo.FechaFinal = filtros[1];
                    modelo.CodigoProducto = filtros[2];
                    modelo.CodigoPunto = filtros[3];
                    modelo.CentroBeneficio = filtros[4];
                }

                var objReporteVentasPorProducto = await GetAsync<IEnumerable<ReporteVentasPorProducto>>($"ReporteVentasPorProducto/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.CodigoProducto}/{modelo.CodigoPunto}/{modelo.CentroBeneficio}");

                strRetorno = objReportes.GenerarReporteVentasPorProducto(objReporteVentasPorProducto);
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte pasajeros atracciones: ", ex.Message);
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