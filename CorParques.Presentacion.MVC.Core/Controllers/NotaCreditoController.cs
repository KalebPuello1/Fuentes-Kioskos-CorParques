using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class NotaCreditoController : ControladorBase
    {
        // GET: NotaCredito
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtener detalle factura
        /// </summary>
        public async Task<ActionResult> DetalleFactura(string codigoFactura)
        {

            var _factura = await GetAsync<Factura>($"Pos/ObtenerFactura/{codigoFactura}");
            //var _listLineaProducto = new int[] { (int)Enumerador.LineaProducto.Propina };
            NotaCredito _model = new NotaCredito();
            ViewBag.Anulada = false;

            if (_factura != null)
            {
                ViewBag.Anulada = _factura.IdEstado != 1;
                _model = new NotaCredito
                {
                    DetalleFactura = _factura.DetalleFactura == null ?
                    new List<Negocio.Entidades.DetalleFactura>() : _factura.DetalleFactura.ToList()
                };

                //Obtener detalle factura

                Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapProdPropina");

                //_model.DetalleFactura = _model.DetalleFactura.Where(x => x.CodigoSap != Enumerador.CodigosSapProducto.Propina).ToList();
                _model.DetalleFactura = _model.DetalleFactura.Where(x => x.CodigoSap != objParametro.Valor).ToList();

                _model.Id = _factura.Id_Factura;
            }
            
            return PartialView("_NotaCredito", _model);
        }

        [HttpPost]
        public async Task<JsonResult> GuardarNotaCredito(NotaCredito modeloNotaCredito)
        {
            modeloNotaCredito.Fecha = DateTime.Now;
            modeloNotaCredito.DetalleFactura = modeloNotaCredito.DetalleFactura.Where(x => x.NotaCredito).ToList();
            modeloNotaCredito.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;            
            modeloNotaCredito.IdPunto = IdPunto;
            var rta = await PostAsync<NotaCredito, string>("Pos/InsertarNotaCredito", modeloNotaCredito);

            /*++Impresion++*/
            try
            {
                string idFactura = modeloNotaCredito.Id.ToString();
                FacturaImprimir resultadoFacturaImprimir = await GetAsync<FacturaImprimir>($"Pos/ObtenerFacturaNotaCredImprimir/{idFactura}/{(string)rta.Elemento}");
                resultadoFacturaImprimir.ConsecutivoNotaCredito = (string)rta.Elemento;

                ServicioImprimir objImprimir = new ServicioImprimir();
                var respImprimir = objImprimir.ImprimirTicketPosFacturaNotaCredito(resultadoFacturaImprimir);

                if (!string.IsNullOrEmpty(respImprimir))
                    Utilidades.RegistrarError(new Exception("Error al imprimir: " + respImprimir) { }, "Pos/PagarCompra-ImprimirTicketPosFactura");

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Pos/PagarCompra-Imprimir");
            }
            


            return Json(rta, JsonRequestBehavior.AllowGet);
        }

    }
}