//Cambioquitar: Este controlador usa el enumerador de perfiles.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteSoporteITController : ControladorBase
    {
        

        public async Task<ActionResult> Index()
        {
            IEnumerable<SoporteIT> soporteIT = null;
            return View(soporteIT);
        }

        [HttpGet]
        public async Task<ActionResult> ConsultarFecha(string Fecha)
        {
           
            IEnumerable<SoporteIT> soporteIT = await GetAsync<IEnumerable<SoporteIT>>($"ReporteSoporteIT/ObtenerSoporteIT/{Fecha}");
            return PartialView("_List", soporteIT);
           
        }
    }
}