using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class InventarioOperacionController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var objParametro = await GetAsync<IEnumerable<ReporteBoleteria>>($"Boleteria/ConsultarInventarioDia");

            return View(objParametro);
        }
        public async Task<ActionResult> ObtenerDetalle(int IdProducto)
        {
            var objParametro = await GetAsync<IEnumerable<RegistroFallasInvOperacion>>($"Boleteria/ConsultarInventarioProdDia/{IdProducto}");
            
            return PartialView("_Detalle", objParametro);
        }
    }
}