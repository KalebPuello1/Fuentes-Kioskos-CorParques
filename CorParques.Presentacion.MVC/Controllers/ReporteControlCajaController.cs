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
    public class ReporteControlCajaController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<TipoGeneral> Taquilleros = await GetAsync<IEnumerable<TipoGeneral>>($"Usuario/GetAllSimple/taquillero");
           IEnumerable<Perfil> Perfiles = await GetAsync<IEnumerable<Perfil>>("Perfil/ObtenerLista");
           
           ViewBag.Taquilleros = Taquilleros;
           ViewBag.Perfiles = Perfiles;
           
            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteControlCaja modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                var controlCaja =
                        await GetAsync<IEnumerable<ReporteControlCaja>>($"ReporteControlCaja/get/{modelo.fechaInicial.ToString("yyyy-MM-dd")}/{modelo.fechaFinal.ToString("yyyy-MM-dd")}/{modelo.idPerfil}/{modelo.idTaquillero}");

                if (controlCaja != null)
                {
                    if (controlCaja.Count() > 0)
                        strRetorno = objReportes.GenerarReporteControlCaja(controlCaja);
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