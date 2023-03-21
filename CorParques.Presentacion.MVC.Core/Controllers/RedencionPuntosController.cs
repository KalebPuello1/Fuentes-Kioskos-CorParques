using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Configuration;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Timers;
using System.IO;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    
    public class RedencionPuntosController : ControladorBase
    {
        public RedencionPuntosController()
        {
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> ObtenerTarjetaPuntos(string consecutivo)
        {
                       
            ClienteFideliacion cliente = null;
            List<TipoGeneral> productos = null;

            try
            {

                cliente = await GetAsync<ClienteFideliacion>($"Fidelizacion/ObtenerTarjetaSaldoPuntos/{consecutivo}");
                if (cliente?.Foto != null)
                {
                    cliente.FotoTexto = "data:image/png;base64," + Convert.ToBase64String(cliente.Foto);
                }
                if (cliente!= null)
                    productos = await GetAsync<List<TipoGeneral>>($"Fidelizacion/ObtenerProductosRedimibles/{cliente.Puntos}");


            }
            catch (Exception)
            {
                throw new ArgumentException("Error en ObtenerTarjetaSaldoPuntos comuniquese con el administrador.");
            }
            return Json(new { rta = cliente != null, msj = cliente == null ? "El codigo de la tarjeta no existe" : "", obj = new { Cliente = cliente, Productos = productos } }, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public async Task<ActionResult> RedimirProducto(string codProducto, string consecutivo)
        {
            var ok = false;
            try
            {
                var rta = await GetAsync<string>($"Fidelizacion/RedimirProducto/{consecutivo}|{codProducto}|{IdUsuarioLogueado}|{IdPunto}");
                ok = rta == "true";
            }
            catch (Exception)
            {
                throw new ArgumentException("Error en RedimirProducto comuniquese con el administrador.");
            }
            return Json(ok, JsonRequestBehavior.AllowGet);

        }

    }
}