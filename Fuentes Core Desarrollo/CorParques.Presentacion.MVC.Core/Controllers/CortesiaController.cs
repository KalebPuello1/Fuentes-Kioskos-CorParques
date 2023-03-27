using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zen.Barcode;
using static CorParques.Transversales.Util.Enumerador;
using System.Data;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class CortesiaController : ControladorBase
    {
        public static List<string> rutaElimina = new List<string>();
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> ObtenerCortesiaUsuarioVisitante(string documento, string numTarjeta, string correoApp, string documentoEjecutivo)
        {
            CortesiaViewModel cortesia = new CortesiaViewModel();
            cortesia.documento = documento;
            cortesia.numTarjeta = numTarjeta;
            cortesia.correoApp = correoApp;
            cortesia.documentoEjecutivo = documentoEjecutivo;
            var rta = await PostAsync<CortesiaViewModel, UsuarioVisitanteViewModel>("Cortesia/ObtenerCortesiaUsuarioVisitante", cortesia);
            return Json(rta, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ObtenerDetalleCortesia(string documento, int IdTipoCortesia, string numeroTarjetaFAN)
        {
            CortesiaViewModel cortesia = new CortesiaViewModel();
            if (IdTipoCortesia == 1)
            {
                cortesia.documento = documento;
            }
            if (IdTipoCortesia == 5)
            {
                cortesia.documentoEjecutivo = documento;
            }
            if (IdTipoCortesia == 2)
            {
                cortesia.numTarjeta = numeroTarjetaFAN;
            }

            var rta = await PostAsync<CortesiaViewModel, UsuarioVisitanteViewModel>("Cortesia/ObtenerCortesiaUsuarioVisitante", cortesia);
            return PartialView("_DetalleCortesias", rta.Elemento);
        }
    
        public async Task<JsonResult> ObtenerProducto(string CodBarra, string Documento, string numtarjeta, List<Producto> productos, int IdTipoCortesia, int impresionLinea, int IdDetalle)
        {
            Producto _producto = new Producto();
            UsuarioVisitanteViewModel listacortesi = new UsuarioVisitanteViewModel();
            bool _blnAplicaCortesiaFAN = false;
            if (IdTipoCortesia == 1)
            {
                listacortesi = await GetAsync<UsuarioVisitanteViewModel>($"Cortesia/ObtenerCortesiaUsuarioVisitante2/{Documento}");
            }
            if (IdTipoCortesia == 5)
            {
                listacortesi = await GetAsync<UsuarioVisitanteViewModel>($"Cortesia/ObtenerCortesiaUsuarioVisitanteEjecutivo/{Documento}");
            }
            if (IdTipoCortesia == 2)
            {
                listacortesi = await GetAsync<UsuarioVisitanteViewModel>($"Cortesia/ObtenerCortesiaTarjetaFAN/{numtarjeta}");
                if (listacortesi != null)
                {
                    if (listacortesi.ListDetalleCortesia != null && IdDetalle == 0)
                    {
                        listacortesi.ListDetalleCortesia = listacortesi.ListDetalleCortesia.Where(l => l.FechaInicial <= DateTime.Now && l.FechaFinal >= DateTime.Now && l.Activo == true).ToArray();
                    }
                    if (IdDetalle != 0) {
                        listacortesi.ListDetalleCortesia = listacortesi.ListDetalleCortesia.Where(l => l.IdDetalleCortesia == IdDetalle && l.Activo == true).ToArray();
                    }

                }
                else
                {
                    _blnAplicaCortesiaFAN = true;
                }
            }


            var resultCortesiasDispo = new List<Producto>();
            if (listacortesi != null)
            {
                //resultCortesiasDispo = listacortesi.ListDetalleCortesia.GroupBy(
                //          p => p.CodigoSap,
                //          (key, g) => new { PersonId = key, Cars = g.Select(x => x.Cantidad).Sum() }).ToList();

                resultCortesiasDispo = (from t in listacortesi.ListDetalleCortesia
                                        group t by new { t.CodigoSap }
                                        into grp
                                        select new Producto()
                                        {
                                            CodigoSap = grp.Key.CodigoSap,
                                            Cantidad = grp.Sum(t => t.Cantidad)
                                        }).ToList();
            }

            var _prod = await GetAsync<Producto>($"Pos/ObtenerBrazaleteConsecutivo/{CodBarra}/{(Session["UsuarioAutenticado"] as Usuario).Id}/0");
            var productoagregar = new Producto();
            if (impresionLinea == 1 && listacortesi != null)
            {
                if (listacortesi.ListDetalleCortesia != null)
                {


                    var prueba = listacortesi.ListDetalleCortesia.Where(l => l.CodigoSap == CodBarra).ToArray();
                    if (prueba.Count() > 0)
                    {
                        productoagregar = (from t in prueba
                                           group t by new { t.CodigoSap, t.IdProducto, t.IdDetalleCortesia, t.Nombre, t.CodSapTipoProducto }
                                       into grp
                                           select new Producto()
                                           {
                                               CodigoSap = grp.Key.CodigoSap,
                                               Cantidad = grp.Sum(t => t.Cantidad),
                                               IdProducto = grp.Key.IdProducto,
                                               IdDetalleProducto = grp.Key.IdDetalleCortesia,
                                               ConseutivoDetalleProducto = grp.Key.CodigoSap,
                                               Nombre = grp.Key.Nombre,
                                               CodSapTipoProducto = grp.Key.CodSapTipoProducto
                                           }).FirstOrDefault();
                    }

                }
            }

            if (_prod != null)
            {
                _producto = _prod;
                if (_prod.IdProducto > 0)
                {
                    if (productos == null)
                    {
                        productos = new List<Producto>();
                    }
                    _producto.Cantidad = 1;
                    productos.Add(_producto);
                }
                else if (impresionLinea == 1)
                {
                    if (productoagregar != null)
                    {
                        _producto = productoagregar;
                        if (productos == null)
                        {
                            productos = new List<Producto>();
                        }
                        _producto.Cantidad = 1;
                        productos.Add(_producto);
                    }
                }

            }
            var resultProdCargado = new List<Producto>();
            if (productos != null)
            {
                //resultProdCargado = productos.GroupBy(
                // p => p.CodigoSap,
                // (key, g) => new { PersonId = key, Cars = g.Select(x => x.Cantidad).Sum() }).ToList();

                resultProdCargado = (from t in productos
                                     group t by new { t.CodigoSap, t.IdProducto }
                           into grp
                                     select new Producto()
                                     {
                                         CodigoSap = grp.Key.CodigoSap,
                                         Cantidad = grp.Sum(t => t.Cantidad)
                                     }).ToList();

            }
            bool _banderadisponible = true;
            if (resultCortesiasDispo != null && resultProdCargado != null)
            {
                foreach (var item in resultProdCargado)
                {
                    foreach (var item2 in resultCortesiasDispo)
                    {
                        if (item.CodigoSap == item2.CodigoSap && item.Cantidad > item2.Cantidad)
                        {
                            _banderadisponible = false;
                        }
                    }
                }
            }

            Parametro _productos = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/ProductosCortesiasPQRS");



            //Validar si el producto aplica
            bool _blnAplica = false;

            if (listacortesi != null && listacortesi.ListDetalleCortesia != null && _producto.IdProducto > 0)
            {
                foreach (var item in listacortesi.ListDetalleCortesia)
                {
                    if (item.CodigoSap == _producto.CodigoSap)
                        _blnAplica = true;
                }
            }


            //if (_productos != null && _productos.Valor != null && _producto.IdProducto > 0)
            //{
            //    foreach (var item in _productos.Valor.Split(','))
            //    {
            //        if (item.Trim() == _producto.CodigoSap)
            //            _blnAplica = true;
            //    }
            //}

            if (!_blnAplica && _producto.IdProducto > 0)
                _producto = new Producto() { MensajeValidacion = "Producto no valido!" };
            else if (!_banderadisponible)
                _producto = new Producto() { MensajeValidacion = "No se pueden cargar mas productos de ese tipo!" };
            else if (_blnAplicaCortesiaFAN)
                _producto = new Producto() { MensajeValidacion = "Producto no valido por fecha!" };


            return Json(_producto, JsonRequestBehavior.AllowGet);


        }
        
        public async Task<JsonResult> GuardarCortesiaUsuarioVisitante(UsuarioVisitanteViewModel usuarioVisitante, List<Producto> productos, int IdDetalle)
        {
            var obj = new GuardarCortesiaUsuarioVisitante()
            {
                idUsuario = IdUsuarioLogueado,
                Documento = usuarioVisitante.NumeroDocumento,
                ListaProductos = productos,
                TipoCortesia = usuarioVisitante.IdTipoCortesia,
                ValorGenerico = usuarioVisitante.TipoDocumento,
                IdDetalleCortesia = IdDetalle
            };
            var rta = await PostAsync<GuardarCortesiaUsuarioVisitante, string>("Cortesia/GuardarCortesiaUsuarioVisitante", obj);
            if (rta.Correcto && string.IsNullOrEmpty(rta.Elemento.ToString()))
            {
                Inventario inventario = new Inventario();
                inventario.FechaInventario = Utilidades.FechaActualColombia;
                inventario.IdPunto = IdPunto;
                inventario.IdUsuarioCeado = IdUsuarioLogueado;
                inventario.Productos = productos;
                await PostAsync<Inventario, string>("Inventario/ActualizarInventario", inventario);
            }

            //Aqui se imprime el ticket comprobante redenciónn de cortesia

            var usuarioAutenticado = (CorParques.Negocio.Entidades.Usuario)Session["UsuarioAutenticado"];

            ServicioImprimir objImprimir = new ServicioImprimir();

            TicketImprimir objRedencionCortesia = new TicketImprimir();
            objRedencionCortesia.TituloRecibo = "Boleta Redención de Cortesia";

            var consecutivo = usuarioVisitante.ListDetalleCortesia.ToList()[0].Consecutivo;

            DataTable tablaDatos = new DataTable();
            tablaDatos.Columns.Add("Taquillero", typeof(string));
            tablaDatos.Columns.Add("Documento", typeof(string));
            tablaDatos.Columns.Add("Nombres", typeof(string));
            tablaDatos.Columns.Add("Cortesia", typeof(string));
            tablaDatos.Columns.Add("Consecutivo", typeof(string));
            tablaDatos.Rows.Add(usuarioAutenticado.Nombre + " " + usuarioAutenticado.Apellido,
                usuarioVisitante.NumeroDocumento,
                usuarioVisitante.Nombres + " " + usuarioVisitante.Apellidos,
                usuarioVisitante.Observacion,
                consecutivo);
            objRedencionCortesia.TablaDetalle = tablaDatos;

            objImprimir.ImprimirComprobanteRedencion(objRedencionCortesia);

            if (!string.IsNullOrEmpty(rta.Elemento.ToString()))
            {
                rta.Correcto = false;
                rta.Mensaje = rta.Elemento.ToString();
            }




            return Json(rta, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GuardarCortesiaClienteFanAPP( List<DetalleCortesia> productos)
        {
            var obj = new GuardarCortesiaUsuarioVisitante()
            {
                ListaProductosAPP = productos,
                TipoCortesia = 3
            };
            var rta = await PostAsync<GuardarCortesiaUsuarioVisitante, string>("Cortesia/GuardarCortesiaUsuarioVisitante", obj);
          

            if (!string.IsNullOrEmpty(rta.Elemento.ToString()))
            {
                rta.Correcto = false;
                rta.Mensaje = rta.Elemento.ToString();
            }




            return Json(rta, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GuardarCortesiaUsuarioVisitanteImpresionLinea(UsuarioVisitanteViewModel usuarioVisitante, List<Producto> productos)
        {
            PosController pos = new PosController();
            ImpresionEnLinea impresion = new ImpresionEnLinea();
            List<Producto> lProducto = new List<Producto>();
            var bandera = 0;
            foreach (var itemproducto in productos)
            {
                impresion.producto = itemproducto;
                IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");

                Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{itemproducto.CodigoSap}");

                if (temp != null)
                {
                    if (temp.Nombre == itemproducto.Nombre && temp.CodigoSap == itemproducto.CodigoSap)
                    {
                        itemproducto.AplicaImpresionLinea = true;
                        bandera = 1;
                    }
                }

                Producto p = new Producto();
               
                if (itemproducto.AplicaImpresionLinea)
                {
                    if (itemproducto.CodSapTipoProducto == "2000" || itemproducto.CodSapTipoProducto == "2055" && itemproducto.AplicaImpresionLinea)
                    {
                        Producto productoI = brazaletes.Where(x => x.CodigoSap == itemproducto.CodigoSap).First();

                        if (productoI != null)
                        {
                            productoI.IdUsuarioModificacion = IdUsuarioLogueado;
                            var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoI);

                            if (itemproducto.AplicaImpresionLinea)
                            {
                                ImpresionEnLinea impresionRegistroRollo = new ImpresionEnLinea();

                                p = await pos.asignarBoleta(impresion, itemproducto, temp, brazaletes, respuesta);

                                if (respuesta != null)
                                {
                                    if (!respuesta.Correcto)
                                        throw new ArgumentException(respuesta.Mensaje);


                                    if (!respuesta.Elemento.ToString().Contains("Error"))
                                    {
                                        var idBoleteria = int.Parse(respuesta.Elemento.ToString().Split('|')[0]);
                                        //item.IdDetalleProducto = idBoleteria;
                                        itemproducto.IdDetalleProducto = idBoleteria;

                                        lProducto.Add(itemproducto);

                                        impresion.CambioBoleta = p;
                                        if (p.CodBarraInicio != null)
                                        {

                                            itemproducto.CodBarraInicio = p.CodBarraInicio;
                                            Producto productoImpresion = brazaletes.Where(x => x.CodigoSap == itemproducto.CodigoSap).First();


                                            string respuestaImpresion = pos.imprimirImpresionEnLinea(itemproducto, productoImpresion, p.CodBarraInicio);


                                        }
                                    }
                                }

                            }

                        }

                    }
                }


            }
            if (bandera == 1)
            {
                var obj = new GuardarCortesiaUsuarioVisitante()
                {
                    idUsuario = IdUsuarioLogueado,
                    Documento = usuarioVisitante.NumeroDocumento,
                    ListaProductos = lProducto,
                    TipoCortesia = usuarioVisitante.IdTipoCortesia,
                    ValorGenerico = usuarioVisitante.TipoDocumento
                };
                var rta = await PostAsync<GuardarCortesiaUsuarioVisitante, string>("Cortesia/GuardarCortesiaUsuarioVisitante", obj);
                return Json(rta, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var rta = new GuardarCortesiaUsuarioVisitante() { idUsuario = 0 };
                return Json(rta, JsonRequestBehavior.AllowGet);
            }
          


            
        }

        public async Task<JsonResult> EnviarCortesiasporCorreoQR(string correo, List<Producto> productos)
        {
            if (correo != null)
            {

                foreach (var item in productos)
                {

                    string preQr = correo;
                    string envQR = preQr.Replace(".", "|");
                    var PRUEBA = GenerarQR(item.ConseutivoDetalleProducto, 2, 2);
                    string ff = PRUEBA.Replace("D:", "");
                    string fff = ff.Replace("\\", "||");
                    string pathQR = fff.Replace(".Jpeg", "");

                    //await GetAsync<string>($"CodigoFechaAbierta/EnviarQR/{qr}/{pathQR}");
                    var probando = await GetAsync<string>($"CodigoFechaAbierta/EnviarQR/{envQR}/{pathQR}");
                }
            }

            return Json("1", JsonRequestBehavior.AllowGet);
        }
        public static string GenerarQR(string strCodigoBarras, int intAlto, int intAncho)
        {

            string strResultado = string.Empty;
            string strPathTemp = string.Empty;

            CodeQrBarcodeDraw BarCode = BarcodeDrawFactory.CodeQr;
            MemoryStream stream = new MemoryStream();

            try
            {
                Image img = BarCode.Draw(strCodigoBarras, intAlto, intAncho);

                try
                {
                    System.Drawing.Image logo = System.Drawing.Image.FromFile("D:/imagenes/logoEjemplo.jpg"); //D:\Usuarios\ITrujillo\Downloads -- D://Usuarios//ITrujillo//Downloads//ele.jfif
                                                                                                              //System.Drawing.Image logo = System.Drawing.Image.FromFile("D:/Usuarios/ITrujillo/Downloads/eleee3.jpeg"); //D:\Usuarios\ITrujillo\Downloads -- D://Usuarios//ITrujillo//Downloads//lol.jfif

                    int left = (img.Width / 2) - (logo.Width / 2);
                    int height = (img.Height / 2) - (logo.Height / 2);
                    Graphics g = Graphics.FromImage(img);

                    g.DrawImage(logo, new Point(left, height));

                }
                catch (Exception e)
                {
                    strResultado = "No se genero el logo para el QR";
                }

                img.Save(stream, ImageFormat.Png);
                strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
                Image CodigoBarras = System.Drawing.Image.FromStream(stream);
                CodigoBarras.Save(strPathTemp, ImageFormat.Jpeg);
                strResultado = strPathTemp;
                //MessageBox.Show(strResultado);
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CodigoBarras_GenerarCodigoDeBarras");
                strResultado = string.Concat("Error en GenerarCodigoDeBarras_CodigoBarras: ", ex.Message);
            }
            finally
            {
                BarCode = null;
                stream = null;
            }

            rutaElimina.Add(strPathTemp);
            //System.Drawing.Image logo = System.Drawing.Image.FromFile(strPathTemp + )
            return strPathTemp;
        }


    }
}