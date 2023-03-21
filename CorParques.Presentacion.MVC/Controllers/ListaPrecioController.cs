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
    public class ListaPrecioController : ControladorBase
    {
        // GET: Groups
        public async Task<ActionResult> Index()
        {
            var listaListaPrecio = await GetAsync<IEnumerable<ListaPrecio>>("ListaPrecio/GetAll");
            return View(listaListaPrecio);
        }


        public async Task<ActionResult> GetList()
        {
            var listaListaPrecio = await GetAsync<IEnumerable<ListaPrecio>>("ListaPrecio/GetAll");
            return PartialView("_List", listaListaPrecio);
        }

        public ActionResult GetPartial()
        {
            return PartialView("_Create");
        }

        public async Task<ActionResult> GetById(int id)
        {
            var item = await GetAsync<ListaPrecio>($"ListaPrecio/GetById/{id}");
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> Insert(ListaPrecio modelo)
        {
            //modelo.Creado = 1;
            if (await PostAsync<ListaPrecio,string>("ListaPrecio/Insert", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el parámetro. Por favor inténtelo de nuevo" });
        }

        public async Task<ActionResult> Update(ListaPrecio modelo)
        {
            //modelo.Modificado = 2;
            if (await PutAsync<ListaPrecio,string>("ListaPrecio/Update", modelo)!="")
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el parámetro. Por favor inténtelo de nuevo" });
        }

        public async Task<ActionResult> Delete(int id)
        {
            //modelo.Modificado = 2;
            if (await DeleteAsync($"ListaPrecio/Delete/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el parámetro. Por favor inténtelo de nuevo" });
        }

    }
}