using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class AlistamientoPendienteSupervisorController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            IEnumerable<AlistamientoPendiente> _list = new List<AlistamientoPendiente>();
            var objAlistamiento = await GetAsync<IEnumerable<AlistamientoPendiente>>($"AlistamientoPendiente/ObtenerAlistamientoPendiente/{1}");
            _list = objAlistamiento;
            return View(_list);
        }
    }
}