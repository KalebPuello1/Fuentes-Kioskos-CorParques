using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class PrioridadCorreoController : ApiController
    {
        private readonly IServicioPrioridadCorreo _servicio;


        public PrioridadCorreoController(IServicioPrioridadCorreo servicio)
        {
            _servicio = servicio;
        }

        [Route("api/PrioridadCorreo/Send")]
        public HttpResponseMessage GetAll()
        {
            var retorno = _servicio.ObtenerTodos();
            return retorno.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, retorno);
        }
    }
}
