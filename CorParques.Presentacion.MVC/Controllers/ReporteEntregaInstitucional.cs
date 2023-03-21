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
    public class ReporteEntregaInstitucionalController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            IEnumerable<TipoGeneral> Clientes = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos");
            IEnumerable<TipoGeneral> Asesores = await GetAsync<IEnumerable<TipoGeneral>>("obtenerTodosVendedores/set");
            IEnumerable<Producto> Productos = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos");
            ViewBag.Clientes = Clientes;
            ViewBag.Asesores = Asesores;
            ViewBag.Productos = Productos;
            return View();
        }
        [HttpGet]
        public async Task<string> GenerarArchivo(string fechaEntrega, string fechaUso, string pedido, string asesor, string cliente, string producto)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try{
                IEnumerable<ReporteEntregaPedido> inv =
                await GetAsync<IEnumerable<ReporteEntregaPedido>>($"ReporteInventarioGeneral/ObtenerEntregasInstitucionales/{fechaEntrega?? "null"}/{fechaUso?? "null"}/{pedido ?? "null"}/{asesor ?? "null"}/{cliente ?? "null"}/{ producto ?? "null"}");

                if (inv != null){
                    if (inv.Count() > 0)
                        strRetorno = objReportes.GenerarReporteEntregaPedidos(inv);
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
    }
}