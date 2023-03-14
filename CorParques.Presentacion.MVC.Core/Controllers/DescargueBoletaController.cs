using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class DescargueBoletaController : ControladorBase
    {
        // GET: DescargueBoleta
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerProductos(string CodBarra)   
        {
            //var rta = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoBoleta/{CodBarra}/{IdUsuarioLogueado}");
          
            var rta = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoBoleta/{CodBarra}/{IdUsuarioLogueado}");
            var rtaa = await GetAsync<List<Producto>>($"Pos/VerPasaportesCodigoPedido/{CodBarra}");
            // var rta = f;
            ImpresionEnLinea impresion = new ImpresionEnLinea();

            ViewBag.Mensaje = rta.Mensaje;

            List<Arqueo> listaRestantes = await GetAsync<List<Arqueo>>($"Arqueo/ObtenerArqueo?idUsuario={IdUsuarioLogueado}&IdPunto={IdPunto}");


            if (rta.Mensaje == "No cuenta con inventario para realizar esta entrega.")
            {

                var f = new DescargueBoletaControl();
                List<Producto> lp = new List<Producto>();

                
                if (rtaa != null)
                {
                    foreach (var item in rtaa)
                    {
                        //if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2055" || item.CodSapTipoProducto == "2005")
                        if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2055")
                        {

                        //if (listaRestantes[0].Brazalete.Count() > 0) 
                        if (listaRestantes != null)
                        {
                             if (listaRestantes[0].Brazalete.ToList().Exists(x => x.EnCaja < item.Cantidad && x.CodigoSap == item.CodigoSap && x.TipoBrazalete == item.Nombre) || !listaRestantes[0].Brazalete.ToList().Exists(x => x.CodigoSap == item.CodigoSap && x.TipoBrazalete == item.Nombre))
                             {
                                    Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSap}");

                                    Producto p = new Producto();
                                    if (temp != null)
                                    {
                                        if (!temp.Nombre.Contains("No existe el producto o es exeption"))
                                        {
                                            p = impresion.validacionDescargueBoleta(rta.Mensaje, listaRestantes, item, temp);
                                        }
                                        else
                                        {
                                            lp = new List<Producto>();
                                            p = null;
                                        }
                                    }
                                    else
                                    {
                                        lp = new List<Producto>();
                                    }
                                    if (p != null)
                                    {
                                        if (!p.Nombre.Contains("Producto no existe, y no puede imprimirse en linea"))
                                        {
                                            lp.Add(p);
                                        }
                                        else
                                        {
                                            lp = new List<Producto>();
                                        }
                                    }
                                    else
                                    {
                                        lp = new List<Producto>();
                                    }
                                }
                             else
                             {
                                    item.existe = true;
                                    item.AplicaImpresionLinea = false;
                                    lp.Add(item);
                             }
                            }
                            else
                            {
                                item.existe = true;
                                item.AplicaImpresionLinea = false;
                                lp.Add(item);
                            }

                        }
                        else
                        {
                            item.existe = true;
                            item.AplicaImpresionLinea = false;
                            lp.Add(item);
                        }
                        //}

                    }

                    if (lp.Count() == rtaa.Count())
                    {
                        rta.Productos = lp;
                        ViewBag.Mensaje = "";
                    }
                    else
                    {
                        lp = new List<Producto>();
                        ViewBag.Mensaje = "No cuenta con inventario, ni impresion en linea para este pedido";
                    }

                }
                else  
                {
                    ViewBag.Mensaje = "No existen productos para este pedido";
                }
                // lp;
                
            }
            else
            {
                foreach (var item in rta.Productos)
                {
                    if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2055")
                    {
                        item.existe = true;
                        item.AplicaImpresionLinea = false;
                    }
                    else
                    {
                        item.AplicaImpresionLinea = false;
                        item.existe = true;
                    }
                }
            }

            rta.Productos = rta.Productos.Count() == 0 ? null : rta.Productos;


            //Esto es nuevo para la validacion 
            var id = IdUsuarioLogueado;
            //
            
            //id = 4452;
            //List<Arqueo> listaRestantes = await GetAsync<List<Arqueo>>($"Arqueo/ObtenerArqueo?idUsuario={id}&IdPunto={IdPunto}");
            
            //List<Apertura> Apertura = await GetAsync<List<Apertura>>($"Apertura/ObtenerAperturasTaquillero/{id}/{8}");

            return PartialView("_ListaProductos", rta);
        }

        public async Task<JsonResult> BuscarBoleta(string CodBarraInicio, string CodBarraFinal, string Codproducto)
        {
            var rta = await GetAsync<ProductoBoleta>($"Pos/BuscarBoleta/{CodBarraInicio}/{CodBarraFinal}/{Codproducto}/{IdUsuarioLogueado}");
            return Json(rta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ImprimirCodBarras(List<Producto> _listprod, string CodBarraInicio, string CodBarraFinal)
        {

            #region Aqui entra a imprimir fila express
            ImpresionEnLinea impresion = new ImpresionEnLinea();
            PosController pos = new PosController();

            //TestPruebaEditar
            //this.editar(_listprod);
            //
            IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
            List<Producto> lp = new List<Producto>();
            ImpresionEnLineaConsecutivos consecutivoAdicion = new ImpresionEnLineaConsecutivos();
            List<ImpresionEnLineaConsecutivos> listConsecutivos = new List<ImpresionEnLineaConsecutivos>();
            var vueltas = 0;
            var ii = 0;

            try
            {
                var i = 0;
                foreach (Producto itemm in _listprod.Where(x => x.AplicaImpresionLinea))
                {
                    ////////
                    ///
                    if (vueltas == 0)
                    {
                        consecutivoAdicion = new ImpresionEnLineaConsecutivos();
                        consecutivoAdicion.idProducto = itemm.IdProducto;
                        consecutivoAdicion.consecutivos = new string[itemm.Cantidad];
                    }

                    ////
                    while (ii < itemm.Cantidad)
                    {
                        var consecutivos = "";
                        Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{itemm.CodigoSap}");
                        Producto p = new Producto();

                        //if (itemm.CodSapTipoProducto == "2000" || itemm.CodSapTipoProducto == "2055" || itemm.CodSapTipoProducto == "2005" && itemm.AplicaImpresionLinea)
                        if (itemm.CodSapTipoProducto == "2000" && itemm.AplicaImpresionLinea || itemm.CodSapTipoProducto == "2055" && itemm.AplicaImpresionLinea)
                        {
                            Producto productoI = null;
                            if (brazaletes.Where(x => x.CodigoSap == itemm.CodigoSap).First() != null)
                            {
                                productoI = brazaletes.Where(x => x.CodigoSap == itemm.CodigoSap).First();
                            }
                            

                            if (productoI != null)
                            {
                             
                                productoI.IdUsuarioModificacion = IdUsuarioLogueado;
                                var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoI);
                                impresion.listaProductos = _listprod;
                                impresion.producto = itemm;
                                //p = await pos.asignarBoleta(_listprod, itemm, temp, brazaletes, respuesta);
                                p = await pos.asignarBoleta(impresion, itemm, temp, brazaletes, respuesta);
                                consecutivoAdicion.consecutivos[vueltas] = p.CodBarraInicio;
                                consecutivos = p.CodBarraInicio;
                            }
                            else
                            {
                                p = null;
                            }

                        }
                        else
                        {
                            p = itemm;
                        }
                        if (p != null)
                        {
                            itemm.Entregado = p.Entregado;
                            itemm.Nombre = p.Nombre;
                            itemm.Cantidad = p.Cantidad;
                            itemm.CodigoSap = p.CodigoSap;
                            itemm.Codigo = p.Codigo;
                            itemm.ArchivoProducto = p.ArchivoProducto;
                            itemm.CodSapTipoProducto = p.CodSapTipoProducto;
                            itemm.Consecutivo = p.Consecutivo;
                            itemm.Entregado = p.Entregado;
                            itemm.Descricpion = p.Descricpion;
                            itemm.existe = p.existe;
                            itemm.AplicaImpresionLinea = p.AplicaImpresionLinea;
                            itemm.AplicaPunto = p.AplicaPunto;
                            itemm.IdEstado = p.IdEstado;
                            itemm.IdProducto = p.IdProducto;
                            itemm.IdUsuarioModificacion = p.IdUsuarioModificacion;
                            itemm.Precio = p.Precio;
                            lp.Add(itemm);
                            //itemm.CodBarraInicio = "";
                            //itemm.CodBarraInicio = CodBarraInicio;
                            //itemm.CodBarraFin = CodBarraFinal;
                        }
                        else
                        {
                            itemm.AplicaImpresionLinea = false;
                        }


                        if (vueltas == 0)
                        {
                            //consecutivoAdicion.primero = itemm.Consecutivo;
                            consecutivoAdicion.primero = consecutivos;
                        }
                        else if (vueltas == itemm.Cantidad - 1)
                        {
                            //consecutivoAdicion.ultimo = itemm.Consecutivo;
                            consecutivoAdicion.ultimo = consecutivos;
                        }

                        ii++;
                        vueltas++;

                    }
                    listConsecutivos.Add(consecutivoAdicion);
                    vueltas = 0;
                    ii = 0;
                }
                foreach (var item in _listprod.Where(x => !x.AplicaImpresionLinea))
                {
                    var usuario = IdUsuarioLogueado.ToString();
                    item.UsuarioCreacion = usuario;
                    item.IdUsuarioModificacion = IdUsuarioLogueado;
                    lp.Add(item);
                }
                impresion.listConsecutivos = listConsecutivos;

            }
            catch (Exception ee)
            {
                string error = ee.ToString();
            }

            /*var _modelo = new ImprimirBoletaControl
            {
                ListaProductos = _listprod,
                CodBarraInicio = CodBarraInicio,
                CodBarraFinal = CodBarraFinal,
                IdUsuario = IdUsuarioLogueado
            };*/

            ///Aqui va el metodo
            ///
            var modelo = new ImprimirBoletaControl();

            if (lp.Count() == _listprod.Count())
            {
             impresion.listaProductos = _listprod;
            
            //var _modelo = await pos.imprimirBoletas(impresion, brazaletes, IdUsuarioLogueado, CodBarraInicio, CodBarraFinal);
            ///

            
            //modelo = await pos.imprimirBoletas(impresion, brazaletes, IdUsuarioLogueado);
            modelo = await pos.imprimirBoletas(impresion, brazaletes, IdUsuarioLogueado);
            

            foreach (var it in _listprod)
            {
                if (it.CodSapTipoProducto == "2000" || it.CodSapTipoProducto == "2055")
                {
                    foreach (var item in listConsecutivos.Where(x => x.idProducto == it.IdProducto))
                    {
                        if (it.IdProducto == item.idProducto)
                        {
                            it.CodBarraInicio = item.primero;
                            it.CodBarraFin = item.ultimo;
                        }
                    }
                }
            }
            
            /*var _modelo = new ImprimirBoletaControl
            {
                ListaProductos = _listprod,
                CodBarraInicio = CodBarraInicio,
                CodBarraFinal = CodBarraFinal,
                IdUsuario = IdUsuarioLogueado
            };*/

                #endregion
            }
            else
            {
                modelo = null;
            }

            if (modelo != null)
            {
                var _modelo = new ImprimirBoletaControl
                {
                    ListaProductos = _listprod,
                    CodBarraInicio = CodBarraInicio,
                    CodBarraFinal = CodBarraFinal,
                    IdUsuario = IdUsuarioLogueado
                };

                if (_listprod.ToList().Exists(x => x.CodSapTipoProducto == "2000" && x.AplicaImpresionLinea))
                {
                    ImpresionEnLinea impresionRegistroRollo = new ImpresionEnLinea();
                    impresionRegistroRollo.listaProductos = _listprod;
                    impresionRegistroRollo.listaAdicionPedidos = null;
                    await pos.registrarRolloInventario(impresionRegistroRollo, IdUsuarioLogueado);
                }

                var rta = await PostAsync<ImprimirBoletaControl, RedencionBoletaControl>("Pos/ObtenerCodBarrasBoletaControl", _modelo);


                var objRespuesta = (RedencionBoletaControl)rta.Elemento;
                if (!string.IsNullOrEmpty(objRespuesta.Mensaje))
                {
                    rta.Correcto = false;
                    rta.Mensaje = objRespuesta.Mensaje;
                }
                else
                {
                    //Imprimir
                    if (objRespuesta.modeloImprimir != null)
                    {
                        try
                        {
                            ServicioImprimir objImprimir = new ServicioImprimir();

                            foreach (var item in objRespuesta.modeloImprimir)
                            {
                                var objTicket = new TicketImprimir();
                                objTicket.TituloRecibo = item.TituloRecibo;
                                objTicket.CodigoBarrasProp = item.CodigoBarrasProp;
                                objTicket.TituloColumnas = item.TituloColumnas;
                                objTicket.ListaArticulos = new List<Articulo>();
                                objTicket.ListaArticulos.Add(new Articulo() { Nombre = item.Nombre, Cantidad = 1, Precio = item.Precio, TituloColumnas = item.TituloColumnas });
                                objImprimir.ImprimirCupoDebito(objTicket);
                            }
                        }
                        catch (Exception ex)
                        {
                            Utilidades.RegistrarError(ex, string.Concat(this.GetType().Name, "//"
                                , System.Reflection.MethodBase.GetCurrentMethod().Name));
                        }
                    }

                }
                return Json(rta, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var rta = new RedencionBoletaControl();
                //rta. = false;
                rta.Mensaje = "No se pudo adicionar pedido, La impresion en linea no a sido cargada o fallo, vuelta intentar mas tarde! ";
                return Json(rta, JsonRequestBehavior.AllowGet);
            }
    
        }


        #region comentadoMetodos

        /* public async Task<Producto> asignarBoleta(List<Producto> _listprod, Producto itemm, Producto temp, IEnumerable<Producto> brazaletes)
         {
             Producto p = new Producto();

             if (temp != null && itemm.existe == false)
             {
                 *//*if (itemm.CodigoSap == temp.CodigoSap)
                 {
                     itemm.AplicaImpresionLinea = true;
                 }*//*

                 if (_listprod.Exists(x => x.CodigoSap == temp.CodigoSap))
                 {
                     //calcular codigo impresion en linea
                     foreach (var item in _listprod.Where(x => x.AplicaImpresionLinea).ToList())
                     {
                         Producto productoI = brazaletes.Where(x => x.CodigoSap == item.CodigoSap).First();
                         if (productoI != null)
                         {

                             var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoI);

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
                         p = item;
                     }
                 }
             }
             return p;
         }

         public async Task<ImprimirBoletaControl> imprimirBoletas(List<Producto> _listprod, IEnumerable<Producto> brazaletes, string CodBarraInicio, string CodBarraFinal)
         {
             string respuesta = "";

             var _modelo = new ImprimirBoletaControl
             {
                 ListaProductos = _listprod,
                 CodBarraInicio = CodBarraInicio,
                 CodBarraFinal = CodBarraFinal,
                 IdUsuario = IdUsuarioLogueado
             };

             foreach (var item in _listprod.Where(x => x.AplicaImpresionLinea).ToList())
             {

                 Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSap}");

                 if (temp != null)
                 {

                     Producto producto = _listprod.Where(x => x.CodigoSap == temp.CodigoSap).First();

                     //IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
                     Producto productoImpresion = brazaletes.Where(x => x.CodigoSap == producto.CodigoSap).First();


                     //if (producto != null && producto.ArchivoProducto != null)
                     //if (producto != null && producto.CodigoSap == "40000414")
                     if (producto != null && item.AplicaImpresionLinea)
                     {
                         producto.IdProducto = 61;
                         string archivo = Path.Combine(Server.MapPath("~/Temp"), productoImpresion.ArchivoProducto.Archivo);
                         WebClient webClient = new WebClient();
                         webClient.DownloadFile(ConfigurationManager.AppSettings["RutaArchivos"].ToString() + productoImpresion.ArchivoProducto.Archivo, archivo);
                         //webClient.DownloadFile("http://localhost:62696/Archivos/Brazaletes/asd3.txt", archivo);

                         StringBuilder contenidoEtiqueta = new StringBuilder();
                         contenidoEtiqueta.Append(System.IO.File.ReadAllText(archivo, Encoding.GetEncoding(1252)));
                         //contenidoEtiqueta.Replace(ConfigurationManager.AppSettings["TagCodigoBarras"].ToString(), item.CodBarraInicio);
                         contenidoEtiqueta.Replace(ConfigurationManager.AppSettings["TagCodigoBarras"].ToString(), item.CodBarraInicio);

                         respuesta = PrintDirect.PrintText(contenidoEtiqueta.ToString(), 0);

                         //Aqui se va a llevar el control
                         //restantesBoletas = restantesBoletas - 1;
                         //MensajeBoletasRestantes = await GetAsync<string>($"Pos/UpdateControlBoleteria/{idBoleta}/{restantesBoletas}");
                         //

                         if (!string.IsNullOrWhiteSpace(respuesta))
                         {
                             //Utilidades.RegistrarError(new Exception("Error al imprimir"), respuesta);
                         }
                     }
                 }
             }
             return _modelo;
         }*/
        #endregion
        public string editar(List<Producto> list)
        {
            foreach (var item in list)
            {
                item.Nombre = "se cambio el nombre";
                item.Cantidad = 999999;
            }
            return "Exitoso";
        }
    }
}