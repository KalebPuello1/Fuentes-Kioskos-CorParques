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

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    
    public class BloqueoTarjetaController : ControladorBase
    {
        public BloqueoTarjetaController()
        {
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> ObtenerTarjeta(string doc)
        {
                       
            ClienteFideliacion cliente = null;

            try
            {

                cliente = await GetAsync<ClienteFideliacion>($"Fidelizacion/ObtenerClienteTarjeta/{doc}");
                if (cliente?.Foto != null)
                {
                    cliente.FotoTexto = "data:image/png;base64," + Convert.ToBase64String(cliente.Foto);
                }

            }
            catch (Exception)
            {
                throw new ArgumentException("Error en ObtenerTarjeta comuniquese con el administrador.");
            }
            return Json(new { rta = cliente != null, msj = cliente == null ? "El numero de documento ingresado no existe" : "", obj = cliente }, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public async Task<ActionResult> BloquearTarjeta(string Consecutivo)
        {
            var ok = false;
            try
            {
                var rta = await GetAsync<string>($"Fidelizacion/BloqueoTarjeta/{Consecutivo}|{IdUsuarioLogueado}|{IdPunto}");
                ok = rta == "true";    
            }
            catch (Exception)
            {
                throw new ArgumentException("Error en BloquearTarjeta comuniquese con el administrador.");
            }
            return Json(ok, JsonRequestBehavior.AllowGet);


        }
        

    }
}