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
    public class OrdenController : ApiController
    {

        private readonly IServicioOrden _service;

        public OrdenController(IServicioOrden service)
        {
            _service = service;
        }

   

        [HttpGet]
        [Route("api/Orden/ObtenerTodos")]
        public HttpResponseMessage ObtenerTodos()
        {
            var list = _service.ObtenerListaOrden();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        

    }
}
