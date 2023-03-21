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
    public class TarifasParqueaderoController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<TarifasParqueadero>>("TarifasParqueadero/ObtenerLista");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<TarifasParqueadero>>("TarifasParqueadero/ObtenerLista");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new TarifasParqueadero();
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TarifasParqueadero}");
            var listaTipoTarifa = await GetAsync<IEnumerable<TipoGeneral>>("TipoTarifaParqueadero/ObtenerTiposTarifasParqueadero");
            var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");
            modelo.ListaEstados = listaEstado;
            modelo.ListaTipoTarifaParqueadero = listaTipoTarifa;
            modelo.ListaTipoVehiculo = listaTipoVehiculo;
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(TarifasParqueadero modelo)
        {

            modelo.FechaCreacion = DateTime.Now;
            modelo.UsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var resultado = await PostAsync<TarifasParqueadero, string>("TarifasParqueadero/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Update(TarifasParqueadero modelo)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.UsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var resultado = await PutAsync<TarifasParqueadero, string>("TarifasParqueadero/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Obtener(int Id)
        {
            var item = await GetAsync<TarifasParqueadero>($"TarifasParqueadero/GetById/{Id}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TarifasParqueadero}");
            var listaTipoTarifa = await GetAsync<IEnumerable<TipoGeneral>>("TipoTarifaParqueadero/ObtenerTiposTarifasParqueadero");
            var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");
            item.ListaEstados = listaEstado;
            item.ListaTipoTarifaParqueadero = listaTipoTarifa;
            item.ListaTipoVehiculo = listaTipoVehiculo;
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> UpdateEstado(int Id)
        {
            var modelo = await GetAsync<TarifasParqueadero>($"TarifasParqueadero/GetById/{Id}");
            modelo.FechaModificacion = DateTime.Now;
            modelo.UsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Inactivo; //Estado inactivo.
            var resultado = await PutAsync<TarifasParqueadero, string>("TarifasParqueadero/Actualizar", modelo);
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
            var item = await GetAsync<TarifasParqueadero>($"TarifasParqueadero/GetById/{Id}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TarifasParqueadero}");
            var listaTipoTarifa = await GetAsync<IEnumerable<TipoGeneral>>("TipoTarifaParqueadero/ObtenerTiposTarifasParqueadero");
            var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");
            item.ListaEstados = listaEstado;
            item.ListaTipoTarifaParqueadero = listaTipoTarifa;
            item.ListaTipoVehiculo = listaTipoVehiculo;
            return PartialView("_Detail", item);
        }

    }
}