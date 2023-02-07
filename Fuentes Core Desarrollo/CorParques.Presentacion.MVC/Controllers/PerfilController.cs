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
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
    public class PerfilController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<Perfil>>("Perfil/ObtenerLista");
            var perfiles = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Perfil}");
            ViewBag.Perfil = perfiles;
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<Perfil>>("Perfil/ObtenerLista");
            var perfiles = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Perfil}");
            ViewBag.Perfil = perfiles;
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new Perfil();
            modelo.ListaMenus = await GetAsync<IEnumerable<Menu>>("Menu/ObtenerListaActivos");
            //modelo.ListaEstados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Perfil}");
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(Perfil modelo, string hdListaMenus)
        {

            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = Enumerador.Estados.Activo.GetHashCode();
            List<Menu> menus = new List<Menu>();

            string[] ids = hdListaMenus.Split(',');
                foreach (string id in ids)
                    menus.Add(new Menu { IdMenu = Convert.ToInt32(string.IsNullOrEmpty(id) ? "0": id) });
                modelo.Menus = menus;
            
            var resultado = await PostAsync<Perfil, string>("Perfil/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Obtener(int Id)
        {
            var modelo = await GetAsync<Perfil>($"Perfil/GetById/{Id}");
            modelo.ListaMenus = await GetAsync<IEnumerable<Menu>>("Menu/ObtenerListaActivos");
            modelo.ListaEstados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Perfil}");

            return PartialView("_Edit", modelo);
        }

        public async Task<ActionResult> PerfilActivos(int IdPerfil)
        {
            var modelo = await GetAsync<Perfil>($"Perfil/GetById/{IdPerfil}");

            modelo.Lista  = await GetAsync<IEnumerable<Perfil>>($"Perfil/PerfilActivos/{IdPerfil}");
            ViewBag.ListaConflicto = await GetAsync<IEnumerable<Perfil>>($"Perfil/ConsultarSegregacion/{IdPerfil}");


            return PartialView("_SegregacionFunciones",modelo);
        }

        public async Task<ActionResult> UpdateSegregacion(int IdPerfil,string hdIdPerfilConflicto)
        {
            List<Perfil> listaPerfil = new List<Perfil>();
            string[] ids = hdIdPerfilConflicto.Split(',');
            foreach (string id in ids)
                listaPerfil.Add(new Perfil { IdPerfil = Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id), IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id });
           
            SegregacionFunciones segregacionFunciones = new SegregacionFunciones();

            segregacionFunciones.IdPerfil = IdPerfil;
            segregacionFunciones.IdUsuarioCreacion= (Session["UsuarioAutenticado"] as Usuario).Id;
            segregacionFunciones.ListaPerfilConflicto = listaPerfil;

            var resultado = await PutAsync<SegregacionFunciones, string>("Perfil/ActualizarSegregacion", segregacionFunciones);
            if (string.IsNullOrEmpty(resultado))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
        }

      

        public async Task<ActionResult> Update(Perfil modelo, string hdListaMenus)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            List<Menu> menus = new List<Menu>();
            string[] ids = hdListaMenus.Split(',');
            foreach (string id in ids)
                menus.Add(new Menu { IdMenu = Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id) });
            modelo.Menus = menus;
            var resultado = await PutAsync<Perfil, string>("Perfil/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateEstado(int Id)
        {
            var modelo = await GetAsync<Perfil>($"Perfil/GetById/{Id}");
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = Enumerador.Estados.Inactivo.GetHashCode();
            var resultado = await PutAsync<Perfil, string>("Perfil/Inactivar", modelo);
            if (string.IsNullOrEmpty(resultado))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
        }

        public static bool Seleccionar(string idMenu, string idsMenusSeleccionados)
        {
            string[] ids = idsMenusSeleccionados.Split(',');
            var idExistente = (from i in ids
                               where i == idMenu
                               select i).FirstOrDefault();
            return idExistente != null;
        }
    }
}