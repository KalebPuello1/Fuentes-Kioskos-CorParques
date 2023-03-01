using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Ventas.Core.Models;
using CorParques.Transversales.Util;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    /// <summary>
    /// Sistema de punto de venta
    /// </summary>
    public class PosController : ControladorBase
    {
        public string RutaInstaladorRedeban = ConfigurationManager.AppSettings["RutaInstaladorRedeban"].ToString();
        public string NombreArchivoSolicitudRedeban = ConfigurationManager.AppSettings["NombreArchivoSolicitudRedeban"].ToString();
        public string NombreArchivoRespuestaRedeban = ConfigurationManager.AppSettings["NombreArchivoRespuestaRedeban"].ToString();
        public string RutaEjecutableCajasRedeban = ConfigurationManager.AppSettings["RutaEjecutableCajasRedeban"].ToString();
        public async Task<ActionResult> Index()
        {
          
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

            var ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/null");
            if (ListMesasActivas == null)
            {
                ViewBag.ListMesasActivas = new List<Mesa>();
            }
            else
            {
                ViewBag.ListMesasActivas = ListMesasActivas.Where(l => (l.Id_Factura == 0) && (l.EstadoMesa == 2)).ToList();
            }




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

            var _mnPos = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapTipoProdMNPOS");
            if (_mnPos == null)
            {
                Parametro _conMN = new Parametro();
                _conMN.Valor = "2045";
                _mnPos = _conMN;
            }

            var _arazaPos = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapTipoProdArazaPOS");
            if (_arazaPos == null)
            {
                Parametro _conARAZAPOS = new Parametro();
                _conARAZAPOS.Valor = "2065";
                _arazaPos = _conARAZAPOS;
            }


            _aybProp.Valor += "," + _mnPos.Valor + "," + _arazaPos.Valor;


            if (_aybProp.Valor.Contains(","))
            {
                var divaybProp = _aybProp.Valor.Split(',');
                List<List<Producto>> listaAYB = new List<List<Producto>>();
                foreach (var item in divaybProp)
                {
                    listaAYB.Add(_ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == item)
                    ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == item).ToList()
                    : new List<Producto>());
                }
                List<Producto> ayb = new List<Producto>();
                if (listaAYB != null)
                {
                    foreach (var item in listaAYB)
                    {
                        foreach (var i in item)
                        {
                            ayb.Add(i);
                        }
                    }
                }
                ViewBag.ListAyB = ayb;
            }
            else
            {
                ViewBag.ListAyB = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _aybProp.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _aybProp.Valor).ToArray()
                : new List<Producto>().ToArray();
            }
            /*ViewBag.ListAyB = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _aybProp.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _aybProp.Valor).ToArray()
                : new List<Producto>().ToArray();*/

            /*ViewBag.ListAyB = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _aybProp.Valor || l.CodSapTipoProducto == "2025")
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _aybProp.Valor || l.CodSapTipoProducto == "2025").ToArray()
                : new List<Producto>().ToArray();*/
            //var list = _ListaTodosProductosSAP.Where(e => e.Nombre == "CEBOLLA PRODUCCIÓN MN").First();

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

            var _expVisitantesPos = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapTipoProdExpVisitantePOS");
            if (_expVisitantesPos == null)
            {
                Parametro _conEXPPOS = new Parametro();
                _conEXPPOS.Valor = "2070";
                _expVisitantesPos = _conEXPPOS;
            }
            var _terapiaEquPos = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapTipoProdTerapiasEquPOS");
            if (_terapiaEquPos == null)
            {
                Parametro _conTerEquiPOS = new Parametro();
                _conTerEquiPOS.Valor = "2075";
                _terapiaEquPos = _conTerEquiPOS;
            }
            var _servicios = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/Servicios_ventaPOS");
            if (_servicios == null)
            {
                Parametro _conSerPOS = new Parametro();
                _conSerPOS.Valor = "2035";
                _servicios = _conSerPOS;
            }

            _servicios.Valor += "," + _terapiaEquPos.Valor + "," + _expVisitantesPos.Valor;



            //var _ListServicios = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{_csServicios.CodigoSap}");
            /*ViewBag.ListServicios = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _servicios.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _servicios.Valor).ToArray()
                : new List<Producto>().ToArray();*/
            //var divservicios = _servicios.Valor.Split(',');
            if (_servicios.Valor.Contains(","))
            {
                var divservicios = _servicios.Valor.Split(',');
                List<List<Producto>> listaServicios = new List<List<Producto>>();
                foreach (var item in divservicios)
                {
                    listaServicios.Add(_ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == item)
                    ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == item).ToList()
                    : new List<Producto>());
                }
                List<Producto> Servicios = new List<Producto>();
                if (listaServicios != null)
                {
                    foreach (var item in listaServicios)
                    {
                        foreach (var i in item)
                        {
                            Servicios.Add(i);
                        }
                    }
                }
                ViewBag.ListServicios = Servicios;
            }
            else
            {
                ViewBag.ListServicios = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _servicios.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _servicios.Valor).ToArray()
                : new List<Producto>().ToArray();
            }

            /*ViewBag.ListServicios = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _servicios.Valor || l.CodSapTipoProducto == "2075")
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _servicios.Valor || l.CodSapTipoProducto == "2075").ToArray()
                : new List<Producto>().ToArray();*/

            /*foreach (var item in divservicios)
            {
                ViewBag.ListServicios += _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == item)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == item).ToArray()
                : new List<Producto>().ToArray();
            }*/
            //var list = _ListaTodosProductosSAP.Where(e => e.Nombre == "VALORACION EQUINOTERAPIA").First();
            var _pasaporte = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/Uso_PasaportePOS");

            //var _ListPasaporte = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{_csPasaporte.CodigoSap}");
            Producto[] pasaportes = _ListaTodosProductosSAP.Any(l => l.CodSapTipoProducto == _pasaporte.Valor)
                ? _ListaTodosProductosSAP.Where(l => l.CodSapTipoProducto == _pasaporte.Valor).ToArray()
                : new List<Producto>().ToArray();

            //var _pasaportesPunto = await GetAsync<List<PuntoBrazalete>>($"Pos/ObtenerPasaporteXPuntoPOS/{IdPunto}/{(Session["UsuarioAutenticado"] as Usuario).Id}");
            int pto = 262;
            int sess = 85;
            var _pasaportesPunto = await GetAsync<List<PuntoBrazalete>>($"Pos/ObtenerPasaporteXPuntoPOS/{pto}/{sess}");
            List<Producto> pasaportesImpresion = new List<Producto>();
            if (_pasaportesPunto != null && _pasaportesPunto.Count > 0)
            {
                _pasaportesPunto.ForEach(x =>
                {
                    Producto producto = _ListaTodosProductosSAP.Where(l => l.IdProducto == x.IdProducto).FirstOrDefault();
                    if (producto != null)
                    {
                        producto.AplicaImpresionLinea = true;
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
            var BanderaFlujoRedeban = await GetAsync<string>($"Redeban/FlujoRedebanXPunto/{IdPunto}");
            ViewBag.TransaccionRedebanHabilitada = BanderaFlujoRedeban;
            if (BanderaFlujoRedeban == "1")
            {
                bool InstaladorRedeban = System.IO.File.Exists(RutaEjecutableCajasRedeban);
                bool ArchivoSolicitudRedeban = System.IO.File.Exists(RutaInstaladorRedeban + NombreArchivoSolicitudRedeban);
                bool ArchivoRespuestaRedeban = System.IO.File.Exists(RutaInstaladorRedeban + NombreArchivoRespuestaRedeban);
                bool EjecutableCajasRedeban = System.IO.File.Exists(RutaEjecutableCajasRedeban);
                if (InstaladorRedeban == false || ArchivoSolicitudRedeban == false || ArchivoRespuestaRedeban == false || EjecutableCajasRedeban == false)
                {
                    ViewBag.TransaccionRedebanHabilitada = "0";
                }
            }

            _time.Stop();

            int segundos = _time.Elapsed.Seconds;

            return View(ViewBag.ListAyB);
        }

        public async Task<ActionResult> ActualizarMesas()
        {

            var resultado = new object();
            try
            {
                var ListMesasActivas = await GetAsync<IEnumerable<Mesa>>($"Puntos/ObtenerMesasActivas/null");
                if (ListMesasActivas == null)
                {
                    ViewBag.ListMesasActivas = new List<Mesa>();
                }
                else
                {
                    ViewBag.ListMesasActivas = ListMesasActivas.Where(l => (l.Id_Factura == 0) && (l.EstadoMesa == 2)).ToList();
                }
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
        public async Task<ActionResult> ObtenerBrazalete(string CodBarra)
        {
            Producto _producto = new Producto();
            var _prod = await GetAsync<Producto>($"Pos/ObtenerBrazaleteConsecutivo/{CodBarra}/{(Session["UsuarioAutenticado"] as Usuario).Id}/0");
            if (_prod != null)
            {
                _producto = _prod;
                //DANR: Adicion de validacion para tarjeta recargable y poder identificarlo
                Parametro _prdTarjetaRecargable = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapTarjetaRecargable");
                if (_producto.CodigoSap == _prdTarjetaRecargable.Valor)
                {
                    _producto.DataExtension = "TR";
                }
            }
            else
            {
                return Json(_producto, JsonRequestBehavior.AllowGet);
            }

            return Json(_producto, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ObtenerTarjetaRecargable(string CodBarra, int accion)
        {
            Producto _producto = new Producto();
            var _prod = await GetAsync<Producto>($"Pos/ObtenerBrazaleteConsecutivo/{CodBarra}/{(Session["UsuarioAutenticado"] as Usuario).Id}/{accion}");
            if (_prod != null)
            {
                _producto = _prod;
            }

            return Json(_producto, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ObtenerProducto(int idProducto)
        {
            Producto _producto = new Producto();
            var _prod = await GetAsync<Producto>($"Pos/ObtenerProducto/{idProducto}");
            if (_prod != null)
                _producto = _prod;

            return Json(_producto, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> ValidarDocumento(string doc)
        {
            return await GetAsync<string>($"TarjetaRecargable/ValidarDocumento/{doc}");
        }

        //Obtener última factura
        public async Task<JsonResult> ObtenerUltimaFacturaPunto()
        {
            var rta = await GetAsync<Factura>($"Pos/ObtenerUltimaFactura/{IdPunto}");
            var factura = new Factura();
            if (rta != null)
                factura = rta;

            return Json(factura, JsonRequestBehavior.AllowGet);
        }

        //Obtener detalle para la descarga
        [HttpPost]
        public ActionResult ObtenerDetalleEntrega(List<Producto> listaProductos)
        {
            return PartialView("_DescargoProducto", listaProductos.Where(x => !x.Entregado).ToList());
        }

        [HttpPost]
        public async Task<ActionResult> PagarCompra(int? IdPedido, List<Producto> ListaProductos
                                                    , List<PagoFacturaMediosPago> MediosPago
                                                    , string CodSapConvenio = ""
                                                    , string ConsecutivoConvenio = ""
                                                    , int IdConvenio = 0
                                                    , string Donante = ""
                                                    )
        {
            //Esto es validacion para impresion en linea
            RespuestaViewModel resultado = new RespuestaViewModel();
            List<Producto> _listaDescarga = new List<Producto>();
            string numBoletasRestantes = "";
            string MensajeBoletasRestantes = "";
            int compararCantidad = 0;
            int CantidadBoletasRestantes = 0;
            try
            {
                int restantesBoletas = 0;
                int restaNumBoletas = 0;
                int numBoletasImprimir = 0;
                int num = 0;
                string productoBoleta = "ImpresionEnLinea";
                var f = 1;
                bool tienen = false;

                List<Producto> lBoletas = new List<Producto>();
                List<Producto> listaLimite = new List<Producto>();
                Producto p = new Producto();
                p.CodigoSap = "40000414";
                listaLimite.Add(p);
                foreach (var item in ListaProductos)
                {

                    //if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2005" || item.CodSapTipoProducto == "2055" && item.AplicaImpresionLinea && item.CodigoSap == "40000414")
                    //if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2005" || item.CodSapTipoProducto == "2055" && item.AplicaImpresionLinea && listaLimite.Exists(x => x.CodigoSap == item.CodigoSap))
                    if (item.CodSapTipoProducto == "2000" && item.AplicaImpresionLinea && listaLimite.Exists(x => x.CodigoSap == item.CodigoSap))
                    {
                        numBoletasRestantes = await GetAsync<string>($"Pos/ConsultarControlBoleteria/{item.IdProducto}");
                        restantesBoletas = numBoletasRestantes == null ? 0 : int.Parse(numBoletasRestantes);
                        MensajeBoletasRestantes = " ** Impresion en linea ** ";
                        if (numBoletasRestantes != null)
                        {
                            CantidadBoletasRestantes = numBoletasRestantes == null ? 0 : int.Parse(numBoletasRestantes);
                            compararCantidad = ListaProductos.Where(e => e.CodigoSap == item.CodigoSap).Count();

                            if (ListaProductos.Exists(x => compararCantidad > CantidadBoletasRestantes && x.AplicaImpresionLinea))
                            {
                                item.existe = false;
                            }
                            else
                            {
                                item.existe = true;
                            }
                        }
                        else
                        {
                            item.existe = false;
                        }
                    }
                    else
                    {
                        if (item.CodSapTipoProducto == "2000")
                        {
                            item.existe = true;
                            //item.AplicaImpresionLinea = true;
                        }
                        else
                        {
                            item.existe = true;
                            item.AplicaImpresionLinea = false;
                        }
                    }
                }



                //if ((!ListaProductos.Exists(e => e.existe == false) && ListaProductos.Exists(e => e.AplicaImpresionLinea)) || (!ListaProductos.Exists(e => e.AplicaImpresionLinea) && ListaProductos.Exists(e => e.existe)))
                if ((ListaProductos.Exists(e => e.existe != false && e.AplicaImpresionLinea)) || (ListaProductos.Exists(e => !e.AplicaImpresionLinea && e.existe)))
                {
                    ///
                    //
                    double totalproductos = 0;
                    double totalBono = 0;
                    bool banderabono = false;
                    foreach (var item in MediosPago.Where(x => x.IdMedioPago == 4).ToList())
                    {
                        totalBono = totalBono + item.Valor;

                    }
                    if (totalBono > 0)
                    {
                        foreach (var item2 in ListaProductos)
                        {
                            totalproductos = totalproductos + item2.Precio;
                        }
                        if (totalBono >= totalproductos)
                        {
                            banderabono = true;
                        }
                    }

                    var idUsuario = IdUsuarioLogueado;

                    PagoFactura model = new PagoFactura
                    {
                        IdUsuario = idUsuario,
                        listaProducto = ListaProductos,
                        listMediosPago = MediosPago,
                        IdPunto = IdPunto,
                        CodSapConvenio = CodSapConvenio,
                        ConsecutivoConvenio = ConsecutivoConvenio,
                        IdConvenio = IdConvenio,
                        EsContingencia = Contingencia,
                        BanderaBonoRegalo = banderabono
                    };


                    //DANR: 22-01-2019 *** Adicion de campo donante
                    if (!string.IsNullOrEmpty(Donante))
                        model.Donante = Donante;
                    //fin DANR: 22-01-2019 *** Adicion de campo donante

                    if (!Contingencia)
                        resultado = await PostAsync<PagoFactura, string>("Pos/ValidarCompra", model);

                    //La contigencia no aplica para parqueadero 
                    string Nombre = "CodSapProdParqueadero";
                    var SapParqueadero = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/{Nombre}");
                    var idProductoDescarga = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodigoSapTipoProductoAyB"); //Tipo producto alimentos y bebidas
                    var idProductoSouvenir = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/SouveniresPOS"); // TipoProducto Souvenir
                    var idProductoServicios = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodigoSapTipoProductoServicios"); //Tipo producto paquetero
                    var idProductoParqueadero = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodigoSapTipoProductoParqueadero"); // parqueadero

                    if (resultado.Elemento == null || resultado.Elemento.ToString() == "")
                    {
                        IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");

                        if (ListaProductos.Exists(x => x.AplicaImpresionLinea))
                        {
                            //calcular codigo impresion en linea
                            foreach (var item in model.listaProducto.Where(x => x.AplicaImpresionLinea).ToList())
                            {
                                Producto producto = brazaletes.Where(x => x.IdProducto == item.IdProducto).First();
                                if (producto != null)
                                {


                                    producto.IdUsuarioModificacion = IdUsuarioLogueado;
                                    var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", producto);


                                    if (!respuesta.Correcto)
                                        throw new ArgumentException(respuesta.Mensaje);


                                    if (!respuesta.Elemento.ToString().Contains("Error"))
                                    {

                                        var idBoleteria = int.Parse(respuesta.Elemento.ToString().Split('|')[0]);
                                        var consecutivo = respuesta.Elemento.ToString().Split('|')[1];
                                        item.CodBarraInicio = consecutivo;
                                        item.IdDetalleProducto = idBoleteria;
                                    }
                                }
                            }
                        }
                        ///Descomentar

                        resultado = await PostAsync<PagoFactura, string>("Pos/InsertarCompra", model);

                        //Validación excepción insertar factura
                        if (!resultado.Correcto)
                            throw new ArgumentException(resultado.Mensaje);
                        if (!Contingencia)
                        {
                            Parametro tipospuntoDescargue = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdTiposPuntoDescargue");

                            var punto = await GetAsync<Puntos>($"Puntos/GetById/{IdPunto}");
                            string[] strsplit;
                            bool existepunto = false;
                            strsplit = tipospuntoDescargue.Valor.Split(',');
                            for (int i = 0; i < strsplit.Length; i++)
                            {
                                if (punto.IdTipoPunto.ToString() == strsplit[i].ToString())
                                {
                                    existepunto = true;
                                    break;
                                }
                            }
                            if (existepunto)
                            {
                                _listaDescarga = ListaProductos.Where(x => (idProductoDescarga.Valor.Contains(x.CodSapTipoProducto) && x.Entregado)
                                                                      || (x.IdDetalleProducto > 0 && x.CodigoSap != SapParqueadero.Valor)
                                                                      || (x.CodSapTipoProducto == idProductoSouvenir.Valor && x.Entregado)).ToList(); //// AYB o boletas preimpresas
                                //_listaDescarga = ListaProductos.Where(x => (x.CodSapTipoProducto == idProductoDescarga.Valor.Split(',')[0] && x.Entregado)
                                //|| (x.CodSapTipoProducto == idProductoDescarga.Valor.Split(',')[1] && x.Entregado)
                                //|| (x.IdDetalleProducto > 0 && x.CodigoSap != SapParqueadero.Valor)
                                //|| (x.CodSapTipoProducto == idProductoSouvenir.Valor && x.Entregado)).ToList(); //// AYB o boletas preimpresas
                            }
                            else
                            {
                                _listaDescarga = ListaProductos.Where(x => x.IdDetalleProducto > 0 && x.CodigoSap != SapParqueadero.Valor).ToList(); //// Solo boletas preimpresas
                            }

                            //retirar brazaletes impresion en linea
                            //_listaDescarga = _listaDescarga.Where(x => x.AplicaImpresionLinea == false).ToList();
                            _listaDescarga = _listaDescarga.Where(x => x.AplicaImpresionLinea == false || x.AplicaImpresionLinea == true).ToList();

                            if (resultado != null && resultado.Elemento.ToString().Length > 0 && _listaDescarga.Count() > 0)
                            {
                                Inventario inventario = new Inventario();
                                inventario.FechaInventario = Utilidades.FechaActualColombia;
                                inventario.IdPunto = IdPunto;
                                inventario.IdUsuarioCeado = IdUsuarioLogueado;
                                inventario.Productos = _listaDescarga; // AYB 
                                await PostAsync<Inventario, string>("Inventario/ActualizarInventario", inventario);

                                if (ListaProductos.ToList().Exists(x => x.CodSapTipoProducto == "2000" && x.AplicaImpresionLinea))
                                {
                                    ImpresionEnLinea impresionRegistroRollo = new ImpresionEnLinea();
                                    impresionRegistroRollo.listaProductos = ListaProductos;
                                    impresionRegistroRollo.listaAdicionPedidos = null;
                                    try
                                    {
                                        await registrarRolloInventario(impresionRegistroRollo, IdUsuarioLogueado);
                                    }
                                    catch (Exception E)
                                    {
                                        var ERROR = "MAL";
                                    }
                                }
                            }
                        }
                        string error = "";
                        /*++Impresion++*/
                        try
                        {
                            var facturas = resultado.Elemento.ToString().Split('|');
                            foreach (var idFactura in facturas)
                            {
                                //int n = 3546716;
                                FacturaImprimir resultadoFacturaImprimir = await GetAsync<FacturaImprimir>($"Pos/ObtenerFacturaImprimir/{idFactura}");
                                //FacturaImprimir resultadoFacturaImprimir = await GetAsync<FacturaImprimir>($"Pos/ObtenerFacturaImprimir/{n}");
                                Factura facturaDetalleImprimir = await GetAsync<Factura>($"Pos/ObtenerFacturaById/{idFactura}");
                                //Factura facturaDetalleImprimir = await GetAsync<Factura>($"Pos/ObtenerFacturaById/{n}");

                                if (IdPedido != null)
                                {
                                    PagoFactura updatePedido = new PagoFactura();
                                    Int64 factnum = Int64.Parse(idFactura);
                                    updatePedido.IdFactura = factnum;
                                    updatePedido.IdPedido = IdPedido;
                                    resultado = await PostAsync<PagoFactura, string>("Pos/UpdatePedidoRestFactura", updatePedido);

                                }
                                //Guardar Base local contingencia

                                var blnActivoContigencia = ConfigurationManager.AppSettings["Contingencia"] == null ? false
                                                           : Convert.ToBoolean(Convert.ToInt32(ConfigurationManager.AppSettings["Contingencia"].ToString()));

                                if (resultadoFacturaImprimir != null && blnActivoContigencia)
                                {
                                    var _logVentaPunto = new LogVentaPunto();
                                    _logVentaPunto.Fecha = DateTime.Now;
                                    _logVentaPunto.Idpunto = IdPunto;
                                    _logVentaPunto.IdTaquillero = IdUsuarioLogueado;
                                    _logVentaPunto.CodigoFactura = resultadoFacturaImprimir.CodigoFactura;
                                    await PostAsyncLocal<LogVentaPunto, string>("Pos/GuardarLogVentaContingencia", _logVentaPunto);
                                }

                                ServicioImprimir objImprimir = new ServicioImprimir();
                                double ValorTotal = 0;
                                foreach (var productos in ListaProductos)
                                {
                                    ValorTotal += productos.PrecioTotal;
                                }

                                if (ValorTotal > 0)
                                {
                                    var respImprimir = objImprimir.ImprimirTicketPosFactura(resultadoFacturaImprimir);
                                    if (!string.IsNullOrEmpty(respImprimir))
                                    {
                                        Utilidades.RegistrarError(new Exception("Error al imprimir: " + respImprimir) { }, "Pos/PagarCompra-ImprimirTicketPosFactura");
                                    }
                                    else
                                    {
                                        var punto = await GetAsync<Puntos>($"Puntos/GetById/{IdPunto}");
                                        if ((punto.DobleFactura == true) && (resultado != null && resultado.Elemento.ToString().Length > 0 && _listaDescarga.Count() > 0))
                                        {
                                            respImprimir = objImprimir.ImprimirTicketPosFactura(resultadoFacturaImprimir);
                                        }
                                    }
                                }

                                Parametro aplicaImprersionParqueadero = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AplicaImpresionParqueadero");

                                if (aplicaImprersionParqueadero.Valor != "0")
                                {
                                    List<Producto> productoImpresionParqueadero = new List<Producto>();
                                    productoImpresionParqueadero = ListaProductos.Where(x => (x.CodSapTipoProducto == idProductoParqueadero.Valor)).ToList();

                                    foreach (var item in productoImpresionParqueadero)
                                    {

                                        if (productoImpresionParqueadero != null)
                                        {
                                            string sCodBarrasPark = item.IdDetalleProducto.ToString("000000000000");
                                            sCodBarrasPark = sCodBarrasPark + Utilidades.GenerarDigitoEAN13(sCodBarrasPark).ToString();
                                            TicketImprimir objServicios = new TicketImprimir();
                                            objServicios.TituloRecibo = "Boleta de Parqueadero";
                                            objServicios.CodigoBarrasProp = sCodBarrasPark;
                                            objServicios.TituloColumnas = "Valido para|Cant";
                                            objServicios.ListaArticulos = new List<Articulo>();
                                            objServicios.ListaArticulos.Add(new Articulo()
                                            {
                                                //Nombre = productoImpresionDescarga.Nombre,
                                                Nombre = item.Nombre,
                                                Cantidad = item.Cantidad,
                                                Precio = item.Precio,
                                                TituloColumnas = "Valido para|Cant"
                                            });

                                            objImprimir.ImprimirCupoDebito(objServicios);
                                            error = "PARQUEADERO";
                                        }
                                    }
                                }
                                Parametro aplicaImprersionServicios = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AplicaImpresionServicios");  //Servicio paquetero

                                if (aplicaImprersionServicios.Valor != "0")
                                {
                                    List<Producto> productoImpresionServicios = new List<Producto>();
                                    productoImpresionServicios = ListaProductos.Where(x => (x.CodSapTipoProducto == idProductoServicios.Valor)).ToList();

                                    foreach (var item in productoImpresionServicios)
                                    {
                                        if (productoImpresionServicios != null)
                                        {
                                            TicketImprimir objServicios = new TicketImprimir();
                                            objServicios.TituloRecibo = "Boleta de Servicios";
                                            objServicios.TituloColumnas = "Valido para|Cant";
                                            objServicios.ListaArticulos = new List<Articulo>();
                                            objServicios.ListaArticulos.Add(new Articulo()
                                            {
                                                //Nombre = productoImpresionDescarga.Nombre,
                                                Nombre = item.Nombre,
                                                Cantidad = item.Cantidad,
                                                Precio = item.Precio,
                                                TituloColumnas = "Valido para|Cant"
                                            });

                                            objImprimir.ImprimirCupoDebito(objServicios);
                                            error = "SERVICIO";
                                        }
                                    }
                                }

                                aplicaImprersionParqueadero = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AplicaImpresionParqueadero");

                                if (aplicaImprersionParqueadero.Valor != "0")
                                {
                                    List<Producto> productoImpresionParqueadero = new List<Producto>();
                                    productoImpresionParqueadero = ListaProductos.Where(x => (x.CodSapTipoProducto == idProductoParqueadero.Valor)).ToList();

                                    foreach (var item in productoImpresionParqueadero)
                                    {

                                        if (productoImpresionParqueadero != null)
                                        {
                                            string sCodBarrasPark = item.IdDetalleProducto.ToString("000000000000");
                                            sCodBarrasPark = sCodBarrasPark + Utilidades.GenerarDigitoEAN13(sCodBarrasPark).ToString();
                                            TicketImprimir objServicios = new TicketImprimir();
                                            objServicios.TituloRecibo = "Boleta de Parqueadero";
                                            objServicios.CodigoBarrasProp = sCodBarrasPark;
                                            objServicios.TituloColumnas = "Valido para|Cant";
                                            objServicios.ListaArticulos = new List<Articulo>();
                                            objServicios.ListaArticulos.Add(new Articulo()
                                            {
                                                //Nombre = productoImpresionDescarga.Nombre,
                                                Nombre = item.Nombre,
                                                Cantidad = item.Cantidad,
                                                Precio = item.Precio,
                                                TituloColumnas = "Valido para|Cant"
                                            });

                                            objImprimir.ImprimirCupoDebito(objServicios);
                                            error = "PARQUEADERO";
                                        }
                                    }
                                }

                                Parametro aplicaImprersionAyB = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AplicaImpresionAyB");

                                if (aplicaImprersionAyB.Valor != "0")
                                {
                                    foreach (var item in facturaDetalleImprimir.DetalleFactura)
                                    {
                                        Producto productoImpresionDescarga = new Producto();
                                        productoImpresionDescarga = ListaProductos.Where(x => (idProductoDescarga.Valor.Contains(x.CodSapTipoProducto) && x.Entregado == false && x.IdProducto == item.Id_Producto)).FirstOrDefault();
                                        //productoImpresionDescarga = ListaProductos.Where(x => (x.CodSapTipoProducto == idProductoDescarga.Valor.Split(',')[0] && x.Entregado == false && x.IdProducto == item.Id_Producto)
                                        //                                                   || (x.CodSapTipoProducto == idProductoDescarga.Valor.Split(',')[1] && x.Entregado == false && x.IdProducto == item.Id_Producto)).FirstOrDefault();

                                        if (productoImpresionDescarga != null)
                                        {
                                            TicketImprimir objAlimento = new TicketImprimir();
                                            objAlimento.TituloRecibo = "Boleta de A&B";
                                            objAlimento.CodigoBarrasProp = string.Concat(item.IdDetalleFactura);
                                            objAlimento.TituloColumnas = "Valido para|Cant";
                                            objAlimento.ListaArticulos = new List<Articulo>();
                                            objAlimento.ListaArticulos.Add(new Articulo()
                                            {
                                                Nombre = productoImpresionDescarga.Nombre,
                                                Cantidad = item.Cantidad,
                                                Precio = item.Precio,
                                                TituloColumnas = "Valido para|Cant"
                                            });
                                            objImprimir.ImprimirCupoDebito(objAlimento);
                                            error = "AYB";
                                        }
                                    }
                                }


                                /*Nueva impresion Souvenir*/
                                Parametro aplicaImprersionSouvenir = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/aplicaImpresionSouvenir");

                                if (aplicaImprersionSouvenir.Valor != "0")
                                {
                                    foreach (var item in facturaDetalleImprimir.DetalleFactura)
                                    {
                                        Producto productoImpresionDescarga = new Producto();
                                        //valor : 2025 --> idProducto: 685
                                        productoImpresionDescarga = ListaProductos.Where(x => (idProductoDescarga.Valor.Contains(x.CodSapTipoProducto) && x.Entregado == false && x.IdProducto == item.Id_Producto)).FirstOrDefault();

                                        if (productoImpresionDescarga != null)
                                        {
                                            TicketImprimir objAlimento = new TicketImprimir();
                                            objAlimento.TituloRecibo = "Souvenir";
                                            objAlimento.CodigoBarrasProp = string.Concat(item.IdDetalleFactura);
                                            objAlimento.TituloColumnas = "Valido para|Cant";
                                            objAlimento.ListaArticulos = new List<Articulo>();
                                            objAlimento.ListaArticulos.Add(new Articulo()
                                            {
                                                Nombre = productoImpresionDescarga.Nombre,
                                                Cantidad = item.Cantidad,
                                                Precio = item.Precio,
                                                TituloColumnas = "Valido para|Cant"
                                            });
                                            objImprimir.ImprimirCupoDebito(objAlimento);
                                            error = "Souvenir";
                                        }
                                    }
                                }


                                if (!Contingencia)
                                {
                                    /* Imprimir Cupo Debito*/
                                    objImprimir = new ServicioImprimir();
                                    foreach (string[] objBoleta in resultadoFacturaImprimir.BoleteriaAdicional)
                                    {

                                        if (objBoleta[4] == "1")
                                        {
                                            //Imprime formato de cupo debito.
                                            TicketImprimir objTicketCupo = new TicketImprimir();
                                            objTicketCupo.TituloRecibo = "Boleta de " + objBoleta[0];
                                            objTicketCupo.CodigoBarrasProp = objBoleta[3];
                                            objTicketCupo.TituloColumnas = "Valido para|Cant";
                                            objTicketCupo.ListaArticulos = new List<Articulo>();
                                            objTicketCupo.ListaArticulos.Add(new Articulo() { Nombre = objBoleta[1], Cantidad = 1, Precio = double.Parse(objBoleta[2]), TituloColumnas = "Valido para|Cant" });
                                            objImprimir.ImprimirCupoDebito(objTicketCupo);
                                            error = "AYB";
                                        }
                                        else
                                        {
                                            //Imprime formato de uso atracciones y destrezas.
                                            TicketImprimir objTicketCupo = new TicketImprimir();
                                            objTicketCupo.TituloRecibo = objBoleta[0];
                                            objTicketCupo.CodigoBarrasProp = objBoleta[3];
                                            objTicketCupo.TituloColumnas = "Valido para|Cant";
                                            objTicketCupo.ListaArticulos = new List<Articulo>();
                                            objTicketCupo.ListaArticulos.Add(new Articulo() { Nombre = objBoleta[1], Cantidad = int.Parse(objBoleta[5]), Precio = double.Parse(objBoleta[2]), TituloColumnas = "Valido para|Cant" });
                                            objTicketCupo.Usuario = NombreUsuarioLogueado;
                                            objImprimir.ImprimirUsoAtraccionDestreza(objTicketCupo);
                                        }
                                    }
                                    /* Imprimir recibo cupo empleado*/
                                    if (MediosPago.Where(x => x.IdMedioPago == (int)Enumerador.MediosPago.DescuentoNomina).Count() > 0)
                                    {
                                        //Se ajusta para nuevo formato de cupo de empleados GALD1
                                        var _ticketImprimir = new TicketImprimir()
                                        {
                                            TituloRecibo = "Autorización descuento por nómina",
                                            //ListaArticulos = new List<Articulo> { new Articulo { Nombre = MediosPago.First(x => x.IdMedioPago == (int)Enumerador.MediosPago.DescuentoNomina).NombreMedioPago,
                                            //    Cantidad = 1,
                                            //    Precio = double.Parse(resultadoFacturaImprimir.MetodosPago[0][1]),
                                            //    TituloColumnas = "Empleado|Cant" } },

                                            //Usuario = "",
                                            //Firma = "Firma: "
                                            Firma = string.Concat(MediosPago.First(x => x.IdMedioPago == (int)Enumerador.MediosPago.DescuentoNomina).NombreMedioPago)
                                        };

                                        objImprimir.ImprimirReciboCupoEmpleado(_ticketImprimir, resultadoFacturaImprimir);

                                    }

                                    if (!resultadoFacturaImprimir.CodigoFactura.StartsWith("DO"))
                                    {
                                        /* DANR: Imprimir recibo tarjeta recargable*/
                                        if (MediosPago.Where(x => x.IdMedioPago == (int)Enumerador.MediosPago.TarjetaRecargable).Count() > 0)
                                        {
                                            foreach (var item in MediosPago.Where(x => x.IdMedioPago == (int)Enumerador.MediosPago.TarjetaRecargable))
                                            {
                                                var _ticketImprimir = new TicketImprimir()
                                                {
                                                    TituloRecibo = "Soporte pago tarjeta recargable",
                                                };

                                                resultadoFacturaImprimir.MetodosPago = new List<string[]>();
                                                resultadoFacturaImprimir.MetodosPago.Add(new string[] { "TARJETA RECARGABLE", item.Valor.ToString() });
                                                objImprimir.ImprimirReciboTarjetaRecargable(_ticketImprimir, resultadoFacturaImprimir);
                                            }
                                            //Se ajusta para nuevo formato de cupo de empleados GALD1
                                        }
                                    }

                                    /* Imprimir recibo Bono regalo*/
                                    if (MediosPago.Where(x => x.IdMedioPago == (int)Enumerador.MediosPago.Bonosregalo).Count() > 0)
                                    {
                                        foreach (var item in MediosPago.Where(x => x.IdMedioPago == (int)Enumerador.MediosPago.Bonosregalo))
                                        {
                                            var _ticketImprimir = new TicketImprimir()
                                            {
                                                TituloRecibo = "Soporte pago Bono Regalo",
                                            };

                                            resultadoFacturaImprimir.MetodosPago = new List<string[]>();
                                            resultadoFacturaImprimir.MetodosPago.Add(new string[] { "TARJETA RECARGABLE", item.Valor.ToString() });
                                            objImprimir.ImprimirReciboTarjetaRecargable(_ticketImprimir, resultadoFacturaImprimir);
                                        }
                                    }

                                    /* Imprimir recibo Bono APP*/
                                    if (MediosPago.Where(x => x.IdMedioPago == (int)Enumerador.MediosPago.APP).Count() > 0)
                                    {
                                        foreach (var item in MediosPago.Where(x => x.IdMedioPago == (int)Enumerador.MediosPago.APP))
                                        {
                                            var _ticketImprimir = new TicketImprimir()
                                            {
                                                TituloRecibo = "Soporte pago APP",
                                            };

                                            resultadoFacturaImprimir.MetodosPago = new List<string[]>();
                                            resultadoFacturaImprimir.MetodosPago.Add(new string[] { "TARJETA RECARGABLE", item.Valor.ToString() });
                                            objImprimir.ImprimirReciboTarjetaRecargable(_ticketImprimir, resultadoFacturaImprimir);
                                        }
                                        //Se ajusta para nuevo formato de cupo de empleados GALD1
                                    }


                                    if (model.listaProducto.Exists(x => x.AplicaImpresionLinea))
                                    {

                                        //imprimir brazalete impresion en linea
                                        //foreach (var item in model.listaProducto.Where(x => x.AplicaImpresionLinea && !x.Nombre.Contains("Rollo")).ToList())
                                        foreach (var item in model.listaProducto.Where(x => x.AplicaImpresionLinea && !x.Nombre.Contains("Rollo")).ToList())
                                        {

                                            Producto producto = brazaletes.Where(x => x.IdProducto == item.IdProducto).First();
                                            if (producto != null && producto.ArchivoProducto != null)
                                            {
                                                string archivo = Path.Combine(Server.MapPath("~/Temp"), producto.ArchivoProducto.Archivo);
                                                WebClient webClient = new WebClient();
                                                webClient.DownloadFile(ConfigurationManager.AppSettings["RutaArchivos"].ToString() + producto.ArchivoProducto.Archivo, archivo);
                                                //webClient.DownloadFile("http://localhost:62696/Archivos/Brazaletes/asd3.txt", archivo);

                                                StringBuilder contenidoEtiqueta = new StringBuilder();
                                                contenidoEtiqueta.Append(System.IO.File.ReadAllText(archivo, Encoding.GetEncoding(1252)));
                                                contenidoEtiqueta.Replace(ConfigurationManager.AppSettings["TagCodigoBarras"].ToString(), item.CodBarraInicio);

                                                //MEtodo de Impresión de brazalete
                                                string respuesta = PrintDirect.PrintText(contenidoEtiqueta.ToString(), 0);

                                                //Aqui se va a llevar el control
                                                restantesBoletas = --restantesBoletas < 0 ? 0 : restantesBoletas;
                                                if (item.CodigoSap == "40000414")
                                                {
                                                    MensajeBoletasRestantes += " *** ";

                                                    var respuestaDto = await GetAsync<string>($"Pos/UpdateControlBoleteria/{item.IdProducto}/{restantesBoletas}");
                                                    if (!MensajeBoletasRestantes.Contains(item.Nombre))
                                                    {
                                                        MensajeBoletasRestantes += respuestaDto;
                                                    }
                                                    else
                                                    {
                                                        MensajeBoletasRestantes = respuestaDto;
                                                    }
                                                }
                                                if (!string.IsNullOrWhiteSpace(respuesta))
                                                {
                                                    Utilidades.RegistrarError(new Exception("Error al imprimir"), respuesta);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Utilidades.RegistrarError(ex, error);
                            //Utilidades.RegistrarError(ex, "Pos/PagarCompra-Imprimir");
                        }
                    }
                }
                else
                {
                    resultado.Elemento = 00;
                    MensajeBoletasRestantes = "**** \n\r"; //+ productoBoleta + " para imprimir o esta excediendo el numero boletas existentes - Desea imprimir " + compararCantidad + " - disponibles " + restantesBoletas;
                    foreach (var item in ListaProductos.Where(e => e.existe == false))
                    {

                        if (item.CodigoSap == "40000414")
                        {
                            numBoletasRestantes = await GetAsync<string>($"Pos/ConsultarControlBoleteria/{item.IdProducto}");
                            int disponibles = numBoletasRestantes == null ? 0 : int.Parse(numBoletasRestantes);
                            if (!MensajeBoletasRestantes.Contains(item.Nombre))
                            {
                                MensajeBoletasRestantes += $"\n {item.Nombre} -  Desea imprimir {ListaProductos.Where(e => e.IdProducto == item.IdProducto).Count()} - disponibles {disponibles}. \n\r ";
                            }
                        }
                    }
                    MensajeBoletasRestantes += "\n\r ****   Verifique que no se acabo el limite de rollo";
                }
            }
            catch (Exception ex)
            {
                resultado.Elemento = "Ha ocurrido un error : " + ex.Message;
            }
            finally
            {
                _listaDescarga = null;
            }

            //Pendiente factura
            //return Json(resultado.Elemento.ToString().Split('|').Last(), JsonRequestBehavior.AllowGet);
            if (MensajeBoletasRestantes != null && MensajeBoletasRestantes != "")
            {
                return Json(new
                {
                    res = resultado.Elemento.ToString().Split('|').Last(),
                    impresionLinea = MensajeBoletasRestantes
                }, JsonRequestBehavior.AllowGet); ;
            }
            else
            {
                return Json(resultado.Elemento.ToString().Split('|').Last(), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<ActionResult> InsertarCortesias(
                                                    string Documento = ""
                                                    , string Nombre = ""
                                                    , string Correo = ""
                                                    , string Genero = "", string Telefono = "", string FechaCumple = "", string Direccion = "", string CodTarjetaFAN = "", string Foto = "")
        {

            try
            {


                UsuarioVisitanteViewModel usuario = new UsuarioVisitanteViewModel();
                List<Producto> listaProductosAgregar = new List<Producto>();
                Producto productoagregar = new Producto();
                var ParametrosTipoProdCortesia = await GetAsync<IEnumerable<Parametro>>("Parameters/ObtenerParametrosGlobales");
                Parametro objparametro = new Parametro();
                if (ParametrosTipoProdCortesia != null)
                {
                    objparametro = ParametrosTipoProdCortesia.Where(x => x.Nombre.Equals("CodSapproductoCortesiasFan")).FirstOrDefault();

                }

                productoagregar.CodigoSap = objparametro.Valor;
                productoagregar.Cantidad = 1;
                listaProductosAgregar.Add(productoagregar);

                usuario.NumeroDocumento = Documento;
                usuario.TipoDocumento = "Cédula de ciudadania";
                usuario.Nombres = Nombre;
                usuario.Correo = Correo;
                usuario.Telefono = Telefono;
                usuario.IdUsuarioCreacion = IdUsuarioLogueado;
                usuario.Activo = true;
                usuario.Aprobacion = false;
                usuario.FechaCreacion = System.DateTime.Now;
                usuario.listaProductosAgregar = listaProductosAgregar;
                usuario.Cantidad = 1;
                usuario.BanderaFan = 1;
                usuario.IdTipoCortesia = 2;
                usuario.NumTarjetaFAN = CodTarjetaFAN;
                DateTime fechaInicialSet = new DateTime();
                DateTime fechaFinalSet = new DateTime();
                var anomas = System.DateTime.Now.Year + 1;
                var mesActual = System.DateTime.Now.Month;
                if (FechaCumple != null)
                {
                    var MesCumple = Convert.ToDateTime(FechaCumple).Month;

                    var mesmas = MesCumple + 1;

                    if (MesCumple > mesActual)
                    {
                        fechaInicialSet = Convert.ToDateTime("01-" + "- " + MesCumple + "- " + System.DateTime.Now.Year);
                        fechaFinalSet = Convert.ToDateTime("01-" + "- " + mesmas + "- " + System.DateTime.Now.Year).AddDays(-1);
                    }
                    else
                    {
                        fechaInicialSet = Convert.ToDateTime("01" + "- " + MesCumple + "- " + anomas);
                        fechaFinalSet = Convert.ToDateTime("01" + "- " + mesmas + "- " + anomas).AddDays(-1);
                    }
                }
                usuario.DescripcionBeneficioFAN = "Cortesía cumpleaños";
                usuario.FechaInicialFan = fechaInicialSet;
                usuario.FechaFinalFan = fechaFinalSet;
                var respuestaCortesiaCumple = await PostAsync<UsuarioVisitanteViewModel, string>("Cortesia/InsertarUsuarioVisitante", usuario);
                usuario.BanderaFan = 2;
                var mesnino = 4;
                if (mesnino > System.DateTime.Now.Month)
                {
                    fechaInicialSet = Convert.ToDateTime("01" + "-04-" + System.DateTime.Now.Year);
                    fechaFinalSet = Convert.ToDateTime("30" + "-04-" + System.DateTime.Now.Year);
                }
                else
                {
                    fechaInicialSet = Convert.ToDateTime("01" + "-04-" + anomas);
                    fechaFinalSet = Convert.ToDateTime("30" + "-04-" + anomas);
                }
                usuario.DescripcionBeneficioFAN = "Cortesia día del niño";
                usuario.FechaInicialFan = fechaInicialSet;
                usuario.FechaFinalFan = fechaFinalSet;
                var respuestaCortesiaDianino = await PostAsync<UsuarioVisitanteViewModel, string>("Cortesia/InsertarUsuarioVisitante", usuario);
                usuario.BanderaFan = 3;
                var meshall = 10;
                if (meshall > System.DateTime.Now.Month)
                {
                    fechaInicialSet = Convert.ToDateTime("01" + "-10-" + System.DateTime.Now.Year);
                    fechaFinalSet = Convert.ToDateTime("31" + "-10-" + System.DateTime.Now.Year);
                }
                else
                {
                    fechaInicialSet = Convert.ToDateTime("01" + "-10-" + anomas);
                    fechaFinalSet = Convert.ToDateTime("31" + "-10-" + anomas);
                }
                usuario.DescripcionBeneficioFAN = "Cortesia Halloween";
                usuario.FechaInicialFan = fechaInicialSet;
                usuario.FechaFinalFan = fechaFinalSet;
                var respuestaCortesiaHallowen = await PostAsync<UsuarioVisitanteViewModel, string>("Cortesia/InsertarUsuarioVisitante", usuario);

                return Json(respuestaCortesiaHallowen, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {

                return Json("Se presento un error creando el usuario visitante. Por favor intentelo de nuevo", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Pos/DetallePagos
        public async Task<ActionResult> DetallePagos()
        {
            IEnumerable<TipoGeneral> _list = new List<TipoGeneral>();
            var _listMediosPago = await GetAsync<IEnumerable<TipoGeneral>>("MediosPago/GetAllSimple");
            if (_listMediosPago != null)
                _list = _listMediosPago;

            ViewBag.list = _list.ToList();

            return PartialView("_DetallePago");
        }


        public ActionResult ObtenerCupoDebito()
        {
            return PartialView("_CupoDebito");
        }

        public async Task<ActionResult> ObtenerTiqueteAtraccion()
        {
            var _rta = await GetAsync<IEnumerable<Puntos>>($"Puntos/GetAllxTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            //Retono los activos
            var lista = _rta.Where(x => x.Estado == Enumerador.Estados.Activo.ToString());
            return PartialView("_TiqueteAtraccion", lista);
        }

        public async Task<ActionResult> ObtenerPasaporteUso()
        {
            var _rta = await GetAsync<IEnumerable<PasaporteUso>>("PasaporteUso/ObtenerTodosPasaporte");
            //Retono los activos
            var lista = _rta.Where(x => x.Estado == Enumerador.Estados.Activo.ToString());

            return PartialView("_TiqueteIngresos", lista);
        }

        public async Task<ActionResult> ObtenerPasaporte(int IdPasaporte)
        {
            var _pasaporte = await GetAsync<PasaporteUso>($"PasaporteUso/Obtener/{IdPasaporte}");
            return Json(_pasaporte, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PagoParqueadero()
        {
            return PartialView("_PagoParqueadero");
        }

        // GET: Parqueadero/Salir
        [HttpPost]
        public async Task<JsonResult> PagoParqueadero(int idParqueadero)
        {

            ControlParqueadero model = new ControlParqueadero();
            model.CodUsuarioSalida = (Session["UsuarioAutenticado"] as Usuario).Id;
            model.Id = idParqueadero;

            var resultado = await PostAsync<ControlParqueadero, string>("ControlParqueadero/CalcularPago", model);

            if (resultado != null && resultado.Correcto)
            {
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultado);
            }

        }

        // GET: Pos/ValidaReservaParqueadero
        [HttpGet]
        public async Task<JsonResult> ValidaReservaParqueadero(string CodigoBarrasBoletaControl)
        {
            var resultado = await GetAsync<string>($"Pos/ValidaReservaParqueadero/{CodigoBarrasBoletaControl}");
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerDetalleConvenio(int IdConvenio)
        {
            var resultado = await GetAsync<IEnumerable<ConvenioDetalle>>($"ConvenioSAP/ObtenerDetalleConvenio/{IdConvenio}");
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerDetalleConvenioByConsec(string Consecutivo)
        {
            var resultado = await GetAsync<IEnumerable<ConvenioDetalle>>($"ConvenioSAP/ObtenerDetalleConvenioByConsec/{Consecutivo}");

            var Empleado = await GetAsync<EstructuraEmpleado>($"Pos/ObtenerEmpleadoPorConsecutivo/{Consecutivo}");
            var Fan = await GetAsync<Boleteria>($"Boleteria/ObtenerPorConsecutivoTarjetaRecargable/{Consecutivo}");
            return Json(new { resultado = resultado, fan = Fan, empleado = Empleado != null ? Empleado.SoloDescuentoEmpleado : false }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Redenciones()
        {
            //Obtiene TODOS LOS PRODUCTOS
            var _ListaTodosProductosSAP = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerTodosProductos");
            ViewBag.ListaTodosProductosSAP = _ListaTodosProductosSAP;

            return View();
        }


        [HttpGet]
        public async Task<JsonResult> ObtenerEmpleadoPorConsecutivo(string Consecutivo)
        {
            var resultado = await GetAsync<EstructuraEmpleado>($"Pos/ObtenerEmpleadoPorConsecutivo/{Consecutivo}");
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerCupoTarjetaRecargable(string Consecutivo)
        {
            var resultado = await GetAsync<Boleteria>($"Boleteria/ObtenerPorConsecutivoTarjetaRecargable/{Consecutivo}");
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> ObtenerCupoAPP(string Consecutivo)
        {
            var resultado = await GetAsync<SaldoApp>($"Usuario/GetSaldoApp?qr={Consecutivo}");
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ConsultarBonoRegalo(string codigo)
        {
            var rta = await GetAsync<Boleteria>($"Boleteria/ConsultarBonoRegalo/{codigo}");
            return Json(new { objeto = rta ?? new Boleteria(), respuesta = rta != null }, JsonRequestBehavior.AllowGet);
        }

        #region ImpresionEnLinea

        public async Task<Producto> asignarCambioBoleta(ImpresionEnLinea _listprod, Producto itemm, Producto temp, IEnumerable<Producto> brazaletes, RespuestaViewModel respuesta = null)
        {
            Producto p = new Producto();

            if (temp != null && itemm.AplicaImpresionLinea)
            {
                /*if (itemm.CodigoSap == temp.CodigoSap)
                {
                    itemm.AplicaImpresionLinea = true;
                }*/

                if (_listprod.CambioBoleta.CodigoSap == temp.CodigoSap)
                {
                    //calcular codigo impresion en linea
                    if (_listprod.CambioBoleta.AplicaImpresionLinea)
                    {
                        //Producto productoI = brazaletes.Where(x => x.CodigoSap == item.CodigoSap).First();
                        /*if (productoI != null)
                        {*/

                        //var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoI);

                        if (respuesta != null)
                        {
                            if (!respuesta.Correcto)
                                throw new ArgumentException(respuesta.Mensaje);


                            if (!respuesta.Elemento.ToString().Contains("Error"))
                            {

                                var idBoleteria = int.Parse(respuesta.Elemento.ToString().Split('|')[0]);
                                var consecutivo = respuesta.Elemento.ToString().Split('|')[1];
                                //_listprod.CambioBoleta.Consecutivo = int.Parse(consecutivo);
                                //_listprod.CambioBoleta.CodBarraInicio = consecutivo;
                                //_listprod.CambioBoleta.IdDetalleProducto = idBoleteria;
                                itemm.CodBarraInicio = consecutivo;
                            }
                        }
                        /* }*/
                        p = itemm;
                    }
                }
            }
            else
            {
                p = itemm;
            }
            return p;
        }


        //public async Task<Producto> asignarBoleta(List<Producto> _listprod, Producto itemm, Producto temp, IEnumerable<Producto> brazaletes, RespuestaViewModel respuesta)
        public async Task<Producto> asignarBoleta(ImpresionEnLinea _listprod, Producto itemm, Producto temp, IEnumerable<Producto> brazaletes, RespuestaViewModel respuesta = null)
        {
            Producto p = new Producto();

            if (temp != null && itemm.existe == false)
            {
                /*if (itemm.CodigoSap == temp.CodigoSap)
                {
                    itemm.AplicaImpresionLinea = true;
                }*/

                if (_listprod.producto.AplicaImpresionLinea)
                {
                    //calcular codigo impresion en linea
                    //foreach (var item in _listprod.listaProductos.Where(x => x.AplicaImpresionLinea).ToList())
                    //{
                    //Producto productoI = brazaletes.Where(x => x.CodigoSap == item.CodigoSap).First();
                    /*if (productoI != null)
                    {*/

                    //var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoI);

                    if (respuesta != null)
                    {
                        if (!respuesta.Correcto)
                            throw new ArgumentException(respuesta.Mensaje);


                        if (!respuesta.Elemento.ToString().Contains("Error"))
                        {

                            var idBoleteria = int.Parse(respuesta.Elemento.ToString().Split('|')[0]);
                            var consecutivo = respuesta.Elemento.ToString().Split('|')[1];
                            _listprod.producto.CodBarraInicio = consecutivo;
                            //item.IdDetalleProducto = idBoleteria;
                        }
                    }
                    /* }*/
                    p = _listprod.producto;
                    //}
                }
            }
            else
            {
                p = _listprod.producto;
            }
            return p;
        }
        //public async Task<AdicionPedidos> asignarBoletaAdicion(List<AdicionPedidos> _listprod, AdicionPedidos itemm, Producto temp, IEnumerable<Producto> brazaletes, RespuestaViewModel respuesta)
        public async Task<AdicionPedidos> asignarBoletaAdicion(AdicionPedidos itemm, Producto temp, IEnumerable<Producto> brazaletes, RespuestaViewModel respuesta)
        {
            AdicionPedidos p = new AdicionPedidos();

            /*  if (temp != null) //Descomentar
              {*/
            /*if (itemm.CodigoSap == temp.CodigoSap)
            {
                itemm.AplicaImpresionLinea = true;
            }*/

            /* if (_listprod.Exists(x => x.CodigoSapProducto == temp.CodigoSap))
             {*/
            //calcular codigo impresion en linea
            /*foreach (var item in _listprod.Where(x => x.AplicaImpresionLinea).ToList())
            {*/
            //Producto productoI = brazaletes.Where(x => x.CodigoSap == item.CodigoSap).First();
            /*if (productoI != null)
            {*/
            if (itemm.AplicaImpresionLinea)
            {



                //var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoI);

                if (!respuesta.Correcto)
                    throw new ArgumentException(respuesta.Mensaje);


                if (!respuesta.Elemento.ToString().Contains("Error"))
                {

                    var idBoleteria = int.Parse(respuesta.Elemento.ToString().Split('|')[0]);
                    var consecutivo = respuesta.Elemento.ToString().Split('|')[1];
                    itemm.Consecutivo = consecutivo;
                }
                /* }*/
                p = itemm;
                //}
                //}
            }
            /*}
            else
            {
                p = itemm;
            }*/
            return p;
        }

        //public async Task<ImprimirBoletaControl> imprimirBoletas(List<Producto> _listprod, IEnumerable<Producto> brazaletes, int usuariolog, string CodBarraInicio = "", string CodBarraFinal = "")
        public async Task<ImprimirBoletaControl> imprimirBoletas(ImpresionEnLinea _listprod, IEnumerable<Producto> brazaletes, int usuariolog, string CodBarraInicio = "", string CodBarraFinal = "")
        {
            string respuesta = "";

            var _modelo = new ImprimirBoletaControl();
            try
            {

                if (_listprod.listaProductos != null)
                {
                    foreach (var item in _listprod.listaProductos.Where(x => x.AplicaImpresionLinea).ToList())
                    {

                        Producto productoImpresion = brazaletes.Where(x => x.CodigoSap == item.CodigoSap).First();
                        //string consecutivo = item.Consecutivo.ToString();
                        string consecutivo = item.CodBarraInicio;
                        //respuesta = imprimirImpresionEnLinea(item, productoImpresion, consecutivo);
                        int a = 0;
                        while (a < item.Cantidad)
                        {
                            foreach (var i in _listprod.listConsecutivos.Where(x => x.idProducto == item.IdProducto))
                            {
                                //respuesta = imprimirImpresionEnLinea(item, productoImpresion, consecutivo);
                                respuesta = imprimirImpresionEnLinea(item, productoImpresion, i.consecutivos[a]);
                            }
                            a++;
                        }
                    }
                }
                else if (_listprod.listaAdicionPedidos != null)
                {
                    foreach (var item in _listprod.listaAdicionPedidos.Where(x => x.AplicaImpresionLinea).ToList())
                    {

                        Producto productoImpresion = brazaletes.Where(x => x.CodigoSap == item.CodigoSapProducto).First();


                        //if (producto != null && producto.CodigoSap == "40000414")
                        //if (producto != null && item.AplicaImpresionLinea)
                        Producto producto = new Producto();
                        //producto.Consecutivo = int.Parse(item.Consecutivo);
                        producto.CodigoSap = item.CodigoSapProducto;
                        producto.CodSapTipoProducto = item.CodSapTipoProducto;
                        producto.AplicaImpresionLinea = item.AplicaImpresionLinea;
                        int a = 0;
                        while (a < item.Cantidad)
                        {
                            foreach (var i in _listprod.listConsecutivos.Where(x => x.idProducto == item.IdProducto))
                            {
                                respuesta = imprimirImpresionEnLinea(producto, productoImpresion, i.consecutivos[a]);
                            }
                            a++;
                        }


                    }
                }
                else if (_listprod.CambioBoleta != null)
                {
                    Producto productoImpresion = brazaletes.Where(x => x.CodigoSap == _listprod.CambioBoleta.CodigoSap).First();


                    //if (producto != null && producto.CodigoSap == "40000414")
                    //if (producto != null && item.AplicaImpresionLinea)
                    //Producto producto = new Producto();
                    //producto.Consecutivo = int.Parse(item.Consecutivo);
                    /*producto.CodigoSap = _listprod.CambioBoleta.CodigoSapProducto;
                    producto.CodSapTipoProducto = _listprod.CambioBoleta.CodSapTipoProducto;
                    producto.AplicaImpresionLinea = item.AplicaImpresionLinea;*/

                    string consecutivo = _listprod.CambioBoleta.CodBarraInicio;
                    respuesta = imprimirImpresionEnLinea(_listprod.CambioBoleta, productoImpresion, consecutivo);



                }

                if (_listprod.listaProductos != null)
                {
                    _modelo = new ImprimirBoletaControl
                    {
                        ListaProductos = _listprod.listaProductos,
                        CodBarraInicio = CodBarraInicio,
                        CodBarraFinal = CodBarraFinal,
                        IdUsuario = usuariolog
                    };
                }
                else if (_listprod.listaAdicionPedidos != null)
                {
                    _modelo = new ImprimirBoletaControl
                    {
                        ListaAdicionPedidos = _listprod.listaAdicionPedidos,
                        CodBarraInicio = CodBarraInicio,
                        CodBarraFinal = CodBarraFinal,
                        IdUsuario = usuariolog
                    };
                }
                return _modelo;
            }
            catch (Exception e)
            {
                return _modelo = null;
            }

        }

        public string imprimirImpresionEnLinea(Producto item, Producto productoImpresion, string consecutivo)
        {
            string respuesta = "";
            try
            {
                if (item.AplicaImpresionLinea)
                {

                    if (productoImpresion.ArchivoProducto.Archivo != null)
                    {

                        string prue = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + "Temp", productoImpresion.ArchivoProducto.Archivo);
                        //string archivo = Path.Combine(Server.MapPath("~/Temp"), productoImpresion.ArchivoProducto.Archivo);
                        string archivo = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + "Temp", productoImpresion.ArchivoProducto.Archivo);
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(ConfigurationManager.AppSettings["RutaArchivos"].ToString() + productoImpresion.ArchivoProducto.Archivo, archivo);
                        //webClient.DownloadFile("http://localhost:62696/Archivos/Brazaletes/asd3.txt", archivo);

                        StringBuilder contenidoEtiqueta = new StringBuilder();
                        contenidoEtiqueta.Append(System.IO.File.ReadAllText(archivo, Encoding.GetEncoding(1252)));
                        //contenidoEtiqueta.Replace(ConfigurationManager.AppSettings["TagCodigoBarras"].ToString(), item.CodBarraInicio);
                        contenidoEtiqueta.Replace(ConfigurationManager.AppSettings["TagCodigoBarras"].ToString(), consecutivo);

                        respuesta = PrintDirect.PrintText(contenidoEtiqueta.ToString(), 0);

                        //Aqui se va a llevar el control
                        //restantesBoletas = restantesBoletas - 1;
                        //MensajeBoletasRestantes = await GetAsync<string>($"Pos/UpdateControlBoleteria/{idBoleta}/{restantesBoletas}");
                        //
                    }

                    if (!string.IsNullOrWhiteSpace(respuesta))
                    {
                        Utilidades.RegistrarError(new Exception("Error al imprimir"), respuesta);
                        respuesta = "Error imprimiendo en linea";
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = "Algo sucedio con el diseño, no se pudo vender";
            }
            return respuesta;
        }

        public async Task<string> registrarRollo(int IdUsuarioLogueadoo = 0, string UsuarioLogueado = "", int IdPuntoo = 0)
        {
            Producto Rollo = new Producto();
            try
            {
                Rollo = new Producto
                {
                    Nombre = "ROLLO IMPL 1 CAVIDAD",
                    Codigo = 20002201,
                    CodigoSap = "20002201",
                    IdProducto = 2764,
                    Cantidad = 1,
                    IdUsuarioModificacion = IdUsuarioLogueadoo,
                    Precio = 0,
                    PrecioTotal = 0,
                    NombreImpuesto = "IVA_0",
                    IdEstado = 1,
                    IdTipoProducto = 1,
                    Entregado = true,
                    IdDetalleProducto = 1,
                    UsuarioCreacion = UsuarioLogueado,
                    IdPuntoDescarga = IdPuntoo
                };
                await PostAsync<Producto, string>($"Pos/RegistroRolloImpresionLinea", Rollo);
            }
            catch (Exception e)
            {
                return "";
            }
            //var res = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", Rollo);

            return "";
        }

        public async Task<string> registrarRolloInventario(ImpresionEnLinea impresionRegistroRollo, int IdUsuarioLogueadoo = 0)
        {

            string NombreProd = "ROLLO";
            var datoparametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/{NombreProd}");
            var i = 0;
            var conteo = 0;
            impresionRegistroRollo.cantidad = 0;
            List<Producto> lproducto = new List<Producto>();
            //var conteo = ListaProductos.ToList().Where(x => x.CodSapTipoProducto == "2000" && x.AplicaImpresionLinea).Count() + ListaProductos.ToList().Where(x => x.CodSapTipoProducto == "2000" && x.AplicaImpresionLinea).Count();
            if (impresionRegistroRollo.listaProductos != null)
            {
                conteo = impresionRegistroRollo.listaProductos.ToList().Where(x => x.CodSapTipoProducto == "2000" && x.AplicaImpresionLinea).Count();
                //impresionRegistroRollo.cantidad = impresionRegistroRollo.listaProductos[conteo].Cantidad;
                foreach (var item in impresionRegistroRollo.listaProductos)
                {
                    if (item.CodSapTipoProducto == "2000")
                    {
                        impresionRegistroRollo.cantidad += item.Cantidad;
                    }
                }
            }
            else if (impresionRegistroRollo.listaAdicionPedidos != null)
            {
                conteo = impresionRegistroRollo.listaAdicionPedidos.ToList().Where(x => x.CodSapTipoProducto == "2000" && x.AplicaImpresionLinea).Count();
                //impresionRegistroRollo.listaAdicionPedidos[conteo].Cantidad
                //impresionRegistroRollo.cantidad = impresionRegistroRollo.listaAdicionPedidos.Cantidad;
                foreach (var item in impresionRegistroRollo.listaAdicionPedidos)
                {
                    if (item.CodSapTipoProducto == "2000")
                    {
                        impresionRegistroRollo.cantidad += item.Cantidad;
                    }
                }

            }

            //while (i < ListaProductos.ToList().Where(x => x.CodSapTipoProducto == "2000" && x.AplicaImpresionLinea).Count())
            while (i < impresionRegistroRollo.cantidad)
            // if (i != conteo)
            {
                if (datoparametro != null)
                {
                    var idProducto = datoparametro.Valor;
                    var id = int.Parse(idProducto);
                    Producto producto = await GetAsync<Producto>($"Pos/ObtenerProducto/{idProducto}");
                    /*lproducto.Add(new Producto
                    {
                        //Nombre = "ROLLO IMPL 1 CAVIDAD",
                        Nombre = producto.Nombre,
                        Codigo = producto.Codigo,
                        CodigoSap = producto.CodigoSap,
                        IdProducto = producto.IdProducto,
                        Cantidad = 1,
                        IdUsuarioModificacion = IdUsuarioLogueadoo,
                        Precio = producto.Precio,
                        PrecioTotal = producto.PrecioTotal,
                        NombreImpuesto = producto.,
                        IdEstado = 1,
                        IdTipoProducto = 1,
                        Entregado = true,
                        IdDetalleProducto = 1
                    });*/
                    lproducto.Add(producto);
                }
                i++;
            }

            Inventario inventario = new Inventario();
            inventario.FechaInventario = Utilidades.FechaActualColombia;
            inventario.IdPunto = IdPunto;
            inventario.IdUsuarioCeado = IdUsuarioLogueadoo;
            inventario.Productos = lproducto; // AYB 
            await PostAsync<Inventario, string>("Inventario/ActualizarInventario", inventario);
            return "";
        }


        [HttpGet]
        public async Task<JsonResult> ObtenerDetallesConsecutivoConvenioDia(string Consecutivo)
        {
            var resultado = await GetAsync<IEnumerable<DetalleFactura>>($"Factura/ObtenerDetallesConsecutivoConvenioDia/{Consecutivo}");

            return Json(new { resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ConsultarVencimientoTarjeta(string Tarjeta)
        {
            var resultado = await GetAsync<String>($"TarjetaRecargable/ConsultarVencimientoTarjeta/{Tarjeta}");

            return Json(new { resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
