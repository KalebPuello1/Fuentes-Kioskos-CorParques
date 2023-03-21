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
    public class ReporteCostoProductoController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<TipoGeneral> Productos = await GetAsync<IEnumerable<TipoGeneral>>("ReporteCostoProductoController/getProd");
            ViewBag.Productos = Productos;
            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteCostoProducto modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                IEnumerable<ReporteCostoProducto> orden =
                await GetAsync<IEnumerable<ReporteCostoProducto>>($"ReporteCostoProductoController/getRep/{modelo.fechaInicial}/{modelo.fechaFinal}/{modelo.CodSap}");

                if (orden != null)
                {
                    if (orden.Count() > 0)
                        strRetorno = objReportes.GenerarReporteCostoProducto(orden);
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