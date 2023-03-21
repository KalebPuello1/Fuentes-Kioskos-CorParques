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
    public class TipoDenominacionController : ApiController
    {

        private readonly IServicioTipoDenominacion _service;

        public TipoDenominacionController(IServicioTipoDenominacion service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/TipoDenominacion/Obtener")]
        public HttpResponseMessage ObtenerTodos()
        {
            var list = _service.ObtenerTodosActivos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}
