using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Redencion.Core.Controllers
{
    [HandleError()]
    public class ErroresController : ControladorBase
    {

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Err403()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Err404()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Err500()
        {
            return View();
        }
    }
}