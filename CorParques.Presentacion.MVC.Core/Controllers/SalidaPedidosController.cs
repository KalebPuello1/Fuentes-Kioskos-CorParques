using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Transversales.Contratos;
using System.Configuration;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class SalidaPedidosController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {           
            ViewBag.Pedidos = await GetAsync<IEnumerable<SolicitudRetorno>>($"Inventario/ObtenerPedidosEntregaAsesor/{IdPunto}");
            ViewBag.Inventario = await GetAsync<List<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");
            return View();
        }

        public async Task<ActionResult> ObtenerUsuario(string usuario)
        {
            var pwd = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            var user = await GetAsync<Usuario>($"Usuario/GetByUserPwd?pwd={Server.UrlEncode(pwd)}&user={usuario}&Punto={IdPunto}");
            if (user != null)
                return Json(new { Nombre = $"{user.Nombre} {user.Apellido}", IdUsuario = user.Id },JsonRequestBehavior.AllowGet);
            return null;

        }

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try
            {
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception)
            {
                return null;
            }
            return objFileContentResult;
        }
        public async Task<string> Excel(string pedidos,string lista)
        {
            var objPedidos = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SolicitudRetorno>>(pedidos);
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            return objReportes.GenerarEntregaPedidos(objPedidos.Where(x => lista.Split('|').Contains(x.CodSapPedido)));

        }
        public async Task<ActionResult> Guardar(IEnumerable<TransladoInventario> modelo)
        {
            foreach (var item in modelo)
            {
                if (item.idUsuario == 0){
                    item.idUsuario = (Session["UsuarioAutenticado"] as Usuario).Id;
                }
                item.IdUsuarioRegistro = (Session["UsuarioAutenticado"] as Usuario).Id;
                item.Procesado = false;

            }
            

            var respuesta = await PostAsync<IEnumerable<TransladoInventario>, string>("Inventario/EntregaPedido", modelo);
            if (!respuesta.Correcto)
                            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al realizar la entrega del pedido. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

        }

       


    }
}