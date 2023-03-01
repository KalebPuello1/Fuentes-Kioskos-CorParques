using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Ventas.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Transversales.Contratos;
using System.Data;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class AdicionPedidosController : ControladorBase
    {

        #region Declaraciones

        private readonly IServicioImprimir _service;

        public AdicionPedidosController(IServicioImprimir service)
        {
            _service = service;
        }

        #endregion

        #region Metodos

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// RDSH: Busca el detalle de un pedido segun su codigo sap.
        /// </summary>
        /// <param name="CodigoSapPedido"></param>
        /// <returns></returns>
        public async Task<ActionResult> DetallePedido(string CodigoSapPedido)
        {

            ImpresionEnLinea impresion = new ImpresionEnLinea();


            IEnumerable<AdicionPedidos> objListaAdicionPedidos = null;
            List<AdicionPedidos> dato = new List<AdicionPedidos>();
            try
            {
                List<Arqueo> listaRestantes = await GetAsync<List<Arqueo>>($"Arqueo/ObtenerArqueo?idUsuario={IdUsuarioLogueado}&IdPunto={IdPunto}");

                objListaAdicionPedidos = await GetAsync<IEnumerable<AdicionPedidos>>($"AdicionPedidos/DetallePedido/{CodigoSapPedido}");

                if (objListaAdicionPedidos != null)
                {
                    foreach (var item in objListaAdicionPedidos)
                    {
                        if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2055")
                        {
                            if (listaRestantes[0].Brazalete.ToList().Exists(x => x.EnCaja < item.Cantidad && x.CodigoSap == item.CodigoSapProducto && x.TipoBrazalete == item.Producto) || !listaRestantes[0].Brazalete.ToList().Exists(x => x.CodigoSap == item.CodigoSapProducto && x.TipoBrazalete == item.Producto))
                            {
                                //if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2055" || item.CodSapTipoProducto == "2005")

                                //foreach (var item in objListaAdicionPedidos)
                                //{
                                Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSapProducto}");
                                //
                                if (temp != null)
                                {
                                    if (!temp.Nombre.Contains("No existe el producto o es exeption"))
                                    {
                                        AdicionPedidos adicion = impresion.ValidacionAdicionPedido(item, listaRestantes, temp);
                                        if (adicion != null)
                                        {
                                            dato.Add(adicion);
                                        }
                                        else
                                        {
                                            dato = new List<AdicionPedidos>();
                                        }
                                    }
                                    else
                                    {
                                        dato = new List<AdicionPedidos>();
                                    }
                                }
                                else
                                {
                                    dato = new List<AdicionPedidos>();
                                }

                            }
                            else
                            {
                                item.AplicaImpresionLinea = false;
                                item.existe = true;
                                item.MostrarTexto = true;
                                dato.Add(item);
                            }
                        }
                        else
                        {
                            if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2055" || item.CodSapTipoProducto == "2005")
                            {
                                item.MostrarTexto = true;
                            }
                            item.AplicaImpresionLinea = false;
                            item.existe = true;
                            dato.Add(item);
                        }
                        //
                    }
                }
                else
                {
                    ViewBag.Mensaje = "Pedido fue redimido. ";
                }

                if (dato.Count() == objListaAdicionPedidos.Count())
                {
                    objListaAdicionPedidos = dato;
                    //ViewBag.Mensaje = "";
                }
                else
                {
                    //lp = new List<AdicionPedidos>();
                    objListaAdicionPedidos = new List<AdicionPedidos>();
                    ViewBag.Mensaje = "No cuenta con inventario, ni impresion en linea para este pedido";
                }
                objListaAdicionPedidos = objListaAdicionPedidos.Count() == 0 ? null : objListaAdicionPedidos;

            }

            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "AdicionPedidosController_DetallePedido");                
            }
            return PartialView("_Detalle", objListaAdicionPedidos);
        }

        /// <summary>
        ///  RDSH: Valida que un rango de brazaletes pueda ser usado en adicion de pedido.
        /// </summary>
        /// <param name="ConsecutivoInicial"></param>
        /// <param name="ConsecutivoFinal"></param>
        /// <param name="Cantidad"></param>
        /// <returns></returns>
        public async Task<string> ValidarRangoConsecutivos(string ConsecutivoInicial, string ConsecutivoFinal, int Cantidad, int IdProducto)
        {
            string strResultado = string.Empty;

            try
            {
                strResultado = await GetAsync<string>($"AdicionPedidos/ValidarRangoConsecutivos/{ConsecutivoInicial}/{ConsecutivoFinal}/{Cantidad}/{IdProducto}/{IdUsuarioLogueado}");
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "AdicionPedidosController_ValidarRangoConsecutivos");
            }

            return strResultado;
        }

        /// <summary>
        /// RDSH: Guarda la información y retorna un objeto para realizar la impresion de los productos que no son de tipo uso boleteria.
        /// </summary>
        /// <param name="CodigoPedido"></param>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Guardar(IEnumerable<AdicionPedidos> Modelo)
        {
            string strResultado = string.Empty;
            PosController pos = new PosController();
            ImpresionEnLinea impresion = new ImpresionEnLinea();
            /*foreach (var item in Modelo)
            {
                Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSapProducto}");

                if (temp != null  && !item.MostrarTexto)
                {
                    item.AplicaImpresionLinea = true;
                    //item.CodSapTipoProducto = "2000";
                }
                *//*else
                {
                    item.AplicaImpresionLinea = false;
                }*//*
            }
*/
            List<ImpresionEnLineaConsecutivos> listConsecutivos = new List<ImpresionEnLineaConsecutivos>();
            var ii = 0;
            var modelo = new ImprimirBoletaControl();
            try
            {
                Modelo.ToList().Where(x => x.ConsecutivoInicial == null).ToList().ForEach(x => x.ConsecutivoInicial = string.Empty);
                Modelo.ToList().Where(x => x.ConsecutivoFinal == null).ToList().ForEach(x => x.ConsecutivoFinal = string.Empty);
                Modelo.ToList().ForEach(x => x.IdUsuario = IdUsuarioLogueado);
                Modelo.ToList().ForEach(x => x.IdPunto = IdPunto);


                IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
                AdicionPedidos[] i = new AdicionPedidos[Modelo.ToList().Where(x => x.CodSapTipoProducto == "2000" || x.CodSapTipoProducto == "2055").Count()];

                ImpresionEnLineaConsecutivos consecutivoAdicion = new ImpresionEnLineaConsecutivos();
                var vueltas = 0;
                List<Arqueo> listaRestantes = await GetAsync<List<Arqueo>>($"Arqueo/ObtenerArqueo?idUsuario={IdUsuarioLogueado}&IdPunto={IdPunto}");

                foreach (var itemm in Modelo.Where(x => x.AplicaImpresionLinea))
                {
                    //if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2055" || item.CodSapTipoProducto == "2005")
                    if (itemm.CodSapTipoProducto == "2000" || itemm.CodSapTipoProducto == "2055")
                    {
                        if (listaRestantes[0].Brazalete.ToList().Exists(x => x.EnCaja < itemm.Cantidad && x.CodigoSap == itemm.CodigoSapPedido && x.TipoBrazalete == itemm.Producto) || !listaRestantes[0].Brazalete.ToList().Exists(x => x.CodigoSap == itemm.CodigoSapPedido && x.TipoBrazalete == itemm.Producto))
                        {
                            //string[] listaConsecutivos = new string[itemm.Cantidad];
                            if (vueltas == 0)
                            {
                                consecutivoAdicion = new ImpresionEnLineaConsecutivos();
                                consecutivoAdicion.idProducto = itemm.IdProducto;
                                consecutivoAdicion.consecutivos = new string[itemm.Cantidad];
                            }

                            while (ii < itemm.Cantidad)
                            {
                                Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{itemm.CodigoSapProducto}");
                                AdicionPedidos p = null;
                                Producto pp = new Producto();

                                AdicionPedidos adicion = new AdicionPedidos();
                                // i[0] = adicion;

                                if (temp != null)
                                {
                                    if (itemm.AplicaImpresionLinea)
                                    {
                                        Producto productoI = brazaletes.Where(x => x.CodigoSap == itemm.CodigoSapProducto).First();
                                        productoI.IdUsuarioModificacion = IdUsuarioLogueado;

                                        if (productoI != null)
                                        {
                                            /*if (itemm.AplicaImpresionLinea)
                                            {
                                                var iiii = 0;
                                                while (iiii < Modelo.Where(x => x.AplicaImpresionLinea).Count())
                                                {
                                                    await pos.registrarRollo(IdUsuarioLogueado, UsuarioLogueado, IdPunto);
                                                    iiii++;
                                                }
                                            }*/
                                            productoI.IdUsuarioModificacion = IdUsuarioLogueado;
                                            var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoI);
                                            /*pp.Nombre = itemm.Producto;
                                            pp.IdProducto = itemm.IdProducto;
                                            pp.CodigoSap = itemm.CodigoSapProducto;
                                            pp.AplicaImpresionLinea = itemm.AplicaImpresionLinea;
                                            pp.existe = itemm.existe;
                                            pp.MostrarTexto = itemm.MostrarTexto;*/
                                            //p = await pos.asignarBoleta(Modelo.ToList(), pp, temp, brazaletes, respuesta);
                                            p = await pos.asignarBoletaAdicion(itemm, temp, brazaletes, respuesta);
                                            string cambioEstado = await GetAsync<string>($"Boleteria/UpdateEstadoCambioboleta/{p.Consecutivo}");
                                            consecutivoAdicion.consecutivos[vueltas] = p.Consecutivo;
                                            
                                        }

                                    }
                                    else
                                    {
                                        p = itemm;
                                    }
                                }
                                if (p != null)
                                {
                                    if (p.AplicaImpresionLinea)
                                    {
                                        itemm.Producto = p.Producto;
                                        itemm.Cantidad = p.Cantidad;
                                        itemm.CodigoSapProducto = p.CodigoSapProducto;
                                        //itemm.ConsecutivoInicial = p.Consecutivo;
                                        //itemm.ConsecutivoFinal = p.ConsecutivoFinal;
                                        itemm.CodSapTipoProducto = p.CodSapTipoProducto;
                                        itemm.existe = p.existe;
                                        itemm.AplicaImpresionLinea = p.AplicaImpresionLinea;
                                        itemm.IdProducto = p.IdProducto;
                                        itemm.IdUsuario = p.IdUsuario;
                                        p = null;
                                    }
                                    else
                                    {
                                        itemm.Producto = p.Producto;
                                        itemm.Cantidad = p.Cantidad;
                                        itemm.CodigoSapProducto = p.CodigoSapProducto;
                                        //itemm.ConsecutivoInicial = p.ConsecutivoInicial;
                                        //itemm.ConsecutivoFinal = p.ConsecutivoFinal;
                                        itemm.CodSapTipoProducto = p.CodSapTipoProducto;
                                        itemm.existe = p.existe;
                                        itemm.AplicaImpresionLinea = p.AplicaImpresionLinea;
                                        itemm.IdProducto = p.IdProducto;
                                        itemm.IdUsuario = p.IdUsuario;
                                        p = null;
                                    }

                                }

                                if (vueltas == 0)
                                {
                                    consecutivoAdicion.primero = itemm.Consecutivo;
                                }
                                else if (vueltas == itemm.Cantidad - 1)
                                {
                                    consecutivoAdicion.ultimo = itemm.Consecutivo;
                                }

                                ii++;
                                vueltas++;
                            }
                            listConsecutivos.Add(consecutivoAdicion);
                            vueltas = 0;
                            ii = 0;
                        }
                        else
                        {
                            itemm.existe = true;
                            itemm.AplicaImpresionLinea = false;
                            //lp.Add(itemm);
                        }
                    }
                    else
                    {
                        itemm.existe = true;
                        itemm.AplicaImpresionLinea = false;
                    }
                }
                foreach (var item in Modelo.Where(x => !x.AplicaImpresionLinea))
                {
                    item.IdUsuario = IdUsuarioLogueado;
                }

                impresion.listConsecutivos = listConsecutivos;
                /*if ()
                {
                    while (i < )
                    {
                        impresion.listaAdicionPedidos = Modelo.ToList();
                        modelo = await pos.imprimirBoletas(impresion, brazaletes, IdUsuarioLogueado);
                    }
                }*/
                impresion.listaAdicionPedidos = Modelo.ToList();
                modelo = await pos.imprimirBoletas(impresion, brazaletes, IdUsuarioLogueado);

                foreach (var it in Modelo)
                {
                    if (it.CodSapTipoProducto == "2000" || it.CodSapTipoProducto == "2055")
                    {
                        foreach (var item in listConsecutivos.Where(x => x.idProducto == it.IdProducto))
                        {
                            if (it.IdProducto == item.idProducto)
                            {
                                it.ConsecutivoInicial = item.primero;
                                it.ConsecutivoFinal = item.ultimo;
                            }
                        }
                    }
                }

                if (Modelo.ToList().Exists(x => x.CodSapTipoProducto == "2000" && x.AplicaImpresionLinea))
                {
                    ImpresionEnLinea impresionRegistroRollo = new ImpresionEnLinea();
                    impresionRegistroRollo.listaAdicionPedidos = Modelo.ToList();
                    impresionRegistroRollo.listaProductos = null;
                    await pos.registrarRolloInventario(impresionRegistroRollo, IdUsuarioLogueado);
                }

                var objRespuesta = await PostAsync<IEnumerable<AdicionPedidos>, IEnumerable<AdicionPedidos>>("AdicionPedidos/Guardar", Modelo);

                //var objRespuesta = await PostAsync<IEnumerable<AdicionPedidos>, IEnumerable<AdicionPedidos>>("AdicionPedidos/Guardar", modelo);

                if (objRespuesta.Elemento != null)
                {
                    strResultado = await GenerarRecibos(objRespuesta.Elemento as List<AdicionPedidos>);
                    if (strResultado.Trim().Length > 0)
                        throw new ArgumentException(strResultado);
                }

            }
            catch (Exception ex)
            {
                if (modelo == null)
                {
                    strResultado = "Ocurrio un error procesando la solucitud, informe al administrador. No se subieron los archivos del diseño para el brazalete";
                }
                else
                {
                    strResultado = "Ocurrio un error procesando la solucitud, informe al administrador.";
                }
                //strResultado = "Ocurrio un error procesando la solucitud, informe al administrador.";
                Utilidades.RegistrarError(ex, "AdicionPedidosController_Guardar");
            }

            return strResultado;
        }
        
        /// <summary>
        /// RDSH: Genera la impresión de las recibos de los productos diferentes a boletaria.
        /// </summary>
        /// <param name="objListaPedidos"></param>
        /// <returns></returns>
        private async Task<string> GenerarRecibos(IEnumerable<AdicionPedidos> objListaPedidos)
        {
            string strRetorno = string.Empty;
            

            try
            {
                if (objListaPedidos != null)
                {

                    foreach (AdicionPedidos objAdicionPedidos in objListaPedidos)
                    {
                        TicketImprimir objTicketImprimir = new TicketImprimir();
                        Articulo objArticulo = new Articulo();
                        List<Articulo> ListaArticulos = new List<Articulo>();

                        objArticulo.Nombre = objAdicionPedidos.Producto;
                        objArticulo.Cantidad = 1;
                        objArticulo.Precio = objAdicionPedidos.Valor;
                        objArticulo.Otro = "";
                        ListaArticulos.Add(objArticulo);

                        objTicketImprimir.TituloRecibo = "Boleta Adición Pedido";
                        objTicketImprimir.CodigoBarrasProp = objAdicionPedidos.Consecutivo;
                        objTicketImprimir.TituloColumnas = "Valido para|Cant";
                        objTicketImprimir.ListaArticulos = ListaArticulos;
                        objTicketImprimir.PieDePagina = string.Empty;

                        strRetorno = _service.ImprimirAdicionPedido(objTicketImprimir);
                        objTicketImprimir = null;
                        objArticulo = null;
                        ListaArticulos = null;

                        if (strRetorno.Trim().Length > 0)
                            throw new ArgumentException(strRetorno);
                    }        
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en AdicionPedidosController_GenerarRecibos: ", ex.Message);
            }            

            return strRetorno;
        }

        #endregion

    }
}