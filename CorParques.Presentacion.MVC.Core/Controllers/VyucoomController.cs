using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System.Data;
using System.Diagnostics;
using System.Configuration;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class VyucoomController : ControladorBase
    {

        public static int totalResul = 0;
        public static List<PagosVyuCoom> pagadoCli;






        // GET: Vyucoom
        public async Task<ActionResult> Index()
        {
            totalResul = 0;
            Stopwatch _time = new Stopwatch();
            _time.Start();

            var _aperturas = await GetAsync<IEnumerable<Apertura>>($"Apertura/ObtenerAperturasTaquillero/{(Session["UsuarioAutenticado"] as Usuario).Id}/{(int)Enumerador.Estados.Entregado}");


            ViewBag.Apertura = true;

            if (_aperturas == null || _aperturas.Count() == 0)
                ViewBag.Apertura = false;

            var mediosPago = await GetAsync<IEnumerable<MediosPago>>("MediosPago/GetAll");
             ViewBag.MediosDePago = mediosPago.Select(x =>
                 new MediosPago { Nombre = x.Nombre , Id = x.Id, IdEstado = x.IdEstado}
             );
             var clientee = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos");
             var ValorFijo = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/ValorFijo");
            var Franquicia = await GetAsync<List<Franquicia>>("Franquicia/ObtenerTodos");
            ViewBag.franquicia = Franquicia ?? new List<Negocio.Entidades.Franquicia>();
            ViewBag.valorFijo = ValorFijo;
            ViewBag.valor = 50;
            Parametro _parametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/PROPINA");
            ViewBag.parametro = _parametro;
            ViewBag.Cliente = clientee.Select( x =>
                 new Cliente { Nombres = x.Nombre }
            );

            //
            Parametro _parametroCodSapTipoProductoAgrupaCantidad = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapTipoProductoAgrupaCantidad");
            ViewBag.CodSapTipoProductoAgrupaCantidad = _parametroCodSapTipoProductoAgrupaCantidad.Valor;
            //

            //
            IEnumerable<TipoGeneral> _donacion = await GetAsync<IEnumerable<TipoGeneral>>($"Pos/ObtenerProductosDonacion");
            ViewBag.donacion = string.Join(",", _donacion.Select(x => x.CodSAP));
            //

            //
            Parametro _parametroCodSapProductoAgrupaCantidad = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapProductoAgrupaCantidad");
            ViewBag.CodSapProductoAgrupaCantidad = _parametroCodSapProductoAgrupaCantidad.Valor;
            //

            //
            ViewBag.UltimaFactura = await GetAsync<Factura>($"Pos/ObtenerUltimaFactura/{IdPunto}");
            Parametro tipospuntoDescargue = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdTiposPuntoDescargue");

            var punto = await GetAsync<Puntos>($"Puntos/GetById/{IdPunto}");
            string[] strsplit;
            bool existepunto = true;
            strsplit = tipospuntoDescargue.Valor.Split(',');
            for (int i = 0; i < strsplit.Length; i++)
            {
                if (punto.IdTipoPunto.ToString() == strsplit[i].ToString())
                {
                    existepunto = false;

                    break;
                }
            }
            //

            var IdUsuarioLogeado = IdUsuarioLogueado;
            var IdPuntoo = IdPunto;
            ViewBag.parametros = new
            {
                IdMedioPagoEfectivo = (int)Enumerador.MediosPago.Efectivo,
                IdMedioPagoTarjetaDebito = (int)Enumerador.MediosPago.TarjetaDebito,
                IdMedioPagoTarjetaCredito = (int)Enumerador.MediosPago.TarjetaCredito,
                IdMedioPagoBonoRegalo = (int)Enumerador.MediosPago.Bonosregalo,
                IdMedioPagoBonoSodexo = (int)Enumerador.MediosPago.ChequesSodexo,
                IdMedioPagoDescuentoNomina = (int)Enumerador.MediosPago.DescuentoNomina,
                IdMedioPagoDocumentoBrinks = (int)Enumerador.MediosPago.DocumentoBrinks,
                IdMedioPagoTarjetaRecargable = (int)Enumerador.MediosPago.TarjetaRecargable,
                IdMedioPagoAPP = (int)Enumerador.MediosPago.APP,
                CodSapPropina = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodSapProdPropina"), // Código Sap producto propina
                CodSapCupoDebito = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodSapProdCupoDebito"), // Código Sap producto cupo débito
                CodSapProdParqueadero = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodSapProdParqueadero"), // Código Sap producto parqueadero
                CodSapProdReimpresionPq = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodSapProdReimpresionPq"), // Código Sap producto reimpresión
                CodSapTipoProdDescargue = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodSapProdNoAplicaDescargue"), // Tipo de productos que no aplican descargue
                CodSapRecargaTarjeta = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapRecargaTarjeta"), // Codigo sap producto recarga tarjeta
                CodSapClienteFan = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodigoSapProductoClienteFan"), // codigo sap cliente fan
                CodSapTarjetaRecargable = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapTarjetaRecargable"), // codigo sap cliente fan
                CodSapPrecioTarjeta = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapPlasticoTarjeta"),// codigo sap cliente fan
                CodSapReposicionTarjeta = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapRepoTarjeta"),
                AplicaDescargue = existepunto
            };
            ViewBag.TransaccionRedebanHabilitada = ConfigurationManager.AppSettings["FlujoRedeban"].ToString();
            _time.Stop();

            int segundos = _time.Elapsed.Seconds;
            return View();
        }

        
        [HttpPost]
        public async Task<JsonResult> BuscarPedido(string dato, string cliente)
        {
            var Total = await GetAsync<string>($"vuycoom/Pedido/{dato}");

            //Mejora
            var Total1 = await GetAsync<Vyucoom>($"vuycoom/PedidoCliente/{dato}");
            //

            string Nomcliente = await GetAsync<string>($"vuycoom/NCliPedido/{dato}");
            //var Total = 102400;
            var ValorFijo = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/ValorFijo");

            //var Mpago = mpago;
            ViewBag.valorFijo = ValorFijo;

            //Nombres enviar
            string nomEnviar = "";
            string docEnviar = "";
            //
            if (Nomcliente != null)
            {
                if (Nomcliente.Contains("-"))
                {
                    string[] separar = Nomcliente.Split('-');
                    docEnviar = separar[0];
                    nomEnviar = separar[1];
                }
            }


            if (Total != null && Total1 != null)
            {
                if (nomEnviar == Total1.Nombres && docEnviar == Total1.Documento)
                {
                    string nomCliente = Total1.Nombres;
                    string documento = Total1.Documento;
                    string Nomcliente1 = documento + "-" + nomCliente;
                    if (Total1.MontoAPagar.ToString().Contains("-") || Total1.MontoAPagar < 0)
                    {
                        Total1.MontoAPagar = int.Parse(Total1.MontoAPagar.ToString().Replace("-", ""));
                    }
                    return Json(new { bn = "Esta bien... ", Valor = Total1.MontoAPagar, p = dato, NombCliente = Nomcliente1 });
                }
                else
                {
                    return Json(new { bn = "Esta bien... ", Valor = "Pedido_no_existe", p = dato, NombCliente = "22222-SIN REGISTROS CLIENTE" });
                }
            }
            else {
                if (Total != null && Total != "-1")
                {
                    return Json(new { bn = "Esta bien... ", Valor = Total, p = dato, NombCliente = Nomcliente });
                }
                else if (Total1 != null && Total != "-1")
                {
                    string nomCliente = Total1.Nombres;
                    string documento = Total1.Documento;
                    string Nomcliente1 = documento + "-" + nomCliente;
                    if (Total1.MontoAPagar.ToString().Contains("-") || Total1.MontoAPagar < 0)
                    {
                        Total1.MontoAPagar = int.Parse(Total1.MontoAPagar.ToString().Replace("-", ""));
                    }
                    return Json(new { bn = "Esta bien... ", Valor = Total1.MontoAPagar, p = dato, NombCliente = Nomcliente1 });
                }
                else
                {
                    return Json(new { bn = "Esta bien... ", Valor = "Pedido_no_existe", p = dato, NombCliente = "22222-SIN REGISTROS CLIENTE" });
                }
            }
            
            //Estaban las codiciones if - return
        }

        [HttpGet]
        public void reiniciarDat()
        {
            totalResul = 0;
            verDiferencias("0","0");
            verDiferenciass("0","0");
        }

        [HttpPost]
        public async Task<JsonResult> verDiferencias(string dato, string Pedido)
        {

            int pago = int.Parse(dato.Replace(".", "").Replace(".", ""));
            int valorPagar = int.Parse(Pedido.Replace(".", "").Replace(".", ""));
            //totalResul = valorPagar - pago;
            totalResul = pago - valorPagar;

            //diferencia - TotalApagar - TotalPagado
            //return Json(new { diferencia = Tot, pagar = totall, pago = totalResul });
            return Json(new { diferencia = totalResul, pagar = valorPagar, pago = pago });
        }

         [HttpPost]
        public async Task<JsonResult> verDiferenciass(string dato, string Pedido)
        {
            int pago = int.Parse(dato.Replace(".", "").Replace(".", ""));
            int valorPagar = int.Parse(Pedido.Replace(".", "").Replace(".", ""));
            //totalResul = valorPagar - pago;
            totalResul = pago - valorPagar;

            return Json(new { diferencia = totalResul, pagar = valorPagar, pago = pago });
            //return Json(new { diferencia = Tot, pagar = totall, pago = totalResul });
        }  

        [HttpPost]
        //public JsonResult Mostrar(List<PagosVyuCoom> medioPago)
        public JsonResult MostrarDato(List<PagosVyuCoom> medioPago)
        {
            var filtra = "";
            try
            {
                pagadoCli = medioPago;
                foreach (var ff in medioPago)
                {
                    filtra = ff.DescMedioPago;
                }
            }
            catch (Exception)
            {
                return Json(new { medios = pagadoCli, filtrado = "Efectivo" });
            }
            return Json(new { medios = medioPago, filtrado = filtra });
        }


        [HttpGet]
        //public async Task<string> InsertDatos(string dato, string cliente, string mpago, string pago, string cambioo, List<PagosVyuCoom> medioPago, int exist) 
        public async Task<string> InsertDatos(string dato, string cliente, string mpago, string pago, string cambioo, List<PagosVyuCoom> medioPago, int exist, string aPagar) 
        {
            //Muestra mensaje al usuario
            string DATO = "";


            ///Insertar los datos en nuevoCliente

            string cambio = cambioo.Replace('-',' ');

            ///

            string[] ConfTest = new string[2];
            ConfTest[0] = int.Parse(cambio) >= 0 ? "Abono a favor" : "Abono faltante";

            var clienteMostrar = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos");

            ViewBag.Cliente = clienteMostrar.Select(x =>
                new Cliente { Nombres = x.Nombre }
            );

            totalResul = 0;
            ///////////////////////////Dato para verificar
            var clieExist = "";
            string CCcliente = "";
            string Nombrecliente = "";
            try
            {
                if (cliente.Contains("-"))
                {
                    string[] clientee = cliente.Split('-');
                    CCcliente = clientee[0];
                    Nombrecliente = clientee[1];
                }
                else
                {
                    CCcliente = cliente;
                }
            }
            catch (Exception)
            {

                clieExist = "NoExiste";
            }


            var mediosPago = await GetAsync<IEnumerable<MediosPago>>("MediosPago/GetAll");
            

            var IdPago = int.MinValue;
            var NombPunto = NombrePunto;
            var IdPuntoo = IdPunto;
            ViewBag.MediosDePago = mediosPago.Select(x =>
                new MediosPago { Nombre = x.Nombre, Id = x.Id }
            );
           /* foreach (var idM in mediosPago)
            {
                if (idM.Nombre == mpago)
                {
                    IdPago = idM.Id;
                }
            }*/

            var IdUsuarioLogeado = IdUsuarioLogueado;
            var IdPuntooo = IdPunto;
           /* Vyucoom vyucoom = new Vyucoom();
            vyucoom.MontoPago = "2";
            vyucoom.NReferenciaCliente = 2;
            vyucoom.IdMedioPago = IdPago.ToString();
*/
            //////////////////////////

            TicketFactura ImprimirFactura = new TicketFactura();
            FacturaImprimir ConfiguracionFactura = new FacturaImprimir();
            var Total = pago;
            
            ////Descometar -> esto lo comente el 03-06-2022
            var ValorFijo = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/ValorFijo");

            var Total1 = await GetAsync<Vyucoom>($"vuycoom/PedidoCliente/{dato}");
            var valorPedido = await GetAsync<string>($"vuycoom/Pedido/{dato}");
            ///



            //valorPedido = valorPedido == null ? "0" : valorPedido;
            if (Total1 != null)
            {
                /*  if (Total1.MontoPagado < int.Parse(valorPedido))
                  {
                      //valorPedido = Total1.MontoPagado == null ? "0" : Total1.MontoPagado.ToString();
                      valorPedido = Total1.MontoPagado == null ? "0" : (Total1.MontoPagado + pago).ToString();
                  }
                  else
                  {
                      valorPedido = Total1.MontoPagado == null ? "0" : Total1.MontoPagado.ToString();
                  }*/
                valorPedido = valorPedido == null ? "0" : valorPedido;
            }
            else
            {
                valorPedido = valorPedido == null ? "0" : valorPedido;
            }
            var Mpago = mpago;


            /*
             * Datos para pasar a FacturaImprimir
             */
            ConfiguracionFactura.NombreCliente = Nombrecliente;
            ConfiguracionFactura.IdentificacionCliente = CCcliente;
            ConfiguracionFactura.Punto = NombPunto;
            ConfiguracionFactura.CodigoFactura = dato;

            //Metodo pago Y Total
            List<string[]> MetodoPago = new List<string[]>();
            string[] ConfMedioPago = new string[2];
            ConfMedioPago[0] = "TOTAL A ABONAR";
            //ConfMedioPago[1] = valorPedido == "0" ? pago : valorPedido;
            ConfMedioPago[1] = aPagar == "0" ? "0" : aPagar;
            MetodoPago.Add(ConfMedioPago);
            //foreach (var MPagoo in medioPago)
            foreach (var MPagoo in pagadoCli)
            {
                ConfMedioPago = new string[2];
                ConfMedioPago[0] = "ABONO " + MPagoo.DescMedioPago;
                
                ConfMedioPago[1] = MPagoo.Valor;
                MetodoPago.Add(ConfMedioPago);
            }
            ConfMedioPago = new string[2];

            //ConfMedioPago[0] = "CAMBIO ";
            ConfMedioPago[0] = int.Parse(cambioo) >= 0 ? "Abono a favor" : "Abono faltante ";
            int diferencia = int.Parse(cambioo);
            ConfMedioPago[1] = diferencia.ToString();
            MetodoPago.Add(ConfMedioPago);
            ConfMedioPago = new string[2];
            ConfMedioPago[0] = "NUEVO SALDO";
            ConfMedioPago[1] = "0";
            MetodoPago.Add(ConfMedioPago);
            ConfiguracionFactura.MetodosPago = MetodoPago;

            //Lista productos
            List<string[]> ListaProductos = new List<string[]>();
            string[] ConfListaProductos = new string[3];
            ConfListaProductos[0] = "1";
            ConfListaProductos[1] = ValorFijo.Valor;
            ConfListaProductos[2] = valorPedido == "0" ? pago : valorPedido;
            ListaProductos.Add(ConfListaProductos);
            ConfiguracionFactura.ListaProductos = ListaProductos;

            //Usuario logueado
            var UsuarioLogeado = NombreUsuarioLogueado;
            ConfiguracionFactura.Usuario = UsuarioLogeado;
           
            DateTime d = new DateTime();
         

            ImprimirFactura.Documento(false);

            ConfiguracionFactura.Fecha = DateTime.Now;

            //Configuracion que toma ImprimirFactura
            ImprimirFactura._FacturaImprimir = ConfiguracionFactura;

            ///sin guardar 
            var existe = 0;
            var cli = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos");
            foreach (var f in cli)
            {
                if (f.Nombre == cliente)
                {
                    existe = 0;
                }
                else 
                {
                    existe = 1;
                }
            }
            
             

            //Datos que se almacenaran en facturas

            /////Insertar varios medios de pago
            Vyucoom vyuConfigg = new Vyucoom();
            var numFacturas = await GetAsync<string>("vuycoom/getNumeroFactura");
            vyuConfigg.Id_Factura = numFacturas;
            vyuConfigg.Id_Punto = IdPuntooo;
            vyuConfigg.Id_Estado = 0;
            vyuConfigg.Id_Usuario = IdUsuarioLogeado;
            vyuConfigg.Id_Apertura = 12;
            vyuConfigg.FechaCreacion = DateTime.Now;
            
            vyuConfigg.Cliente = Nombrecliente;
            vyuConfigg.Cantidad = 1;
            vyuConfigg.IdDetalleProducto = 1;
            vyuConfigg.Cambio = diferencia;
            vyuConfigg.Id_Producto = 1;
            vyuConfigg.CodFactura = dato;
            vyuConfigg.Precio = int.Parse(pago);
            //vyucoom.NumReferencia = "";

            string lol = IdPuntooo + "|" + dato + "|" + IdUsuarioLogeado;


            //var datoFact = await GetAsync<string>($"vuycoom/InsertarFacturaVer/{lol}");

            var datoFact = await PostAsync<Vyucoom,string>("vuycoom/InsertarFacturaVer",vyuConfigg);
            if (datoFact != null)
            {
               /* if (datoFact.Elemento.ToString().Contains("Fallo") || datoFact.Elemento.ToString().Contains("Error"))
                {
                    vyuConfigg.CodFactura = string.Concat("C-", vyuConfigg.CodFactura);
                    datoFact = await PostAsync<Vyucoom, string>("vuycoom/InsertarFacturaVer", vyuConfigg);
                }*/
            }

            if (datoFact != null)
            {
                if (!datoFact.Elemento.ToString().Contains("Fallo insertar y traer dato "))
                {
                    DATO += " --- TB_FACTURA " + "Inserto correctamente";
                }
                else
                {
                    DATO += " --- TB_FACTURA " + datoFact.Elemento;
                }
            }
            

            //////////////////////////
            
            foreach (var MPagoo in pagadoCli)
            {
                //vyuConfig.Id_Factura = dato;
                vyuConfigg.CodFactura = datoFact.Elemento.ToString();
                vyuConfigg.IdMedioPago = MPagoo.IdMedioPago;
                vyuConfigg.MontoPago = MPagoo.Valor;
                //vyuConfig.Cliente = cliente;
                //Inserta los datos que imprime
                var datoos = await PostAsync<Vyucoom, string>("vuycoom/InsertaDato", vyuConfigg);
                DATO += " --- TB_DETALLEFACTURA MPAGOS " +datoos.Elemento;
            }

            //ESTO SE PUEDE REVERSAR
            //var Total1 = await GetAsync<Vyucoom>($"vuycoom/PedidoCliente/{dato}");

            if (exist == 1)
            {
                try
                {
                    Vyucoom datoReciboCaja = new Vyucoom();
                    datoReciboCaja.Cliente = Nombrecliente;
                    datoReciboCaja.Documento = CCcliente;
                    datoReciboCaja.TipDocumento = "CC";
                    datoReciboCaja.IdFactura = datoFact.Elemento.ToString();
                    datoReciboCaja.MontoAPagar = int.Parse(cambio);
                    datoReciboCaja.MontoPago = "0";
                    datoReciboCaja.MontoPagado = int.Parse(pago);
                    datoReciboCaja.CodPedido = long.Parse(dato);
                    var datt = await PostAsync<Vyucoom, string>($"vuycoom/InsertarRecCaja", datoReciboCaja);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else if (Total1 != null)
            {
                if (exist == 0 && Total1.Nombres == Nombrecliente)
                {
                    Vyucoom datoReciboCaja = new Vyucoom();
                    datoReciboCaja.Cliente = Nombrecliente;
                    datoReciboCaja.Documento = CCcliente;
                    datoReciboCaja.TipDocumento = "CC";
                    datoReciboCaja.IdFactura = datoFact.Elemento.ToString();
                    datoReciboCaja.MontoAPagar = int.Parse(cambio);
                    datoReciboCaja.MontoPago = "0";
                    datoReciboCaja.MontoPagado = int.Parse(pago);
                    datoReciboCaja.CodPedido = long.Parse(dato);
                    string datoo = "Modifico el dato ";
                    string enviado = await PutAsync<Vyucoom, string>("vyucoom/EditarPagoReciboCaja", datoReciboCaja);
                }
                else if (Total1.Nombres == Nombrecliente)
                {
                    Vyucoom datoReciboCaja = new Vyucoom();
                    datoReciboCaja.Cliente = Nombrecliente;
                    datoReciboCaja.Documento = CCcliente;
                    datoReciboCaja.TipDocumento = "CC";
                    datoReciboCaja.IdFactura = datoFact.Elemento.ToString();
                    datoReciboCaja.MontoAPagar = int.Parse(cambio);
                    datoReciboCaja.MontoPago = "0";
                    datoReciboCaja.MontoPagado = int.Parse(pago);
                    datoReciboCaja.CodPedido = long.Parse(dato);
                    string datoo = "Modifico el dato ";
                    string enviado = await PutAsync<Vyucoom, string>("vyucoom/EditarPagoReciboCaja", datoReciboCaja);
                }

            }
            else if (exist == 0 && Total1 == null)
            {
                Vyucoom datoReciboCaja = new Vyucoom();
                datoReciboCaja.Cliente = Nombrecliente;
                datoReciboCaja.Documento = CCcliente;
                datoReciboCaja.TipDocumento = "CC";
                datoReciboCaja.IdFactura = datoFact.Elemento.ToString();
                datoReciboCaja.MontoAPagar = int.Parse(cambio);
                datoReciboCaja.MontoPago = "0";
                datoReciboCaja.MontoPagado = int.Parse(pago);
                datoReciboCaja.CodPedido = long.Parse(dato);
                var datt = await PostAsync<Vyucoom, string>($"vuycoom/InsertarRecCaja", datoReciboCaja);
            } 


            //var nImpresiones = 2;
            var nImpresiones = 1;

            string fact = "";
            while (nImpresiones > 0)
            {
                nImpresiones--;
                string comprobar =  ImprimirFactura.ImprimirTicketVyuCoom(exist);
                
                if (comprobar == "Factura realizada correctamente")
                {
                    fact = " Factura realizada correctamente";
                }
                else
                {
                    DATO += comprobar;
                }
                
            }
            //DATO += fact;

            DATO = "Abono realizado correctamente";

            //int actura = Factura;
            ///


            /*  Vyucoom vyuConfig = new Vyucoom();
          //vyuConfig.Id_Factura = dato;
          var numFactura = await GetAsync<string>("vuycoom/getNumeroFactuar");
          vyuConfig.Id_Factura = numFactura;
          vyuConfig.Id_Punto = IdPuntooo;
          vyuConfig.Id_Estado = 0;
          vyuConfig.Id_Usuario = IdUsuarioLogeado;
          vyuConfig.Id_Apertura = 12;
          vyuConfig.FechaCreacion = DateTime.Now;
          vyuConfig.IdMedioPago = IdPago.ToString();
          vyuConfig.MontoPago = Total;
          //vyuConfig.Cliente = cliente;
          vyuConfig.CodFactura = dato;
          vyuConfig.Cliente = Nombrecliente;
          vyuConfig.Cantidad = 1;
          vyuConfig.IdDetalleProducto = 1;
          vyuConfig.Cambio = diferencia;
          vyuConfig.Id_Producto = 1;


          //Inserta los datos que imprime
          var datoo = await PostAsync<Vyucoom, string>("vuycoom/InsertaDato", vyuConfig);*/
            //Reimprimir();
            return DATO;
        }

        public async Task<string> Reimprimir(string NPedido)
        {
            int exist = 0;
            string DATO = "";
            #region Reimpresion Vyucoom
            TicketFactura ImprimirFactura = new TicketFactura();
            FacturaImprimir ConfiguracionFactura = new FacturaImprimir();

            IEnumerable<Vyucoom>[] test = await GetAsync<IEnumerable<Vyucoom>[]>("vuycoom/Reimprimir");

            IEnumerable<Vyucoom> testt = test[0];
            IEnumerable<Vyucoom> testtt = test[1];
            Vyucoom usuario;
            if (testt.Count() == 0)
            {
                exist = 1;
                var f = "";
                foreach (var ff in testtt)
                {
                    f = ff.Id_Factura;
                }
                usuario = await GetAsync<Vyucoom>($"vuycoom/Reimprimir/{f}");
            }
            else
            {
                foreach (var item in testt)
                {
                    ConfiguracionFactura.NombreCliente = item.Nombres;
                    ConfiguracionFactura.IdentificacionCliente = item.Documento;
                    
                    ConfiguracionFactura.CodigoFactura = NPedido;
                }
                foreach (var item in testtt)
                {
                    ConfiguracionFactura.Punto = item.IdPunto.ToString();
                }
            }

            var nImpresiones = 2;
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
