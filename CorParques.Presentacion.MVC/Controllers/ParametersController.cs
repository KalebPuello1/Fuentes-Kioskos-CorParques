using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ParametersController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<Parametro>>("Parameters/GetAll");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<Parametro>>("Parameters/GetAll");
            return PartialView("_List",lista);
        }

        public ActionResult GetPartial()
        {
            return PartialView("_Create");
        }

        public async Task<ActionResult> GetById(int id)
        {
            var item = await GetAsync<Parametro>($"Parameters/GetById/{id}");
            return PartialView("_Edit",item);
        }
        public async Task<ActionResult> Insert(Parametro modelo)
        {
            modelo.Creado = ((Usuario)Session["UsuarioAutenticado"]).Id;
            if (await PostAsync<Parametro,string>("Parameters/Insert", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el parámetro. Por favor inténtelo de nuevo" });
        }
        
        public async Task<ActionResult> Update(Parametro modelo)
        {
            modelo.Modificado = 2;
            if (await PutAsync<Parametro,string>("Parameters/Update", modelo)!=null)
                return Json(new RespuestaViewModel { Correcto = true },JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el parámetro. Por favor inténtelo de nuevo" });
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (await DeleteAsync($"Parameters/Delete/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error eliminando el parámetro. Por favor inténtelo de nuevo" });
        }
    }
}