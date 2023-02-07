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
    public class ReporteDestrezasController : ControladorBase
    {
        // GET: ReporteDestrezas
        public async Task<ActionResult> Index()
        {
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllxTipoPunto/5");
            IEnumerable<TipoGeneral> objTipoBoleta = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
            IEnumerable<TipoGeneral> objClientes = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos");
            
            ViewBag.Puntos = _listPuntos;
            ViewBag.TipoBoleta = objTipoBoleta.Where(x=>x.CodSAP=="2015" || x.CodSAP=="2020");
            ViewBag.Clientes = objClientes;          

            return View();
        }

        public async Task<string> GenerarArchivo(ReporteDestrezaFiltros modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();

            modelo.FechaInicial = modelo.FechaInicial == null ? "null" : modelo.FechaInicial.ToString().Replace(@"/", "-");
            modelo.FechaFinal = modelo.FechaFinal == null ? "null" : modelo.FechaFinal.ToString().Replace(@"/", "-");
            modelo.CodigoPunto = modelo.CodigoPunto == null ? "null" : modelo.CodigoPunto;
            //modelo.CodigoSeries = modelo.CodigoSeries == null ? "null" : modelo.CodigoSeries;
            modelo.NombreTipoBoleta = modelo.NombreTipoBoleta == null ? "null" : modelo.NombreTipoBoleta;
            modelo.NombreCliente = modelo.NombreCliente == null ? "null" : modelo.NombreCliente;
            modelo.TipoVenta = modelo.TipoVenta== null ? "null" : modelo.TipoVenta;


            try
            {
                var objReportDestrezas = await GetAsync<IEnumerable<ReporteDestrezas>>($"ReporteDestreza/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.CodigoPunto}/{modelo.NombreTipoBoleta}/{modelo.NombreCliente}/{modelo.TipoVenta}");

                if (objReportDestrezas != null)
                {
                    if (objReportDestrezas.Count() > 0)
                        strRetorno = objReportes.GenerarReporteDestreza(objReportDestrezas);
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