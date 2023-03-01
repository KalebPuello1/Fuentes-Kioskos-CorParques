using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class ReimpresionReciboCajaVController : ControladorBase
    {
        // GET: ReimpresionReciboCajaV
        public  ActionResult Index()
        {
            var R = "DATO";

            R = "";

            return View();
        }

        // GET: ReimpresionReciboCajaV/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(string datoI, string datoF)
        {
            var dato = await GetAsync<IEnumerable<ReimpresionReciboCajaV>>($"ReimpresionReciboCajaV/datosReimpresion/{datoI}/{datoF}");
            var filtrado = new List<ReimpresionReciboCajaV>();

            foreach (var item in dato)
            {
                if (!string.IsNullOrEmpty(item.CodigoFactura) && !item.CodigoFactura.Contains("C")) 
                {
                    filtrado.Add(item);
                }
            } 

            var fff = " ffffff ";

            return PartialView("View", filtrado);
        }

        [HttpGet]
        public async Task<string> Reimprimir(string numPedido)
        {
            int exist = 0;
            string DATO = "";
            #region Reimpresion Vyucoom
            TicketFactura ImprimirFactura = new TicketFactura();
            FacturaImprimir ConfiguracionFactura = new FacturaImprimir();

            IEnumerable<Vyucoom>[] test = await GetAsync<IEnumerable<Vyucoom>[]>($"vuycoom/Reimprimir/{numPedido}");

            IEnumerable<Vyucoom> testt = test[0];
            IEnumerable<Vyucoom> testtt = test[1];
            IEnumerable<Vyucoom> testtPrecio = test[2];

            Vyucoom usuario;
            List<string[]> MetodoPago = new List<string[]>();
            List<string[]> ListaProductos = new List<string[]>();
            string[] ConfMedioPago = new string[2];
            var f = "";
            var cambio = "";
            var TotCambio = "";
            var nImpresionProducto = 1;
            var precioDetalle = "";
            var IdUsuarioVenta = 0;
            var idPunto = 0;
            DateTime FechaHora = DateTime.Now;
            if (testt.Count() == 0)
            {
                foreach (var item in testtPrecio)
                {
                    precioDetalle = item.Precio.ToString();
                }

                exist = 1;
                foreach (var ff in testtt)
                {
                    IdUsuarioVenta = ff.IdUsuarioCreacion;
                    idPunto = ff.IdPunto;
                    f = ff.Id_Factura;
                    FechaHora = ff.FechaCreacion;
                    var desPago = ff.IdMedioPago == "1" ? "Efectivo" : "Targeta";
                    //Configuracion medio pago seting factura 

                    if (nImpresionProducto == 1)
                    {
                        ConfMedioPago = new string[2];
                        ConfMedioPago[0] = "TOTAL A ABONAR";
                        //ConfMedioPago[1] = ff.Precio.ToString();
                        ConfMedioPago[1] = precioDetalle;
                        MetodoPago.Add(ConfMedioPago);
                    }

                    ConfMedioPago = new string[2];
                    ConfMedioPago[0] = "ABONO " + desPago;
                    ConfMedioPago[1] = ff.Valor.ToString();
                    MetodoPago.Add(ConfMedioPago);





                    //Config Productos
                    if (nImpresionProducto == 1)
                    {

                        cambio = ff.Cambio >= 0 ? "Cambio " : "Abono faltante ";
                        TotCambio = ff.Cambio.ToString();


                        string[] ConfListaProductos = new string[3];
                        ConfListaProductos[0] = "1";
                        ConfListaProductos[1] = "Fiesta";
                        //ConfListaProductos[2] = ff.Precio.ToString();
                        ConfListaProductos[2] = precioDetalle;
                        ListaProductos.Add(ConfListaProductos);
                        nImpresionProducto = 0;
                    }

                }


                //Config final impresion 
                usuario = await GetAsync<Vyucoom>($"vuycoom/UsuarioReimprimir/{f}");
                if (usuario != null)
                {
                    ConfiguracionFactura.NombreCliente = usuario.Nombres;
                    ConfiguracionFactura.IdentificacionCliente = usuario.Documento;
                }
                else
                {
                    ConfiguracionFactura.NombreCliente = "No encontrado";
                    ConfiguracionFactura.IdentificacionCliente = "sin dato cc";
                }
                 
                
                
            }
            else
            {
                foreach (var item in testtPrecio)
                {
                    precioDetalle = item.Precio.ToString();
                }

                foreach (var item in testt)
                {
                    ConfiguracionFactura.NombreCliente = item.Nombres;
                    ConfiguracionFactura.IdentificacionCliente = item.Documento;
                    ConfiguracionFactura.CodigoFactura = numPedido;
                }
                foreach (var ff in testtt)
                {
                    FechaHora = ff.FechaCreacion;
                    IdUsuarioVenta = ff.IdUsuarioCreacion;
                    idPunto = ff.IdPunto;
                    f = ff.Id_Factura;
                    
                    var desPago = ff.IdMedioPago == "1" ? "Efectivo" : "Targeta";
                    //Configuracion medio pago seting factura 
                    if (nImpresionProducto == 1)
                    {
                        ConfMedioPago = new string[2];
                        ConfMedioPago[0] = "TOTAL A ABONAR";
                        //ConfMedioPago[1] = ff.Precio.ToString();
                        ConfMedioPago[1] = precioDetalle;
                        MetodoPago.Add(ConfMedioPago);
                    }
                    ConfMedioPago = new string[2];
                    ConfMedioPago[0] = "ABONO " + desPago;
                    ConfMedioPago[1] = ff.Valor.ToString();
                    MetodoPago.Add(ConfMedioPago);

                 /*   ConfMedioPago = new string[2];
                    ConfMedioPago[0] = ff.Cambio >= 0 ? "Cambio " : "Abono faltante ";
                    ConfMedioPago[1] = ff.Cambio.ToString();
                    MetodoPago.Add(ConfMedioPago);
*/

                    //Config Productos
                    if (nImpresionProducto == 1)
                    {

                        cambio = ff.Cambio >= 0 ? "Cambio " : "Abono faltante ";
                        TotCambio = ff.Cambio.ToString();


                        string[] ConfListaProductos = new string[3];
                        ConfListaProductos[0] = "1";
                        ConfListaProductos[1] = "Fiesta";
                        //ConfListaProductos[2] = ff.Precio.ToString();
                        ConfListaProductos[2] = precioDetalle;
                        ListaProductos.Add(ConfListaProductos);
                        nImpresionProducto = 0;
                    }
                }
            }


            ConfMedioPago = new string[2];
            ConfMedioPago[0] = cambio;
            ConfMedioPago[1] = TotCambio;
            MetodoPago.Add(ConfMedioPago);

            ConfMedioPago = new string[2];
            ConfMedioPago[0] = "NUEVO SALDO";
            ConfMedioPago[1] = "0";
            MetodoPago.Add(ConfMedioPago);

            var UsuarioVendedor = "";
            var PuntoVendedor = "";

            PuntoVendedor = await GetAsync<string>($"vuycoom/PuntoPorId/{idPunto}");
            UsuarioVendedor = await GetAsync<string>($"vuycoom/UsuarioPorId/{IdUsuarioVenta}");

            ConfiguracionFactura.Fecha = FechaHora;
            ConfiguracionFactura.Usuario = UsuarioVendedor;
            ConfiguracionFactura.Punto = PuntoVendedor;
            ConfiguracionFactura.ListaProductos = ListaProductos;
            ConfiguracionFactura.MetodosPago = MetodoPago;
            ConfiguracionFactura.CodigoFactura = numPedido;
            ImprimirFactura._FacturaImprimir = ConfiguracionFactura;
            var nImpresiones = 1;
            string fact = "";


            while (nImpresiones > 0)
            {

                nImpresiones--;
                string comprobar = ImprimirFactura.ImprimirTicketVyuCoom(exist);

                if (comprobar == "Factura realizada correctamente")
                {
                    fact = " Factura realizada correctamente";
                }
                else
                {
                    DATO += comprobar;
                }

            }
            return DATO += fact;
            #endregion
        }

    }
}
