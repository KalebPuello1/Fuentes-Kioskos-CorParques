using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class NotificacionController : ControladorBase
    {
        // GET: Groups
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var listaGrupos = await GetAsync<IEnumerable<Grupo>>("Grupo/ObtenerGruposActivos");
            return View(listaGrupos);
        }
        [HttpGet]
        public async Task<ActionResult> setView(int id)
        {
            var rta = await GetAsync<string>($"Notificacion/SetView/{id}");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        
        public async Task<JsonResult> EnviarNotificacion(Notificacion modelo)
        {
            modelo.UsuarioCreacion = ((Usuario)Session["UsuarioAutenticado"]).Id;
            var resultado = await PostAsync<Notificacion, string>("Notificacion/Set", modelo);
            
            return Json(resultado,JsonRequestBehavior.AllowGet);
        }
    }
}
