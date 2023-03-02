using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Ventas.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class FidelizacionController : ControladorBase
    {
        // GET: Brinks
        public async Task<ActionResult> Index(string doc = null)
        {
            return View();
        }

        public async Task<ActionResult> Buscar(string doc) {
            var resultado = await GetAsync<ClienteFideliacion>($"Fidelizacion/Buscar/{doc}");
            if (resultado != null)
            {
                if (resultado.Foto != null)
                {
                    resultado.FotoTexto = "data:image/png;base64,"+ Convert.ToBase64String(resultado.Foto);
                }
                return Json(new { rta = true, obj = new { resultado.Consecutivo, resultado.Nombre, resultado.Telefono, resultado.Correo, resultado.Documento, resultado.Fecha, resultado.Foto, resultado.FotoTexto, resultado.Direccion, resultado.Genero, FechaNacimiento = (resultado.FechaNacimiento != null ? resultado.FechaNacimiento.Value.ToShortDateString() : "") } }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { rta = false, msj = "El documento digitado no ha sido registrado."}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Guardar(ClienteFideliacion modelo)
        {
            if (!string.IsNullOrEmpty(modelo.FotoTexto))
                modelo.Foto = Convert.FromBase64String(modelo.FotoTexto.Split(',')[1]);
            var resultado = await PostAsync<ClienteFideliacion, string>("Fidelizacion/Guardar", modelo);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}