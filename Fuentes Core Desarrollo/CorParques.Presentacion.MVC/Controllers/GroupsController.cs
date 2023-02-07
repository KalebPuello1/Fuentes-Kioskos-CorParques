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
    public class GroupsController : ControladorBase
    {
        // GET: Groups
        public async Task<ActionResult> Index()
        {
            var listaGrupo = await GetAsync<IEnumerable<Grupo>>("Grupo/GetAll");
            return View(listaGrupo);
        }

        public async Task<ActionResult> GetList()
        {
            var listaGrupo = await GetAsync<IEnumerable<Grupo>>("Grupo/GetAll");
            return PartialView("_List", listaGrupo);
        }

        public async Task<ActionResult> GetPartial()
        {
            var item = new Grupo();
            item.Puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");            
            return PartialView("_Create", item);
        }

        public async Task<ActionResult> GetById(int id)
        {
            //var item = await GetAsync<Grupo>($"Grupo/GetById/{id}");
            var item = await GetAsync<Grupo>($"Grupo/ObtenerIdGrupo/{id}");
            var lista = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.GrupoNotificacion}");
            item.ListaEstados = lista;
            item.Puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");            
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> Insert(Grupo modelo)
        {
            //modelo.Creado = 1;
            //if (await PostAsync<Grupo,string>("Grupo/Insert", modelo) != null)
            //    return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            //return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el parámetro. Por favor intentelo de nuevo" });
            modelo.EstadoId = (int)Enumerador.Estados.Activo;
            modelo.Creado = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.Modificado = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.FechaModificado = DateTime.Now;
            modelo.FechaCreado = DateTime.Now;
            modelo.ValidaNombre = true; 
            var respuesta = await PostAsync<Grupo, string>("Grupo/ActualizarGrupo", modelo);
            if (string.IsNullOrEmpty(respuesta.Mensaje))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el grupo. verifique que no exista el grupo" });
            
        }

        public async Task<ActionResult> Update(Grupo modelo)
        {
            //modelo.Modificado = 2;
            //if (await PutAsync<Grupo,string>("Grupo/Update", modelo)!=null)
            //    return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            //return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el parámetro. Por favor intentelo de nuevo" });            
            modelo.Creado = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.Modificado = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.FechaModificado = DateTime.Now;
            modelo.FechaCreado = DateTime.Now;
            //modelo.ValidaNombre = false;            
            //if (await PostAsync<Grupo, string>("Grupo/ActualizarGrupo", modelo) != null)
            modelo.ValidaNombre = true;
            var respuesta = await PostAsync<Grupo, string>("Grupo/ActualizarGrupo", modelo);
            if (string.IsNullOrEmpty(respuesta.Mensaje))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el grupo. Por favor intentelo de nuevo" });
        }

        public async Task<ActionResult> UpdateStatus(Grupo modelo)
        {
            modelo.Modificado = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.FechaModificado = DateTime.Now;
            if (await PutAsync<Grupo, string>("Grupo/Update", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el grupo. Por favor intentelo de nuevo" });
        }

        public async Task<ActionResult> Delete(int id)
        {
            //modelo.Modificado = 2;
            if (await DeleteAsync($"Grupo/Delete/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el parámetro. Por favor intentelo de nuevo" });
        }

        //public async Task<ActionResult> Detail(int id)
        //{            
        //    var item = await GetAsync<Grupo>($"Grupo/ObtenerIdGrupo/{id}");            
        //    item.Puntos= await GetAsync<IEnumerable<Puntos>>($"UsuarioGrupo/ObtenerUsuariosPorGrupo/{id}");
        //    return PartialView("_Detail", item);
        //}

        public static bool Asociada(string strIdAtraccion, string strIdAtraccionesXBrazalete)
        {

            bool blnRetorno = false;
            string[] split;

            if (!string.IsNullOrEmpty(strIdAtraccionesXBrazalete))
            {
                split = strIdAtraccionesXBrazalete.Split(',');
                for (int i = 0; i < split.Length; i++)
                {
                    if (strIdAtraccion == split[i])
                    {
                        blnRetorno = true;
                        break;
                    }

                }
            }
            return blnRetorno;
        }

    }
}
