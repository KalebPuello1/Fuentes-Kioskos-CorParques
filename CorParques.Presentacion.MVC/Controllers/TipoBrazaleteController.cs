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
    public class TipoBrazaleteController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerTodosBrazalete");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerTodosBrazalete");
            return PartialView("_List",lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new TipoBrazalete();
            var lista = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TipoBrazalete}");
            modelo.Estados = listaEstado;
            modelo.Atracciones = lista;
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Obtener(int Id)
        {
            var item = await GetAsync<TipoBrazalete>($"TipoBrazalete/Obtener/{Id}");
            var lista = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TipoBrazalete}");
            item.Estados = listaEstado;
            item.Atracciones = lista;
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> ObtenerAtraccionxBrazalete(int Id)
        {
            var item = await GetAsync<TipoBrazalete>($"TipoBrazalete/ObtenerAtraccionxBrazalete/{Id}");
            var lista = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TipoBrazalete}");
            item.Estados = listaEstado;
            item.Atracciones = lista;
            return PartialView("_Edit",item);
        }
        public async Task<ActionResult> ActualizarTipoBrazalete(TipoBrazalete modelo)
        {
            
            modelo.UsuarioModicifacion = ((Usuario)Session["UsuarioAutenticado"]).Id;
            modelo.FechaModificacion = DateTime.Now;            
            modelo.UsuarioCreacion = ((Usuario)Session["UsuarioAutenticado"]).Id;
            modelo.FechaCreacion = DateTime.Now;            
            if (await PostAsync<TipoBrazalete, string>("TipoBrazalete/ActualizarTipoBrazalete",  modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el parámetro. Por favor intentelo de nuevo" });
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (await DeleteAsync($"TipoBrazalete/block/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error eliminando el parámetro. Por favor intentelo de nuevo" });
        }

        public static int Asociada(string strIdAtraccion, string strIdAtraccionesXBrazalete)
        {

            int blnRetorno = -1;
            string[] split;
            string[] splitText;

            if (!string.IsNullOrEmpty(strIdAtraccionesXBrazalete))
            {
                split = strIdAtraccionesXBrazalete.Split(',');
                for (int i = 0; i < split.Length; i++)
                {
                    splitText = split[i].Split('|');
                    if (strIdAtraccion == splitText[0])
                    {
                        blnRetorno = int.Parse(splitText[1]);
                        break;
                    }

                }
            }
            return blnRetorno;
        }
    }
}