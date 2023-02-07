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
    public class TarjetaRecargableController : ApiController
    {

        private readonly IServicioTarjetaRecargable _service;

        public TarjetaRecargableController(IServicioTarjetaRecargable service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/TarjetaRecargable/ValidarDocumento/{doc}")]
        public HttpResponseMessage ValidarDocumento(string doc)
        {
            string valor = _service.ValidarDocumento(doc);
            return Request.CreateResponse(HttpStatusCode.OK, valor);
        }

       

    }
}
