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
    public class PasaporteUsoController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<PasaporteUso>>("PasaporteUso/ObtenerTodosPasaporte");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<PasaporteUso>>("PasaporteUso/ObtenerTodosPasaporte");
            return PartialView("_List",lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new PasaporteUso();
            var lista = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TipoBrazalete}");
            modelo.Estados = listaEstado;
            modelo.Atracciones = lista;
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Obtener(int Id)
        {
            var item = await GetAsync<PasaporteUso>($"PasaporteUso/Obtener/{Id}");
            var lista = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TipoBrazalete}");
            item.Estados = listaEstado;
            item.Atracciones = lista;
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> ActualizarPasaporteUso(PasaporteUso modelo)
        {
            
            modelo.UsuarioModicifacion = ((Usuario)Session["UsuarioAutenticado"]).Id;
            modelo.FechaModificacion = DateTime.Now;            
            modelo.UsuarioCreacion = ((Usuario)Session["UsuarioAutenticado"]).Id;
            modelo.FechaCreacion = DateTime.Now;
            if (UsosNoValidos(modelo.AtraccionesSeleccionadas))
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Existen atracciones seleccionadas sin numero de usos especificados." });
            }
            if (await PostAsync<PasaporteUso, string>("PasaporteUso/ActualizarPasaporteUso",  modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el Pasaporte. Por favor intentelo de nuevo" });
        }

        public async Task<ActionResult> Delete(int id)
        {
            var item = await GetAsync<PasaporteUso>($"PasaporteUso/Obtener/{id}");
            item.IdEstado = (int)Enumerador.Estados.Inactivo;
            item.UsuarioModicifacion = ((Usuario)Session["UsuarioAutenticado"]).Id;
            item.FechaModificacion = DateTime.Now;
            item.FechaCreacion = DateTime.Now;

            if (await PostAsync<PasaporteUso, string>("PasaporteUso/ActualizarPasaporteUso", item) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el Pasaporte. Por favor intentelo de nuevo" });
            
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

        public static bool UsosNoValidos(string AtraccionesSeleccionadas)
        {

            bool blnRetorno = false;
            string[] split;
            string[] splitText;

            if (!string.IsNullOrEmpty(AtraccionesSeleccionadas))
            {
                split = AtraccionesSeleccionadas.Split(',');
                for (int i = 0; i < split.Length; i++)
                {
                    splitText = split[i].Split('|');
                    if (int.Parse(splitText[1]) <= 0)
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