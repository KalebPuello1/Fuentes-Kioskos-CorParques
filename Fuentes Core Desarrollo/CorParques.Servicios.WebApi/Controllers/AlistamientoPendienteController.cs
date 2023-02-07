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
    public class AlistamientoPendienteController : ApiController
    {
        private readonly IServicioAlistamientoPendiente _service;

        public AlistamientoPendienteController(IServicioAlistamientoPendiente service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/AlistamientoPendiente/ObtenerAlistamientoPendiente/{IdReporte}")]
        public HttpResponseMessage ObtenerAlistamientoPendiente(int IdReporte)
        {
            var item = _service.ObtenerAlistamientosPendientes(IdReporte);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}