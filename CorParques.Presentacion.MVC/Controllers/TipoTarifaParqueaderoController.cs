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
    public class TipoTarifaParqueaderoController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<TipoTarifaParqueadero>>("TipoTarifaParqueadero/ObtenerLista");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<TipoTarifaParqueadero>>("TipoTarifaParqueadero/ObtenerLista");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new TipoTarifaParqueadero();
            var lista = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TipoTarifaParqueadero}");
            modelo.ListaEstados = lista;
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(TipoTarifaParqueadero modelo)
        {

            modelo.FechaCreacion = DateTime.Now;
            modelo.UsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var resultado = await PostAsync<TipoTarifaParqueadero, string>("TipoTarifaParqueadero/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }
        
        public async Task<ActionResult> Obtener(int Id)
        {
            var item = await GetAsync<TipoTarifaParqueadero>($"TipoTarifaParqueadero/GetById/{Id}");            
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TipoTarifaParqueadero}");
            item.ListaEstados = listaEstado;            
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> Update(TipoTarifaParqueadero modelo)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.UsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var resultado = await PutAsync<TipoTarifaParqueadero, string>("TipoTarifaParqueadero/Actualizar", modelo);
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
            var modelo = await GetAsync<TipoTarifaParqueadero>($"TipoTarifaParqueadero/GetById/{Id}");
            modelo.FechaModificacion = DateTime.Now;
            modelo.UsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Inactivo; //Estado inactivo.
            var resultado = await PutAsync<TipoTarifaParqueadero, string>("TipoTarifaParqueadero/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Detalle(int Id)
        {
            var item = await GetAsync<TipoTarifaParqueadero>($"TipoTarifaParqueadero/GetById/{Id}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TipoTarifaParqueadero}");
            item.ListaEstados = listaEstado;
            return PartialView("_Detail", item);
        }

    }
}