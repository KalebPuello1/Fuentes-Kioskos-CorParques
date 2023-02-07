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
    public class TipoClienteController : ApiController
    {
        private readonly IServicioTipoCliente _servicio;

        public TipoClienteController(IServicioTipoCliente servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        [Route("api/TipoCliente/GetAllSimple")]
        public HttpResponseMessage GetAllSimple()
        {
            var list = _servicio.ObtenerListaSimple();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}