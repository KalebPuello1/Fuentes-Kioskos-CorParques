using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    /// <summary>
    /// NMSR CU006-DE-1CI-ARS-001-Configuración centro médico
    /// </summary>
    public class CentroMedicoController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<CentroMedico>>("CentroMedico/ObtenerLista");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<CentroMedico>>("CentroMedico/ObtenerLista");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            ViewBag.Zonas = await GetAsync<IEnumerable<TipoGeneral>>("CentroMedico/Zonas");
            return PartialView("_Create");
        }

        public async Task<ActionResult> Insert(CentroMedico modelo)
        {

            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = Enumerador.Estados.Activo.GetHashCode();
            var resultado = await PostAsync<CentroMedico, string>("CentroMedico/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Obtener(int Id)
        {
            var item = await GetAsync<CentroMedico>($"CentroMedico/GetById/{Id}");
            ViewBag.Zonas = await GetAsync<IEnumerable<TipoGeneral>>("CentroMedico/Zonas");
            item.Id = ((IEnumerable<TipoGeneral>)ViewBag.Zonas).FirstOrDefault(x => x.Nombre.Equals(item.Descripcion)).Id;
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> Update(CentroMedico modelo)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = Enumerador.Estados.Activo.GetHashCode();
            var resultado = await PutAsync<CentroMedico, string>("CentroMedico/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> UpdateEstado(int Id)
        {
            var modelo = await GetAsync<CentroMedico>($"CentroMedico/GetById/{Id}");
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = Enumerador.Estados.Inactivo.GetHashCode();
            var resultado = await PutAsync<CentroMedico, string>("CentroMedico/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}