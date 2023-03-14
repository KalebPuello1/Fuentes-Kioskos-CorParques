using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    [HandleError()]
    public class ErroresController : ControladorBase
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Err403()
        {
            return View();
        }
        
        public ActionResult Err404()
        {
            return View();
        }
        
        public ActionResult Err500()
        {
            return View();
        }
    }
}