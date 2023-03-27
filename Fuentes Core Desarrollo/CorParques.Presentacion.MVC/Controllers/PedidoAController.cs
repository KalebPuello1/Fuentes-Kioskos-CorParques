using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Transversales.Util;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class PedidoAController : ControladorBase
    {
        //public int IdUsuarioAnularPedidosRestaurante = Convert.ToInt32(ConfigurationManager.AppSettings["IdUsuarioAnularPedidosRestaurante"].ToString());
        //se trae informacion para mostrar productos , mesas para cargar en pedido 
        public async Task<ActionResult> Index()
        {
            ViewBag.ListPuntos = await GetAsync<IEnumerable<Mesa>>("Puntos/ObtenerMesas");

            ViewBag.ListZonas = await GetAsync<IEnumerable<ZonaRestaurante>>("Puntos/ObtenerZonasRestaurante");
            var ParametrosPedido = await GetAsync<IEnumerable<Parametro>>("Parameters/ObtenerParametrosGlobales");
            var objParametro = ParametrosPedido.Where(x => x.Nombre.Equals("IdUsuarioAnularPedidosRestaurante")).FirstOrDefault();
            Int64 objParametrovalue = 0;
            if (objParametro != null)
            {
                if (objParametro.Valor != null)
                {
                    if (Regex.IsMatch(objParametro.Valor, @"^[0-9]+$"))
                    {
                        objParametrovalue = Convert.ToInt64(objParametro.Valor);
                    }
                }

            }
            ViewBag.BotonAnular = "";
            if (objParametrovalue == (Session["UsuarioAutenticado"] as Usuario).Id)
            {
                ViewBag.BotonAnular = "";
            }
            else
            {
                ViewBag.BotonAnular = "hidden";
            }

            var ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/{(Session["UsuarioAutenticado"] as Usuario).Id}");
            if (ListMesasActivas == null)
            {
                ViewBag.ListMesasActivas = new List<Mesa>();
            }
            else
            {
                ViewBag.ListMesasActivas = ListMesasActivas.Where(l => (l.Estado == 1)).ToList();
            }


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



            Parametro _parametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/PROPINA");
            ViewBag.parametro = _parametro;
            //DANR: traer productos 
            IEnumerable<TipoGeneral> _donacion = await GetAsync<IEnumerable<TipoGeneral>>($"Pos/ObtenerProductosDonacion");
            ViewBag.donacion = string.Join(",", _donacion.Select(x => x.CodSAP));



            //Obtiene TODOS LOS PRODUCTOS
            var _ListaTodosProductosSAP = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerTodosProductos");
            ViewBag.ListaTodosProductosSAP = _ListaTodosProductosSAP;



            var _aybProp = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AYB_Mundo_NaturalPOS");

            ViewBag.ListAyB = _ListaTodosProductosSAP.Where(l => l.CtgProducto == 1).ToArray();

            var _souvenir = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/SouveniresPOS");

            ViewBag.ListSouvenir = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _souvenir.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _souvenir.Valor).ToArray()
                : new List<Producto>().ToArray();

            var _deztresas = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/Uso_DestrezasPOS");


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

            ViewBag.ListServicios = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _servicios.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _servicios.Valor).ToArray()
                : new List<Producto>().ToArray();

            var _pasaporte = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/Uso_PasaportePOS");


            Producto[] pasaportes = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _pasaporte.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _pasaporte.Valor).ToArray()
                : new List<Producto>().ToArray();


            List<Producto> tmpPasaportes = pasaportes.ToList();

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

            Parametro tipospuntoDescargue = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdTiposPuntoDescargue");



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
                //AplicaDescargue = existepunto
            };

            _time.Stop();

            int segundos = _time.Elapsed.Seconds;

            return View(ViewBag.ListAyB);
        }

        public async Task<ActionResult> GetByIdTipoAcompa(int idTipoAcompa)

        {
            var item = await GetAsync<TipoAcompanamiento>($"Puntos/ObtenerTipoAcompaRestauranteXId/{idTipoAcompa}");
            var estados1 = new TipoGeneral();
            var estados2 = new TipoGeneral();
            var listaestados = new List<TipoGeneral>();
            estados1.Id = 1;
            estados1.Nombre = "Activo";
            listaestados.Add(estados1);
            estados2.Id = 2;
            estados2.Nombre = "Inactivo";
            listaestados.Add(estados2);

            ViewBag.Estados = listaestados;
            return PartialView("../AdminRestaurante/_EditTipoAcompanamiento", item);
        }
        public async Task<ActionResult> DeleteTipoAcompa(int idTipoAcompa)
        {
            var item = await GetAsync<TipoAcompanamiento>($"Puntos/EliminarTipoAcompaRestaurante/{idTipoAcompa}");
            return Json(item, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> UpdateTipoAcompa(TipoAcompanamiento modelo)
        {
            //Pos / ActualizarProducto

            var resultado = await PostAsync<TipoAcompanamiento, string>("Pos/ActualizarTipoAcompaRestaurante", modelo);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetPartialTipoAcompa()
        {
            var item = await GetAsync<TipoAcompanamiento>($"Puntos/ObtenerTipoAcompaRestauranteXId/{3}");
            var tipoacom = new TipoAcompanamiento();
            var estados1 = new TipoGeneral();
            var estados2 = new TipoGeneral();
            var listaestados = new List<TipoGeneral>();
            estados1.Id = 1;
            estados1.Nombre = "Activo";
            listaestados.Add(estados1);
            estados2.Id = 2;
            estados2.Nombre = "Inactivo";
            listaestados.Add(estados2);

            ViewBag.Estados = listaestados;
            return PartialView("../AdminRestaurante/_CreateTipoAcompanamiento", tipoacom);
        }

        public async Task<ActionResult> GetPartialProductos()
        {
            var _ListaTodosProductosSAP = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos"); ;
            var ListaTodosProductosSAP = _ListaTodosProductosSAP;
            ViewBag.TipoAcompaRestaurante = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/ObtenerTipoAcompaRestaurante");
            ViewBag.TipoProductosRestaurante = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/ObtenerTipoProductosRestaurante");

            return PartialView("../AdminRestaurante/_ListaProductosG", ListaTodosProductosSAP);
        }


        [HttpPost]
        public async Task<ActionResult> AgregarProducAraza(List<Producto> listaProductosAgregar)
        {

            var resultado = new object();
            try
            {
                var idUsuario = IdUsuarioLogueado;
                PagoFactura model = new PagoFactura
                {
                    IdUsuario = idUsuario,
                    listaProducto = listaProductosAgregar
                };
                resultado = await PostAsync<PagoFactura, string>("Pos/AgregarProducAraza", model);
                //ViewBag.ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/{(Session["UsuarioAutenticado"] as Usuario).Id}");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            //return PartialView("_ListaMesas", ViewBag.ListMesasActivas);
            //Pendiente factura
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ObtenerProdRestaurante(int id)
        {

            var item = await GetAsync<IEnumerable<Producto>>($"Puntos/ObtenerProductoAdminRestaurante/{id}");
            ViewBag.TipoAcompaRestaurante = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/ObtenerTipoAcompaRestaurante");
            ViewBag.TipoProductosRestaurante = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/ObtenerTipoProductosRestaurante");
            return PartialView("../AdminRestaurante/_Edit", item.FirstOrDefault());
        }
        public async Task<ActionResult> ObtenerAcompaTipo(int id)
        {
            //var item = await GetAsync<Producto>($"Pos/ObtenerProducto/{id}");

            var _ListaAcompa = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerTodosProductosRestaurante");

            if (_ListaAcompa == null)
            {
                ViewBag.ListaAcompana = new List<Producto>();
            }
            else
            {
                ViewBag.ListaAcompana = _ListaAcompa.Where(l => (l.CtgProducto == 1) && (l.Id_TipoAcomp > 0) && (l.IdProducto != id)).ToArray();
            }


            var TipoAcompaRestaurante = await GetAsync<IEnumerable<TipoAcompanamiento>>($"Puntos/ObtenerTipoAcompaXProductoRestaurante/{id}");
            ViewBag.TipoAcompaRestaurante = TipoAcompaRestaurante;
            if (TipoAcompaRestaurante == null)
            {
                ViewBag.TipoAcompaRestaurante = new List<TipoAcompanamiento>();
            }
            var AcompaProductosRestaurante = await GetAsync<IEnumerable<Acompanamiento>>($"Puntos/ObtenerAcompaXproductoAdmin/{id}");
            ViewBag.AcompaProductosRestaurante = AcompaProductosRestaurante;
            if (AcompaProductosRestaurante == null)
            {
                ViewBag.AcompaProductosRestaurante = new List<Acompanamiento>();
            }

            return PartialView("../AdminRestaurante/_Acompanamientos", ViewBag.ListaAcompana);
        }
        [HttpPost]
        public async Task<ActionResult> PagarCompra(List<Producto> ListaProductos, List<Acompanamiento> ListaAcompa
                                                    , int Mesa, string NombreCliente)
        {

            var resultado = new object();
            try
            {
                var idUsuario = IdUsuarioLogueado;
                PagoFactura model = new PagoFactura
                {
                    IdUsuario = idUsuario,
                    listaProducto = ListaProductos,
                    listaAcomp = ListaAcompa,
                    IdMesa = Mesa,
                    NombreCliente = NombreCliente
                };
                resultado = await PostAsync<PagoFactura, string>("Pos/InsertarPedidoR", model);
                //ViewBag.ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/{(Session["UsuarioAutenticado"] as Usuario).Id}");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            //return PartialView("_ListaMesas", ViewBag.ListMesasActivas);
            //Pendiente factura
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> GuardarAcompaProduAdmin(List<Acompanamiento> lstProductosAcompanamientosAdmin
                                                   , int IdProducto)
        {

            var resultado = new object();
            try
            {
                var idUsuario = IdUsuarioLogueado;
                ProductoxAcompanamiento model = new ProductoxAcompanamiento
                {
                    IdProducto = IdProducto,
                    listaAcomp = lstProductosAcompanamientosAdmin

                };
                resultado = await PostAsync<ProductoxAcompanamiento, string>("Pos/GuardarAcompaProduAdmin", model);
                //ViewBag.ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/{(Session["UsuarioAutenticado"] as Usuario).Id}");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            //return PartialView("_ListaMesas", ViewBag.ListMesasActivas);
            //Pendiente factura
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> ActualizarProductoAdminRestaurante(Producto model)
        {

            var resultado = new object();
            try
            {

                resultado = await PostAsync<Producto, string>("Pos/ActualizarProductoAdminRestaurante", model);
                //ViewBag.ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/{(Session["UsuarioAutenticado"] as Usuario).Id}");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            //return PartialView("_ListaMesas", ViewBag.ListMesasActivas);
            //Pendiente factura
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ActualizarTablaAompa(List<Acompanamiento> ListaAcompa)
        {

            var resultado = new object();
            try
            {

                return PartialView("../AdminRestaurante/_AcompaSeleccion", ListaAcompa);
                //ViewBag.ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/{(Session["UsuarioAutenticado"] as Usuario).Id}");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            //return PartialView("_ListaMesas", ViewBag.ListMesasActivas);
            //Pendiente factura
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> ActualizarMesas()
        {

            var resultado = new object();
            try
            {
                ViewBag.ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/{(Session["UsuarioAutenticado"] as Usuario).Id}");
                return PartialView("_ListaMesas", ViewBag.ListMesasActivas);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }

        [HttpPost]
        public async Task<ActionResult> CerrarMesa(int IdPedido)
        {

            var resultado = new object();
            try
            {
                var model = await GetAsync<string>($"Puntos/CerrarMesa/{IdPedido}");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }
        [HttpPost]
        public async Task<ActionResult> ListarMesasZona(int IdZona)
        {

            var resultado = new object();
            try
            {
                var ListMesas = await GetAsync<IEnumerable<Mesa>>("Puntos/ObtenerMesas");
                if (ListMesas == null)
                {
                    ListMesas = new List<Mesa>();
                }
                else
                {
                    ListMesas = ListMesas.Where(l => (l.IdZona == IdZona)).ToList();
                }
                return Json(ListMesas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }
        [HttpPost]
        public async Task<ActionResult> AnularPedido(int IdPedido)
        {

            var resultado = new object();
            try
            {
                var model = await GetAsync<string>($"Puntos/AnularPedido/{IdPedido}");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }

        public async Task<ActionResult> Acompa(int IdProducto, int IdMesa)
        {
            var ListMesas = await GetAsync<IEnumerable<Mesa>>("Puntos/ObtenerMesas");
            ViewBag.BanderaTipoMesa = "";
            if (ListMesas != null)
            {
                ListMesas = ListMesas.Where(l => ((l.IdTipo == 2) && (l.Id == IdMesa))).ToList();
                if (ListMesas.Count() > 0)
                {
                    ViewBag.BanderaTipoMesa = "checked";
                }
            }

            var model = await GetAsync<IEnumerable<AcompanamientoXProducto>>($"Puntos/ObtenerAcompaXproducto/{IdProducto}");
            if (model != null)
            {
                var ListarTipoAcompGroup = await GetAsync<IEnumerable<TipoAcompanamiento>>("Puntos/ListarTipoAcompGroup");
                if (ListarTipoAcompGroup == null)
                {
                    ViewBag.ListarTipoAcompGroup = new List<TipoAcompanamiento>();
                }
                else
                {
                    ViewBag.ListarTipoAcompGroup = ListarTipoAcompGroup;
                }
                return PartialView("_Acompa", model);
            }
            else
            {
                model = new List<AcompanamientoXProducto>();
                ViewBag.ListarTipoAcompGroup = new List<TipoAcompanamiento>();
                return PartialView("_Acompa", model);
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
        [HttpPost]
        public async Task<ActionResult> ListarProductosFactura(int IdFactura)
        {
            var model = await GetAsync<ListaAcomProductos>($"Puntos/ListarProductosFactura/{IdFactura}");
            if (model != null)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(model, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult ImprimirPrefactura(List<Producto> ListaProductos, string IdMesa, string Total)
        {
            ServicioImprimir Imprimir = new ServicioImprimir();
            TicketImprimir objTicket = new TicketImprimir();
            bool Correcto = false;

            try
            {
                objTicket.TituloRecibo = IdMesa;
                objTicket.TituloColumnas = "Producto|Cant|Precio";
                objTicket.Usuario = string.Concat((Session["UsuarioAutenticado"] as Usuario).Nombre, " ", (Session["UsuarioAutenticado"] as Usuario).Apellido);
                objTicket.ListaArticulos = new List<Articulo>();
                foreach (var producto in ListaProductos)
                {
                    objTicket.ListaArticulos.Add(new Articulo()
                    {
                        Nombre = producto.Nombre,
                        Cantidad = producto.Cantidad,
                        Precio = producto.Precio
                    });
                }
                //throw new NullReferenceException();
                Imprimir.ImprimirTicketPreFactura(objTicket);
                Correcto = true;
            }
            catch (Exception e)
            {
                Correcto = false;
            }

            return Json(Correcto, JsonRequestBehavior.AllowGet);

        }
    }
}
