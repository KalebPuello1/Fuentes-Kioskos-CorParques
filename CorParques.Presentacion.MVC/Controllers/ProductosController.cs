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
    public class ProductosController : ControladorBase
    {
        // GET: Productos
        public async Task<ActionResult> Index()
        {
            var _model = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos");
          
            return View(_model);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos");
            return PartialView("_List", lista);
        }
        public async Task<ActionResult> PuntosEntrega()
        {
            var _model = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductosPtoEntrega");
            ViewBag.ListaPuntos = await GetAsync<List<Puntos>>("Apertura/ObtenerPuntosSinApertura");
            return View(_model);
        }
        public async Task<ActionResult> GetListPuntosEntrega()
        {

            var _model = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductosPtoEntrega");
            ViewBag.ListaPuntos = await GetAsync<List<Puntos>>("Apertura/ObtenerPuntosSinApertura");
            return PartialView("_ListPuntosEntrega", _model);
        }
        public async Task<ActionResult> GetById(int id)
        {
            var item = await GetAsync<Producto>($"Pos/ObtenerProducto/{id}");
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Puntos}");
            ViewBag.LineaProductos = await GetAsync<IEnumerable<TipoGeneral>>("Pos/ObtenerLineaProductos");
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> GetByIdPuntosEntrega(int id)
        {
            var item = await GetAsync<Producto>($"Pos/ObtenerProductoPtoEntrega/{id}");
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Puntos}");
            ViewBag.LineaProductos = await GetAsync<IEnumerable<TipoGeneral>>("Pos/ObtenerLineaProductos");
            ViewBag.ListaPuntos = await GetAsync<List<Puntos>>("Puntos/GetAll");
            return PartialView("_EditPuntosEntrega", item);
        }
        public async Task<ActionResult> UpdatePuntosEntrega(Producto modelo)
        {
            //Pos / ActualizarProducto

            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var imageOrigin = modelo.Imagen;

           

            var rta = await PutAsync<Producto, string>("Pos/ActualizarProductoPuntosEntrega", modelo);

            return Json(new RespuestaViewModel { Correcto = rta != string.Empty, Mensaje = rta }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Update(Producto modelo)
        {
            //Pos / ActualizarProducto

            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var imageOrigin = modelo.Imagen;

            if (!string.IsNullOrEmpty(modelo.Imagen))
            {
                imageOrigin = modelo.Imagen;
                modelo.Imagen = $"{modelo.IdProducto}.png";
            }

            var rta = await PutAsync<Producto, string>("Pos/ActualizarProducto", modelo);
            if (rta != string.Empty)
            {
                if (!string.IsNullOrEmpty(modelo.Imagen))
                {
                    moveFile(imageOrigin, modelo.IdProducto.ToString());
                }
            }

            return Json(new RespuestaViewModel { Correcto = rta != string.Empty, Mensaje = rta }, JsonRequestBehavior.AllowGet);
        }
        

        public void RemoveFile(string name)
        {
            var path = Path.Combine(Server.MapPath("~/Images/temp/"), name);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        [HttpPost]
        public void SaveFile()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/temp/"), fileName);
                    file.SaveAs(path);
                }
            }

        }

        private string moveFile(string name, string newName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/temp/"), name);
            var newFile = newName + ".png";
            var pathDest = Path.Combine(Server.MapPath("~/Images/Productos/"), newFile);
            if (System.IO.File.Exists(pathDest))
                System.IO.File.Delete(pathDest);
            if (System.IO.File.Exists(path))
                System.IO.File.Move(path, pathDest);
            return newFile;
        }

    }
}