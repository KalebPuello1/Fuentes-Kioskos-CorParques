using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ReservaSkycoasterController : ApiController
    {
        private readonly IServicioReservaSkycoaster _service;

        public ReservaSkycoasterController(IServicioReservaSkycoaster service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Reserva/ObtenerListaReservas")]
        public HttpResponseMessage ObtenerLista()
        {
            var item = _service.ObtenerListaReservas();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Reserva/InsertarReserva")]
        public HttpResponseMessage Insertar(ReservaSkycoaster modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPost]
        [Route("api/Reserva/LiberarReserva")]
        public HttpResponseMessage LiberarReserva(ReservaSkycoaster modelo)
        {
            string strError;
            var item = _service.LiberarReserva(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }


        [HttpPost]
        [Route("api/Reserva/CerrarReserva")]
        public HttpResponseMessage CerrarReserva(ReservaSkycoaster modelo)
        {
            string strError;
            var item = _service.CerrarReserva(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/Reserva/ObtenerReserva/{horaInicio}")]
        public HttpResponseMessage ObtenerReservaHora(string horaInicio)
        {
            var item = _service.ObtenerReservaHora(horaInicio);
            return item > 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
       
    }
}
