using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class OrdenMantenimientoController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            IEnumerable<Operaciones> _ordenes = new List<Operaciones>();
            IEnumerable<TipoGeneral> _list = new List<TipoGeneral>();
            //var _punto = ConfigurationManager.AppSettings["IdPunto"].ToString();

            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            if (_listPuntos != null)
                _list = _listPuntos;
            ViewBag.Puntos = _listPuntos;
            ViewBag.Detalle = false;
            return View(_ordenes);
        }

        [HttpPost]
        public async Task<ActionResult> Index(IEnumerable<Operaciones> modelo)
        {
            return View("Index", modelo);
        }

        [HttpGet]
        public async Task<ActionResult> ListarOrdenes(int IdPunto)
        {
            IEnumerable<Operaciones> _ordenes = null;
            ViewBag.Detalle = false;
            var _punto = await GetAsync<Puntos>($"Puntos/GetById/{IdPunto}");
            var usuario = (Usuario)Session["UsuarioAutenticado"];

            var _listOrdenes = await GetAsync<IEnumerable<Operaciones>>($"Operacion/ObtenerOrdenes/{_punto.CodigoSap}/{usuario.NombreUsuario}");
            if (_listOrdenes != null)
            {
                _ordenes = new List<Operaciones>();
                foreach (var item in _listOrdenes)
                {
                    var objOrdenesMantenimiento = await GetAsync<IEnumerable<OrdenMantenimiento>>($"OrdenMantenimiento/ObtenerOrdenesMantenimiento/{_punto.CodigoSap}/{item.NumeroOrden}");
                    item.objOrdenMantenimiento = objOrdenesMantenimiento;
                }
                _ordenes = _listOrdenes;
            }
            return PartialView("_List", _ordenes);
        }

        [HttpGet]
        public async Task<ActionResult> BuscarOrdenes(long NumeroOrden, int CodPunto)
        {

            OrdenMantenimiento objOrdenMantenimiento = null;
            IEnumerable<Operaciones> objListaOperaciones = null;

            try
            {
                ViewBag.Detalle = true;
                
                ///var _punto = await GetAsync<Puntos>($"Puntos/GetById/{CodPunto}");               

                var retorno = await GetAsync<string>($"OrdenMantenimiento/ObtenerRetornoPorNumeroOrden/{NumeroOrden}");

                if (retorno == null)
                {
                    var modelo = new Retorno();
                    modelo.IdPunto = CodPunto;  //IdPunto Cambio de de Id Punto por cambio a Admin;
                    modelo.IdUsuarioAprobador = IdUsuarioLogueado;
                    modelo.Procesado = 0;
                    modelo.Aprobado = 0;
                    modelo.IdUsuarioAprobador = IdUsuarioLogueado;
                    modelo.NumeroOrden = NumeroOrden;
                    modelo.HoraInicio = DateTime.Now;
                    modelo.HoraFin = null;
                    var rta = await PostAsync<Retorno, string>("Retorno/Insertar", modelo);
                }

                objOrdenMantenimiento = await GetAsync<OrdenMantenimiento>($"OrdenMantenimiento/ObtenerOrdenMantenimiento/{NumeroOrden}");
                if (objOrdenMantenimiento != null)
                {
                    objListaOperaciones = await GetAsync<IEnumerable<Operaciones>>($"Operacion/ObtenerOperacionesPorOrden/{NumeroOrden}");
                }

                objOrdenMantenimiento.ListaOperaciones = objListaOperaciones;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "OrdenMantenimientoController_BuscarOrdenes");
            }

            return PartialView("_Wizard", objOrdenMantenimiento);
        }

        public async Task<ActionResult> InsertOrden(Retorno modelo)
        {
            var codSap = modelo.CodSapPunto;
            modelo.IdUsuarioAprobador = IdUsuarioLogueado;
            modelo.Procesado = 1;
            string observacion = Regex.Replace(modelo.Observacion, @"\r\n?|\n", string.Empty);
            var respuesta = await PutAsync<Retorno, string>($"OrdenMantenimiento/ActualizaHoraOrden?Observaciones={observacion}&idUsuarioAprobador={modelo.IdUsuarioAprobador}&NumeroOrden={modelo.NumeroOrden}&Aprobado={modelo.Aprobado}&IdOperaciones={modelo.IdOperaciones}&Procesado={modelo.Procesado}&CodSapPunto={codSap}", modelo);
            return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> InsertOrdenMantenimiento(RetornoDetalle modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = IdUsuarioLogueado;
            modelo.Procesado = true;
            var rta = await PostAsync<RetornoDetalle, string>("Retorno/InsertarDetalle", modelo);
            if (string.IsNullOrEmpty(rta.Mensaje))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = rta.Mensaje });
        }
    }
}