using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using CorParques.Transversales.Util;
using System.IO;
using System.Net;
using System.Configuration;
using System.Text;
using CorParques.Presentacion.MVC.Core.Models;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class KioscosController : ControladorBase
    {
        // GET: Kioscos
        public ActionResult Index()
        {
            return View();
        }

        // GET: Kioscos/IngresoCodigo
        public ActionResult IngresoCodigo()
        {
            return View();
        }

        public async Task<ActionResult> Menu(string Codigo)
        {
            int bolRestantes = await ConsultarRestantes();

            if (bolRestantes == 0) ViewBag.ImpresionEnLinea = false;
            else
            {
                if (Codigo.StartsWith("BC"))
                {
                    var codBarras = Codigo.Replace("BC", "");
                    ViewBag.ImpresionEnLinea = false;
                    ViewBag.Consecutivos = Codigo;
                    //var rta = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoBoleta/{codBarras}/{IdUsuarioLogueado}");
                }
                else if (Codigo.StartsWith("Blt"))
                {
                    var consecutivo = Codigo.Replace("Blt", "");
                    var impEnLinea = await ValidarImpresion(consecutivo);
                    if (impEnLinea.Equals("")) ViewBag.ImpresionEnLinea = true;
                    else
                    {
                        ViewBag.ImpresionEnLinea = false;
                        ViewBag.Mensaje = impEnLinea;
                    }
                    ViewBag.Consecutivos = Codigo;
                }
                else if (Codigo.StartsWith("FC|"))
                {
                    string codFactura = Codigo.Replace("FC|", "");
                    var rta = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoFactura/{codFactura}");
                    var factura = await GetAsync<Factura>($"Pos/ObtenerFactura/{codFactura}");
                    if (rta != null && factura.IdEstado == 1) //&& (codFactura.Contains("VIRT") || codFactura.Contains("C106"))
                        ViewBag.ImpresionEnLinea = true;

                    else
                        ViewBag.ImpresionEnLinea = false;

                    ViewBag.Consecutivos = Codigo;
                }

            }
            return View();
        }

        public async Task<ActionResult> Consulta(string Codigo)
        {
            ConsultaKiosco consultas = new ConsultaKiosco();
            if (Codigo.StartsWith("Blt"))
            {
                ViewBag.Tipo = "General";
                var consecutivo = Codigo.Replace("Blt", "");
                var boleta = await GetAsync<Boleteria>($"Boleteria/ObtenerBoleta/{consecutivo}");
                var detalleBoleta = await GetAsync<DetalleBoleta>($"HistoricoBoleta/ObtenerHistoricoBoleta/{consecutivo}");
                consultas.DetalleBoleta = detalleBoleta;
                consultas.Boleta = boleta;
            }
            else if (Codigo.StartsWith("FC|"))
            {
                ViewBag.Tipo = "UnoUno";
                var CodFactura = Codigo.Replace("FC|", "");
                var factura = await GetAsync<Factura>($"Pos/ObtenerFactura/{CodFactura}");
                FacturaImprimir facturaImp = await GetAsync<FacturaImprimir>($"Pos/ObtenerFacturaImprimir/{factura.Id_Factura}");
                DescargueBoletaControl descargueBoleta = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoFactura/{CodFactura}");
                ViewBag.EstadoFactura = (Enumerador.Estados)factura.IdEstado;
                if (descargueBoleta == null) ViewBag.ImpFactura = "No disponible";
                else ViewBag.ImpFactura = "Disponible";
                consultas.Factura = factura;
                consultas.FacturaImprimir = facturaImp;
                var boletasFactura = factura.DetalleFactura
                    .Where(x => x.Id_Producto == 7681 || x.Id_Producto == 8291 || x.Id_Producto == 8292 || x.Id_Producto == 8293).ToList();
                if (boletasFactura.Count() > 0)
                {
                    consultas.Boleta = await GetAsync<Boleteria>($"Boleteria/GetById/{boletasFactura.First().IdDetalleProducto}");
                    if (consultas.Boleta != null)
                        consultas.DetalleBoleta = await GetAsync<DetalleBoleta>($"HistoricoBoleta/ObtenerHistoricoBoleta/{consultas.Boleta.Consecutivo}");
                }
            }
            else if (Codigo.StartsWith("BC"))
            {
                ViewBag.Tipo = "FechaAbierta";
                var CodBolCtrl = Codigo.Replace("BC", "");
                var pedido = await GetAsync<ConsultaMovimientoBoletaControl>($"Boleteria/ConsultaMovimientoBoletaControl/{CodBolCtrl}");
                consultas.BolControl = pedido;
            }
            return View(consultas);
        }

        public async Task<ActionResult> ImpresionBoleta(string Codigo)
        {
            string nombrePasaporte = string.Empty;
            int restantes = await ConsultarRestantes();
            int boletasValidas = 0;
            if (Codigo.StartsWith("BC"))
            {
                string codBoletaCtrl = Codigo.Replace("BC", "");
                var l_productos = await GetAsync<List<Producto>>($"Pos/VerPasaportesCodigoPedido/{codBoletaCtrl}");
                foreach (var item in l_productos)
                {
                    var rta = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSap}");
                    if (item.CodSapTipoProducto == "2000" && rta.Nombre != "No existe el producto o es exeption")
                    {
                        boletasValidas += 1;
                        nombrePasaporte = rta.Nombre.Replace("PASAPORTE ", "");
                    }
                }
                ViewBag.Consecutivos = Codigo;

            }
            else if (Codigo.StartsWith("Blt"))
            {
                string consecutivo = Codigo.Replace("Blt", "");
                var impValida = await ValidarImpresion(consecutivo);
                if (impValida.Equals("")) boletasValidas += 1;
                nombrePasaporte = await GetAsync<string>($"Boleteria/Cambioboleta/{consecutivo}");
                nombrePasaporte = nombrePasaporte.Replace("PASAPORTE ", "");
                ViewBag.Consecutivos = Codigo;
            }
            else if (Codigo.StartsWith("FC|"))
            {
                var CodFactura = Codigo.Replace("FC|", "");
                var factura = await GetAsync<Factura>($"Pos/ObtenerFactura/{CodFactura}");
                //var prodFactura = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoFactura/{CodFactura}"); // l productos
                //if (prodFactura != null)
                //    nombrePasaporte = prodFactura.Productos.First().Nombre.Replace("PASAPORTE ", "");
                //else
                //nombrePasaporte = "Ticket";
                #region Nombre a mostrar
                foreach (var item in factura.DetalleFactura)
                {
                    var bol = await GetAsync<Boleteria>($"Boleteria/GetById/{item.IdDetalleProducto}");
                    if (bol != null)
                    {
                        var producto = await GetAsync<Producto>($"Boleteria/CambioboletaDato/{bol.Consecutivo}");
                        if (producto != null && producto.CodSapTipoProducto == "2000")
                        {
                            nombrePasaporte = producto.Nombre.Replace("PASAPORTE ", "");
                            break;
                        }
                        else nombrePasaporte = "Ticket";
                    }
                }
                #endregion
                ViewBag.Consecutivos = Codigo;
            }

            if (restantes < boletasValidas)
                ViewBag.Mensaje = "Existen solamente " + restantes + " boletas disponibles";

            ViewBag.NombrePasaporte = nombrePasaporte;
            return View();
        }

        public async Task<JsonResult> ObtenerCodigoManual(string Codigo)
        {
            var boleta = await GetAsync<Boleteria>($"Boleteria/ObtenerBoleta/{Codigo}");
            string productovalido = await ValidarImpresion(Codigo);
            if (boleta != null)
            {
                if (productovalido.Equals("Producto invalido")) return null;
                else
                {
                    var redirectToUrl = new { redirectToUrl = "Menu?Codigo=Blt" + Codigo };
                    return Json(redirectToUrl, JsonRequestBehavior.AllowGet);
                }
            }
            else return null;
        }

        public async Task<JsonResult> ObtenerCodigoBarras(string CodBarras)
        {
            string codigo = "";
            var boleta = await GetAsync<Boleteria>($"Boleteria/ObtenerBoleta/{CodBarras}");
            if (boleta != null) codigo = "Blt" + CodBarras;
            else
            {
                var factura = await GetAsync<Factura>($"Pos/ObtenerFactura/{CodBarras}");
                if (factura != null) codigo = "FC|" + CodBarras;
                else
                {
                    var rta = await GetAsync<List<Producto>>($"Pos/VerPasaportesCodigoPedido/{CodBarras}");
                    if (rta != null && rta.Count() != 0) codigo = "BC" + CodBarras;
                }
            }
            if (codigo == "") return null; ;
            var redirectToUrl = new { redirectToUrl = "Menu?Codigo=" + codigo };

            return Json(redirectToUrl, JsonRequestBehavior.AllowGet);
        }

        private async Task<string> ValidarImpresion(string Consecutivo)
        {
            var boleta = await GetAsync<Boleteria>($"Boleteria/ObtenerBoleta/{Consecutivo}");
            var producto = await GetAsync<Producto>($"Boleteria/CambioboletaDato/{Consecutivo}");
            var rta = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{producto.CodigoSap}");
            string strMensaje = string.Empty;

            if (boleta != null && producto != null)
            {
                if (producto.CodSapTipoProducto != "2000") strMensaje = "El producto no corresponde a un brazalete para impresion en linea";
                if (rta.Nombre == "No existe el producto o es exeption") strMensaje = "Invalido impresión en linea";
                if (producto.IdEstado != 1) strMensaje = "Pasaporte inactivo";
                if (boleta.IdEstado != 2 && (boleta.IdEstado != 1 && (DateTime.Today < boleta.FechaUsoInicial) || (DateTime.Today > boleta.FechaUsoFinal)))
                    strMensaje = "Estado boleta invalida";
                if (DateTime.Now < boleta.FechaInicioEvento || DateTime.Now > boleta.FechaFinEvento) strMensaje = "La boleta no tiene vigencia";
            }
            else strMensaje = "Producto invalido";
            return strMensaje;
        }

        public async Task<JsonResult> Imprimir(string Codigo)
        {
            if (Codigo.StartsWith("BC"))
            {
                string codBoletaCtrl = Codigo.Replace("BC", "");
                //Imprimir bol boleta control
                return null;
            }
            else if (Codigo.StartsWith("Blt"))
            {
                var consecutivo = Codigo.Replace("Blt", "");
                var rta = await CambiarImprimirBoleta(consecutivo);
                //var rta = new {Correcto = false, Mensaje= "Prueba", Elemento= consecutivo };
                //return Json(rta, JsonRequestBehavior.AllowGet);
                return rta;
            }
            else if (Codigo.StartsWith("FC|"))
            {
                var codFactura = Codigo.Replace("FC|", "");
                var rta = await DescargueFactura(codFactura);
                return rta;
            }
            else return null;
        }

        public async Task<JsonResult> CambiarImprimirBoleta(string Codigo)
        {
            PosController pos = new PosController();
            ImpresionEnLinea impresion = new ImpresionEnLinea();
            ImpresionEnLinea registroRollo = new ImpresionEnLinea();
            List<Producto> l_productos = new List<Producto>();
            int restantes = await ConsultarRestantes();
            List<string> consecutivos = Codigo.Split('|').ToList();
            consecutivos.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            string resultado = string.Empty;
            RespuestaViewModel respuestaViewModel = new RespuestaViewModel();

            foreach (var consecutivo in consecutivos)
            {
                string consecutivoNuevo = string.Empty;
                if (restantes > 0)
                {
                    string impresionValida = await ValidarImpresion(consecutivo);
                    if (impresionValida.Equals(""))
                    {
                        Producto p_boletaNueva = new Producto();
                        IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
                        Producto productoBoleta = await GetAsync<Producto>($"Boleteria/CambioboletaDato/{consecutivo}");
                        impresion.CambioBoleta = productoBoleta;
                        Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{productoBoleta.CodigoSap}");
                        Producto productoImpresion = new Producto();

                        if (temp != null)
                            if (temp.Nombre == productoBoleta.Nombre && temp.CodigoSap == productoBoleta.CodigoSap)
                            {
                                productoBoleta.AplicaImpresionLinea = true;
                                productoImpresion = brazaletes.Where(x => x.CodigoSap == productoBoleta.CodigoSap).First();

                                if (productoImpresion != null)
                                {
                                    productoImpresion.IdUsuarioModificacion = IdUsuarioLogueado;
                                    var rta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoImpresion);
                                    if (rta.Correcto)
                                    {
                                        List<string> boletaN = rta.Elemento.ToString().Split('|').ToList();
                                        consecutivoNuevo = boletaN[1];

                                        p_boletaNueva = await pos.asignarCambioBoleta(impresion, productoBoleta, temp, brazaletes, rta);
                                        await GetAsync<string>($"Boleteria/UpdateEstadoCambioboleta/{p_boletaNueva.CodBarraInicio}");
                                        impresion.CambioBoleta = p_boletaNueva;
                                    }
                                }
                            }

                        var rtaAsignacion = await AsignarBoleta(consecutivo, consecutivoNuevo);
                        if (rtaAsignacion != null)
                        {
                            //var rtaImpresion = pos.imprimirImpresionEnLinea(p_boletaNueva,productoImpresion,p_boletaNueva.CodBarraInicio);
                            string rtaImpresion = imprimirImpresionEnLinea(p_boletaNueva, productoImpresion, p_boletaNueva.CodBarraInicio);
                            //string rtaImpresion = pruebaImpError(); //
                            if (!rtaImpresion.Equals("")) resultado += " Error al imprimir. " + rtaImpresion;
                            else
                            {
                                restantes -= 1;
                                l_productos.Add(p_boletaNueva);
                            }
                        }
                        else resultado += "Error al cambiar boleta " + consecutivo + " -> " + consecutivoNuevo;
                    }
                    else resultado += impresionValida;
                }

                if (!resultado.Equals(""))
                {
                    var bolGenerada = await GetAsync<Boleteria>($"Boleteria/ObtenerBoleta/{consecutivoNuevo}");
                    bolGenerada.IdEstado = 2;
                    await PutAsync<Boleteria, string>($"Boleteria/Actualizar", bolGenerada);
                    respuestaViewModel.Mensaje = resultado;
                    respuestaViewModel.Elemento += consecutivoNuevo + "|";
                }
            }

            await RegistrarControlBoleteria(restantes);
            if (l_productos.Count() != 0)
            {
                registroRollo.listaProductos = l_productos;
                await pos.registrarRolloInventario(registroRollo, IdUsuarioLogueado);
            }

            if (respuestaViewModel.Elemento == null) respuestaViewModel.Correcto = true;
            else respuestaViewModel.Correcto = false;

            return Json(respuestaViewModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> EnviarCorreo(SolicitudCodigo modelo)
        {
            var rta = await PostAsync<SolicitudCodigo, string>($"Cortesia/SolicitudCodConfirmacion", modelo);
            return Json(rta, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> EnviarCodConfirmacion(int codConfirmacion)
        {
            var rta = await GetAsync<SolicitudCodigo>($"Cortesia/ConsultarCodConfirmacion/{codConfirmacion}");
            return Json(rta, JsonRequestBehavior.AllowGet); ;
        }
        public async Task<string> AsignarBoleta(string codigo1, string codigo2)
        {
            var modelo = new List<Boleteria>();
            modelo.Add(new Boleteria
            {
                Consecutivo = codigo1.Trim(),
                IdUsuarioCreacion = IdUsuarioLogueado,
                IdProducto = IdPunto
            });
            modelo.Add(new Boleteria
            {
                Consecutivo = codigo2.Trim(),
                IdUsuarioCreacion = IdUsuarioLogueado
            });

            var rta = await PostAsync<List<Boleteria>, string>("Boleteria/CambiarBoleta", modelo);
            if (rta.Correcto) return rta.Elemento.ToString();
            else return null;
        }

        public string imprimirImpresionEnLinea(Producto item, Producto productoImpresion, string consecutivo)
        {
            string respuesta = "";
            try
            {
                if (item.AplicaImpresionLinea && productoImpresion.ArchivoProducto.Archivo != null)
                {
                    string archivo = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + "Temp", productoImpresion.ArchivoProducto.Archivo);
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(ConfigurationManager.AppSettings["RutaArchivos"].ToString() + productoImpresion.ArchivoProducto.Archivo, archivo);

                    StringBuilder contenidoEtiqueta = new StringBuilder();
                    contenidoEtiqueta.Append(System.IO.File.ReadAllText(archivo, Encoding.GetEncoding(1252)));
                    contenidoEtiqueta.Replace(ConfigurationManager.AppSettings["TagCodigoBarras"].ToString(), consecutivo);

                    respuesta = PrintDirect.PrintText(contenidoEtiqueta.ToString(), 0);

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

        private async Task<int> ConsultarRestantes()
        {
            #region pasaportes activos
            IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
            int restantes = 0;
            foreach (var brazalete in brazaletes)
            {
                var restantesTipo = await GetAsync<string>($"Pos/ConsultarControlBoleteria/{brazalete.IdProducto}");
                restantes += int.Parse(restantesTipo);
            }
            #endregion

            return restantes;
        }

        private async Task RegistrarControlBoleteria(int nBoletasRestantes)
        {
            #region brazaletes activos y validos para impresion en linea
            IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
            List<int> l_validos = new List<int>();
            foreach (var item in brazaletes)
            {
                var temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSap}");
                if (!temp.Nombre.Contains("No existe el producto o es exeption")) l_validos.Add(item.IdProducto);
            }
            int porbrazalete = nBoletasRestantes / l_validos.Count;
            int resi = nBoletasRestantes % l_validos.Count;
            for (int i = 0; i < l_validos.Count; i++)
            {
                if (resi != 0 && i == 0) await GetAsync<string>($"Pos/UpdateControlBoleteria/{l_validos[i]}/{porbrazalete + resi}");
                else await GetAsync<string>($"Pos/UpdateControlBoleteria/{l_validos[i]}/{porbrazalete}");
            }
            #endregion
        }

        public async Task<JsonResult> Reimprimir(string Codigo) //Sí existio error
        {
            PosController pos = new PosController();
            int restantes = await ConsultarRestantes();
            string rtaImpresion = string.Empty;
            string consecutivosError = string.Empty;
            ImpresionEnLinea registroRollo = new ImpresionEnLinea();
            List<Producto> l_productos = new List<Producto>();
            List<string> consecutivos = Codigo.Split('|').ToList();
            consecutivos.RemoveAll(x => string.IsNullOrWhiteSpace(x));

            foreach (var consecutivo in consecutivos)
            {
                Producto productoBoleta = await GetAsync<Producto>($"Boleteria/CambioboletaDato/{consecutivo}");
                productoBoleta.AplicaImpresionLinea = true;
                IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
                Producto productoImpresion = brazaletes.Where(x => x.CodigoSap == productoBoleta.CodigoSap).First();

                rtaImpresion = imprimirImpresionEnLinea(productoBoleta, productoImpresion, productoBoleta.CodBarraInicio);
                if (!rtaImpresion.Equals("")) consecutivosError += consecutivo + "|";
                else
                {
                    restantes -= 1;
                    l_productos.Add(productoBoleta);
                    var bolGenerada = await GetAsync<Boleteria>($"Boleteria/ObtenerBoleta/{consecutivo}");
                    bolGenerada.IdEstado = 1;
                    await PutAsync<Boleteria, string>($"Boleteria/Actualizar", bolGenerada);
                }
            }

            await RegistrarControlBoleteria(restantes);

            if (rtaImpresion.Equals(""))
            {
                registroRollo.listaProductos = l_productos;
                await pos.registrarRolloInventario(registroRollo, IdUsuarioLogueado);
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = rtaImpresion, Elemento = consecutivosError }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> DescargueFactura(string Codigo)
        {
            List<Producto> ProductosImpresion = new List<Producto>();
            RespuestaViewModel respuesta = new RespuestaViewModel();
            ServicioImprimir objImprimir = new ServicioImprimir();
            PosController pos = new PosController();
            List<Producto> l_productos = new List<Producto>();
            ImpresionEnLinea registroRollo = new ImpresionEnLinea();
            DescargueBoletaControl descargueBoleta = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoFactura/{Codigo}");
            Factura factura = await GetAsync<Factura>($"Pos/ObtenerFactura/{Codigo}");
            var l_detalleFactura = factura.DetalleFactura.ToList();

            if (descargueBoleta != null)
            {
                foreach (var item in descargueBoleta.Productos)
                {
                    item.IdEstado = (int)Enumerador.Estados.Entregado;
                    item.IdPuntoDescarga = IdPunto;
                    item.Entregado = true;
                }
                var rta = await PostAsync<List<Producto>, string>("Pos/DescargueBoletaFactura", descargueBoleta.Productos.ToList()); //Campo Entregado en BD
                ProductosImpresion = descargueBoleta.Productos.Where(x => x.Entregado == true).OrderBy(x => x.CodSapTipoProducto == "2000").ToList();

                foreach (var item in ProductosImpresion.Where(x => x.CodSapTipoProducto != "2000" && x.CodSapTipoProducto != "2015"))
                {
                    TicketImprimir objTicket = new TicketImprimir();
                    objTicket.TituloRecibo = "Soporte redención";
                    //objTicket.Usuario = $"Nombre: {@NombreUsuarioLogueado}";
                    objTicket.TituloColumnas = "Valido para|Cant:";
                    //objTicket.CodigoBarrasProp = item2.IdDetalleFactura.ToString();
                    objTicket.CodigoBarrasProp = item.IdDetalleProducto.ToString();
                    objTicket.ListaArticulos = new List<Articulo>();
                    objTicket.ListaArticulos.Add(new Articulo()
                    {
                        Nombre = item.Nombre + "        |",
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                        TituloColumnas = "Valido para|Cant:  "
                    });
                    objImprimir.ImprimirCupoDebito(objTicket);
                }

                int restantes = await ConsultarRestantes();

                IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
                foreach (var item in ProductosImpresion.Where(x => x.CodSapTipoProducto == "2000" || x.CodSapTipoProducto == "2015"))
                {
                    //var pasaportesFactura = factura.DetalleFactura.Where(x => x.Id_Producto == item.IdProducto);
                    foreach (var item2 in l_detalleFactura.ToList())
                    {
                        if (item.IdDetalleProducto == item2.IdDetalleFactura) //item.IdProducto == item2.Id_Producto && 
                        {
                            if (item.CodSapTipoProducto == "2000")
                            {
                                //Imprime pasaportes
                                l_detalleFactura.Remove(item2); //
                                Boleteria boleta = await GetAsync<Boleteria>($"Boleteria/GetById/{item2.IdDetalleProducto}");
                                item.AplicaImpresionLinea = true;
                                Producto productoImpresion = brazaletes.Where(x => x.CodigoSap == item.CodigoSap).First();

                                var rtaImpresion = imprimirImpresionEnLinea(item, productoImpresion, boleta.Consecutivo);
                                if (!rtaImpresion.Equals("")) respuesta.Elemento += boleta.Consecutivo + "|";
                                else
                                {
                                    restantes -= 1;
                                    l_productos.Add(item);
                                }
                            }
                            else
                            {
                                //Imprime formato de uso atracciones y destrezas.
                                Boleteria boleta = await GetAsync<Boleteria>($"Boleteria/GetById/{item2.IdDetalleProducto}");
                                TicketImprimir objTicket = new TicketImprimir();
                                objTicket.TituloRecibo = "Soporte redencion";
                                objTicket.CodigoBarrasProp = boleta.Consecutivo;
                                objTicket.TituloColumnas = "Valido para|Cant";
                                objTicket.ListaArticulos = new List<Articulo>();
                                objTicket.ListaArticulos.Add(new Articulo()
                                {
                                    Nombre = item.Nombre,
                                    Cantidad = item.Cantidad,
                                    Precio = item.Precio,
                                    TituloColumnas = "Valido para|Cant"
                                });
                                objTicket.Usuario = NombreUsuarioLogueado;
                                objImprimir.ImprimirUsoAtraccionDestreza(objTicket);
                                l_detalleFactura.Remove(item2); //

                            }
                        }
                    }

                }

                if (rta.Correcto)
                {
                    var _listaDescarga = descargueBoleta.Productos.Where(x => x.Entregado);
                    if (_listaDescarga.Count() > 0)
                    {
                        foreach (var item in _listaDescarga)
                            item.IdDetalleProducto = 0;
                        Inventario inventario = new Inventario();
                        inventario.FechaInventario = Utilidades.FechaActualColombia;
                        inventario.IdPunto = IdPunto;
                        inventario.IdUsuarioCeado = IdUsuarioLogueado;
                        inventario.Productos = _listaDescarga;
                        await PostAsync<Inventario, string>("Inventario/ActualizarInventario", inventario);
                    }
                }

                await RegistrarControlBoleteria(restantes);
                if (respuesta.Elemento == null)
                {
                    registroRollo.listaProductos = l_productos;
                    await pos.registrarRolloInventario(registroRollo, IdUsuarioLogueado);
                    respuesta.Correcto = true;
                }
                else
                {
                    respuesta.Correcto = false;
                    respuesta.Mensaje = "Error imprimiendo pasaportes en linea";
                }
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        public string pruebaImpError()
        {
            return "Error imprimiendo en linea";
            //return "";
        }
    }
}