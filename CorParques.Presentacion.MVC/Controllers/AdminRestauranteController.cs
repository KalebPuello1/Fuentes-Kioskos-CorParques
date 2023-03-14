using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class AdminRestauranteController :ControladorBase
    {
        // GET: AdminRestaurante
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Productos()
        {
            var _ListaTodosProductosSAP = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerTodosProductosRestaurante");
            ViewBag.ListaTodosProductosSAP = _ListaTodosProductosSAP;

            ViewBag.ListAyB = _ListaTodosProductosSAP.Where(l => l.CtgProducto == 1).ToArray();
            return View(ViewBag.ListAyB);
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerProdRestaurante(int id)
        {
            var item = await GetAsync<Producto>($"Pos/ObtenerProducto/{id}");
            ViewBag.TipoAcompaRestaurante = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/ObtenerTipoAcompaRestaurante");
            ViewBag.TipoProductosRestaurante = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/ObtenerTipoProductosRestaurante");
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> TipoAcompanamiento()
        {
            var _ListaTodosTipoAcompa = await GetAsync<IEnumerable<TipoAcompanamiento>>("Puntos/ObtenerTipoAcompaRestaurante");

            return View(_ListaTodosTipoAcompa);
        }
        public async Task<ActionResult> EditTipoAcompanamiento()
        {
            var _ListaTodosTipoAcompa = await GetAsync<IEnumerable<TipoAcompanamiento>>("Puntos/ObtenerTipoAcompaRestaurante");

            return View(_ListaTodosTipoAcompa);
        }
    
        public async Task<ActionResult> DeleteipoAcompanamiento()
        {
            var _ListaTodosTipoAcompa = await GetAsync<IEnumerable<TipoAcompanamiento>>("Puntos/ObtenerTipoAcompaRestaurante");

            return View(_ListaTodosTipoAcompa);
        }
        public async Task<ActionResult> GetById(int idTipoAcompa)

        {
            var item = await GetAsync<TipoAcompanamiento>($"Pos/ObtenerTipoAcompaRestauranteXId/{idTipoAcompa}");
            var estados = new TipoGeneral();
            var listaestados =  new List<TipoGeneral>();
            estados.Id = 1;
            estados.Nombre = "Activo";
            listaestados.Add(estados);
            estados.Id = 2;
            estados.Nombre = "Inactivo";
            listaestados.Add(estados);

            ViewBag.Estados = listaestados;
            return PartialView("_EditTipoAcompanamiento", item);
        }

        public async Task<ActionResult> Update(TipoAcompanamiento modelo)
        {
            //Pos / ActualizarProducto

           var  resultado = await PostAsync<TipoAcompanamiento, string>("Pos/ActualizarTipoAcompaRestaurante", modelo);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


    }
}