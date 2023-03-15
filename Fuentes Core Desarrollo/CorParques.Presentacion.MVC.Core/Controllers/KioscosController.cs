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
                    var rta = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoBoleta/{codBarras}/{IdUsuarioLogueado}");

                    if (rta.Mensaje.Contains("boleta no"))
                        ViewBag.ImpresionEnLinea = false;
                    else
                        ViewBag.ImpresionEnLinea = true;

                    ViewBag.Consecutivos = Codigo;
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
                    .Where(x => x.CodSapTipoProducto== "2000" || x.CodSapTipoProducto == "2005").ToList();
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
            string head = string.Empty;
            string nombrePasaporte = string.Empty;
            string content = string.Empty;
            int restantes = await ConsultarRestantes();
            int boletasValidas = 0;
            if (Codigo.StartsWith("BC"))
            {
                string codBoletaCtrl = Codigo.Replace("BC", "");
                var l_productos = await GetAsync<List<Producto>>($"Pos/VerPasaportesCodigoPedido/{codBoletaCtrl}");
                var pedido = await GetAsync<ConsultaMovimientoBoletaControl>($"Boleteria/ConsultaMovimientoBoletaControl/{codBoletaCtrl}");
                byte pasaportes = 0, tickets = 0;

                foreach (var item in l_productos)
                {
                    if (item.CodSapTipoProducto == "2000")
                        pasaportes += 1;
                    else
                        tickets += 1;
                }
                head = "Tu Boleta";
                nombrePasaporte = "0" + codBoletaCtrl + ", Pedido " + pedido.CodSapPedido.ToString();
                content = "Se han de imprimir " + pasaportes + " pasaportes y " + tickets + " comprobantes de redención";
                ViewBag.BolControl = pedido.NombreCliente;
                ViewBag.Consecutivos = Codigo;

            }
            else if (Codigo.StartsWith("Blt"))
            {
                string consecutivo = Codigo.Replace("Blt", "");
                var impValida = await ValidarImpresion(consecutivo);
                if (impValida.Equals("")) boletasValidas += 1;
                Producto boletaProducto = await GetAsync<Producto>($"Boleteria//CambioboletaDato/{consecutivo}");
                var l_tiposProducto = await GetAsync<List<TipoGeneral>>($"TipoProducto/ObtenerListaTipoProduto");
                var tipo = l_tiposProducto.Where(x => x.CodSAP == boletaProducto.CodSapTipoProducto).First();
                nombrePasaporte = boletaProducto.Nombre;
                head = tipo.Nombre.ToLower();
                if (boletaProducto.CodSapTipoProducto == "2000" || boletaProducto.CodSapTipoProducto == "2005"
                    || boletaProducto.CodSapTipoProducto == "2010" || boletaProducto.CodSapTipoProducto == "2015")
                    content = "En la fecha indicada podrá hacer uso de las atracciones y/o destrezas incluidas";
                else
                    content = "Por favor acérquese a los puntos correspondientes para reclamar su producto";
                ViewBag.Consecutivos = Codigo;
            }
            else if (Codigo.StartsWith("FC|"))
            {
                var CodFactura = Codigo.Replace("FC|", "");
                var factura = await GetAsync<Factura>($"Pos/ObtenerFactura/{CodFactura}");
                DescargueBoletaControl descargueFactura = await GetAsync<DescargueBoletaControl>($"Pos/ObtenerListaProductoFactura/{CodFactura}");
                byte pasaportes = 0, tickets = 0;

                foreach (var item in descargueFactura.Productos)
                {
                    if (item.CodSapTipoProducto == "2000")
                        pasaportes += 1;
                    else
                        tickets += 1;
                }
                head = "Tu Factura";
                nombrePasaporte = "0" + CodFactura + " Emitida " + factura.FechaCreacion.ToString("dd MMMM yyyy");
                content = "Se han de imprimir " + pasaportes + " pasaportes y " + tickets + " comprobantes de redención";
                ViewBag.Consecutivos = Codigo;
            }

            if (restantes < boletasValidas)
                ViewBag.Mensaje = "Existen solamente " + restantes + " boletas disponibles";

            ViewBag.Head = head;
            ViewBag.Nombre = nombrePasaporte;
            ViewBag.Content = content;
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
            //var rta = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{producto.CodigoSap}");//
            string strMensaje = string.Empty;

            if (boleta != null && producto != null)
            {
                //if (producto.CodSapTipoProducto != "2000") strMensaje = "El producto no corresponde a un brazalete para impresion en linea";//
                //if (rta.Nombre == "No existe el producto o es exeption") strMensaje = "Invalido impresión en linea";//
                if (producto.IdEstado != 1) strMensaje = "Producto inactivo";
                if (boleta.IdEstado != 1 && boleta.IdEstado != 2)
                    strMensaje = "Estado boleta Invalida";
                if (boleta.IdEstado == 1 && (DateTime.Now < boleta.FechaUsoInicial) || (DateTime.Now > boleta.FechaUsoFinal))
                    strMensaje = "La boleta no tiene vigencia";
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
                var rta = await DescargueBoleta(codBoletaCtrl);
                return rta;
            }
            else if (Codigo.StartsWith("Blt"))
            {
                JsonResult rta = null;
                var consecutivo = Codigo.Replace("Blt", "");
                Producto productoBoleta = await GetAsync<Producto>($"Boleteria/CambioboletaDato/{consecutivo}");
                var productoImpresionEnLinea = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{productoBoleta.CodigoSap}");

                if (productoImpresionEnLinea.Nombre != "No existe el producto o es exeption")
                    rta = await CambiarImprimirBoleta(consecutivo);
                else
                {
                    productoBoleta.CodBarraInicio = consecutivo;
                    productoBoleta.Cantidad = 1;
                    rta = ImprimirTicketBoleteria(productoBoleta);
                }
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

        private JsonResult ImprimirTicketBoleteria(Producto productoBoleta)
        {
            RespuestaViewModel rta = new RespuestaViewModel();
            ServicioImprimir objImprimir = new ServicioImprimir();
            TicketImprimir objTicket = new TicketImprimir();

            objTicket.TituloRecibo = "Soporte redencion";
            objTicket.CodigoBarrasProp = productoBoleta.CodBarraInicio;
            objTicket.TituloColumnas = "Valido para|Cant";
            objTicket.ListaArticulos = new List<Articulo>();
            objTicket.ListaArticulos.Add(new Articulo()
            {
                Nombre = productoBoleta.Nombre,
                Cantidad = productoBoleta.Cantidad,
                Precio = productoBoleta.Precio,
                TituloColumnas = "Valido para|Cant"
            });
            objTicket.Usuario = NombreUsuarioLogueado;

            try
            {
                objImprimir.ImprimirUsoAtraccionDestreza(objTicket);
                rta.Correcto = true;
            }
            catch (Exception e)
            {
                Utilidades.RegistrarError(e, string.Concat(this.GetType().Name, "//"
                               , System.Reflection.MethodBase.GetCurrentMethod().Name));
                rta.Correcto = false;
                rta.Mensaje = "Error imprimiendo ticket";
                rta.Elemento = productoBoleta;
            }

            return Json(rta, JsonRequestBehavior.AllowGet);
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

        private async Task<JsonResult> DescargueBoleta(string codBoletaCtrl)
        {
            ImpresionEnLinea impresion = new ImpresionEnLinea();
            PosController pos = new PosController();
            List<Producto> lp = new List<Producto>();
            List<ImpresionEnLineaConsecutivos> listConsecutivos = new List<ImpresionEnLineaConsecutivos>();
            RespuestaViewModel respuestaViewModel = new RespuestaViewModel();
            //ImprimirBoletaControl modelo = new ImprimirBoletaControl();

            IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
            var productosImpresion = await GetAsync<List<Producto>>($"Pos/VerPasaportesCodigoPedido/{codBoletaCtrl}");
            impresion.listaProductos = productosImpresion.OrderBy(x => x.CodSapTipoProducto == "2000").ToList();

            foreach (var item in productosImpresion)
            {
                Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSap}");
                if (item.CodSapTipoProducto == "2000" && temp.Nombre != "No existe el producto o es exeption")
                {
                    Producto producto = new Producto();
                    item.AplicaImpresionLinea = true;
                    Producto productoImpresion = brazaletes.Where(x => x.CodigoSap == item.CodigoSap).First();  //
                    productoImpresion.IdUsuarioModificacion = IdUsuarioLogueado;                                //
                    var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoImpresion);
                    impresion.producto = item;
                    producto = await pos.asignarBoleta(impresion, item, temp, brazaletes, respuesta);
                    if (producto != null)
                    {
                        ImpresionEnLineaConsecutivos consecutivoAdicion = new ImpresionEnLineaConsecutivos
                        {
                            idProducto = producto.IdProducto,
                            consecutivos = new string[1],
                            primero = producto.CodBarraInicio
                        };
                        consecutivoAdicion.consecutivos[0] = producto.CodBarraInicio;
                        listConsecutivos.Add(consecutivoAdicion);
                    }
                    lp.Add(item);
                }
                else
                {
                    item.UsuarioCreacion = IdUsuarioLogueado.ToString();
                    item.IdUsuarioModificacion = IdUsuarioLogueado;
                    lp.Add(item);
                }
            }
            impresion.listConsecutivos = listConsecutivos;

            if (lp.Count() == productosImpresion.Count())
            {
                ImpresionEnLinea registroRollo = new ImpresionEnLinea();
                List<Producto> l_productos = new List<Producto>();
                int restantes = await ConsultarRestantes();
                ImprimirBoletaControl modelo = new ImprimirBoletaControl
                {
                    ListaProductos = productosImpresion,
                    CodBarraInicio = codBoletaCtrl,
                    CodBarraFinal = "",
                    IdUsuario = IdUsuarioLogueado
                };

                var rta = await PostAsync<ImprimirBoletaControl, RedencionBoletaControl>("Pos/ObtenerCodBarrasBoletaControl", modelo);//
                if (rta.Correcto)
                {
                    foreach (var productoImpEnLinea in productosImpresion.Where(x => x.AplicaImpresionLinea))
                    {
                        Producto productoImpresion = brazaletes.Where(x => x.CodigoSap == productoImpEnLinea.CodigoSap).First();
                        var rtaImpresion = imprimirImpresionEnLinea(productoImpEnLinea, productoImpresion, productoImpEnLinea.CodBarraInicio);
                        if (!rtaImpresion.Equals("")) respuestaViewModel.Elemento += productoImpEnLinea.CodBarraInicio + "|";
                        else
                        {
                            restantes -= 1;
                            l_productos.Add(productoImpEnLinea);
                        }
                    }

                    var objRespuesta = (RedencionBoletaControl)rta.Elemento;
                    if (objRespuesta.modeloImprimir != null)
                    {
                        ServicioImprimir objImprimir = new ServicioImprimir();
                        foreach (var ticket in objRespuesta.modeloImprimir)
                        {
                            var objTicket = new TicketImprimir();
                            objTicket.TituloRecibo = ticket.TituloRecibo;
                            objTicket.CodigoBarrasProp = ticket.CodigoBarrasProp;
                            objTicket.TituloColumnas = ticket.TituloColumnas;
                            objTicket.ListaArticulos = new List<Articulo>();
                            objTicket.ListaArticulos.Add(new Articulo() { Nombre = ticket.Nombre, Cantidad = 1, Precio = ticket.Precio, TituloColumnas = ticket.TituloColumnas });
                            objImprimir.ImprimirCupoDebito(objTicket);
                        }
                    }
                }

                await RegistrarControlBoleteria(restantes);
                if (respuestaViewModel.Elemento == null)
                {
                    registroRollo.listaProductos = l_productos;
                    //await pos.registrarRolloInventario(registroRollo, IdUsuarioLogueado);
                    respuestaViewModel.Correcto = true;
                }
                else
                {
                    respuestaViewModel.Correcto = false;
                    respuestaViewModel.Mensaje = "Error imprimiendo los pasaportes en linea";
                }
            }
            return Json(respuestaViewModel, JsonRequestBehavior.AllowGet);
        }

        public string pruebaImpError()
        {
            return "Error imprimiendo en linea";
            //return "";
        }
    }
}