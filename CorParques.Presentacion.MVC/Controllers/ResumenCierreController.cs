using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ResumenCierreController : ControladorBase
    {        
        public async  Task<ActionResult> Index()
        {
            var rta = await GetAsync<List<ResumenCierre>>("Inventario/ObtenerResumenCierre");
            return View(rta);
        }
     
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var rta = await GetAsync<List<ResumenCierre>>("Inventario/ObtenerResumenCierre");
            return PartialView("_List", rta);
        } 
    }
}