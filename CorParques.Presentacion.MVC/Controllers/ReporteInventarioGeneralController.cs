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
    public class ReporteInventarioGeneralController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            //IEnumerable<Puntos> Punto = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            IEnumerable<Almacen> almacen = await GetAsync<IEnumerable<Almacen>>("Almacen/GetAll");
            ViewBag.Punto = almacen;
            IEnumerable<Materiales> Material = await GetAsync<IEnumerable<Materiales>>("ReporteInventarioGeneral/ObtenerMaterialesTodos");
            ViewBag.Material = Material;
            IEnumerable<TipoGeneral> CentroBeneficio = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
            ViewBag.centroB = CentroBeneficio;
            return View();
        }
        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteInventarioGeneral modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try{
                IEnumerable<ReporteInventarioGeneral> inv =
                await GetAsync<IEnumerable<ReporteInventarioGeneral>>($"ReporteInventarioGeneral/set/{modelo.fechaInicial?? "null"}/{modelo.fechaFinal?? "null"}/{modelo.Almacen}/{modelo.idMaterial}/{modelo.CB}");

                if (inv != null){
                    if (inv.Count() > 0)
                        strRetorno = objReportes.GenerarReporteInventarioGeneral(inv);
                }
            }
            catch (Exception ex){
                
                if (ex.Message == "Se canceló una tarea.")
                {
                    strRetorno = string.Concat("Error generando reporte: ", ex.Message, " porque hay muchos datos para exportar - Elegir fechas mas cortas - MAX 4 meses");
                }
                else
                {
                    strRetorno = string.Concat("Error generando reporte: ", ex.Message);
                }
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
    }
}