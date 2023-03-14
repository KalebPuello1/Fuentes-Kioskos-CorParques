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
    public class ReporteNotificacionesController : ControladorBase
    {

        #region Metodos

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// /// RDSH: retorna la ruta de generacion del reporte de notificaciones.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteNotificacionesFiltros modelo)
        {
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();

            modelo.FechaInicial = modelo.FechaInicial == null ? "null" : modelo.FechaInicial.ToString().Replace(@"/", "");
            modelo.FechaFinal = modelo.FechaFinal == null ? "null" : modelo.FechaFinal.ToString().Replace(@"/", "");

            try
            {
                var objReporteNotificaciones =
                    await GetAsync<IEnumerable<ReporteNotificaciones>>($"ReporteNotificaciones/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}");

                if (objReporteNotificaciones != null)
                {
                    if (objReporteNotificaciones.Count() > 0)
                        strRetorno = objReportes.GenerarReporteNotificaciones(objReporteNotificaciones);
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReporteNotificacionesController_GenerarArchivo");
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }
            return strRetorno;
        }

        /// <summary>
        /// RDSH: Descarga el archivo de reporte de notificaciones.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try
            {
                objFileContentResult = File(Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception)
            {
                return null;
            }
            return objFileContentResult;
        }

        /// <summary>
        /// RDSH: Presenta los datos en la grilla para una vista previa.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> VistaPrevia(ReporteNotificacionesFiltros modelo)
        {
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();

            modelo.FechaInicial = modelo.FechaInicial == null ? "null" : modelo.FechaInicial.ToString().Replace(@"/", "");
            modelo.FechaFinal = modelo.FechaFinal == null ? "null" : modelo.FechaFinal.ToString().Replace(@"/", "");           

            try
            {
                var objReporteNotificaciones =
                    await GetAsync<IEnumerable<ReporteNotificaciones>>($"ReporteNotificaciones/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}");

                if (objReporteNotificaciones.Count() > 0)
                    return PartialView("_Grid", objReporteNotificaciones);
                else
                    return null;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReporteNotificacionesController_VistaPrevia");
                return null;
            }            
        }

        #endregion

    }
}