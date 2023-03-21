using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteVentasPorConvenioController : ControladorBase
    {
        #region Metodos

        public ActionResult Index()
        {
            var modelo = new ReporteVentasPorConvenioFiltros();
            return View(modelo);
        }

        [HttpGet]
        public async Task<string> ObtenerReporte(ReporteVentasPorConvenioFiltros modelo)
        {
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();
            try
            {
                var VentasDirectas = await GetAsync<IEnumerable<ReporteVentasPorConvenio>>($"ReporteVentasPorConvenio/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}");

                if (VentasDirectas != null)
                {
                    if (VentasDirectas.Count() > 0)
                        strRetorno = objReportes.GenerarReporteVentasPorConvenio(VentasDirectas);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }

            if (strRetorno.Contains("No hay datos"))
                strRetorno = "";
            return strRetorno;
        }


        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult;
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

        #endregion Metodos
    }
}