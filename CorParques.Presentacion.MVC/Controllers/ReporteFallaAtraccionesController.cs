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
    public class ReporteFallaAtraccionesController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<Puntos> ListaPuntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            IEnumerable<TipoGeneral> ListaAreas = await GetAsync<IEnumerable<TipoGeneral>>("Area/ObtenerListaAreas");

            ViewBag.ListaPuntos = ListaPuntos;
            ViewBag.ListaAreas = ListaAreas;

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteFallaAtraccion modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                IEnumerable<ReporteFallaAtraccion> orden =
                await GetAsync<IEnumerable<ReporteFallaAtraccion>>($"ReporteFallaAtraccion/set/{modelo.fechaInicial}/{modelo.fechaFinal}/{modelo.idAtraccion}/{modelo.idArea}");

                if (orden != null)
                {
                    if (orden.Count() > 0)
                        strRetorno = objReportes.GenerarReporteFallasAtracciones(orden);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }
            return strRetorno;
        }

        [HttpGet]
        public async Task<ActionResult> VistaPrevia(ReporteFallaAtraccion modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                IEnumerable<ReporteFallaAtraccion> orden =
                await GetAsync<IEnumerable<ReporteFallaAtraccion>>($"ReporteFallaAtraccion/set/{modelo.fechaInicial}/{modelo.fechaFinal}/{modelo.idAtraccion}/{modelo.idArea}");

                if (orden != null)
                {
                    if (orden.Count() > 0)
                        return PartialView("_Grid", orden);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
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