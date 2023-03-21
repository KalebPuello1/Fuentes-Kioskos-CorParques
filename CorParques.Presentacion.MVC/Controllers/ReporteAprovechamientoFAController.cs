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
    public class ReporteAprovechamientoFAController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            var _listClientes = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos");

            ViewBag.Clientes = _listClientes;
            ViewBag.Filtros = null;

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarReporte(ReporteAprovechamientoFA modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                IEnumerable<ReporteAprovechamientoFA> orden =
                await GetAsync<IEnumerable<ReporteAprovechamientoFA>>($"ReporteAprovechamientoFA/ObtenerReporte/{modelo.fechaInicial}/{modelo.fechaFinal}/{modelo.cliente}/{modelo.pedido}/{modelo.factura}");

                if (orden != null)
                {
                    if (orden.Count() > 0)
                        strRetorno = objReportes.GenerarReporteArovechamientoFA(orden);
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