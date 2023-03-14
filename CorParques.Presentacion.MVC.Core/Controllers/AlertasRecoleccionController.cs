using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class AlertasRecoleccionController : ControladorBase
    {
        // GET: AlertasRecoleccion
        public async Task<ActionResult> Index()
        {
            var list = await GetAsync<IEnumerable<NotificacionAlerta>>("Recoleccion/ObtenerAlertas");
            list = list.OrderByDescending(x => x.TotalCaja);
            return View(list);
        }
    }
}