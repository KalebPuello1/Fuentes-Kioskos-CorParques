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

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class TrasladoPedidosController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {           
            ViewBag.ListPuntos = await GetAsync<IEnumerable<Puntos>>("Puntos/ObtenerPuntosConAlmacen");
            ViewBag.Taquillero = await GetAsync<IEnumerable<Usuario>>("Usuario/ObtenerSinAdmin");
            ViewBag.Pedidos = await GetAsync<IEnumerable<ProductosPedidos>>("Inventario/ObtenerPedidosTraslado");
            return View();
        }

        public async Task<ActionResult> ObtenerMaterialesxPunto(int IdPunto)
        {
            var item = new TransladoInventario();
            IEnumerable<Materiales> _Materiales = await GetAsync<IEnumerable<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");
            item .Materiales = _Materiales;
            item.MaterialesAplicados = null;
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        

        public async Task<ActionResult> ObtenerLogin()
        {
            return PartialView("_Login");
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
            

            var respuesta = await PostAsync<IEnumerable<TransladoInventario>, string>("Inventario/TrasladoPedido", modelo);
            if (!string.IsNullOrEmpty(respuesta.Mensaje))
                            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al realizar el translado. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> ObtenerMaterialesxPuntoAjax(int IdPunto)
        {
            var listaMateriales = await GetAsync<List<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");

            if (listaMateriales == null)
                listaMateriales = new List<Materiales>();
            
            return Json(new RespuestaViewModel { Correcto = true, Elemento = listaMateriales }, JsonRequestBehavior.AllowGet);
        }


    }
}