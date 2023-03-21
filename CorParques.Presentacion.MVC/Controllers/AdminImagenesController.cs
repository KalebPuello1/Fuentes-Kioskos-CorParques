using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class AdminImagenesController : ControladorBase
    {
        // GET: AdminImagenes
        public async Task<ActionResult> Index()
        {
            ViewBag.CategoriaImagenes = await GetAsync<IEnumerable<CategoriaImagenes>>("Cortesia/ListarCategoriaImagenes");
            ViewBag.VisualPantallas = await GetAsync<IEnumerable<VisualPantallas>>($"Cortesia/ListarVisualPantallas/{0}");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ListarVisualPantallas(int IdCategoria)
        {

            var resultado = new object();
            try
            {
                var ListPantalla = await GetAsync<IEnumerable<VisualPantallas>>($"Cortesia/ListarVisualPantallas/{IdCategoria}");
                if (ListPantalla == null)
                {
                    ListPantalla = new List<VisualPantallas>();
                }
              
                return Json(ListPantalla, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }

        [HttpPost]
        public async Task<ActionResult> ObtenerImagenAdminXCodpantalla(string CodPantalla)
        {

            var resultado = new object();
            try
            {
                var ListPantalla2 = await GetAsync<IEnumerable<ImagenAdmin>>($"Cortesia/ObtenerImagenAdminXCodpantalla/{CodPantalla}");
                var ListPantalla = new ImagenAdmin();
                if (ListPantalla2 != null)
                {
                    ListPantalla = ListPantalla2.FirstOrDefault();
                }
                else
                {
                   
                }

                //return Json(ListPantalla, JsonRequestBehavior.AllowGet);
                return PartialView("_Content", ListPantalla);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }
           
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
            var newFile = newName ;
            var pathDest = Path.Combine(Server.MapPath("~/Images/Productos/"), newFile);
            if (System.IO.File.Exists(pathDest))
                System.IO.File.Delete(pathDest);
            if (System.IO.File.Exists(path))
                System.IO.File.Move(path, pathDest);
            return newFile;
        }

        public async Task<ActionResult> Update(ImagenAdmin modelo)
        {
            //Pos / ActualizarProducto

            modelo.UsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).NombreUsuario;
            modelo.UsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).NombreUsuario;
            var imageOrigin = modelo.NombreImagen;

            if (!string.IsNullOrEmpty(modelo.NombreImagen))
            {
                imageOrigin = modelo.NombreImagen;
                modelo.NombreImagen = modelo.NombreImagen;
            }

            var rta = await PostAsync<ImagenAdmin, string>("Cortesia/ActualizarAdminImagenes", modelo);
            if (rta != null)
            {
                if (!string.IsNullOrEmpty(modelo.NombreImagen))
                {
                    moveFile(imageOrigin, modelo.NombreImagen.ToString());
                }
            }

            return Json(new RespuestaViewModel { Correcto = rta != null, Mensaje = rta.ToString() }, JsonRequestBehavior.AllowGet);
        }

        // GET: AdminImagenes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminImagenes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminImagenes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminImagenes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminImagenes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminImagenes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminImagenes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
