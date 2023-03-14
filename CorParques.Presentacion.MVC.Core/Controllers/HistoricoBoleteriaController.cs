using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class HistoricoBoleteriaController : ControladorBase
    {
        // GET: HistoricoBoleteria
        public async  Task<ActionResult> Index()
        {   

            return View();
        }

        public async Task<ActionResult> ObtenerBoleta(string consecutivo)
        {
            var rta = await GetAsync<DetalleBoleta>($"HistoricoBoleta/ObtenerHistoricoBoleta/{consecutivo}");
            if (rta != null && rta.historicoUso != null && rta.historicoUso.Count() > 0)
            {
                var _list = rta.historicoUso.First();
                if (_list.Fecha.Date.ToShortDateString() != DateTime.Now.Date.ToShortDateString())
                    ViewBag.dia = true;
            }
            else
            {
                if(rta == null || rta.IdBoleta == 0)
                    rta = new DetalleBoleta { BoletaInvalida = true };
            }

            return PartialView("_DetalleHistorico", rta);
        }

        public async Task<ActionResult> BloquearBoleta(int IdBoleta)
        {
            Boleteria _boleta = new Boleteria
            {
                IdBoleteria = IdBoleta, IdUsuarioModificacion = IdUsuarioLogueado, IdUsuarioBloqueo = IdUsuarioLogueado, PuntoBloqueo = IdPunto,
                IdEstado = (int)Enumerador.Estados.Bloqueado
            };
            var rta = await PostAsync<Boleteria, string>("Boleteria/Bloquear", _boleta);
            return Json(rta, JsonRequestBehavior.AllowGet);
        }
    }
}