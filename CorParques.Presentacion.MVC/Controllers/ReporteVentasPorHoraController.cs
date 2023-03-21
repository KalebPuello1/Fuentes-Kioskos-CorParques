using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System.IO;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteVentasPorHoraController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            IEnumerable<TipoGeneral> Productos = await GetAsync<IEnumerable<TipoGeneral>>("ReporteCostoProductoController/getProd");
            IEnumerable<TipoGeneral> CentroBeneficio = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
            ViewBag.Puntos = _listPuntos;
            ViewBag.Productos = Productos;
            ViewBag.centroB = CentroBeneficio;
            return View();
        }


        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteVentasPorHoraFiltros modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();

            modelo.FechaInicial = modelo.FechaInicial == null ? "null" : modelo.FechaInicial.ToString().Replace(@"/", "");
            modelo.FechaFinal = modelo.FechaFinal == null ? "null" : modelo.FechaFinal.ToString().Replace(@"/", "");
            modelo.HoraInicial = modelo.HoraInicial == null ? "null" : modelo.HoraInicial;
            modelo.HoraFinal = modelo.HoraFinal == null ? "null" : modelo.HoraFinal; 
            modelo.CodigoProducto = modelo.CodigoProducto == null ? "null" : modelo.CodigoProducto;
            modelo.NombreProducto = modelo.NombreProducto == null ? "null" : modelo.NombreProducto;
            modelo.CodigoPunto = modelo.CodigoPunto == null ? "null" : modelo.CodigoPunto;
            modelo.CB = modelo.CB == null ? "null" : modelo.CB;

            try
            {
                var objReporteVentasPorHora = 
                    await GetAsync<IEnumerable<ReporteVentasPorHora>>($"ReporteVentasPorHora/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.HoraInicial}/{modelo.HoraFinal}/{modelo.CodigoProducto}/{modelo.NombreProducto}/{modelo.CodigoPunto}/{modelo.CB}");

                if (objReporteVentasPorHora != null)
                {
                    if (objReporteVentasPorHora.Count() > 0)
                        strRetorno = objReportes.GenerarReporteVentasPorHora(objReporteVentasPorHora);
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

        #region Metodos

        /// <summary>
        /// RDSH: Presenta los datos en la grilla para una vista previa.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> VistaPrevia(ReporteVentasPorHoraFiltros modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();

            modelo.FechaInicial = modelo.FechaInicial == null ? "null" : modelo.FechaInicial.ToString().Replace(@"/", "");
            modelo.FechaFinal = modelo.FechaFinal == null ? "null" : modelo.FechaFinal.ToString().Replace(@"/", "");
            modelo.HoraInicial = modelo.HoraInicial == null ? "null" : modelo.HoraInicial;
            modelo.HoraFinal = modelo.HoraFinal == null ? "null" : modelo.HoraFinal;
            modelo.CodigoProducto = modelo.CodigoProducto == null ? "null" : modelo.CodigoProducto;
            modelo.NombreProducto = modelo.NombreProducto == null ? "null" : modelo.NombreProducto;
            modelo.CodigoPunto = modelo.CodigoPunto == null ? "null" : modelo.CodigoPunto;
            modelo.CB = modelo.CB == null ? "null" : modelo.CB;

            try
            {
                var objReporteVentasPorHora =
                    await GetAsync<IEnumerable<ReporteVentasPorHora>>($"ReporteVentasPorHora/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.HoraInicial}/{modelo.HoraFinal}/{modelo.CodigoProducto}/{modelo.NombreProducto}/{modelo.CodigoPunto}/{modelo.CB}");

                if (objReporteVentasPorHora.Count() > 0)
                    return PartialView("_Grid", objReporteVentasPorHora);
                else
                    return null;
            }
            catch (Exception ex)
            {
                Transversales.Util.Utilidades.RegistrarError(ex, "ReporteVentasPorHoraController_VistaPrevia");
                return null;
            }            
        }

        #endregion
    }
}