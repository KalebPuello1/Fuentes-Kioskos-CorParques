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
    public class TipoVehiculoPorParqueaderoController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<TipoVehiculoPorParqueadero>>("TipoVehiculoPorParqueadero/ObtenerTipoVehiculoPorParqueadero");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TarifasParqueadero}");

            foreach (var item in lista)
            {
                if (listaEstado.Any(e => e.Id == item.IdEstado))
                    item.Estado = listaEstado.FirstOrDefault(e => e.Id == item.IdEstado).Nombre;
            }

            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<TipoVehiculoPorParqueadero>>("TipoVehiculoPorParqueadero/ObtenerTipoVehiculoPorParqueadero");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TarifasParqueadero}");

            foreach (var item in lista)
            {
                if (listaEstado.Any(e => e.Id == item.IdEstado))
                    item.Estado = listaEstado.FirstOrDefault(e => e.Id == item.IdEstado).Nombre;
            }

            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new TipoVehiculoPorParqueadero();
            var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TarifasParqueadero}");
            modelo.ListaTipoVehiculo = listaTipoVehiculo;
            modelo.ListaEstados = listaEstado;
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(TipoVehiculoPorParqueadero modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var resultado = await PostAsync<TipoVehiculoPorParqueadero, string>("TipoVehiculoPorParqueadero/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Update(TipoVehiculoPorParqueadero modelo)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id; ;
            var resultado = await PutAsync<TipoVehiculoPorParqueadero, string>("TipoVehiculoPorParqueadero/Actualizar", modelo);
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
            var item = await GetAsync<TipoVehiculoPorParqueadero>($"TipoVehiculoPorParqueadero/GetById/{Id}");                        
            var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.TarifasParqueadero}");
            item.ListaEstados = listaEstado;
            item.ListaTipoVehiculo = listaTipoVehiculo;
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> UpdateEstado(int Id)
        {
            var modelo = await GetAsync<TipoVehiculoPorParqueadero>($"TipoVehiculoPorParqueadero/GetById/{Id}");
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id; ;
            modelo.IdEstado = (int)Enumerador.Estados.Inactivo;
            var resultado = await PutAsync<TipoVehiculoPorParqueadero, string>("TipoVehiculoPorParqueadero/Eliminar", modelo);
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