using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Transversales.Contratos;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class CortesiaDestrezaController : ControladorBase
    {

        #region Declaraciones

        private readonly IServicioImprimir _service;

        public CortesiaDestrezaController(IServicioImprimir service)
        {            
            _service = service;
        }

        #endregion

      
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<CortesiaDestreza>>($"CortesiaDestreza/ObtenerPorDestrezaAtraccion/{IdPunto}/0");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<CortesiaDestreza>>($"CortesiaDestreza/ObtenerPorDestrezaAtraccion/{IdPunto}/0");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new CortesiaDestreza();
            var listaDestrezas = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Destreza}");
            var listaAtracciones = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");            
            modelo.ListaDestrezas = listaDestrezas;
            modelo.ListaAtracciones = listaAtracciones;
           // modelo.ListaEstados = listaEstado;            
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(CortesiaDestreza modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Activo;
            var resultado = await PostAsync<CortesiaDestreza, string>("CortesiaDestreza/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Update(CortesiaDestreza modelo)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            var resultado = await PutAsync<CortesiaDestreza, string>("CortesiaDestreza/Actualizar", modelo);
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
            var item = await GetAsync<CortesiaDestreza>($"CortesiaDestreza/GetById/{Id}");
            var listaDestrezas = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Destreza}");
            var listaAtracciones = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");
            item.ListaDestrezas = listaDestrezas;
            item.ListaAtracciones = listaAtracciones;
           // item.ListaEstados = listaEstado;
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> UpdateEstado(int Id)
        {
            var modelo = await GetAsync<CortesiaDestreza>($"CortesiaDestreza/GetById/{Id}");
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Inactivo;
            var resultado = await PutAsync<CortesiaDestreza, string>("CortesiaDestreza/Eliminar", modelo);
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
            var item = await GetAsync<CortesiaDestreza>($"CortesiaDestreza/GetById/{Id}");
            var listaDestrezas = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Destreza}");
            var listaAtracciones = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");
            item.ListaDestrezas = listaDestrezas;
            item.ListaAtracciones = listaAtracciones;
            //item.ListaEstados = listaEstado;
            return PartialView("_Detail", item);
        }

        public async Task<ActionResult> GenerarCortesia(int IdAtraccion)
        {

            RespuestaViewModel objRespuestaViewModel = new RespuestaViewModel();

            try
            {
                CortesiaDestreza modelo = new CortesiaDestreza();
                modelo.IdPuntoDestreza = IdPunto;
                modelo.IdProducto = IdAtraccion;
                modelo.Cantidad = 1;
                modelo.IdEstado = (int)Enumerador.Estados.Activo;
                modelo.FechaCreacion = DateTime.Now;
                modelo.IdUsuarioCreacion = IdUsuarioLogueado;
                string strResultado = string.Empty;

                objRespuestaViewModel = await PostAsync<CortesiaDestreza, string>("CortesiaDestreza/Insertar", modelo);
                if (objRespuestaViewModel.Correcto)
                {
                    strResultado = await GenerarReciboRetorno(objRespuestaViewModel.Elemento.ToString());
                    if (strResultado.Trim().Length > 0)
                    {
                        objRespuestaViewModel.Correcto = false;
                        objRespuestaViewModel.Mensaje = strResultado;
                    }                    
                }
            }
            catch (Exception ex)
            {
                objRespuestaViewModel.Correcto = false;
                objRespuestaViewModel.Mensaje = string.Concat("Se ha presentado un problema al generar la cortesía: ", ex.Message);
            }
                        
            return Json(objRespuestaViewModel, JsonRequestBehavior.AllowGet);

        }

        private async Task<ActionResult> GenerarRecibo(string strCodigoBarras)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = new Articulo();
            List<Articulo> ListaArticulos = new List<Articulo>();

            try
            {

                var objCortesiaDestreza = await GetAsync<CortesiaDestreza>($"CortesiaDestreza/ObtenerPorCodigoBarras/{strCodigoBarras}"); 
                objArticulo.Nombre = objCortesiaDestreza.Atraccion.ToUpper();
                objArticulo.Cantidad = 1;
                objArticulo.Precio = 0;
                objArticulo.Otro = "";
                ListaArticulos.Add(objArticulo);

                objTicketImprimir.CodigoBarrasProp = strCodigoBarras;
                objTicketImprimir.TituloRecibo = string.Concat("CORTESIA - ", objCortesiaDestreza.Destreza.ToUpper());
                objTicketImprimir.TituloColumnas = "Valido para|Cant";
                objTicketImprimir.ListaArticulos = ListaArticulos;
                objTicketImprimir.PieDePagina = "Cortesia valida para un solo ingreso en la atracción seleccionada.";
                objTicketImprimir.Usuario = objCortesiaDestreza.Usuario;

                _service.ImprimirTicketCortesias(objTicketImprimir);

            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarRecibo_CortesiaDestrezaController: ", ex.Message));
            }
            finally
            {
                objTicketImprimir = null;
                objArticulo = null;
                ListaArticulos = null;
            }

            return null;
        }

        private async Task<string> GenerarReciboRetorno(string strCodigoBarras)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = new Articulo();
            List<Articulo> ListaArticulos = new List<Articulo>();
            string strRetorno = string.Empty;

            try
            {

                var objCortesiaDestreza = await GetAsync<CortesiaDestreza>($"CortesiaDestreza/ObtenerPorCodigoBarras/{strCodigoBarras}");
                objArticulo.Nombre = objCortesiaDestreza.Atraccion.ToUpper();
                objArticulo.Cantidad = 1;
                objArticulo.Precio = 0;
                objArticulo.Otro = "";
                ListaArticulos.Add(objArticulo);

                objTicketImprimir.CodigoBarrasProp = strCodigoBarras;
                objTicketImprimir.TituloRecibo = string.Concat("CORTESIA ", objCortesiaDestreza.Destreza.ToUpper());
                objTicketImprimir.TituloColumnas = "Valido para|Cant";
                objTicketImprimir.ListaArticulos = ListaArticulos;
                objTicketImprimir.PieDePagina = "Esta boleta es cortesía, sin valor comercial.";
                objTicketImprimir.Usuario = NombreUsuarioLogueado;

                strRetorno = _service.ImprimirTicketCortesias(objTicketImprimir);

            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarRecibo_CortesiaDestrezaController: ", ex.Message);
            }
            finally
            {
                objTicketImprimir = null;
                objArticulo = null;
                ListaArticulos = null;
            }

            return strRetorno;
        }

    }
}