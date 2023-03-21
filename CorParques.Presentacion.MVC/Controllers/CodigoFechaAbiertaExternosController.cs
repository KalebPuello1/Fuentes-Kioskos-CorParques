/// <summary>
/// Reportes
/// </summary>
using ClosedXML.Excel;
using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
/// <summary>
/// Trae de mails
/// </summary>
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Windows.Forms;
using Zen.Barcode;
using static CorParques.Transversales.Util.Enumerador;


namespace CorParques.Presentacion.MVC.Controllers
{

    public class CodigoFechaAbiertaExternosController : ControladorBase
    {
        #region Properties
        public static string location = "";
        //Este dato es para traer una lista de datos 
        public static string Buscar = "";
        //Esta dato es para poder eliminar el archivo excel
        public static string rutaEliminar;
        //Este parametro es para eliminar las imagenes en codigo de barras
        public static List<string> rutaElimina = new List<string>();
        //Este parametro es para guardar las direciones de los codigos de barras
        public static List<string> codSap;
        //Este parametro es para comparar los datos que se piden con los datos que se van a imprimir en el excel, 
        public static IEnumerable<CodigoFechaAbierta> codExistente;
        #endregion

        #region Constructor
        public CodigoFechaAbiertaExternosController()
        {
            location = "mom";
        }
        #endregion

        #region metodos

        /// <summary>
        /// Levanta la pagina principal, puede recibir datos de entrada
        /// </summary>
        /// <returns>Una vista con estilos</returns>
        [HttpGet, Route("api/CodigoFechaAbiertaExternos/{codSap}")]
        public async Task<ActionResult> Index(string codSapPedido, string mensaje = "", string error = "")
        {
            /*var f = 1;
            List<CodigoFechaAbierta> coddi = new List<CodigoFechaAbierta>();
            var rf = f == 1;
            f = f == 1 ? 0 : 1;*/

            //Se debe consultar el usuraio de ingreso
            var user = (Usuario)Session["UsuarioAutenticado"];

            //Hacer consulta a los pedidos que tiene este usuario
            var idUsuario = user.Id;
            var NomUsuario = user.NombreUsuario;
            var pedidos = await GetAsync<string>($"CodigoFechaAbierta/ObtenerPedidosPorUsuairo/{idUsuario}/{NomUsuario}");

            //Algo que mire el id del clente y verifique en los pedidos para ver si existen mas pedidos
            //Se necesita agregar un control para que vaya mostrando los pedidos nuevos, para que el cliente tenga los nuevos pedidos del cliente
            //Puedo utilizar el IdSapCliente

            //Se van a consultar ls pedidos en un SP
            var datoo = await GetAsync<CodigoFechaAbierta>($"CodigoFechaAbierta/VerPedidosClienteExternoMultiple/{pedidos}");


            


            /*if (dato.ListaListFechaAbierta != null)
            {

            }
            else
            {

            }*/
                
            var check = 0;
            /*ViewBag.mensaje = null;
            if (mensaje != "" && error != "")
            {
                ViewBag.mensaje = mensaje;
                ViewBag.error = error;
            }*/
            //var datoInciales = await GetAsync<IEnumerable<CodigoFechaAbierta>>($"CodigoFechaAbierta/VerPedidosClienteExterno/{codSapPedido}");

            if (datoo.ListaListFechaAbierta != null)
            {
                datoo.Mostrar = "true";
                datoo.ListaCodigoFechaAbierta = null;
                /*var dato = datoo.ListaListFechaAbierta;
                return View(dato);*/
                if (mensaje.Length > 0)
                {
                    ViewBag.error = error;
                    ViewBag.Mensaje = mensaje;
                }
                else
                {
                    ViewBag.error = null;
                    ViewBag.Mensaje = null;
                }

                return View(datoo);
            }
            else
            {
                if (mensaje.Length > 0)
                {
                    ViewBag.error = error;
                    ViewBag.Mensaje = mensaje;
                }
                else
                {
                    ViewBag.error = null;
                    ViewBag.Mensaje = null;
                }
                datoo.Mostrar = "false";
                datoo.ListaListFechaAbierta = null;
                /*var dato = datoo.ListaCodigoFechaAbierta;
                return View(dato);*/
                return View(datoo);
            }
            
        }


        [HttpGet]
        public async Task<JsonResult> img(string img)
        {
            var f = $"/D:/Usuarios/User/itrujillo/Pictures/Screenshots/Captura de pantalla (100).png";
            ///System.IO.File.OpenRead(f);
            FileContentResult objFileContentResult;
            objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(f), System.Net.Mime.MediaTypeNames.Application.Octet, f);

            return Json(new { ruta = img});
        }


        /// <summary>
        /// Va editar las redenciones a redimidos para poder tener control de los datos que se exportaron 
        /// </summary>  
        /// <param name="daT"></param>
        /// <returns>Vista con los datos actualizado</returns>
        [HttpPut]
        public async Task<ActionResult> editar(string id)
        {
            CodigoFechaAbierta edi = new CodigoFechaAbierta();
            //var listaa = await PutAsync<CodigoFechaAbierta, string>("CodigoFechaAbierta/Editar", edi);
            return Json(new { F = true });
        }
   
        public async Task<ActionResult> getPedido(string cod)
        {
            var user = (Usuario)Session["UsuarioAutenticado"];

            //Hacer consulta a los pedidos que tiene este usuario
            var idUsuario = user.Id;
            var NomUsuario = user.NombreUsuario;
            var pedidos = await GetAsync<string>($"CodigoFechaAbierta/ObtenerPedidosPorUsuairo/{idUsuario}/{NomUsuario}");
            //Se van a consultar ls pedidos en un SP
            var datoo = await GetAsync<CodigoFechaAbierta>($"CodigoFechaAbierta/VerPedidosClienteExternoMultiple/{pedidos}");

            if (datoo.ListaListFechaAbierta != null)
            //{
            //    datoo.ListaCodigoFechaAbierta = null;
            //    /*var dato = datoo.ListaListFechaAbierta;
            //    return View(dato);*/
            //    ViewBag.cod = cod;
            //    return PartialView("_check", datoo);
            //}
            //else
            {
                //datoo.ListaListFechaAbierta = null;
                datoo.Mostrar = "false";
                ViewBag.cod = cod;
                datoo.ListaCodigoFechaAbierta = new List<CodigoFechaAbierta>();
                foreach (var item in datoo.ListaListFechaAbierta)
                {
                    foreach (var i in item)
                    {
                        if (i.CodSapPedido == cod)
                        {
                            datoo.ListaCodigoFechaAbierta.Add(i);
                        }
                    }
                }
                
                /*var dato = datoo.ListaCodigoFechaAbierta;
                return View(dato);*/

            }

            /* ViewBag.cod = cod;
             var dato = await GetAsync<IEnumerable<CodigoFechaAbierta>>($"CodigoFechaAbierta/VerPedidosClienteExterno/{cod}");
             CodigoFechaAbierta codigo = new CodigoFechaAbierta();
             codigo.ListaCodigoFechaAbierta = dato.ToList();
             return PartialView("_check", codigo);*/
            return PartialView("_check", datoo);

        }
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Recibir(IEnumerable<CodigoFechaAbierta> CodigoFechaAbierta)
        public async Task<ActionResult> Recibir(CodigoFechaAbierta CodigoFechaAbierta)
        {
            ReportePDF Rpdf = new ReportePDF();
            string muestradato = "";
            if (CodigoFechaAbierta.ListaCodigoFechaAbierta != null) 
            {
                foreach (var item in CodigoFechaAbierta.ListaCodigoFechaAbierta)
                {
                    muestradato += "dato " + item.Nombre + " " + item.CantEnviar + " - " + item.Correo;
                }
            }
            
            if (CodigoFechaAbierta.ListaListFechaAbierta != null)
            {
                foreach (var item in CodigoFechaAbierta.ListaListFechaAbierta)
                {
                    foreach (var i in item)
                    {
                        muestradato += "dato " + i.Nombre + " " + i.CantEnviar + " - " + i.Correo;
                    }
                }
            }

                Rpdf.MuestraDato(muestradato);
            //MessageBox.Show("paso");
            var CantidadEnvinar = 0;
            var correo = CodigoFechaAbierta.Correo;
            var NomAventurita = CodigoFechaAbierta.NombreEnviado;
            var codPedido = "";
            //CantidadEnvinar = CodigoFechaAbierta.ListaCodigoFechaAbierta.First().CantEnviar == null ? 0 : 
            var codSapProducto = "";

            //Lista de productos enviar con consecutivos de boletControl???
            //var codigoControl
            List<CodigoFechaAbierta> mdifica = new List<CodigoFechaAbierta>();
            if (CodigoFechaAbierta.ListaListFechaAbierta != null)
            {
                var cantidad = 0;
                CodigoFechaAbierta.ListaCodigoFechaAbierta = new List<CodigoFechaAbierta>();
                
                foreach (var item in CodigoFechaAbierta.ListaListFechaAbierta)
                {
                    foreach(var i in item)
                    {
                        codSapProducto =item.First().CodSapProducto;
                        codPedido =item.First().CodSapPedido;
                        cantidad = item.First().CantEnviar;
                        i.CantEnviar = cantidad;
                        i.NombreEnviado = NomAventurita;
                        i.Correo = correo;
                        CodigoFechaAbierta.ListaCodigoFechaAbierta.Add(i);
                    }
                    //modifica
                    mdifica.Add(item.First());
                }
                CantidadEnvinar = CodigoFechaAbierta.CantEnviar;
                //CodigoFechaAbierta.ListaListFechaAbierta = null;
            }
            else
            {
                codSapProducto = CodigoFechaAbierta.ListaCodigoFechaAbierta.First().CodSapProducto == null ? "" : CodigoFechaAbierta.ListaCodigoFechaAbierta.First().CodSapProducto;
                codPedido = CodigoFechaAbierta.ListaCodigoFechaAbierta.First().CodSapPedido == null ? "0" : CodigoFechaAbierta.ListaCodigoFechaAbierta.First().CodSapPedido;
                foreach (var i in CodigoFechaAbierta.ListaCodigoFechaAbierta)
                {
                    i.CantEnviar = CodigoFechaAbierta.CantEnviar;
                    CantidadEnvinar = CodigoFechaAbierta.CantEnviar;
                    i.Correo = correo;
                    i.NombreEnviado = NomAventurita;
                }
                //mdifica = CodigoFechaAbierta.ListaCodigoFechaAbierta;
                mdifica.Add(CodigoFechaAbierta.ListaCodigoFechaAbierta.First());
            }
            
            string Nomcliente = await GetAsync<string>($"vuycoom/NCliPedido/{codPedido}");
            var existe = true;
            var pedido = "";
            var check = 0;
            var mensaje = "";
            var error = "";

            /*foreach (var item in CodigoFechaAbierta)  
            {*/

            //if (CodigoFechaAbierta.ListaListFechaAbierta == null)
            if (CodigoFechaAbierta.ListaCodigoFechaAbierta != null)
            {
                var numProductos = 0;

                /*var codSapProducto = item.CodSapProducto;
                pedido = item.CodSapPedido;*/
                if (CodigoFechaAbierta.ListaListFechaAbierta != null)
                {
                    if (CodigoFechaAbierta.ListaCodigoFechaAbierta.ToList().Exists(x => x.CantEnviar <= x.cantidadDisponible))
                    {
                        existe = true;
                    }
                    else
                    {
                        existe = false;
                        //break;
                    }
                }
                else
                {
                    if (CodigoFechaAbierta.ListaCodigoFechaAbierta.ToList().Exists(x => CantidadEnvinar <= x.cantidadDisponible))
                    {
                        existe = true;
                    }
                    else
                    {
                        existe = false;
                    }
                }

                List<CodigoFechaAbierta> FechaAbierta = new List<CodigoFechaAbierta>();
                foreach (var item in CodigoFechaAbierta.ListaCodigoFechaAbierta)
                {
                    if (item.CantEnviar > 0)
                    {
                        FechaAbierta.Add(item);
                    }
                }
                CodigoFechaAbierta.ListaCodigoFechaAbierta = FechaAbierta;
                //Se va encargar de pasar los boleta control

                //Aca debo ingressar los datos para ver si es esto por lo que no envia consecutivos 
                //CodigoFechaAbierta.ListaListFechaAbierta

                /* List<List<CodigoFechaAbierta>> ListaListFechaAbierta = new List<List<CodigoFechaAbierta>>();



                 List<CodigoFechaAbierta> ListFechaAbierta = new List<CodigoFechaAbierta>();
                 CodigoFechaAbierta LFechaAbierta = new CodigoFechaAbierta();
                 LFechaAbierta.Nombre = "FILA EXPRESS";
                 LFechaAbierta.IdProducto = 3465;
                 LFechaAbierta.CodSapProducto = "40000414";
                 LFechaAbierta.CodSapTipoProducto = "2000";
                 LFechaAbierta.CantEnviar = 3;
                 LFechaAbierta.CodSapPedido = "1000005594000010";

                 ListFechaAbierta.Add(LFechaAbierta);
                 ListaListFechaAbierta.Add(ListFechaAbierta);

                 CodigoFechaAbierta.ListaListFechaAbierta = ListaListFechaAbierta;

                 mdifica.Add(LFechaAbierta);
 */
                ///

                ReportePDF pdf = new ReportePDF();
                //if (existe)
                if (existe)
                {
                    numProductos = CantidadEnvinar;

                    if (CodigoFechaAbierta.ListaListFechaAbierta != null)
                    {
                        foreach (var item in CodigoFechaAbierta.ListaListFechaAbierta)
                        {
                            var d = await this.ConvertirAQR(item, item.First().CantEnviar);
                            
                            pdf.repositorio("PASO POR ConvertirAQR Y SALIIO ");
                            if (d.Contains("La cantidad de productos no es valida"))
                            {
                                continue;
                            }
                            item.First().RutaPDF = d;
                        }
                    }
                    else
                    {
                        var d = await this.ConvertirAQR(CodigoFechaAbierta.ListaCodigoFechaAbierta, numProductos);
                        pdf.repositorio("PASO POR ConvertirAQR2 Y SALIIO ");
                        CodigoFechaAbierta.ListaCodigoFechaAbierta.First().RutaPDF = d;
                    }

                    //Esto modifica la cantidad de boletas
                    //await GetAsync<string>($"CodigoFechaAbierta/UpdatePedidosClienteExterno/{codPedido}/{numProductos}/{codSapProducto}");
                    pdf.repositorio("Entro a modificar  ---- " + mdifica.First().CantEnviar + " " + mdifica.First().CodSapPedido);
                    foreach (var item in mdifica)
                    {
                        //MessageBox.Show("Entro a modificar " + item.CodSapPedido + " " + item.CantEnviar + " " + codSapProducto);
                        //MessageBox.Show("Entro a modificar " + item.CodSapPedido + " " + item.CantEnviar);
                        //await DisplayAlert("Alert", "You have been alerted", "OK");
                        pdf.repositorio("Entro a modificar " + item.CantEnviar + " " + item.CodSapPedido);
                        //await GetAsync<string>($"CodigoFechaAbierta/UpdatePedidosClienteExterno/{item.CodSapPedido}/{item.CantEnviar}");
                        string pedidoo = item.CodSapPedido;
                        int numProductoss = item.CantEnviar;
                        await GetAsync<string>($"CodigoFechaAbierta/UpdatePedidosClienteExterno/{pedidoo}/{numProductoss}");
                        //pdf.repositorio("finalizo modificar " + item.CantEnviar + " " + item.CodSapPedido);
                    }
                    //

                    mensaje = "Se envio el pedido satisfactoriamente";
                    error = "success";
                    if (mensaje.Contains("Se envio el pedido satisfactoriamente"))
                    {
                        //Inserta en las tablas para AsignacionBoletaControl y AsignacionBoletaControlDetalle


                        /*//Necesito correo
                        //Nombre quien se asigna
                        //idSapCLiente
                        //Pedido
                        //NumAsigancion
                        //Asignadopor
                        //Boletacontrol
                        //Boleteria
                        await GetAsync<string>($"CodigoFechaAbierta/InsertarAsigancionBoleta/{pedido}/{numProductos}/{codSapProducto}");*/
                    }
                }
                else
                {
                    mensaje = "no se pudo enviar el pedido";
                    error = "error";
                }

                /*}*/
                //pdf.repositorio("inicio VerPedidosClienteExterno");
                var dato = await GetAsync<IEnumerable<CodigoFechaAbierta>>($"CodigoFechaAbierta/VerPedidosClienteExterno/{codPedido}");
                //pdf.repositorio("finalizo VerPedidosClienteExterno");

                //return View("Index",dato);
                //return Redirect("Views/CodigoFechaAbiertaExternos/Index.cshtml");
                //return RedirectToAction("Index", "CodigoFechaAbiertaExternos", new { codSapPedido = codPedido, mensaje = mensaje, error = error });
                //return RedirectToAction("Index", "CodigoFechaAbiertaExternos", new { codSapPedido = 0, mensaje = mensaje, error = error  });
                //return RedirectToAction("Index", "CodigoFechaAbiertaExternos", new { codSapPedido = 0, mensaje = mensaje, error = error  });

                //pdf.repositorio("mando al index");
                return RedirectToAction("Index", "CodigoFechaAbiertaExternos");
            }
            else
            {
                //Ai se deben recibir los datos multiples

                return RedirectToAction("Index", "CodigoFechaAbiertaExternos");
            }
        }


        public async Task<bool> ModificarCantidadPedido(string pedido, int numProductos, string codSapProducto)
        {
            int cantidadEnviada = 0;
            try
            {
                await GetAsync<string>($"CodigoFechaAbierta/UpdatePedidosClienteExterno/{pedido}/{numProductos}/{codSapProducto}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> RutaLogoMundoAventura(string IdSAPCliente)
        {
            //Tener en cuenta
            string pdfpath = System.AppDomain.CurrentDomain.BaseDirectory + "pdf";
            string imagepath = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
            
            string strResultado = string.Empty;
            string strPathTemp = string.Empty;
            /*///
            //
            string strCodigoBarras = $"{IdSAPCliente}";
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            *//**//*

            MemoryStream streamm = new MemoryStream();
            *//*CodigoFechaAbierta codFecha = new CodigoFechaAbierta();
            codFecha.IdSAPCliente = IdSAPCliente;
            codFecha.rtaLogo = "D:/Usuarios/User/itrujillo/Pictures/Screenshots/imgpru.png";

            var dato = await PostAsync<CodigoFechaAbierta, MemoryStream>("CodigoFechaAbierta/ObtenerLogoCliente", codFecha);
*//*
            //Image.FromStream(dato, true).Save();
            //Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);

            var newImg = new Bitmap(90, 90);
            //Graphics.FromImage(newImg).DrawIcon
            //Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);

            Image imgGuarda = Image.FromFile(imagepath + "/logo_ma_color.jpg");

            Graphics.FromImage(newImg).DrawImage(imgGuarda, 0, 0, 20, 20);
            //Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);
            newImg.Save(strPathTemp, ImageFormat.Jpeg);


            strCodigoBarras = $"{IdSAPCliente}";
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");

            imgGuarda.Save(streamm, ImageFormat.Jpeg);
            imgGuarda.Save(strPathTemp, ImageFormat.Jpeg);

            var newImgg = new Bitmap(100, 100);
            //Graphics.FromImage(newImg).DrawIcon
            Image imgGuardaa = Image.FromStream((MemoryStream)dato.Elemento, true);
            Graphics.FromImage(newImgg).DrawImage(imgGuardaa, 0, 0, 20, 20);
            //Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);
            newImg.Save(strPathTemp, ImageFormat.Jpeg);
            //

            var fff = imgGuarda;
            imgGuarda = null;*/
            return strPathTemp;
        }

        public async Task<string> RutaLogo(string IdSAPCliente)
        {
            ReportePDF Rpdf  = new ReportePDF();
            Rpdf.log("Inicio rutaLogo");
            //Tener en cuenta
            string pdfpath = System.AppDomain.CurrentDomain.BaseDirectory + "pdf";
            string imgpath = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
            ///
            string strResultado = string.Empty;
            string strPathTemp = string.Empty;
            //
            string strCodigoBarras = $"{IdSAPCliente}";
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            /**/
            Rpdf.log("rutaLogo.....");
            MemoryStream streamm = new MemoryStream();
            CodigoFechaAbierta codFecha = new CodigoFechaAbierta();
            codFecha.CodCliente = IdSAPCliente;
            codFecha.rtaLogo = imgpath;

            Rpdf.log("Va obtener logo .....");
            Rpdf.log("Va..... " + codFecha.rtaLogo + codFecha.CodCliente);
            //var dato = await PostAsync<CodigoFechaAbierta, MemoryStream>("CodigoFechaAbierta/ObtenerLogoCliente", codFecha);
            var dato = await GetAsync<MemoryStream>($"CodigoFechaAbierta/ObtenerLogoCliente/{IdSAPCliente}");
            Rpdf.log("Obtuvo el logo .....");

            

            if (dato != null)
            {
                //Image.FromStream(dato, true).Save();
                //Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);
                Rpdf.log("Entro porque tiene dato del logo .....");
                var newImg = new Bitmap(140, 70);
                //Graphics.FromImage(newImg).DrawIcon
                //Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);
                Image imgGuarda = Image.FromStream((MemoryStream)dato, true);
                Graphics.FromImage(newImg).DrawImage(imgGuarda, 0, 0, 140, 70);
                //Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);
                newImg.Save(strPathTemp, ImageFormat.Jpeg);

                Rpdf.log("Viendo logo .....");
                strCodigoBarras = $"{IdSAPCliente}";
                strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
                strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");

                imgGuarda.Save(streamm, ImageFormat.Jpeg);
                imgGuarda.Save(strPathTemp, ImageFormat.Jpeg);

                var newImgg = new Bitmap(100, 100);
                //Graphics.FromImage(newImg).DrawIcon
                //Image imgGuardaa = Image.FromStream((MemoryStream)dato.Elemento, true);
                Image imgGuardaa = Image.FromStream((MemoryStream)dato, true);
                Graphics.FromImage(newImgg).DrawImage(imgGuardaa, 0, 0, 20, 20);
                //Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);
                newImg.Save(strPathTemp, ImageFormat.Jpeg);
                //

                var fff = imgGuarda;
                imgGuarda = null;
                return strPathTemp;
                Rpdf.log("finalizo rutaLogo");
            }
            else
            {
                strPathTemp = imgpath + "/user.png";
                Rpdf.log("Obtuvo el logo de admin por defecto .....");
                return strPathTemp;
            }
        }

            public async Task<string> ConvertirAQR(IEnumerable<CodigoFechaAbierta> ListaCodigoFechaAbierta, int CantEnviar)
        {
            int cantidadEnviada = 0;
            var codPedido = ListaCodigoFechaAbierta.First().CodSapPedido == null ? "0" : ListaCodigoFechaAbierta.First().CodSapPedido;
            var correo = ListaCodigoFechaAbierta.First().Correo == null ? "0" : ListaCodigoFechaAbierta.First().Correo;
            var CantidadEnvinar = CantEnviar;
            var aventurita = ListaCodigoFechaAbierta.First().NombreEnviado == null ? "0" : ListaCodigoFechaAbierta.First().NombreEnviado;
            //var codSapProducto = ListaCodigoFechaAbierta.First().CodSapProducto == null ? "" : ListaCodigoFechaAbierta.First().CodSapProducto;
            var rutapdf = "";


            int idBoleta = 0;
            try
            {
                //this.crearConsecutivos(ListaCodigoFechaAbierta, , CantEnviar);

                /*while (cantidadEnviada < CantEnviar)
                {*/

                ReportePDF Rpdf = new ReportePDF();
                

                //IEnumerable<CodigoFechaAbierta>  Boleteria  = ListaCodigoFechaAbierta.Where(x => x. == ).ToList(); //.CodSapProducto;
                //idBoleta = Boleteria.First().IdProducto;
                
                var json = await this.crearConsecutivos(ListaCodigoFechaAbierta, CantEnviar);
                    
                    // Verificamos si es pasaporte o comida/destreza
                bool ff = true;
                string rutaQR = "";
                string consecutivo = "";
                var NombreCliente = "";
                List<CodigoFechaAbierta> listaProductosEnvia = new List<CodigoFechaAbierta>();

                var pedidoCLiente = ListaCodigoFechaAbierta.First().CodSapPedido;
                //Debo encontrar el idUsuario segun un numero de pedido
                string cliente = await GetAsync<string>($"vuycoom/NCliPedido/{pedidoCLiente}");

                var codSapCliente = "";
                var nombreCliente = "";
                foreach (var item in json)
                {

                    //

                    
                    if (cliente != null)
                    {
                        if (cliente.Contains("-"))
                        {
                            codSapCliente = cliente.Split('-')[0];
                            nombreCliente = cliente.Split('-')[1];
                        }
                    }


                    //string strPathTemp = await RutaLogo(item.IdSAPCliente);
                    string strPathTemp = "";
                    
                    strPathTemp = await RutaLogo(codSapCliente);
                    
                    //string strPathTemp = await RutaLogo(codSapCliente);
                    if (item.boleteria)
                    {
                        //rutaQR += "|" + ImprimirCodigoBarras(item.Consecutivo);
                        rutaQR = ImprimirCodigoBarras(item.Consecutivo);
                        consecutivo += "|" + item.Consecutivo;
                        item.RutaQR = rutaQR;
                        item.rtaLogo = strPathTemp;
                        listaProductosEnvia.Add(item);
                    
                        ///Esto tiene que leer el listado de los producto va tener los consecutivos que va a convertir en QR

                    }
                    else
                    {
                        rutaQR = ImprimirCodigoBarras(item.Consecutivo);
                        consecutivo += "|" + item.Consecutivo;
                        item.RutaQR = rutaQR;
                        item.rtaLogo = strPathTemp;
                        listaProductosEnvia.Add(item);
                        //
                        
                        string genera = "Genero las boletas de comida/destreza";
                    }
                   
                    //ImprimirCodigoBarras(ii);
                }

                string Nomcliente = await GetAsync<string>($"vuycoom/NCliPedido/{codPedido}");
                if (Nomcliente.Contains("-"))
                {
                    string[] datosCliente = Nomcliente.Split('-');
                    NombreCliente = datosCliente[1];
                }
                var idCliente = 0;
                var id = await GetAsync<string>($"CodigoFechaAbierta/ObteneridCliente/{codPedido}");
                idCliente = int.Parse(id);

                

                if (true)
                {
                    
                    string enviarMensaje = "";
                    //string[] consecutivos = consecutivo.Split('|');
                    foreach (var item in listaProductosEnvia)
                    {
                        ///Aqui consultaremos el usuario ingresado
                        ///




                        var user = (Usuario)Session["UsuarioAutenticado"];
                        
                        //item.Nombre = user.Nombre;
                        item.NombreCliente = user.Nombre;
                        item.IdSAPCliente = idCliente;
                        item.NombreEnviado = aventurita;
                        //item.NombreCliente = NombreCliente;
                        item.CodSapPedido = codPedido;
                        item.NombreClientePDF = nombreCliente;
                        item.Correo = correo;
                        enviarMensaje += item.Nombre + " - " + item.IdProducto;
                    }



                    //this.EnviarCorreoConsecutivos("");


                    //Necesito correo
                    //Nombre quien se asigna
                    //idSapCLiente
                    //Pedido
                    //NumAsigancion
                    //Asignadopor
                    //Boletacontrol
                    //Boleteria


                    //ListaCodigoFechaAbierta.First().NombreCliente = NombreCliente; //idCliente
                    //ListaCodigoFechaAbierta.First().IdSAPCliente = idCliente;

                    if (listaProductosEnvia != null && listaProductosEnvia.Count() > 0)
                    {
                        //Rpdf.log("Entro al true y va insertar");
                        await PostAsync<IEnumerable<CodigoFechaAbierta>, string>($"CodigoFechaAbierta/InsertarAsigancionBoleta", listaProductosEnvia);
                        //Rpdf.log("Entro al true y inserto");

                        /*
                         consecutivo.Nombre = item.Nombre;
                           consecutivo.IdProducto = item.IdProducto;
                           consecutivo.CodSapProducto = item.CodSapProducto;
                           consecutivo.FechaInicial = item.FechaInicial;
                           consecutivo.FechaFinal = item.FechaFinal;
                           item.Consecutivo = consecutivo.Consecutivo; 
                         */
                        //Aqui se va generar el excel para poder pasar los datos a gmail
                        /*Reportes reporte = new Reportes();
                        List<Parametro> parametros = new List<Parametro>();
                        parametros.Add(new Parametro { Nombre = "IdProducto", Valor = "Tipo producto"});
                        parametros.Add(new Parametro { Nombre = "FechaInicial", Valor = "Fecha Inicial" });
                        parametros.Add(new Parametro { Nombre = "FechaFinal", Valor = "Fecha Final" });
                        parametros.Add(new Parametro { Nombre = "RutaQR", Valor = "Codigo QR" });
                        //reporte.GenerarReporteExcel(json, parametros, "Boleteria usuario");
                        IEnumerable<object> comparar = json;
                        GenerarReporteExcel(comparar, json.ToList(), parametros, "Boleteria usuario");*/

                        //Necesito validar los datos que se van a pasar


                        ReportePDF pdfEnviar = new ReportePDF();
                    List<CodigoFechaAbierta> listaBorrarLogo = json.Where(e => e.rtaLogo.Length > 0 || e.RutaQR.Length > 0).ToList();
                        //Rpdf.log("Entro al true paSo ENTRARA generarPDF");
                        rutapdf = pdfEnviar.generarPDF(json.ToList(), rutaQR);
                        
                        //Aqui podemos enviar el pdf al correo 
                        ListaCodigoFechaAbierta.First().enviarProductos = true;
                    CodigoFechaAbierta boletaUsario = new CodigoFechaAbierta();
                    boletaUsario.usuario = (Usuario)Session["UsuarioAutenticado"];
                    boletaUsario.Correo = ListaCodigoFechaAbierta.First().Correo;
                    boletaUsario.RutaPDF = rutapdf;
                    boletaUsario.enviarProductos = true;

                        //Rpdf.log(": " + boletaUsario.usuario.NombreUsuario + " " + boletaUsario.Correo + " " + boletaUsario.RutaPDF + " " + boletaUsario.enviarProductos);
                        await PostAsync<CodigoFechaAbierta, string>("CodigoFechaAbierta/EnviarUsuario", boletaUsario);
                        //Rpdf.log("Envio el usuario datos");
                        foreach (var item in listaBorrarLogo)
                        {
                            if (!item.rtaLogo.Contains("user.png"))
                            {
                                if (System.IO.File.Exists(item.rtaLogo))
                                {
                                    System.IO.File.Delete(item.rtaLogo);
                                }
                            }
                            if (System.IO.File.Exists(item.RutaQR))
                            {
                                System.IO.File.Delete(item.RutaQR);
                            }
                        }
                        if (System.IO.File.Exists(rutapdf))
                        {
                            System.IO.File.Delete(rutapdf);
                        }
                    }
                    else
                    {
                        rutapdf = "La cantidad de productos no es valida";
                    }
                }

                //} 
                //Rpdf.log("Esta salinedo de la ruta ");
                return rutapdf;
            }
            catch (Exception e)
            {
                ReportePDF pdfEnviar = new ReportePDF();
                pdfEnviar.log(" fallo Esta salinedo de la ruta ");
                return rutapdf;
            }
        }

        public async Task<IEnumerable<CodigoFechaAbierta>> crearConsecutivos(IEnumerable<CodigoFechaAbierta> ListaCodigoFechaAbierta, int cantidadCrear)
        {
            ReportePDF Rpdf = new ReportePDF();
            Rpdf.log("entro en consecutivo");
            var cantidadCreada = 0;
            List<CodigoFechaAbierta> consecutivos = new List<CodigoFechaAbierta>();
            int limite = ListaCodigoFechaAbierta.Count();
            int[] idProductos = new int[limite];
            List<CodigoFechaAbierta> retornaConsecutivos = new List<CodigoFechaAbierta>();
            cantidadCreada = 0;
            var indice = 0;
            try
            {
              foreach (var item in ListaCodigoFechaAbierta)
              {
                cantidadCreada = 0;
                //var codPedido = ListaCodigoFechaAbierta.First().CodSapPedido;
                var codPedido = item.CodSapPedido;
                //while (cantidadCreada < cantidadCrear)
                while (cantidadCreada < item.CantEnviar)
                {
                    Rpdf.log("entrar a generar consecutivo");
                    CodigoFechaAbierta consecutivo = await this.GenerarConsecutivo(item);
                    if (consecutivo.boleteria)
                    {
                       consecutivo.Nombre = item.Nombre;
                       consecutivo.IdProducto = item.IdProducto;
                       consecutivo.CodSapProducto = item.CodSapProducto;
                       consecutivo.FechaInicial = item.FechaInicial;
                       consecutivo.FechaFinal = item.FechaFinal;
                       item.Consecutivo = consecutivo.Consecutivo;
                       item.CodSapPedido = codPedido;
                       consecutivo.CodSapPedido = codPedido;
                       retornaConsecutivos.Add(consecutivo);
                       await PostAsync<CodigoFechaAbierta,string>($"CodigoFechaAbierta/UpdateFechaBoletas", item);
                       cantidadCreada += 1;    
                        Rpdf.log("Genera boleteria y modifica fecha");
                        }
                    else
                    {
                            consecutivo.Nombre = item.Nombre;
                            consecutivo.IdProducto = item.IdProducto;
                            consecutivo.CodSapProducto = item.CodSapProducto;
                            consecutivo.FechaInicial = item.FechaInicial;
                            consecutivo.FechaFinal = item.FechaFinal;
                            item.Consecutivo = consecutivo.Consecutivo;
                            item.CodSapPedido = codPedido;
                            consecutivo.CodSapPedido = codPedido;
                            retornaConsecutivos.Add(consecutivo);
                            Rpdf.log("Guarda consecutivo de fuera del 2000");
                            cantidadCreada += 1;
                    }        
                }
                  
              }
                //return Json(new { Bandera = true, Listconsecutivos = consecutivos});  
                //retornaConsecutivos.Add(consecutivos);
                return retornaConsecutivos;  
            }
            catch (Exception e)
            {
                //consecutivos = new List<CodigoFechaAbierta>();
                retornaConsecutivos = new List<CodigoFechaAbierta>();
                Rpdf.log("Genero error " + e.ToString());
                //return Json(new { Bandera = false, Listconsecutivos = consecutivos});
                return retornaConsecutivos;
            }
            Rpdf.log("Finalizo crearConsecutivos");
        }

        public async Task<CodigoFechaAbierta> GenerarConsecutivo(CodigoFechaAbierta productoBase)
        {
            ReportePDF Rpdf = new ReportePDF();
            Rpdf.log("entro en GenerarConsecutivo");
            CodigoFechaAbierta codigos = new CodigoFechaAbierta();
            if (productoBase.CodSapTipoProducto == "2000")
            {
                IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");

                //Producto producto = brazaletes.Where(x => x.IdProducto == item.IdProducto).First();
                Producto producto = brazaletes.Where(x => x.IdProducto == productoBase.IdProducto).First();
                var consecutivoGenerado = "";
                if (producto != null)
                {

                    codigos.boleteria = true;
                    producto.IdUsuarioModificacion = IdUsuarioLogueado;
                    var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", producto);

                    if (!respuesta.Correcto)
                        throw new ArgumentException(respuesta.Mensaje);


                    if (!respuesta.Elemento.ToString().Contains("Error"))
                    {

                        var idBoleteria = int.Parse(respuesta.Elemento.ToString().Split('|')[0]);
                        var consecutivo = respuesta.Elemento.ToString().Split('|')[1];
                        consecutivoGenerado = consecutivo;

                        //Aqui debe entrar el update que va a cambiar las fechas 
                        /*
                         *
                         */
                        //Modificar la fecha 
                        //await GetAsync<string>($"CodigoFechaAbierta/UpdateFechaBoletas/{consecutivoGenerado}/{FechaInicial}/{FechaFinal}");
                        //

                        codigos.IdProducto = producto.IdProducto;
                        codigos.Consecutivo = consecutivoGenerado;
                    }
                    Rpdf.log("RegistrarCodigoBoleteriaImpresionLinea");
                }
                //consecutivoGenerado  = "";
                return codigos;
            }
            /*else if (CodSapProducto == "2030")
            {
                codigos = new CodigoFechaAbierta();
                codigos.boleteria = false;
                codigos.Consecutivo = "0000000";
                return codigos;
            }*/
            else
            {
                Producto p = await GetAsync<Producto>($"Pos/ObtenerProducto/{productoBase.IdProducto}");
                codigos.FechaFinal= productoBase.FechaFinal;
                codigos.FechaInicial = productoBase.FechaInicial;
                codigos.CodSapPedido = productoBase.CodSapPedido;
                codigos.IdProducto = productoBase.IdProducto;
                //codigos.valor = productoBase.valor;
                codigos.Valor = productoBase.Valor;
                codigos.Nombre = p.Nombre;
                codigos.CodSapProducto = p.CodigoSap;
                codigos.CodSapTipoProducto = p.CodSapTipoProducto;
                codigos.posicion = p.Posicion;
                var resultado = await PostAsync<CodigoFechaAbierta, string>("CodigoFechaAbierta/InsertarBoleteriaCodigosExternos", codigos);
                codigos = new CodigoFechaAbierta();
                codigos.boleteria = false;
                codigos.Consecutivo = (string)resultado.Elemento;
                codigos.IdProducto = productoBase.IdProducto;
                Rpdf.log("Inserto la boleteria codigos");
                return codigos;
            }
            Rpdf.log("Inserto y genero el consecutivo -- ");
        }

        public async Task<ActionResult> vista(string data)
        {
            return PartialView("_check", new { 
            
            });
        }

        /// <summary> 
        /// Busca codigos para mostrar en pantalla y poder exportar en el archivo excel 
        /// </summary>
        /// <param name="daT"></param>
        /// <param name="check"></param>
        /// <returns>La vista con los datos que se consultaron</returns>
        [HttpGet]
        public async Task<ActionResult> BuscarCodigos(string daT, string check)
        {
            var datt = await GetAsync<IEnumerable<CodigoFechaAbierta>>($"CodigoFechaAbierta/VerFacturas/{daT}" + "/" + check);

            var dato = await GetAsync<IEnumerable<CodigoFechaAbierta>>($"CodigoFechaAbierta/VerPedidosClienteExterno/{daT}");
            CodigoFechaAbierta codigo = new CodigoFechaAbierta();
            codigo.ListaCodigoFechaAbierta = dato.ToList();


            codSap = new List<string>();
            if (datt != null)
            {
                foreach (CodigoFechaAbierta c in datt)
                {
                    if (c.Redencion == 0)
                    {
                        codSap.Add(c.CodBarrasBoletaControl);
                    }
                }
            }
            //return Json(new RespuestaViewModel { Correcto = false }, JsonRequestBehavior.AllowGet);
            codExistente = datt;
            GenerarQR("345324", 2, 2);
            //return PartialView("_check", dato);
            return PartialView("_check", codigo);
        }


        #region EnviarMails

        public async Task<bool> EnviarCorreoConsecutivos(string consecutivos)
        {
            try
            {
                ///await GetAsync<string>("");
                return true;
            }
            catch (Exception e)
            {
                return false;
            }        
        }


        public bool EnviarCorreo(string sTo, string sSubject, string sMensaje, MailPriority mpPriority, List<string> attachmentList)
        {

            var mail = new MailMessage();

            var smtpServer = new SmtpClient("smtp.gmail.com", 587);
            var MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
            var MailPass = ConfigurationManager.AppSettings["MailPass"].ToString();
            var Port = ConfigurationManager.AppSettings["Port"].ToString();
            mail.From = new MailAddress((string)MailFrom, ConfigurationManager.AppSettings["Mask"].ToString());
            mail.To.Add(sTo);
            mail.Subject = sSubject;
            string tbody = sMensaje;
            mail.Body = tbody;
            mail.IsBodyHtml = true;
            mail.Priority = mpPriority;

            var ms = new MemoryStream();
            if (attachmentList != null)
            {
                foreach (var cln in attachmentList)
                {
                    ms = new MemoryStream(System.IO.File.ReadAllBytes(cln));

                    try
                    {
                        mail.Attachments.Add(new Attachment(ms, new FileInfo(cln).Name));
                    }
                    catch (Exception) { }
                }
            }
            smtpServer.Port = int.Parse(Port);
            smtpServer.EnableSsl = true;
            smtpServer.Credentials = new System.Net.NetworkCredential(MailFrom, MailPass);

            try
            {
                smtpServer.Send(mail);
                ms.Close();
                ms.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        #region generar reporte 
        private string[] ObtenerEncabezado(object elemento)
        {
            List<string> encabezado = new List<string>();
            foreach (var item in elemento.GetType().GetProperties())
            {
                encabezado.Add(item.Name);
            }
            return encabezado.ToArray();
        }

        private IEnumerable<string[]> ObtenerDetalle(IEnumerable<object> lista)
        {
            List<string[]> detalle = new List<string[]>();

            foreach (var item in lista)
            {
                List<string> registro = new List<string>();
                foreach (var reg in item.GetType().GetProperties())
                {

                    var value = reg.GetValue(item, null) == null ? string.Empty : reg.GetValue(item, null).ToString();
                    registro.Add(value);
                }
                detalle.Add(registro.ToArray());
            }
            return detalle;
        }

        /// <summary>
        /// Genera los reportes de una hoja
        /// </summary>
        /// <param name="lista">lista del elemento donde se encuentran los datos,los campos que tienen numeros, seran en fomato numerico</param>
        /// <param name="list">Esto contiene los datos para poder compararlo el tamaño de los datos del detalles para poder capturar solo el numerp de datos que se necesitan</param>
        /// <param name="mapeo">nombre de los encabezados, de como se mapean de base de datos al reporte</param>
        /// <param name="NombreReporte">nombre de como se llamara el reporte</param>
        /// <param name="decimales">las columnas que deben tener decimales, EJ: new string[]{"columna","A","D"};</param>
        /// <returns></returns>
        public string GenerarReporteExcel(IEnumerable<object> lista, List<CodigoFechaAbierta> list, IEnumerable<Parametro> mapeo, string NombreReporte = "", string[] decimales = null, string[] sumatoria = null)
        {
            if (lista.Count() == 0)
            {
                throw new Exception("No hay datos en la consulta");
            }

            var Encabezado = ObtenerEncabezado(lista.FirstOrDefault());
            var Detalle = ObtenerDetalle(lista);
            string strNombreArchivo = string.Empty;
            string strRutaArchivo = string.Empty;
            string strRutaReportes = string.Empty;
            string semiRuta = string.Empty;
            List<int> idsMapeo = new List<int>();
            XLWorkbook wb = new XLWorkbook(); //new XLWorkbook()
            try
            {
                strRutaReportes = Utilidades.RutaReportes();
                if (strRutaReportes.Trim().IndexOf("Error") >= 0)
                {
                    throw new ArgumentException(strRutaReportes);
                }

                strNombreArchivo = string.Concat(NombreReporte, "_", Utilidades.FechaString(), ".xlsx");//tellez 2017/05/24 - ahora exporrta en archivo excel
                semiRuta = "excel/";
                semiRuta = string.Concat(semiRuta, strNombreArchivo);
                //Ruta del aplicativo
                //strRutaReportes = System.AppDomain.CurrentDomain.BaseDirectory;
                //strRutaReportes = string.Concat(strRutaReportes, "excel/");
                //strRutaReportes = "D:\\Usuarios\\ITrujillo\\Downloads\\";
                strRutaArchivo = string.Concat(strRutaReportes, strNombreArchivo);
                var ws = wb.Worksheets.Add("Reporte");

                int contadorFila = 1;
                int contador = 2;
                if (NombreReporte.Equals("ReporteVentasDiarias"))
                {
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = "Comprobante Informe Diario";
                    contador = 3;
                }


                for (int i = 1; i <= Encabezado.Count(); i++)
                {
                    if (mapeo.Any(M => M.Nombre.Equals(Encabezado[i - 1])))
                    {
                        Parametro valorParametro = mapeo.Where(M => M.Nombre.Equals(Encabezado[i - 1])).FirstOrDefault();
                        if (NombreReporte.Equals("ReporteVentasDiarias"))
                        {
                            ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}2").Value = valorParametro.Valor;
                        }
                        else
                        {
                            ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = valorParametro.Valor;
                        }

                        idsMapeo.Add(i - 1);
                        contadorFila++;
                    }
                }
                //DataGridView f = new DataGridView();
                int n = 0;
                contadorFila = 1;
                foreach (var item in Detalle)
                {
                    var col = "E" + n.ToString();
                    for (int i = 1; i <= item.Count(); i++)
                    {
                        if (idsMapeo.Any(M => M == i - 1))
                        {
                            double valor;
                            if (double.TryParse(item[i - 1], out valor))
                            {
                                ws.RowHeight = 95;
                                ws.ColumnWidth = 95;
                                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = valor;
                                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Style.NumberFormat.NumberFormatId = 1;// 0_General - 2_0.00 - 3_#,##0
                            }
                            else if (item[10] != null && n < list.Count())
                            {
                                ws.AddPicture((item[i - 1].ToString())).MoveTo(ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Address).ScaleHeight(1, false);//Scale(1.0);
                            }
                            else
                                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = item[i - 1];
                            contadorFila++;
                        }
                    }
                    contadorFila = 1;
                    contador++;
                }

                if (decimales != null)
                    for (int i = 0; i < decimales.Length; i++)
                    {
                        if (decimales[i].Split(':').Length > 1)
                        {
                            ws.Column(decimales[i].Split(':')[0]).Style.NumberFormat.Format = string.Concat("0.", new String('0', int.Parse(decimales[i].Split(':')[1].ToString())));
                        }
                        else
                        {
                            ws.Column(decimales[i]).Style.NumberFormat.NumberFormatId = 2;
                        }
                    }

                if (sumatoria != null)
                {
                    double suma = 0;
                    int k = 0;
                    for (int i = 0; i < sumatoria.Length; i++)
                    {
                        for (k = 0; k < lista.Count(); k++)
                            suma += double.Parse(ws.Cell(sumatoria[i] + (k + 2)).Value.ToString());
                        ws.Cell(k + 2, sumatoria[i]).Value = suma;
                        ws.Cell(k + 2, sumatoria[i]).Style.NumberFormat.NumberFormatId = 1;
                        suma = 0;
                    }
                }


                ws.Columns().AdjustToContents();
                wb.SaveAs(strRutaArchivo);
            }
            catch (Exception ex)
            {
                strNombreArchivo = string.Concat("Error en GenerarReporte: ", ex.Message, " ", strNombreArchivo);
            }


            location = strRutaArchivo;
            //return semiRuta;
            return strNombreArchivo;
        }
        #endregion
        #region codigo barras
        public static string GenerarCodigoDeBarras(string strCodigoBarras, int intAlto, int intAncho)
        {

            string strResultado = string.Empty;
            string strPathTemp = string.Empty;

            Code128BarcodeDraw BarCode = BarcodeDrawFactory.Code128WithChecksum;
            MemoryStream stream = new MemoryStream();

            try
            {
                Image img = BarCode.Draw(strCodigoBarras, intAlto, intAncho);
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
            return strPathTemp;
        }
        #endregion


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
                    System.Drawing.Image logo = System.Drawing.Image.FromFile("D:/Usuarios/ITrujillo/Downloads/iielee.jpeg"); //D:\Usuarios\ITrujillo\Downloads -- D://Usuarios//ITrujillo//Downloads//ele.jfif
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

        /// <summary>
        /// Este metodo funciona para poder exportar los codigos y la direcion de las imagenes para dibujarlas en el excel
        /// </summary>
        /// <param name="l">Recibe datos para exportar a excel</param>
        /// <returns>Retorna la ruta del archivo</returns>
        [HttpPost]
        public async Task<string> exportarCod(Array l)
        {
            List<string> corr = new List<string>();
            CorParques.Transversales.Util.Reportes r = new CorParques.Transversales.Util.Reportes();
            List<Parametro> parametro = new List<Parametro>();
            /* Estos datos son los parametros que se pintaran en excel */
            //parametro.Add(new Parametro { Nombre = "IdPedidoBoletaControl" });
            //parametro.Add(new Parametro { Nombre = "CodSapPedido" });
            parametro.Add(new Parametro { Nombre = "CodBarrasBoletaControl" });
            parametro.Add(new Parametro { Nombre = "codigoBarras" });
            IEnumerable<CodigoFechaAbierta> datoExistente = codExistente;
            List<CodigoFechaAbierta> preDato = new List<CodigoFechaAbierta>();
            foreach (var it in datoExistente)
            {
                foreach (string ii in codSap)
                {
                    if (it.CodBarrasBoletaControl == ii)
                    {
                        string ruta = ImprimirCodigoBarras(ii);
                        it.codigoBarras = ruta;
                        preDato.Add(it);
                    }
                }
            }
            List<CodigoFechaAbierta> otro = preDato;
            IEnumerable<CodigoFechaAbierta> datoFinal = preDato;
            string fg = GenerarReporteExcel(datoFinal, otro, parametro, "Codigos_fecha_abierta");
            foreach (string f in rutaElimina)
            {
                if (System.IO.File.Exists(f))
                {
                    System.IO.File.Delete(f);
                }
            }
            //rutaEliminar = fg;
            //return Json(new { name = fg });
            return fg;
        }




        /// <summary>
        ///Genera el codigo de barras en imagen y las almacena en la temporal -> despues se eliminaran
        /// </summary>
        /// <param name="c">Ya es el dato descomprimido de un objeto que sera convertido en codigo de barras</param>
        /// <returns>La ruta de la imagen en codigo de barras</returns>
        public string ImprimirCodigoBarras(string c)
        {
            var str = string.Empty;
            Reportes r = new Reportes();
            //str = GenerarCodigoDeBarras(c, 100, 1);
            str = GenerarQR(c, 2, 2);
            rutaEliminar += str + " ,";
            return str;
        }

        /// <summary>
        /// Este metodo va a eliminar el archivo excel
        /// </summary>
        /// <param name="rutaEliminar">Recibe un parametro con el nombre del archivo para eliminar</param>
        /// <returns>"Termino el proceso"</returns>
        [HttpGet]
        public async Task<ActionResult> InsertarUsuarioExterno(Usuario modelo)
        {
            modelo.ListaPerfiles = new List<TipoGeneral>();
            modelo.ListaPuntos = new List<Puntos>();
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;


            //if (!string.IsNullOrEmpty(hdListaPuntos))
            //    foreach (var item in hdListaPuntos.Split(','))
            //        modelo.ListaPuntos.Add(new Puntos { Id = Convert.ToInt32(item) });

            modelo.IdEstado = (int)Enumerador.Estados.Activo;
            if (string.IsNullOrEmpty(modelo.Password))
                modelo.Password = ConfigurationManager.AppSettings["pwdGeneric"];


            //modelo.Password = Encripcion.Encriptar(modelo.Password, ConfigurationManager.AppSettings["llaveEncripcion"]);
            modelo.Password = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            modelo.Password2 = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            modelo.CambioPassword = true;
            var respuesta = await PostAsync<Usuario, string>("Usuario/InsertarUsuarioExterno", modelo);
            if (string.IsNullOrEmpty(respuesta.Mensaje))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el usuario. Por favor intentelo de nuevo" });

        }

        [HttpPost]
        public string Eliminar()
        {
            //Ruta del aplicactivo
            string ubicacion = System.AppDomain.CurrentDomain.BaseDirectory;
            string dir = ubicacion + rutaEliminar;
            if (System.IO.File.Exists(dir))
            {
                System.IO.File.Delete(dir);
            }
            return "Termino el proceso";
        }


        [HttpGet]
        public async Task<ActionResult> Obtener(string id)
        {
            var dato = await GetAsync<CodigoFechaAbierta>("CodigoFechaAbierta/ObtenerId/" + id);
            return PartialView("_editar", dato);
            //return Json(new { f = dato });

        }

        [HttpGet]
        public ActionResult EnviarCorreo(string id, string lol)
        {
            var dato = "relol";
            return PartialView("EnviarCorreo", dato);
        }

        [HttpGet]
        public async Task<string> EnviandoCorreo(string qr)
        {
            string preQr = "itrujillo@corparques.co";
            string envQR = preQr.Replace(".", "|");
            string PpathQR = GenerarQR("345324", 2, 2);
            //string PpathQR = "D:||rel||lol||ff.Jpeg";
            string ff = PpathQR.Replace("D:", "");
            string fff = ff.Replace("\\", "||");
            string pathQR = fff.Replace(".Jpeg", "");

            //await GetAsync<string>($"CodigoFechaAbierta/EnviarQR/{qr}/{pathQR}");
            await GetAsync<string>($"CodigoFechaAbierta/EnviarQR/{envQR}/{pathQR}");
            return $"Enviando correo con QR = {qr}";
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerTodo(string CodSapPedido)
        {
            var datosTodos = await GetAsync<IEnumerable<CodigoFechaAbierta>>("CodigoFechaAbierta/ObtenerTodo/" + CodSapPedido);
            return Json(new { ff = datosTodos });
        }

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try
            {
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception e)
            {
                return null;
            }
            return objFileContentResult;
        }

        [HttpGet]
        public void enviarMail()
        {
            List<string> attachment = new List<string>();
            EnviarCorreo("trujilloivanzx@gmail.com", "tested", "this is test", MailPriority.Low, attachment);
        }

        public async Task<ActionResult> test()
        {
            return Json(new { f = "data" });
        }
        #endregion


    }

}