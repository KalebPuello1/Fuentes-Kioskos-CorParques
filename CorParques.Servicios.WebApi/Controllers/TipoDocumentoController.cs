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
    public class TipoDocumentoController : ApiController
    {

        private readonly IServicioTipoDocumento _service;

        public TipoDocumentoController(IServicioTipoDocumento service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/TipoDocumento/ObtenerTipoDocumento")]
        public HttpResponseMessage ObtenerTipoDocumento()
        {
            var list = _service.ObtenerTipoDocumento();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}
