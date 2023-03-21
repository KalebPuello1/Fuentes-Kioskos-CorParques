using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class FranquiciaController : ApiController
    {

        public readonly IServicioFranquicia _servicio;

        public FranquiciaController(IServicioFranquicia servicio)
        {
            _servicio = servicio;
        }
    
        [HttpGet]
        [Route("api/Franquicia/ObtenerTodos")]
        public HttpResponseMessage ObtenerListaFranquicia()
        {
            var resp = _servicio.ObtenerTodos();
            return resp == null || resp.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, resp);
        }
    }
}