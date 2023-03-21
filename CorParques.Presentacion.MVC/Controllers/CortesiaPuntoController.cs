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
    public class CortesiaPuntoController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<CortesiaPunto>>("CortesiaPunto/ObtenerPorDestrezaAtraccion/0/0");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<CortesiaPunto>>("CortesiaPunto/ObtenerPorDestrezaAtraccion/0/0");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new CortesiaPunto();

            var _csAtracciones = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Atracciones}");
            var _csDestrezas = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Destrezas}");

            var listaDestrezas = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/ObtenerTodosPuntosActivos");
            var listaAtracciones = await GetAsync<IEnumerable<TipoGeneral>>($"CortesiaPunto/ObtenerProductos/{_csAtracciones.CodigoSap + ","+ _csDestrezas.CodigoSap}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");            
            modelo.ListaDestrezas = listaDestrezas;
            modelo.ListaAtracciones = listaAtracciones.OrderBy(xx=>xx.Id);
            modelo.ListaEstados = listaEstado;            
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(CortesiaPunto modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Activo;
            var resultado = await PostAsync<CortesiaPunto, string>("CortesiaPunto/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Update(CortesiaPunto modelo)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var resultado = await PutAsync<CortesiaPunto, string>("CortesiaPunto/Actualizar", modelo);
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
            var item = await GetAsync<CortesiaPunto>($"CortesiaPunto/GetById/{Id}");
            var _csAtracciones = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Atracciones}");
            var _csDestrezas = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Destrezas}");

            var listaDestrezas = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/ObtenerTodosPuntosActivos");
            var listaAtracciones = await GetAsync<IEnumerable<TipoGeneral>>($"CortesiaPunto/ObtenerProductos/{_csAtracciones.CodigoSap + "," + _csDestrezas.CodigoSap}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");
            item.ListaDestrezas = listaDestrezas;
            item.ListaAtracciones = listaAtracciones;
            item.ListaEstados = listaEstado;
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> UpdateEstado(int Id)
        {
            var modelo = await GetAsync<CortesiaPunto>($"CortesiaPunto/GetById/{Id}");
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Inactivo;
            var resultado = await PutAsync<CortesiaPunto, string>("CortesiaPunto/Eliminar", modelo);
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
            var item = await GetAsync<CortesiaPunto>($"CortesiaPunto/GetById/{Id}");
            var _csAtracciones = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Atracciones}");
            var _csDestrezas = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.Destrezas}");

            var listaDestrezas = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/ObtenerTodosPuntosActivos");
            var listaAtracciones = await GetAsync<IEnumerable<TipoGeneral>>($"CortesiaPunto/ObtenerProductos/{_csAtracciones.CodigoSap + "," + _csDestrezas.CodigoSap}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");
            item.ListaDestrezas = listaDestrezas;
            item.ListaAtracciones = listaAtracciones;
            item.ListaEstados = listaEstado;
            return PartialView("_Detail", item);
        }
    }
}