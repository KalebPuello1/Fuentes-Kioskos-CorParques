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
    public class PuntoComidaController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<Puntos>>($"Puntos/GetAllxTipoPunto/{(int)Enumerador.TiposPuntos.Comida}");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<Puntos>>($"Puntos/GetAllxTipoPunto/{(int)Enumerador.TiposPuntos.Comida}");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var _Punto = new Puntos();
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Atracciones}");
            _Punto.TipoPuntos = await GetAsync<IEnumerable<TipoGeneral>>("TipoPunto/ObtenerListaSimple");
            return PartialView("_Create", _Punto);
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
        public void RemoveFile(string name)
        {
            var path = Path.Combine(Server.MapPath("~/Images/temp/"), name);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
        public async Task<ActionResult> GetById(int id)
        {
            var item = await GetAsync<Puntos>($"Puntos/GetById/{id}");
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Atracciones}");
            item.TipoPuntos = await GetAsync<IEnumerable<TipoGeneral>>("TipoPunto/ObtenerListaSimple");
            return PartialView("_Edit", item);
        }
        public async Task<ActionResult> Insert(Puntos modelo)
        {
            modelo.UsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id; 
            var rta = await PostAsync<Puntos, string>("Puntos/Insert", modelo);
            if (rta.Correcto)
            {
                moveFile(modelo.Imagen, rta.Elemento.ToString());
            }
            return Json(rta, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Update(Puntos modelo)
        {
            modelo.UsuarioModicifacion = (Session["UsuarioAutenticado"] as Usuario).Id; 
            var imageOrigin = modelo.Imagen;

            if (!string.IsNullOrEmpty(modelo.Imagen))
            {
                imageOrigin = modelo.Imagen;
                modelo.Imagen = $"{modelo.Id}.png";
            }
            modelo.IdTipoPunto = (int)Enumerador.TiposPuntos.Comida;
            if (modelo.ActualizaHoraTipopunto)
            {
                var respuesta = await PutAsync<Puntos, string>($"Puntos/ActualizarHora?HoraInicial={modelo.HoraInicio}&HoraFin={modelo.HoraFin}&IdTipoPunto={modelo.IdTipoPunto}", modelo);
            }

            var rta = await PutAsync<Puntos, string>("Puntos/Update", modelo);
            if (rta == string.Empty)
            {
                if (!string.IsNullOrEmpty(modelo.Imagen))
                {
                    moveFile(imageOrigin, modelo.Id.ToString());
                }
            }
            return Json(new RespuestaViewModel { Correcto = rta == string.Empty, Mensaje = rta }, JsonRequestBehavior.AllowGet);
        }

        
        private string moveFile(string name, string newName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/temp/"), name);
            var newFile = newName + ".png";
            var pathDest = Path.Combine(Server.MapPath("~/Images/Attractions/"), newFile);
            if (System.IO.File.Exists(pathDest))
                System.IO.File.Delete(pathDest);
            if (System.IO.File.Exists(path))
                System.IO.File.Move(path, pathDest);
            return newFile;
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (await DeleteAsync($"Puntos/Delete/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error eliminando el parámetro. Por favor intentelo de nuevo" });
        }
    }
}