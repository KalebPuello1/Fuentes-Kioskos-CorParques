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
    public class ReporteRecaudosVentasController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<MediosPago> MediosDePagos = await GetAsync<IEnumerable<MediosPago>>("MediosPago/GetAll");
            ViewBag.MediosDePagos = MediosDePagos;

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteRecaudosVentas modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                IEnumerable<ReporteRecaudosVentas> recaudo =
                await GetAsync<IEnumerable<ReporteRecaudosVentas>>($"ReporteRecaudosVentas/set/{modelo._FechaInicial}/{modelo._FechaFinal}/{modelo._Consecutivo}/{modelo._Cliente}/{modelo._FormaPago}/{modelo._Entidad}");

                if (recaudo != null)
                {
                    if (recaudo.Count() > 0)
                        strRetorno = objReportes.GenerarReporteRecaudosVentas(recaudo);
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