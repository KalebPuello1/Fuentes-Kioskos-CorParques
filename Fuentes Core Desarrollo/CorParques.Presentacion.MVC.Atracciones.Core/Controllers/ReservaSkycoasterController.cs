using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Atracciones.Core.Models;
using System.Globalization;
using System.Threading;
using CorParques.Transversales.Util;
using CorParques.Transversales.Contratos;
using System.Data;

namespace CorParques.Presentacion.MVC.Atracciones.Core.Controllers
{
    public class ReservaSkycoasterController : ControladorBase
    {
        // GET: ReservaSkycoaster
        private readonly IServicioImprimir _service;

        public ReservaSkycoasterController(IServicioImprimir service)
        {

            _service = service;
        }

        public async Task<ActionResult> Index()
        {
            var puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            ViewBag.Puntos = puntos.Where(x => x.IdTipoPunto == (int)Enumerador.TiposPuntos.Atraccion
                                && x.CantidadCupos != null && x.IntervaloTurno != null && x.EstadoId.Equals((int)Enumerador.Estados.Activo));
            return View();
        }

        public async Task<ActionResult> InsertaReserva(ReservaSkycoaster modelo)
        {
            RespuestaViewModel objRespuestaViewModel = new RespuestaViewModel();
            DateTime _fechaInicio = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + modelo.HoraInicio, new CultureInfo("es-Co"));
            DateTime _fechaFin = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + modelo.HoraFin, new CultureInfo("es-Co"));

            modelo.HoraInicio = _fechaInicio.ToString("HHmm");
            modelo.HoraFin = _fechaFin.ToString("HHmm");
            modelo.IdUsuarioCreacion = IdUsuarioLogueado;

            string strResultado = string.Empty;          

            objRespuestaViewModel = await PostAsync<ReservaSkycoaster, string>("Reserva/InsertarReserva", modelo);

            if (objRespuestaViewModel.Correcto)
            {
                strResultado = await GenerarRecibo(modelo);
                if (strResultado.Trim().Length > 0)
                {
                    objRespuestaViewModel.Correcto = false;
                    objRespuestaViewModel.Mensaje = strResultado;
                }
            }
            return Json(objRespuestaViewModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> LiberarReserva(ReservaSkycoaster modelo)
        {

            DateTime _fechaInicio = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + modelo.HoraInicio, new CultureInfo("es-Co"));
            DateTime _fechaFin = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + modelo.HoraFin, new CultureInfo("es-Co"));

            modelo.HoraInicio = _fechaInicio.ToString("HHmm");
            modelo.HoraFin = _fechaFin.ToString("HHmm");
            modelo.IdUsuarioModificacion = IdUsuarioLogueado;

            var resultado = await PostAsync<ReservaSkycoaster, string>("Reserva/LiberarReserva", modelo);

            return Json(new RespuestaViewModel { Correcto = true, Mensaje = "OK" }, JsonRequestBehavior.AllowGet);

        }


        public async Task<ActionResult> CerrarReserva(ReservaSkycoaster modelo)
        {

            DateTime _fechaInicio = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + modelo.HoraInicio, new CultureInfo("es-Co"));
            DateTime _fechaFin = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + modelo.HoraFin, new CultureInfo("es-Co"));

            modelo.HoraInicio = _fechaInicio.ToString("HHmm");
            modelo.HoraFin = _fechaFin.ToString("HHmm");
            modelo.IdUsuarioCreacion = IdUsuarioLogueado;
            modelo.IdUsuarioModificacion = IdUsuarioLogueado;

            var _ListaReserva = await GetAsync<IEnumerable<ReservaSkycoaster>>("Reserva/ObtenerListaReservas");

            modelo.IdReserva = _ListaReserva.LastOrDefault().IdReserva + 1;
            modelo.Consecutivo = modelo.IdReserva.ToString().PadLeft(13, '0');
            modelo.FechaReserva = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            var rta = await PostAsync<ReservaSkycoaster, string>("Reserva/CerrarReserva", modelo);

            return Json(new RespuestaViewModel { Correcto = true, Mensaje = "OK" }, JsonRequestBehavior.AllowGet);

        }


       
        public async Task<ActionResult> ReservaModal(ReservaSkycoaster modelo)
        {
            var model = await GetAsync<IEnumerable<ReservaSkycoaster>>("Reserva/ObtenerListaReservas");
            if (model.FirstOrDefault() == null)
            { modelo.IdReserva = 1; }
            else
            { modelo.IdReserva = model.LastOrDefault().IdReserva + 1; }

            modelo.Consecutivo = modelo.IdReserva.ToString().PadLeft(13, '0');
            modelo.FechaReserva = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            return PartialView("_Reserva", modelo);
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerLista(int id)
        {
            if (id == 0)
                return PartialView("_Lista");

            CultureInfo ci = new CultureInfo("es-Co");
            Thread.CurrentThread.CurrentCulture = ci;

            var puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            var _ListaReserva = await GetAsync<IEnumerable<ReservaSkycoaster>>("Reserva/ObtenerListaReservas");
            List<ReservaSkycoaster> reservas = new List<ReservaSkycoaster>();
            Puntos _punto = puntos.Where(x => x.Id == id).SingleOrDefault();



            //EDSP Se recibe la fracción desde la tabla puntos 

            var _fraccion = Convert.ToDouble(_punto.IntervaloTurno ?? 10);
            var _apertura = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " " + _punto.HoraInicio.ToString());
            var _cierre = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " " + _punto.HoraFin.ToString());


            string _horaInicio, _horaFin;
            var contHoras = (_cierre - _apertura).Hours * ((60 / _fraccion));

            DateTime result = _apertura;

            for (int i = 0; i < contHoras; i++)
            {


                var _reserva = new ReservaSkycoaster();

                if (_ListaReserva != null && _ListaReserva.Count() > 0)
                {
                    var rta = _ListaReserva.Where(x => x.HoraInicio == result.ToString("HH:mm") && x.IdPunto == id);
                    if (rta != null && rta.Count() > 0)
                        _reserva = rta.Single();
                }


                _horaInicio = result.ToString("HH:mm tt", ci);
                var FechaInicio = result;
                result = result.AddMinutes(_fraccion); // Se adiciona los minutos de la fracción
                _horaFin = result.ToString("HH:mm tt", ci);

                //Mostrar la reserva activa a la fecha
                if (DateTime.Now < FechaInicio)
                    reservas.Add(new ReservaSkycoaster
                    {
                        IdReserva = _reserva.IdReserva,
                        IdTicket = _reserva.IdTicket,
                        HoraInicio = _horaInicio,
                        HoraFin = _horaFin,
                        FechaReserva = _reserva.FechaReserva,
                        Consecutivo = _reserva.Consecutivo,
                        IdPunto = _punto.Id,
                        Capacidad = _punto.CantidadCupos ?? 0,
                        CapacidadDisponible = _reserva.IdReserva > 0 ? _reserva.CapacidadDisponible : _punto.CantidadCupos ?? 0,
                        Cerrar = _reserva.Cerrar
                    });

            }

            return PartialView("_Lista", reservas);
        }

        private async Task<string> GenerarRecibo(ReservaSkycoaster modelo)
        {
            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = new Articulo();
            List<Articulo> ListaArticulos = new List<Articulo>();
            string strRetorno = string.Empty;
            var item = await GetAsync<Puntos>($"Puntos/GetById/{modelo.IdPunto}");
            try
            {
                var user = await GetAsync<Usuario>($"Usuario/GetById?id={IdUsuarioLogueado}&Punto={0}");

                objArticulo.Nombre = "CONSECUTIVO:";
                objArticulo.Cantidad = int.Parse(modelo.Consecutivo);
                ListaArticulos.Add(objArticulo);
                objArticulo = null;

                objTicketImprimir.TituloRecibo = string.Concat("RESERVA - ", item.Nombre.ToUpper());
                objTicketImprimir.TituloColumnas = "Detalle Reserva|";
                objTicketImprimir.ListaArticulos = ListaArticulos;
                objTicketImprimir.Usuario = string.Concat(user.Nombre, " ", user.Apellido);

                strRetorno = _service.ImprimirTicketCortesias(objTicketImprimir);
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarRecibo_ReservaSkycoasterController: ", ex.Message);
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