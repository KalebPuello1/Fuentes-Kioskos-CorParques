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
    public class ReporteMovimientoInventarioController : ControladorBase
    {
        // GET: ReporteMovimientoInventario
        public async Task<ActionResult> Index()
        {
            var lis = new List<int>();
            IEnumerable<Materiales> Material = await GetAsync<IEnumerable<Materiales>>("ReporteInventarioGeneral/ObtenerMaterialesTodos");
            //var puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            var almacen = await GetAsync<IEnumerable<Almacen>>("Almacen/GetAll");
            var _parametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdTiposPuntoDescargue");
            foreach (var item in _parametro.Valor.Split(','))
                lis.Add(Convert.ToInt32(item.ToString().Trim()));

            var usuarios = await GetAsync<IEnumerable<Usuario>>("Usuario/GetAll");

            var ListaUnidadMedida = await GetAsync<string[]>("ReporteMovimientoInventario/ObtenerUnidadMedida");
            var ListaTipoMovimiento = await GetAsync<IEnumerable<TipoMovimiento>>("ReporteMovimientoInventario/ObtenerTipoMovimiento");

            ViewBag.Material = Material;
            //ViewBag.puntos = puntos.Where(x => lis.Contains(x.IdTipoPunto)).ToList();
            ViewBag.puntos = almacen;
            ViewBag.Usuario = usuarios;
            ViewBag.UnidadMedida = ListaUnidadMedida.ToList();
            ViewBag.TiposMovimiento = ListaTipoMovimiento;

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteMovimientoInventario modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                var datoMOrigen = modelo.CodSapAlmacenOrigen == null ? "null" : modelo.CodSapAlmacenOrigen;
                var datoMDestino = modelo.CodSapAlmacenDestino == null ? "null" : modelo.CodSapAlmacenDestino;
                IEnumerable<ReporteMovimientoInventario> inv =
                //await GetAsync<IEnumerable<ReporteMovimientoInventario>>($"ReporteMovimientoInventario/ObtenerReporte/{modelo.FechaInicial ?? "null"}/{modelo.FechaFinal ?? "null"}/{modelo.CodigoMaterial ?? "null"}/{modelo.IdTipoMovimiento}/{modelo.IdPuntoOrigen}/{modelo.PuntoDestino}/{modelo.IdPersonaResponsable}/{modelo.UnidadMedida ?? "null"}");
                await GetAsync<IEnumerable<ReporteMovimientoInventario>>($"ReporteMovimientoInventario/ObtenerReporte/{modelo.FechaInicial ?? "null"}/{modelo.FechaFinal ?? "null"}/{modelo.CodigoMaterial ?? "null"}/{modelo.IdTipoMovimiento}/{datoMOrigen}/{datoMDestino}/{modelo.IdPersonaResponsable}/{modelo.UnidadMedida ?? "null"}");

                if (inv != null)
                {
                    if (inv.Count() > 0)
                        strRetorno = objReportes.GenerarReporteMovimientoInventario(inv);
                }
            }
            catch (Exception ex)
            {
                
                if (ex.Message.Contains("Se produjo una excepción de tipo 'System.OutOfMemoryException'."))
                {
                    strRetorno = string.Concat("Error generando reporte: ", "Muchos datos para exportar - MAX 4 - 12 dias para consultar ");
                }
                else
                {
                    strRetorno = string.Concat("Error generando reporte: ", ex.Message);
                }
            }
            //"Error en GenerarReporte: Se produjo una excepción de tipo 'System.OutOfMemoryException'. MovimientoInventario_01_02_2022_173336.xlsx"
            if (strRetorno.Contains("Error en GenerarReporte: Se produjo una excepción de tipo 'System.OutOfMemoryException'."))
            {
                strRetorno = string.Concat("Error generando reporte: ", "Muchos datos para exportar - MAX 4 - 12 dias para consultar ");
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