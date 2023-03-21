using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using System.Configuration;
using System.Diagnostics;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class PedidoAController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.ListPuntos = await GetAsync<IEnumerable<Mesa>>("Puntos/ObtenerMesas");

            ViewBag.ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/{(Session["UsuarioAutenticado"] as Usuario).Id}");
            if (ViewBag.ListMesasActivas == null)
            {
                ViewBag.ListMesasActivas = new List<Mesa>();
            }
            
            //Proceso POS 
            Session["PosCont"] = true;


            //Validar tiempo 

            Stopwatch _time = new Stopwatch();
            _time.Start();

            //Validar por taquillero la apertura
            var _aperturas = await GetAsync<IEnumerable<Apertura>>($"Apertura/ObtenerAperturasTaquillero/{(Session["UsuarioAutenticado"] as Usuario).Id}/{(int)Enumerador.Estados.Entregado}");

            ViewBag.Apertura = true;

            if (_aperturas == null || _aperturas.Count() == 0)
                ViewBag.Apertura = false;

            IEnumerable<TipoGeneral> _list = new List<TipoGeneral>();
            var _listMediosPago = await GetAsync<IEnumerable<TipoGeneral>>("MediosPago/GetAllSimple");
            if (_listMediosPago != null)
                _list = _listMediosPago;

            ViewBag.list = _list.Where(x => x.Id != (int)Enumerador.MediosPago.DocumentoBrinks).ToList();

            var Franquicia = await GetAsync<List<Franquicia>>("Franquicia/ObtenerTodos");
            ViewBag.franquicia = Franquicia ?? new List<Negocio.Entidades.Franquicia>();

            //_list = await GetAsync<IEnumerable<TipoGeneral>>("TipoCliente/GetAllSimple");
            //ViewBag.listTipoCliente = _list;

            Parametro _parametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/PROPINA");
            ViewBag.parametro = _parametro;
            //DANR: traer productos 
            IEnumerable<TipoGeneral> _donacion = await GetAsync<IEnumerable<TipoGeneral>>($"Pos/ObtenerProductosDonacion");
            ViewBag.donacion = string.Join(",", _donacion.Select(x => x.CodSAP));

            //var _rta = await GetAsync<IEnumerable<Puntos>>($"Puntos/GetAllxTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            ////Retono los activos - atraccion
            //ViewBag.atraccion = _rta.Where(x => x.Estado == Enumerador.Estados.Activo.ToString());

            //var _pasaporte = await GetAsync<IEnumerable<PasaporteUso>>("PasaporteUso/ObtenerTodosPasaporte");
            //Retono los activos - PasaporteUso
            //ViewBag.pasaporteUso = _pasaporte.Where(x => x.Estado == Enumerador.Estados.Activo.ToString());


            //Obtiene el codigo sap por cada tipo de producto
            //var _csAtracciones = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Atracciones}");
            //var _csDestrezas = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Destrezas}");
            //var _csAyB = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.AyB}");
            //var _csSouvenires = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Souvenires}");
            //var _csServicios = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Servicios}");
            //var _csPasaporte = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Pasaporte}");

            //Obtiene TODOS LOS PRODUCTOS
            var _ListaTodosProductosSAP = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerTodosProductos");
            ViewBag.ListaTodosProductosSAP = _ListaTodosProductosSAP;

            //Obtienes los productos asociados a un codigoSapTipoProducto
            //var _model = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{_csAyB.CodigoSap}");

            var _aybProp = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AYB_Mundo_NaturalPOS");

            ViewBag.ListAyB = _ListaTodosProductosSAP.Where(l => l.CtgProducto == 1).ToArray();

            var _souvenir = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/SouveniresPOS");

            //var _ListSouvenir = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{_csSouvenires.CodigoSap}");
            ViewBag.ListSouvenir = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _souvenir.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _souvenir.Valor).ToArray()
                : new List<Producto>().ToArray();

            var _deztresas = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/Uso_DestrezasPOS");

            //var _ListAyD = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{ _csAtracciones.CodigoSap + "," + _csDestrezas.CodigoSap}");
            List<Producto> listaAtraccionesDestrezas = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _deztresas.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _deztresas.Valor).ToList()
                : new List<Producto>();

            var _atracciones = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/Uso_AtraccionesPOS");

            List<Producto> listaAtracciones = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _atracciones.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _atracciones.Valor).ToList()
                : new List<Producto>();

            listaAtraccionesDestrezas.AddRange(listaAtracciones);
            ViewBag.ListAyD = listaAtraccionesDestrezas.ToArray();


            var _servicios = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/Servicios_ventaPOS");
            //var _ListServicios = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{_csServicios.CodigoSap}");
            ViewBag.ListServicios = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _servicios.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _servicios.Valor).ToArray()
                : new List<Producto>().ToArray();

            var _pasaporte = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/Uso_PasaportePOS");

            //var _ListPasaporte = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{_csPasaporte.CodigoSap}");
            Producto[] pasaportes = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _pasaporte.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _pasaporte.Valor).ToArray()
                : new List<Producto>().ToArray();

            var _pasaportesPunto = await GetAsync<List<PuntoBrazalete>>($"Pos/ObtenerPasaportesXPunto/{IdPunto}");
            List<Producto> pasaportesImpresion = new List<Producto>();
            if (_pasaportesPunto != null && _pasaportesPunto.Count > 0)
            {
                _pasaportesPunto.ForEach(x => {
                    Producto producto = _ListaTodosProductosSAP.Where(l => l.IdProducto == x.IdProducto).FirstOrDefault();
                    if (producto != null)
                    {
                        pasaportesImpresion.Add(producto);
                    }
                });
            }

            List<Producto> tmpPasaportes = pasaportes.ToList();
            tmpPasaportes.AddRange(pasaportesImpresion);
            ViewBag.PasaportesImpresion = pasaportesImpresion.ToArray();
            ViewBag.ListPasaporte = tmpPasaportes.ToArray();

            //Parametros de agruparmiento de cantidad
            Parametro _parametroCodSapTipoProductoAgrupaCantidad = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapTipoProductoAgrupaCantidad");
            ViewBag.CodSapTipoProductoAgrupaCantidad = _parametroCodSapTipoProductoAgrupaCantidad.Valor;

            Parametro _parametroCodSapProductoAgrupaCantidad = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapProductoAgrupaCantidad");
            ViewBag.CodSapProductoAgrupaCantidad = _parametroCodSapProductoAgrupaCantidad.Valor;

            //Lista de convenios
            var _listConvenios = await GetAsync<IEnumerable<Convenio>>("ConvenioSAP/ObtenerLista");
            if (_listConvenios == null)
                _listConvenios = new List<Convenio>().ToArray();

            ViewBag.ConveniosSeleccion = _listConvenios.Any(c => !c.EsCodigoBarras) ? _listConvenios.Where(c => !c.EsCodigoBarras).ToArray() : new List<Convenio>().ToArray();
            ViewBag.ConveniosCodigoBarras = _listConvenios.Any(c => c.EsCodigoBarras) ? _listConvenios.Where(c => c.EsCodigoBarras).ToArray() : new List<Convenio>().ToArray();

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

            //Obtener parámetros aplicación 

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

            _time.Stop();

            int segundos = _time.Elapsed.Seconds;

            return View(ViewBag.ListAyB);
        }
                                                                                                                                                                     
        [HttpPost]
        public async Task<ActionResult> PagarCompra(List<Producto> ListaProductos, List<Acompanamiento> ListaAcompa
                                                    , int Mesa)
        {

            var resultado = new object() ;
            try
            {
                var idUsuario = IdUsuarioLogueado;
                PagoFactura model = new PagoFactura
                {
                    IdUsuario = idUsuario,
                    listaProducto = ListaProductos,
                    listaAcomp = ListaAcompa,
                    IdMesa = Mesa

                };
                 resultado = await PostAsync<PagoFactura, string>("Pos/InsertarPedidoR", model);
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
               
            }

            //Pendiente factura
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CerrarMesa(int IdPedido)
        {

            var resultado = new object();
            try
            {
                var model = await GetAsync<string>($"Puntos/CerrarMesa/{IdPedido}");
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }

        public async Task<ActionResult> Acompa(int IdProducto)
        {
            var model = await GetAsync<IEnumerable<AcompanamientoXProducto>>($"Puntos/ObtenerAcompaXproducto/{IdProducto}");
            if (model != null)
            {
                return PartialView("_Acompa", model);
            }
            else
            {

                return Json(1, JsonRequestBehavior.AllowGet); ;
            }


        }
        public async Task<ActionResult> ValidarEstadoPedido(int IdPedido)
        {
            var model = await GetAsync<string>($"Puntos/ValidarEstadoPedido/{IdPedido}");

                return Json(model, JsonRequestBehavior.AllowGet); 


        }


        [HttpPost]
        public async Task<ActionResult> ListarProductosMesa(int IdMesa)
        {        
            var model = await GetAsync<ListaAcomProductos>($"Puntos/ListarProductosMesa/{IdMesa}");
            if (model != null)
            {
                return Json(model, JsonRequestBehavior.AllowGet); 
            }
            else
            {

                return Json(model, JsonRequestBehavior.AllowGet); 
            }    
         
           
        }
    }
}