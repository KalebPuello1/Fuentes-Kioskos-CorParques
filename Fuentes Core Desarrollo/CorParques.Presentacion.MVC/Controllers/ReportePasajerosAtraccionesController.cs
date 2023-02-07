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
    public class ReportePasajerosAtraccionesController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/GetAllxTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            var _listTipoProd = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
            ViewBag.Puntos = _listPuntos;
            ViewBag.TipoProducto = _listTipoProd;

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(ReportePasajerosFiltros modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                var objReportePasajeros = 
                    await GetAsync<IEnumerable<ReportePasajeros>>($"ReportePasajerosAtracciones/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.IdPunto}/{modelo.TipoProducto}");

                if (objReportePasajeros != null)
                {
                    if (objReportePasajeros.Count() > 0)
                        strRetorno = objReportes.GenerarReportePasajerosAtracciones(objReportePasajeros);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }
            return strRetorno;
        }

        public async Task<ActionResult> VistaPrevia(ReportePasajerosFiltros modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                var objReportePasajeros =
                    await GetAsync<IEnumerable<ReportePasajeros>>($"ReportePasajerosAtracciones/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.IdPunto}/{modelo.TipoProducto}");

                if (objReportePasajeros.Count() > 0)
                    return PartialView("_Grid", objReportePasajeros);
                else
                    return null;
            }
            catch (Exception ex)
            {
                Transversales.Util.Utilidades.RegistrarError(ex, "ReportePasajerosAtraccionesController_VistaPrevia");
                return null;
            }
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