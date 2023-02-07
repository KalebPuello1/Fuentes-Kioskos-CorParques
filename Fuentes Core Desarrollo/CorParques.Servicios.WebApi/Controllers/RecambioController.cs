using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class RecambioController : ApiController
    {
        private readonly IServicioRecambio _service;

        public RecambioController(IServicioRecambio service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("api/Recambio/Insertar")]
        public HttpResponseMessage Insertar(Recambio modelo)
        {
            int IdRecambio;
            string strError;
            var item = _service.InsertarRecambio(modelo, out strError, out IdRecambio);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }
    }
}