using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using System;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class BitacoraDiaController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<BitacoraDia> listaBitacora = await GetAsync<IEnumerable<BitacoraDia>>($"BitacoraDia/Get/{DateTime.Now.ToString("yyyy-MM-dd")}");
            IEnumerable<TipoGeneral> clima = await GetAsync<IEnumerable<TipoGeneral>>($"Clima/GetAll");
            IEnumerable<TipoGeneral> segmentoDia = await GetAsync<IEnumerable<TipoGeneral>>($"SegmentoDia/GetAll");
            int CanntidadPersonas = int.Parse((await GetAsync<object>($"BitacoraDia/ObtenerCantidadPersonas")).ToString());
            
            BitacoraDiaLista bitacora = new BitacoraDiaLista();
            bitacora.BitacoraDiaList = listaBitacora;
            bitacora.Clima = clima;
            bitacora.SegmentoDia = segmentoDia;
            bitacora.CantidadPersonas = CanntidadPersonas;

            return View(bitacora);
        }

        public async Task<JsonResult> ObtenerTodos()
        {
            var resultado = await GetAsync<object>("BitacoraDia/GetAll");
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Obtener(string fecha)
        {
            var resultado = await GetAsync<object>($"BitacoraDia/Get/{fecha}");
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Asignar(BitacoraDiaLista modelo)
        {
            var bitacora = await PostAsync<BitacoraDiaLista, BitacoraDiaLista>("BitacoraDia/Set", modelo);
            return Json(bitacora, JsonRequestBehavior.AllowGet);
        }
    }
}