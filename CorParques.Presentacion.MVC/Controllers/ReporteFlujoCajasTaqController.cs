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
    public class ReporteFlujoCajasTaqController : ControladorBase
    {
        [HttpGet]   
        public async Task<ActionResult> Index()
        {
            IEnumerable<TipoGeneral> ListaMediosPagos = await GetAsync<IEnumerable<TipoGeneral>>("MediosPago/GetAllSimple");

            ViewBag.ListaMediosPagos = ListaMediosPagos;

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteCuadreDiarioFlujoCajasTaq modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try{
                IEnumerable<ReporteCuadreDiarioFlujoCajasTaq> orden =
                await GetAsync<IEnumerable<ReporteCuadreDiarioFlujoCajasTaq>>($"ReporteCuadreDiarioFlujoCajasTaq/get/{modelo.fechaInicial}/{modelo.fechaFinal}/{modelo.idTipIngreso}/null/null");

                if (orden != null){
                    if (orden.Count() > 0)
                        strRetorno = objReportes.GenerarReporteCuadreDiarioFlujoCajasTaq(orden);
                }
            }
            catch (Exception ex){
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }
            return strRetorno;
        }

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try{
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception){
                return null;
            }
            return objFileContentResult;
        }

        [HttpGet]
        public async Task<ActionResult> VistaPrevia(ReporteCuadreDiarioFlujoCajasTaq modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                IEnumerable<ReporteCuadreDiarioFlujoCajasTaq> orden =
                await GetAsync<IEnumerable<ReporteCuadreDiarioFlujoCajasTaq>>($"ReporteCuadreDiarioFlujoCajasTaq/get/{modelo.fechaInicial}/{modelo.fechaFinal}/{modelo.idTipIngreso}/null/null");

                if (orden != null)
                {
                    if (orden.Count() > 0)
                        return PartialView("_Grid", orden);
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Transversales.Util.Utilidades.RegistrarError(ex, "ReporteCuadreDiarioFlujoCajasTaq_VistaPrevia");
                return null;
            }
            
        }

    }
}