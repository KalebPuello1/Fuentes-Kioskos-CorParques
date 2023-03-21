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
    public class RetornoPedidosController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {           
            var pedidos = await GetAsync<IEnumerable<SolicitudRetorno>>($"Inventario/ObtenerSolicitudesDevolucion");
            ViewBag.Motivos = await GetAsync<IEnumerable<TipoGeneral>>($"Inventario/ObtenerMotivos");
            return View(pedidos);
        }

        public async Task<ActionResult> Ingreso()
        {
            var pedidos = await GetAsync<IEnumerable<SolicitudRetorno>>($"Inventario/ObtenerSolicitudesDevolucion");
            return View(pedidos);
        }

        public async Task<ActionResult> ObtenerPedido(string codigo)
        {
            var pedido = await GetAsync<IEnumerable<SolicitudRetorno>>($"Inventario/ConsultarPedidoRetorno/{codigo}"); 
            return Json(pedido, JsonRequestBehavior.AllowGet);

        }


        public async Task<ActionResult> Crear(SolicitudRetorno modelo)
        {
            modelo.UsuarioCrea = (Session["UsuarioAutenticado"] as Usuario).Id.ToString();
            modelo.Id = 1;
            var respuesta = await PostAsync<SolicitudRetorno, string>("Inventario/CrearSolicitudRetorno", modelo);
            if (!respuesta.Correcto)
                            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al crear la solicitud. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Eliminar(SolicitudRetorno modelo)
        {
            modelo.UsuarioCrea = (Session["UsuarioAutenticado"] as Usuario).Id.ToString();
            modelo.Id = 3;
            var respuesta = await PostAsync<SolicitudRetorno, string>("Inventario/CrearSolicitudRetorno", modelo);
            if (!respuesta.Correcto)
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al eliminar la solicitud. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> Recibir(SolicitudRetorno modelo)
        {
            modelo.UsuarioCrea = (Session["UsuarioAutenticado"] as Usuario).Id.ToString();
            modelo.Id = 2;
            var respuesta = await PostAsync<SolicitudRetorno, string>("Inventario/CrearSolicitudRetorno", modelo);
            if (!respuesta.Correcto)
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al recibir la solicitud. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

        }




    }
}