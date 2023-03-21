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
    public class MatrizPuntosController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            var lista = await ObtenerDatosIniciales();
            return View(lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var lista = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerTodosProductos");
            var excluir = (await ObtenerDatosIniciales()).Select(x=>x.Id).ToList();
            lista = lista.Where(x => !excluir.Contains(x.IdProducto)).ToList();
            return PartialView("_Create", lista);
        }
        

        public async Task<ActionResult> Insert(TipoGeneral modelo)
        {
            //modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            //modelo.FechaCreacion = DateTime.Now;
            //modelo.IdEstado = Enumerador.Estados.Activo.GetHashCode();
            var rta = await PostAsync<TipoGeneral, string>("MatrizPuntos/Insertar", modelo);
            if (string.IsNullOrEmpty(rta.Mensaje))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = rta.Mensaje });
        }

        public async Task<ActionResult> GetList()
        {
            var modelo = await ObtenerDatosIniciales();
            return PartialView("_List", modelo);
        }

        private async Task<IEnumerable<TipoGeneral>> ObtenerDatosIniciales()
        {
            return await GetAsync<IEnumerable<TipoGeneral>>("MatrizPuntos/ObtenerTodos");
            
        }
        

        public async Task<ActionResult> Delete(int id)
        {
            if (await DeleteAsync($"MatrizPuntos/Eliminar/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error eliminando el registro. Por favor intentelo de nuevo" });
        }
       
    }
}