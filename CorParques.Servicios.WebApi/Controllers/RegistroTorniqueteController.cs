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
    public class RegistroTorniqueteController : ApiController
    {

        private readonly IServicioRegistroTorniquete _service;

        public RegistroTorniqueteController(IServicioRegistroTorniquete service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/RegistroTorniquete/ObtenerRegistroTorniquete/{IdPunto}")]
        public HttpResponseMessage ObtenerRegistroTorniquete(int IdPunto)
        {
            var item = _service.ObtenerRegistroTorniquete(IdPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
              

        [HttpPost]
        [Route("api/RegistroTorniquete/Insertar")]
        public HttpResponseMessage Insertar(RegistroTorniquete modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/RegistroTorniquete/Actualizar")]
        public HttpResponseMessage Actualizar(RegistroTorniquete modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }       

    }
}
