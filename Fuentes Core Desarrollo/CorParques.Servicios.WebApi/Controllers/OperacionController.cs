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
    public class OperacionController : ApiController
    {
        private readonly IServicioOperacion _service;

        public OperacionController(IServicioOperacion service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Operacion/ObtenerOrdenes/{Punto}/{NumDocumento}")]
        public HttpResponseMessage ObtenerOrdenes(string Punto, string NumDocumento)
        {
            var list = _service.ObtenerOrdenes(Punto, NumDocumento);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Operacion/ObtenerOrdenPorNumeroOrden/{NumeroOrden}")]
        public HttpResponseMessage ObtenerOrdenPorNumeroOrden(long NumeroOrden)
        {
            var item = _service.ObtenerOrdenPorNumeroOrden(NumeroOrden);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Retorna las operaciones por numero de orden.
        /// </summary>
        /// <param name="NumeroOrden"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Operacion/ObtenerOperacionesPorOrden/{NumeroOrden}")]
        public HttpResponseMessage ObtenerOperacionesPorOrden(long NumeroOrden)
        {
            var list = _service.ObtenerOperacionesPorOrden(NumeroOrden);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

    }
}