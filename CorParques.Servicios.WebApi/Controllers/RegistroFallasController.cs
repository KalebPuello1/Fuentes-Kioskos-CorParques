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
    public class RegistroFallasController : ApiController
    {
        private readonly IServicioRegistroFallas _service;

        public RegistroFallasController(IServicioRegistroFallas service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/RegistroFallas/set")]
        public HttpResponseMessage GetReimpresion(Negocio.Entidades.RegistroFallas modelo)
        {
            var item = _service.insertarRegistroFalla(modelo);
            return !item? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}