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
    public class ConvenioDescuentoController : ControladorBase
    {
        //public async Task<ActionResult> Index()
        //{
        //    var lista = await GetAsync<IEnumerable<ConvenioDescuento>>("ConvenioDescuento/ObtenerConvenioDescuento");
        //    return View(lista);
        //}

        //public async Task<ActionResult> GetList()
        //{
        //    var lista = await GetAsync<IEnumerable<ConvenioDescuento>>("ConvenioDescuento/ObtenerConvenioDescuento");
        //    return PartialView("_List", lista);
        //}

        //public async Task<ActionResult> GetPartial()
        //{
        //    var modelo = new ConvenioDescuento();
        //    modelo.ListaLineas = await GetAsync<IEnumerable<TipoGeneralValor>>("Pos/ObtenerLineaProductos");
        //    modelo.ListaLineas = modelo.ListaLineas.Where(l => l.Id != 7);
        //    modelo.ListaEstados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioDescuento}");

        //    var AuxListaProductos = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos");
        //    List<TipoGeneralValor> auxTemList = new List<TipoGeneralValor>();
        //    foreach (var item in AuxListaProductos.Where(p=>p.CodSapTipoProducto == "7"))
        //    {
        //        auxTemList.Add(new TipoGeneralValor() {  Id = item.IdProducto, Nombre = item.Nombre });
        //    }
        //    modelo.ListaProductos = auxTemList.ToArray();
        //    return PartialView("_Create", modelo);
        //}

        //public async Task<ActionResult> Insert(ConvenioDescuento modelo)
        //{
        //    modelo.FecUsuarioCrea = DateTime.Now;
        //    modelo.IdUsuarioCrea = (Session["UsuarioAutenticado"] as Usuario).Id;
        //    modelo.IdEstado = (int)Enumerador.Estados.Activo;
        //    var resultado = await PostAsync<ConvenioDescuento, string>("ConvenioDescuento/Insertar", modelo);
        //    return Json(resultado, JsonRequestBehavior.AllowGet);

        //}

        //public async Task<ActionResult> Update(ConvenioDescuento modelo)
        //{
        //    modelo.FecUsuarioMod = DateTime.Now;
        //    modelo.IdUsuarioMod = (Session["UsuarioAutenticado"] as Usuario).Id;
        //    var resultado = await PutAsync<ConvenioDescuento, string>("ConvenioDescuento/Actualizar", modelo);
        //    if (string.IsNullOrEmpty(resultado))
        //    {
        //        return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public async Task<ActionResult> Obtener(int Id)
        //{
        //    var item = await GetAsync<ConvenioDescuento>($"ConvenioDescuento/GetById/{Id}");
        //    //var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");
        //    //var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioDescuento}");
        //    //var listaTipoConvenio = await GetAsync<IEnumerable<TipoGeneral>>("TipoConvenioDescuento/ObtenerLista");
        //    //item.ListaTipoVehiculo = listaTipoVehiculo;
        //    //item.ListaEstados = listaEstado;
        //    //item.ListaTipoConvenios = listaTipoConvenio;
        //    return PartialView("_Edit", item);
        //}

        //public async Task<ActionResult> UpdateEstado(int Id)
        //{
        //    var modelo = await GetAsync<ConvenioDescuento>($"ConvenioDescuento/GetById/{Id}");
        //    modelo.FecUsuarioCrea = DateTime.Now;
        //    modelo.IdUsuarioCrea = (Session["UsuarioAutenticado"] as Usuario).Id; ;
        //    modelo.IdEstado = (int)Enumerador.Estados.Inactivo;
        //    var resultado = await PutAsync<ConvenioDescuento, string>("ConvenioDescuento/Eliminar", modelo);
        //    if (string.IsNullOrEmpty(resultado))
        //    {
        //        return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public async Task<ActionResult> Detalle(int Id)
        //{
        //    var item = await GetAsync<ConvenioDescuento>($"ConvenioDescuento/GetById/{Id}");
        //    //var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");
        //    //var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioDescuento}");
        //    //var listaTipoConvenio = await GetAsync<IEnumerable<TipoGeneral>>("TipoConvenioDescuento/ObtenerLista");
        //    //item.ListaTipoVehiculo = listaTipoVehiculo;
        //    //item.ListaEstados = listaEstado;
        //    //item.ListaTipoConvenios = listaTipoConvenio;
        //    return PartialView("_Detail", item);
        //}
    }
}