using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Drawing.Imaging;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class CrearUsuarioExternoController : ControladorBase
    {
        // GET: CrearUsuarioExterno
        public async Task<ActionResult> Index()
        {
            Usuario usu = new Usuario();
            usu.ListaPuntos = new List<Puntos>();
            ViewBag.lista = await GetAsync<IEnumerable<TipoGeneral>>("Perfil/ObtenerListaSimple");  
            ViewBag.listaPunto = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Puntos}");
            ViewBag.listaTipoPunto = await GetAsync<IEnumerable<TipoGeneral>>("TipoPunto/ObtenerListaSimple");
            ViewBag.listaEmpleados = await GetAsync<IEnumerable<EstructuraEmpleado>>("ConvenioParqueadero/ObtenerListaEmpleados");
            ViewBag.Clientes = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos"); ;

            //return PartialView("_Create", usuario);
                return View(usu);
        }
        public async Task<ActionResult> verClientes(string cliente)
        {
            Usuario usu = new Usuario();
            usu.ListaPuntos = new List<Puntos>();
            
            ViewBag.Clientes = await GetAsync<IEnumerable<TipoGeneral>>("Cliente/ObtenerTodos"); ;

            //return PartialView("_Create", usuario);
            return View(usu);
        }

        //[HttpGet]
        [HttpPost]
        //public async Task<ActionResult> Insert(string usuario, string Nombres,string apellido,string cliente)
        public async Task<ActionResult> Insert(string usuario, string Nombres, string apellido, string cliente, string correo)
        ///public async Task<ActionResult> Insert(string usuario, string Nombres,string apellido,string cliente, string correo)
        {



            //Aqui se tienen que inserta el logo, asi que debe venir la ruta del archivo 


            Usuario creado = new Usuario();
            creado.NombreUsuario = usuario;
            creado.Password = "C0RP4RQU35";
            var correoAventurita = correo;
            creado.Correo = correoAventurita == null ? "itrujillo@corparques.co": correoAventurita;
            CodigoFechaAbierta boletaUsario = new CodigoFechaAbierta();
            boletaUsario.enviarProductos = false;
            boletaUsario.usuario = creado;
            //await PostAsync<CodigoFechaAbierta, string>("CodigoFechaAbierta/EnviarUsuario", boletaUsario);


            Usuario modelo = new Usuario();
            modelo.Nombre = Nombres;
            modelo.NombreUsuario = usuario;
            modelo.Apellido = apellido;
            //modelo.Empresa = empresa;
            modelo.ListaPerfiles = new List<TipoGeneral>();
            modelo.ListaPuntos = new List<Puntos>();
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var datos = "";
            var datosPedido = "";
            string[] idClientes = null;
            if (cliente.Contains("|"))
            {
                var pedidoss = cliente.Split('|');
                idClientes = new string[pedidoss.Length - 1];
                var a = 0;
                foreach (var item in pedidoss)
                {
                    if (item != "")
                    {
                        string dato = item.Split('-')[0];
                        idClientes[a] = dato;
                    }
                    a = a + 1;
                }
                foreach (var item in idClientes)
                {
                    if (!item.Contains("|"))
                    {
                        datos += item + "|";
                    }
                    else
                    {
                        datos += item;
                    }
                }

            }
            else
            {
                if (cliente != "")
                {
                    var id = cliente.Split('.')[0];
                    datos = id;
                }
            }


            //Se deben consultar lo pedidos activos por id de cliente ya creado 
            //datos = await GetAsync<string>($"CodigoFechaAbierta/verPedidosPorIdCliente/{datos}");
            List<PedidosPorCliente> listaPedido = new List<PedidosPorCliente>();
            if (cliente.Length > 0)
            {
                listaPedido = await GetAsync<List<PedidosPorCliente>>($"CodigoFechaAbierta/verPedidosPorIdCliente/{datos}");
            }

            //Esto ya estaba se debe descomentar
            //List<PedidosPorCliente> listaPedido= await GetAsync<List<PedidosPorCliente>>($"CodigoFechaAbierta/verPedidosPorIdCliente/{datos}");

            var pedidos = new string[0];
            var strPedidos = "";
            if (listaPedido.First().Pedido.Contains("|"))
            {
                pedidos = listaPedido.First().Pedido.Split('|');
                foreach (var item in pedidos)
                {
                    if (item != "")
                    {
                        if (!item.Contains("|"))
                        {
                            strPedidos += item + "|";
                        }
                        else
                        {
                            strPedidos += item;
                        }
                    }
                }
            }
            else
            {
                strPedidos = listaPedido.First().Pedido;
            }


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
            //var respuesta = await PostAsync<Usuario, string>("Usuario/Insert", modelo);
            var respuesta = await PostAsync<Usuario, string>("Usuario/InsertarUsuarioExterno", modelo);

            if (respuesta.Elemento.ToString().Contains("Usuario ya existe"))
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el usuario. Por favor intentelo de nuevo o pruebe con otro usuario" });
            }

            PedidosPorCliente datoPedido = new PedidosPorCliente();
            //Modifica el pedido con los clientes correspondientes
            string idCreado = respuesta.Elemento.ToString();
                if (datos != "")
                {
                var documentoCliente = "";
                if (datos.Contains("|"))
                {
                    documentoCliente = datos.Replace('|', ' ');
                }
                else
                {
                    documentoCliente = datos;
                }


                /*PedidosPorCliente datoPedido = new PedidosPorCliente();
                datoPedido.IdCliente = item.IdCliente;
                datoPedido.IdUsuario = int.Parse(idCreado);
                datoPedido.Pedido = item.Pedido;*/
                
                datoPedido.IdCliente = listaPedido.First().IdCliente;
                datoPedido.IdUsuario = int.Parse(idCreado);
                datoPedido.Pedido = listaPedido.First().Pedido;


                foreach (var item in listaPedido)
                {
                    if (!item.Pedido.Contains("Este cliente no tiene pedidos habilitados"))
                    {
                        item.IdUsuario = int.Parse(idCreado);
                        var d = await PostAsync<PedidosPorCliente, string>("CodigoFechaAbierta/InsertarPedidoUsuarioExterno", item);
                    }
                    
                }

            }


                //var d = await PostAsync<PedidosPorCliente, string>("Usuario/InsertarPedidoUsuarioExterno", datoPedido);
           
              


            if (string.IsNullOrEmpty(respuesta.Mensaje))
              await PostAsync<CodigoFechaAbierta, string>("CodigoFechaAbierta/EnviarUsuario", boletaUsario);
              return Json(new RespuestaViewModel { Correcto = true, Mensaje = "Usuario creado correctamente" }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el usuario. Por favor intentelo de nuevo" });

        }


        public async Task<string> guardaImg(string idCliente, string codPedido)
        {
            /*var BUrl = url.Replace("blob:","");
            
            Stream s = getUrl(BUrl);
            string ruta = "D:/Usuarios/User/itrujillo/Pictures/imgpru.png";
            Image imgGuardar = Image.FromStream(s);
            imgGuardar.Save(s, ImageFormat.Jpeg);
            imgGuardar.Save(ruta, ImageFormat.Jpeg);

            var urll = url;*/
            string imgpath = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
            var ruta = System.AppDomain.CurrentDomain.BaseDirectory;
            string nombreArchivo = string.Empty;
            string archivo = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                nombreArchivo = string.Concat(nombreArchivo, Path.GetExtension(file.FileName));
                archivo = Path.Combine(imgpath, nombreArchivo);
                file.SaveAs(archivo);
            }

            string url = null;
            string strResultado = string.Empty;
            string strPathTemp = string.Empty;
            MemoryStream streamm = new MemoryStream();
            CodigoFechaAbierta codFecha = new CodigoFechaAbierta();
            codFecha.CodCliente = idCliente;
            
            if (archivo.Length > 0)
            {
                    codFecha.rtaLogo = archivo;
            }
            else
            {
                codFecha.rtaLogo = $"{imgpath}/logo_verde.png";
            }
            var dato = await PostAsync<CodigoFechaAbierta, MemoryStream>("CodigoFechaAbierta/intentoImgBD", codFecha);

            //Image.FromStream(dato, true).Save();
            Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);

            string strCodigoBarras = "RELOL";
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            imgGuarda.Save(streamm, ImageFormat.Jpeg);
            imgGuarda.Save(strPathTemp, ImageFormat.Jpeg);

            var fff = imgGuarda;
            imgGuarda = null;

            if (System.IO.File.Exists(archivo))
            {
                System.IO.File.Delete(archivo);
            };

            return "Exitoso";
        }

        public Stream getUrl(string url)
        {
            HttpWebRequest request = ((HttpWebRequest)WebRequest.Create(url));
            HttpWebResponse response = ((HttpWebResponse)request.GetResponse());
            return response.GetResponseStream();
        }

        // GET: CrearUsuarioExterno/Create
        [HttpPost]
        public async Task<JsonResult> verPedidoDocumentoCliente(string docCliente)
        {
            List<PedidosPorCliente> listaPedido = await GetAsync<List<PedidosPorCliente>>($"CodigoFechaAbierta/verPedidosPorIdCliente/{docCliente}");

            //Se debe veirificar si tiene logo o no el cliente 
            //Aqui vamos a identificar si este cliente tiene o no un logo

            if (listaPedido.First().Pedido.Contains("Este cliente no tiene pedidos habilitados"))
            {
                return Json(new
                {
                    pedidosCliente = listaPedido.First().Pedido
                });
            }
            else
            {
                var i = 0;
                var espacio = listaPedido.First().Pedido.Split('|');
                if (espacio.Length > 0 && espacio.First().Contains("|"))
                {
                    foreach (var item in espacio)
                    {
                        if (item != "")
                        {
                            i = i + 1;
                        }
                    }
                }
                return Json(new
                {
                    pedidosCliente = listaPedido.First().Pedido,
                    numPedidos = i,
                    tieneLogo = 1
                });
            }
        }

        
    }
}
