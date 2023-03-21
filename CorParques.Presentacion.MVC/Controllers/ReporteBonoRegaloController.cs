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
    public class ReporteBonoRegaloController : ControladorBase
    {
        // GET: ReporteBonoRegalo
        public async Task<ActionResult> Index()
        {
            var lis = new List<int>();
            IEnumerable<Materiales> Material = await GetAsync<IEnumerable<Materiales>>("ReporteInventarioGeneral/ObtenerMaterialesTodos");
            //var puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            var almacen = await GetAsync<IEnumerable<Almacen>>("Almacen/GetAll");
            var _parametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdTiposPuntoDescargue");
            foreach (var item in _parametro.Valor.Split(','))
                lis.Add(Convert.ToInt32(item.ToString().Trim()));

         

            ViewBag.Clientes = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos");
            IEnumerable<TipoGeneral> Vendedores = await GetAsync<IEnumerable<TipoGeneral>>("obtenerTodosVendedores/set");
            ViewBag.Vendedores = Vendedores;
         

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteBonoRegalo modelo)
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                if (modelo.IdTipoPedido == null)
                {
                    modelo.IdTipoPedido = 0;
                }
               
                IEnumerable<ReporteBonoRegalo> inv =
                //await GetAsync<IEnumerable<ReporteMovimientoInventario>>($"ReporteMovimientoInventario/ObtenerReporte/{modelo.FechaInicial ?? "null"}/{modelo.FechaFinal ?? "null"}/{modelo.CodigoMaterial ?? "null"}/{modelo.IdTipoMovimiento}/{modelo.IdPuntoOrigen}/{modelo.PuntoDestino}/{modelo.IdPersonaResponsable}/{modelo.UnidadMedida ?? "null"}");
                await GetAsync<IEnumerable<ReporteBonoRegalo>>($"ReporteBonoRegalo/ObtenerReporteBonoRegalo/{modelo.FechaInicialP ?? "null"}/{modelo.FechaFinalP ?? "null"}/{modelo.IdTipoPedido  }/{modelo.CodCliente ?? "null"}/{modelo.CodVendedor ?? "null"}/{modelo.CodPedido ?? "null"}");

                if (inv != null)
                {
                    if (inv.Count() > 0)
                    {
                        //strRetorno = objReportes.GenerarReporteBonoRegalo(inv);
                    }

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

        // GET: ReporteBonoRegalo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReporteBonoRegalo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReporteBonoRegalo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ReporteBonoRegalo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReporteBonoRegalo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ReporteBonoRegalo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReporteBonoRegalo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
