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
    public class TipoConvenioParqueaderoController : ApiController
    {

        private readonly IServicioTipoConvenioParqueadero _service;

        public TipoConvenioParqueaderoController(IServicioTipoConvenioParqueadero service)
        {
            _service = service;
        }
        
        [HttpGet]
        [Route("api/TipoConvenioParqueadero/ObtenerLista")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerListaSimple();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}
