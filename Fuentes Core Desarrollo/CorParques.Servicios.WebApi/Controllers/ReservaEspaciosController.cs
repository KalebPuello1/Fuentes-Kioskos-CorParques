using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ReservaEspaciosController : ApiController
    {
        private readonly IServicioReservaEspacios _service;

        public ReservaEspaciosController(IServicioReservaEspacios service)
        {
            _service = service;
        }

        /// <summary>
        /// Combo tipo espacios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ReservaEspacios/ObtenerTipoEspacios")]
        public HttpResponseMessage ObtenerTipoEspacios()
        {
            var item = _service.ObtenerTipoEspacios();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// Combo espacios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ReservaEspacios/obtenerEspacios")]
        public HttpResponseMessage obtenerEspacios()
        {
            var item = _service.ObtenerEspacios();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// Combo espacios
        /// </summary>
        /// <param name="IdTipoEspacio"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ReservaEspacios/ObtenerEspaciosxTipo/{IdTipoEspacio}")]
        public HttpResponseMessage ObtenerEspaciosxTipo(int IdTipoEspacio)
        {
            var item = _service.ObtenerEspaciosxTipo(IdTipoEspacio);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Retorna las reservas que se han realizado para el espacio y la fecha seleccionada.
        /// </summary>
        /// <param name="IdEspacio"></param>
        /// <param name="FechaReserva"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ReservaEspacios/ObtenerReservaEspacios/{IdReserva}/{FechaReserva}")]
        public HttpResponseMessage ObtenerReservaEspacios(int IdReserva, string FechaReserva)
        {
            var item = _service.ObtenerReservaEspacios(IdReserva, FechaReserva);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Inserta una reserva de espacios.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ReservaEspacios/Insertar")]
        public HttpResponseMessage Insertar(ReservaEspacios modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, strError)
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        /// <summary>
        /// RDSH:  Actualiza una reserva de espacios.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/ReservaEspacios/Actualizar")]
        public HttpResponseMessage Actualizar(ReservaEspacios modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, strError)
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        /// <summary>
        /// Radios tipo reserva
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ReservaEspacios/ObtenerTiposReserva")]
        public HttpResponseMessage ObtenerTiposReserva()
        {
            var item = _service.ObtenerTiposReserva();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Retorna el detalle de un pedido sap para mostrarlo en la reserva de espacios.
        /// </summary>
        /// <param name="CodigoSapPedido"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ReservaEspacios/ObtenerDetallePedido/{CodigoSapPedido}/{FechaReserva}")]
        public HttpResponseMessage ObtenerDetallePedido(string CodigoSapPedido, string FechaReserva)
        {
            var item = _service.ObtenerDetallePedido(CodigoSapPedido, FechaReserva);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Elimina una reserva.
        /// </summary>
        /// <param name="IdReserva"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ReservaEspacios/Eliminar/{IdReserva}/{IdUsuario}")]
        public HttpResponseMessage Eliminar(int IdReserva, int IdUsuario)
        {
            string strError;
            var item = _service.Eliminar(IdReserva, IdUsuario, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

    }
}