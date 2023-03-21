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
    public class ReporteUsuarioAppController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            var _listClientes = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos");

            ViewBag.Clientes = _listClientes;
            ViewBag.Filtros = null;
            Reportes R = new Reportes();

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarReporte(string Correoelectronico)
        {
            Correoelectronico = Correoelectronico.Replace(".", "|");
            var RR = await GetAsync<IEnumerable<ReporteUsuarioApp>[]>("ReporteReporteUsuarioApp/ObtenerReporte/" + Correoelectronico);
            Reportes R = new Reportes();
            string da = R.GenerarReporteUsuarioApp(RR);
            return da;
        }
        /* [HttpGet]
         public async Task<string> GenerarReporte(ReporteUsuarioApp modelo)
         {
             string strRetorno = string.Empty;
             Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
             try
             {
                 IEnumerable<ReporteUsuarioApp> orden =
                 await GetAsync<IEnumerable<ReporteUsuarioApp>>($"ReporteUsuarioApp/ObtenerReporte/{modelo.Correoelectronicousuario");

                 if (ReporteUsuarioApp != null)
                 {
                     if (orden.Count() > 0)
                         strRetorno = objReportes.GenerarReporteUsuarioApp(ReporteUsuarioApp);
                 }
             }
             catch (Exception ex)
             {
                 strRetorno = string.Concat("Error generando reporte: ", ex.Message);
             }
             return strRetorno;

         } */

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