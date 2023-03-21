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
    public class DescargueMaterialController : ControladorBase
    {
        public static string Factura = string.Empty;
        public bool FacturaVirtual = false;
        DescargueBoletaControl ListaProductosADescargar = new DescargueBoletaControl();

        // GET: DescargueMaterial
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ObtenerProductos(string CodBarra)
        {
            var rta = new DescargueBoletaControl();
            var ProdMostrar = new DescargueBoletaControl();

            bool bandera;
            int numericValue;
            bool isNumber = int.TryParse(CodBarra, out numericValue);
            if (isNumber) //Si es numero, es IdDetalleFactura o Boletería
            {
                rta = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoFactura/{CodBarra}");
                if (rta == null)
                {
                    ListaProductosADescargar.Productos = ProdMostrar.Productos;
                    ListaProductosADescargar.Mensaje = rta.Mensaje;
                    bandera = false;
                }
                else
                {
                    ViewBag.Mensaje = rta.Mensaje;
                    ListaProductosADescargar.Productos = rta.Productos;
                    ListaProductosADescargar.Mensaje = rta.Mensaje;
                    bandera = true;
                }
            }
            else //Si es AlfaNumerico, es el CodigoFactura
            {
         
                rta = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoFactura/{CodBarra}");
                if (rta == null)
                {
                    //ListaProductosADescargar.Mensaje = "Error en la lectura del codigo, vuelva a intentarlo.";
                    ListaProductosADescargar.Productos = ProdMostrar.Productos;
                    ListaProductosADescargar.Mensaje = "Factura no valida";
                    bandera = false;
                }
                else
                {
                    ViewBag.Mensaje = rta.Mensaje;
                    Factura = CodBarra;
                    ListaProductosADescargar.Productos = rta.Productos;
                    ListaProductosADescargar.Mensaje = rta.Mensaje;
                    bandera = true;
                }
            }

            if (bandera)
            {
                var InfoPunto = await GetAsync<Puntos>($"Puntos/GetById/{IdPunto}"); //Consulta el Tipo de Punto, para discriminar los productos a entregar. 
                //var ProdMostrar = new List<Producto>();
                switch (InfoPunto.IdTipoPunto)
                {
                    case 3:  //Taquilla
                        ProdMostrar.Productos = ListaProductosADescargar.Productos.ToList(); //Muestra Todos los productos.
                        ProdMostrar.Mensaje = ListaProductosADescargar.Mensaje;
                        break;
                    case 4: //Punto AyB
                        ProdMostrar.Productos = ListaProductosADescargar.Productos.Where(x => x.CodSapTipoProducto == "2025").ToList();
                        ProdMostrar.Mensaje = ListaProductosADescargar.Mensaje;
                        break;
                    case 5: //Destreza
                        ProdMostrar.Productos = ListaProductosADescargar.Productos.Where(x => x.CodSapTipoProducto == "2015").ToList();
                        ProdMostrar.Mensaje = ListaProductosADescargar.Mensaje;
                        break;
                    case 7: //Almacenes & Souvenir
                        ProdMostrar.Productos = ListaProductosADescargar.Productos.Where(x => x.CodSapTipoProducto == "2030").ToList();
                        ProdMostrar.Mensaje = ListaProductosADescargar.Mensaje;
                        break;
                    case 8: //ARAZA
                        ProdMostrar.Productos = ListaProductosADescargar.Productos.Where(x => x.CodSapTipoProducto == "2065").ToList();
                        ProdMostrar.Mensaje = ListaProductosADescargar.Mensaje;
                        break;
                    case 9: //Mundo Natural
                        ProdMostrar.Productos = ListaProductosADescargar.Productos.Where(x => x.CodSapTipoProducto == "2045" || x.CodSapTipoProducto == "2025").ToList();
                        ProdMostrar.Mensaje = ListaProductosADescargar.Mensaje;
                        break;
                }
            }
            else
            {
                ProdMostrar.Cantidad = 0;
                ProdMostrar.Mensaje = ListaProductosADescargar.Mensaje;
            }
            //return PartialView("_ListaProductos", rta.Productos.Count() == 0 ? null : rta.Productos);
            return PartialView("_ListaProductos", ProdMostrar.Productos);
        }

        public async Task<JsonResult> Descargue(List<Producto> Productos, string Factura)
        {
            if (Factura.Contains("VIRT"))
            { FacturaVirtual = true; }
            else if (Factura.Contains("C106"))
            { FacturaVirtual = true; }

            var InfoPunto = await GetAsync<Puntos>($"Puntos/GetById/{IdPunto}"); //Consulta el Tipo de Punto, para discriminar los productos a entregar.            
            List<Producto> ProImpri = new List<Producto>();
            /*Descargar Productos*/
            //Filtran solo los entregados
            var _producto = Productos.Where(x => x.Entregado);
            var producto = _producto.First();
            foreach (var item2 in _producto)
            {
                item2.IdUsuarioModificacion = IdUsuarioLogueado;
                item2.IdEstado = (int)Enumerador.Estados.Entregado;
                item2.IdPuntoDescarga = IdPunto;
            }
            var rta = await PostAsync<List<Producto>, string>("Pos/DescargueBoletaFactura", _producto.ToList()); //Metodo que genera la descarga
            ProImpri = _producto.Where(x => x.Entregado == true).ToList();

            //Descargue de inventario
            if (rta.Correcto)
            {
                var _listaDescarga = _producto.Where(x => x.Entregado); //// AYB aplica para todos los productos
                if (_listaDescarga.Count() > 0)
                {
                    foreach (var item in _listaDescarga)
                        item.IdDetalleProducto = 0;
                    Inventario inventario = new Inventario();
                    inventario.FechaInventario = Utilidades.FechaActualColombia;
                    inventario.IdPunto = IdPunto;
                    inventario.IdUsuarioCeado = IdUsuarioLogueado;
                    inventario.Productos = _listaDescarga; // AYB 
                    await PostAsync<Inventario, string>("Inventario/ActualizarInventario", inventario);
                }
            }
            /*------------Fin Descargue------------------------------*/

            if (FacturaVirtual == true)//Imprime papelito, solo si es factura de los puntos tiendaWeb y APP
            {
                string Nombre = "CodSapProdParqueadero";
                var SapParqueadero = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/{Nombre}");
                var idProductoAyB = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodigoSapTipoProductoAyB"); //Tipo producto alimentos y bebidas
                var idProductoSouvenir = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/SouveniresPOS"); // TipoProducto Souvenir
                var idProductoServicios = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodigoSapTipoProductoServicios"); //Tipo producto Servicios
                var idProductoParqueadero = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodigoSapTipoProductoParqueadero"); // parqueadero

                ServicioImprimir objImprimir = new ServicioImprimir();
                Factura facturaDetalleImprimir = await GetAsync<Factura>($"Pos/ObtenerListaProductoFactura/{Factura}");
                Factura ProductosFactura = await GetAsync<Factura>($"Pos/ObtenerListaProductoFactura/{Factura}");

                Parametro aplicaImpresionAyB = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AplicaImpresionAyB");
                if (aplicaImpresionAyB.Valor != "0")
                {
                    List<Producto> productoImpresionAyB = new List<Producto>();
                    productoImpresionAyB = ProImpri.Where(x => (x.CodSapTipoProducto == idProductoAyB.Valor.Split(',')[0] && x.Entregado == true)
                                                            || (x.CodSapTipoProducto == idProductoAyB.Valor.Split(',')[1] && x.Entregado == true)).ToList();

                    foreach (var item in productoImpresionAyB)
                    {
                        if (productoImpresionAyB != null)
                        {
                            TicketImprimir objAlimento = new TicketImprimir();
                            objAlimento.TituloRecibo = "Soporte Redención AyB"; //objAlimento.CodigoBarrasProp = string.Concat(item.IdDetalleFactura);
                            objAlimento.Usuario = $"Nombre: {@NombreUsuarioLogueado}";
                            objAlimento.TituloColumnas = "Producto redimido:|Cant: ";
                            objAlimento.ListaArticulos = new List<Articulo>();
                            objAlimento.ListaArticulos.Add(new Articulo()
                            {
                                Nombre = item.Nombre + "        |",
                                Cantidad = item.Cantidad,
                                Precio = item.Precio,
                                TituloColumnas = "Producto redimido:|Cant:  "
                            });
                            objImprimir.ImprimirCupoDebito(objAlimento);
                        }
                    }
                }


                Parametro aplicaImpresionSouvenir = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/aplicaImpresionSouvenir");
                if (aplicaImpresionSouvenir.Valor != "0")
                {
                    List<Producto> productoImpresionSouvenir = new List<Producto>();
                    //valor : 2025 --> idProducto: 685
                    productoImpresionSouvenir = ProImpri.Where(x => (x.CodSapTipoProducto == idProductoSouvenir.Valor.Split(',')[0] && x.Entregado == true)).ToList();

                    foreach (var item in productoImpresionSouvenir)
                    {
                        if (productoImpresionSouvenir != null)
                        {
                            TicketImprimir objSouvenir = new TicketImprimir();
                            objSouvenir.TituloRecibo = "Soporte Redención Souvenir"; //objAlimento.CodigoBarrasProp = string.Concat(item.IdDetalleFactura);
                            objSouvenir.Usuario = $"Nombre: {@NombreUsuarioLogueado}";
                            objSouvenir.TituloColumnas = "Producto redimido:|Cant: ";
                            objSouvenir.ListaArticulos = new List<Articulo>();
                            objSouvenir.ListaArticulos.Add(new Articulo()
                            {
                                Nombre = item.Nombre,
                                Cantidad = item.Cantidad,
                                Precio = item.Precio,
                                TituloColumnas = "Producto redimido:|Cant: "
                            });
                            objImprimir.ImprimirCupoDebito(objSouvenir);
                            //error = "Souvenir";
                        }
                    }
                }


                Parametro aplicaImpresionServicios = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AplicaImpresionServicios");  //Servicio paquetero
                if (aplicaImpresionServicios.Valor != "0")
                {
                    List<Producto> productoImpresionServicios = new List<Producto>();
                    productoImpresionServicios = ProImpri.Where(x => (x.CodSapTipoProducto == idProductoServicios.Valor)).ToList();

                    foreach (var item in productoImpresionServicios)
                    {
                        if (productoImpresionServicios != null)
                        {
                            TicketImprimir objServicios = new TicketImprimir();
                            objServicios.TituloRecibo = "Soporte Redención Servicios";
                            objServicios.Usuario = $"Nombre: {@NombreUsuarioLogueado}";
                            objServicios.TituloColumnas = "Producto redimido: | Cant: ";
                            objServicios.ListaArticulos = new List<Articulo>();
                            objServicios.ListaArticulos.Add(new Articulo()
                            {
                                Nombre = item.Nombre, //Nombre = productoImpresionDescarga.Nombre,
                                Cantidad = item.Cantidad,
                                Precio = item.Precio,
                                TituloColumnas = "Producto redimido:|Cant: "
                            });
                            objImprimir.ImprimirCupoDebito(objServicios);
                        }
                    }
                }


                Parametro aplicaImprersionParqueadero = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AplicaImpresionParqueadero");
                if (aplicaImprersionParqueadero.Valor != "0")
                {
                    List<Producto> productoImpresionParqueadero = new List<Producto>();
                    productoImpresionParqueadero = ProImpri.Where(x => (x.CodSapTipoProducto == idProductoParqueadero.Valor)).ToList();

                    foreach (var item in productoImpresionParqueadero)
                    {
                        if (productoImpresionParqueadero != null)
                        {
                            TicketImprimir objParqueadero = new TicketImprimir();
                            objParqueadero.TituloRecibo = "Soporte Redención Parqueadero";
                            objParqueadero.Usuario = $"Nombre: {@NombreUsuarioLogueado}";
                            objParqueadero.TituloColumnas = "Producto redimido:|Cant: ";
                            objParqueadero.ListaArticulos = new List<Articulo>();
                            objParqueadero.ListaArticulos.Add(new Articulo()
                            {
                                //Nombre = productoImpresionDescarga.Nombre,
                                Nombre = item.Nombre,
                                Cantidad = item.Cantidad,
                                Precio = item.Precio,
                                TituloColumnas = "Producto redimido:|Cant: "
                            });

                            objImprimir.ImprimirCupoDebito(objParqueadero);
                        }
                    }
                }
            }
            return Json(rta, JsonRequestBehavior.AllowGet);
        }
    }
}