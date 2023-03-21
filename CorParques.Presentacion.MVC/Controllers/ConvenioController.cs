using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using ClosedXML.Excel;
using CorParques.Negocio.Entidades;
using System.Globalization;
using System.Threading.Tasks;
using CorParques.Transversales.Util;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ConvenioController : ControladorBase
    {
        
        /// <summary>
        /// RDSH: Inicializa la lista de convenios.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var modelo = await GetAsync<IEnumerable<Convenio>>("Convenio/ObtenerListaConvenios");
            return View(modelo);
        }

        /// <summary>
        /// RDSH: Refresca la lista de convenios luego de guardar / actualizar.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<Convenio>>("Convenio/ObtenerListaConvenios");
            return PartialView("_List", lista);
        }

        /// <summary>
        /// RDSH: Abre la pantalla de creacion de convenio.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPartial()
        {

            Convenio objConvenio = new Convenio();
            IEnumerable<Producto> objListaProductos = null;
            objListaProductos = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos");            
            objConvenio.ListaTipoProducto = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
            objListaProductos = objListaProductos.Where(x => x.IdEstado == Enumerador.Estados.Activo.GetHashCode()).ToList();
            objConvenio.ListaProducto = objListaProductos.Select(x => new ListaProducto { CodSapTipoProducto = x.CodSapTipoProducto, CodSapProducto = x.CodigoSap, Nombre = x.Nombre, Precio = x.Precio }).ToList().OrderBy(x => x.Nombre);
            return PartialView("_Create", objConvenio);
        }

        /// <summary>
        /// RDSH: Inserta un convenio.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Insert(Convenio modelo)
        {
            try
            {
                modelo.FechaCreacion = DateTime.Now;
                modelo.IdUsuarioCreacion = IdUsuarioLogueado;
                if (modelo.CodSapPedido == null)
                    modelo.CodSapPedido = string.Empty;

                modelo.FechaInicial = Utilidades.FormatoFechaValido(modelo.FechaInicialString);
                modelo.FechaFinal = Utilidades.FormatoFechaValido(modelo.FechaFinalString);

                var resultado = await PostAsync<Convenio, string>("Convenio/Insertar", modelo);
                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Insert_ConvenioController");
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Hubo un inconveniente al insertar el convenio, informe al administrador." }, JsonRequestBehavior.AllowGet);
            }     

        }

        /// <summary>
        /// RDSH: Actualiza la informacion de un convenio.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Update(Convenio modelo)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = IdUsuarioLogueado;
            modelo.FechaInicial = Utilidades.FormatoFechaValido(modelo.FechaInicialString);
            modelo.FechaFinal = Utilidades.FormatoFechaValido(modelo.FechaFinalString);
            
            var resultado = await PutAsync<Convenio, string>("Convenio/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Busca por ID de convenio para la edición
        /// EDSP: 29/12/2017
        /// Se Envia el precio del producto actualizar para calcular el porcentaje
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Obtener(int Id)
        {
            var item = await GetAsync<Convenio>($"Convenio/GetById/{Id}");
            IEnumerable<Producto> objListaProductos = null;
            objListaProductos = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos");
            objListaProductos = objListaProductos.Where(x => x.IdEstado == Enumerador.Estados.Activo.GetHashCode()).ToList();
            item.ListaProducto = objListaProductos.Select(x => new ListaProducto { CodSapTipoProducto = x.CodSapTipoProducto, CodSapProducto = x.CodigoSap, Nombre = x.Nombre, Precio = x.Precio }).ToList().OrderBy(x => x.Nombre);
            item.FechaInicialString = item.FechaInicial.ToString("dd/MM/yyyy");
            item.FechaFinalString = item.FechaFinal.ToString("dd/MM/yyyy");
            item.ListaTipoProducto = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
            return PartialView("_Edit", item);
        }

        /// <summary>
        /// RDSH: Retorna la lista de productos asociada al tipo de producto.
        /// </summary>
        /// <param name="CodigoSapTipoProducto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ObtenerProductos(string CodigoSapTipoProducto)
        {

            IEnumerable<TipoGeneral> objListaTipoGeneral = new List<TipoGeneral>();
            IEnumerable<Producto> objProducto = null;

            try
            {                
                if (CodigoSapTipoProducto.Trim().Length > 0)
                {
                    objProducto = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{CodigoSapTipoProducto}");
                    objListaTipoGeneral = objProducto.Select(x => new TipoGeneral { CodSAP = x.CodigoSap, Nombre = x.Nombre }).ToList();
                }
               
            }
            catch (Exception)
            {
                throw;
            }

            return Json(objListaTipoGeneral, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// RDSH: Muestra detalle de un convenio.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Detalle(int Id)
        {
            var item = await GetAsync<Convenio>($"Convenio/GetById/{Id}");
            IEnumerable<Producto> objListaProductos = null;
            objListaProductos = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos");
            objListaProductos = objListaProductos.Where(x => x.IdEstado == Enumerador.Estados.Activo.GetHashCode()).ToList();
            item.ListaProducto = objListaProductos.Select(x => new ListaProducto { CodSapTipoProducto = x.CodSapTipoProducto, CodSapProducto = x.CodigoSap, Nombre = x.Nombre, Precio = x.Precio }).ToList().OrderBy(x => x.Nombre);
            item.FechaInicialString = item.FechaInicial.ToString("dd/MM/yyyy");
            item.FechaFinalString = item.FechaFinal.ToString("dd/MM/yyyy");
            item.ListaTipoProducto = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
            return PartialView("_Detail", item);
        }

        /// <summary>
        /// EDSP: Obtener vista parcial de exclusiones
        /// </summary>
        public async Task<ActionResult> Exclusion(int id)
        {
            ViewBag.IdConvenio = id;
            var listConvenio = await GetAsync<IEnumerable<ExclusionConvenio>>($"Convenio/ObtenerExclusionesPorConvenio/{id}");
            if (listConvenio != null && listConvenio.Count() > 0)
                listConvenio = listConvenio.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo);
            return PartialView("_Exclusion", listConvenio);
        }

        /// <summary>
        /// EDSP: Actualizar precios convenios 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ActualizarPreciosConvenios()
        {
            var modelo = new Convenio();
            modelo.ListaTipoProducto = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
            var objListaProductos = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos");
            objListaProductos = objListaProductos.Where(x => x.IdEstado == Enumerador.Estados.Activo.GetHashCode()).ToList();
            modelo.ListaProducto = objListaProductos.Select(x => new ListaProducto { CodSapTipoProducto = x.CodSapTipoProducto, CodSapProducto = x.CodigoSap, Nombre = x.Nombre, Precio = x.Precio }).ToList().OrderBy(x => x.Nombre);
            return View(modelo);
        }

        /// <summary>
        /// EDSP: Método que invoca el evento de actualizar precio 
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ActualizarPrecio(ActualizarPrecios modelo)
        {
            var resultado = await PutAsync<ActualizarPrecios, string>("Convenio/ActualizarPrecios", modelo);
            return Json(string.IsNullOrEmpty(resultado) ? new RespuestaViewModel { Correcto = true }
                                    : new RespuestaViewModel { Correcto = false, Mensaje = resultado }
                , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// EDSP: Vista de exclusiones
        /// </summary>
        public async Task<ActionResult> ExclusionesConvenio()
        {
            var modelo = await GetAsync<IEnumerable<ExclusionConvenio>>("Convenio/ObtenerListaExclusion");
            if (modelo != null && modelo.Count() > 0)
                modelo = modelo.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo);
            return View(modelo);
        }

        /// <summary>
        /// EDSP: Vista productos convenio fechas especiales
        /// </summary>
        public async Task<ActionResult> ProductoConvenio(int id)
        {

            var ListaTipoProducto = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
            var objListaProductos = await GetAsync<IEnumerable<Producto>>("Pos/ObtenerProductos");
            objListaProductos = objListaProductos.Where(x => x.IdEstado == Enumerador.Estados.Activo.GetHashCode()).ToList();

            var productosConvenio = await GetAsync<IEnumerable<ConvenioProducto>>($"Convenio/ObtenerProductoConvenio/{id}");
            if (productosConvenio != null && productosConvenio.Count() > 0)
                productosConvenio = productosConvenio.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo);

            ViewBag.productos = objListaProductos;
            ViewBag.TipoProducto = ListaTipoProducto;
            ViewBag.idConvenio = id;
            ViewBag.productosConvenio = productosConvenio;

            return PartialView("_ProductosConvenio");
        }

        /// <summary>
        /// EDSP: Agregar exclusión
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AgregarExclusion(ExclusionConvenio modelo)
        {
            modelo.FechaInicio = Utilidades.FormatoFechaValido(modelo.strFechaInicio);
            modelo.FechaFin = Utilidades.FormatoFechaValido(modelo.strFechaFin);
            modelo.IdUsuarioCreacion = IdUsuarioLogueado;
            modelo.IdEstado = (int)Enumerador.Estados.Activo;

            var rta = await PostAsync<ExclusionConvenio, string>("Convenio/InsertarExclusion", modelo);
            return Json(new RespuestaViewModel { Correcto = string.IsNullOrEmpty(rta.Elemento.ToString()), Mensaje = rta.Elemento.ToString()}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// EDSP: Deshabilitar exclusión
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> DeshabilitarExclusion(int IdExclusion)
        {
            var modelo = new ExclusionConvenio
            {
                FechaModificacion = DateTime.Now,
                IdUsuarioModificacion = IdUsuarioLogueado,
                Id = IdExclusion,
                IdEstado = (int)Enumerador.Estados.Inactivo
            };

            var rta = await PutAsync<ExclusionConvenio, string>("Convenio/DeshabilitarExclusion", modelo);

            return Json(string.IsNullOrEmpty(rta) ? new RespuestaViewModel { Correcto = true }
                                  : new RespuestaViewModel { Correcto = false, Mensaje = rta }
              , JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// EDSP: Obtener exclusión por id de convenio
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> ObtenerExclusion(int IdConvenio)
        {
            var modelo = await GetAsync<IEnumerable<ExclusionConvenio>>($"Convenio/ObtenerExclusionesPorConvenio/{IdConvenio}");
            if (modelo != null && modelo.Count() > 0)
                modelo = modelo.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo);

            foreach (var item in modelo)
            {
                item.strFechaInicio = item.FechaInicio.ToString("dd/MM/yyyy");
                item.strFechaFin = item.FechaFin.ToString("dd/MM/yyyy");
            }

            return Json(new RespuestaViewModel { Correcto = modelo != null && modelo.Count() > 0, Elemento = modelo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// EDSP: Insertar o actualizar producto convenio
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ActualizarProductosConvenio(IEnumerable<ConvenioProducto> modelo)
        {
            try
            {

                foreach (var item in modelo)
                {
                    item.FechaCreacion = DateTime.Now;
                    item.IdEstado = (int)Enumerador.Estados.Activo;
                    item.IdUsuarioCreacion = IdUsuarioLogueado;
                }

                var resultado = await PostAsync<IEnumerable<ConvenioProducto>, string>("Convenio/ActualizarProductosConvenio", modelo);
                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ActualizarProductosConvenio");
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Hubo un inconveniente al insertar productos al convenio, informe al administrador." }, JsonRequestBehavior.AllowGet);
            }

        }




    }
}