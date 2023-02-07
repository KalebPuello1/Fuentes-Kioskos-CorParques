using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class BrazaleteArchivoController : ControladorBase
    {
        // GET: BrazaleteArchivo
        public async Task<ActionResult> Index()
        {
            IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
            return View(brazaletes);
        }


        public async Task<ActionResult> GetList()
        {
            IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
            return PartialView("_List", brazaletes);
        }


        public async Task<ActionResult> GetById(int idProducto)
        {
            return PartialView("_Edit", idProducto);
        }

        

        public async Task<ActionResult> Update(string observacion, int idProducto)
        {
            string nombreArchivo = string.Empty;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                nombreArchivo = string.Concat(nombreArchivo, "_", DateTime.Now.ToString("ddMMyyyymmss"), Path.GetExtension(file.FileName));
                string archivo=  Path.Combine(Server.MapPath("~/Archivos/Brazaletes/"), nombreArchivo);
                file.SaveAs(archivo); 
            }

            ArchivoBrazalete archivoBrazalete = new ArchivoBrazalete();
            archivoBrazalete.IdProducto = idProducto;
            archivoBrazalete.Observacion = observacion;
            archivoBrazalete.IdEstado = 1;
            archivoBrazalete.Archivo = nombreArchivo.Trim();
            archivoBrazalete.FechaCreacion = DateTime.Now;
            archivoBrazalete.UsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).NombreUsuario;

            var rta = await PutAsync<ArchivoBrazalete, string>("Pos/ActualizarArchivoBrazalete", archivoBrazalete);
            if (rta != string.Empty)
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el el archivo brazalete." });
                
        }

        public FileResult Download(string nombre)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/Archivos/Brazaletes/"), nombre));
            string fileName = nombre;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
