using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class NotificacionesAppController : ControladorBase
    {
        // GET: NotificacionesApp
        public ActionResult Index()
        {
            return View();
        }
    }
}