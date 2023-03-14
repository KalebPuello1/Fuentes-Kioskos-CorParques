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
    public class CargueBrazaleteController : ApiController
    {

        private readonly IServicioCargueBrazalete _service;

        public CargueBrazaleteController(IServicioCargueBrazalete service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("api/CargueBrazalete/ObtenerCargueBrazalete")]
        public HttpResponseMessage ObtenerCargueBrazalete()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [Route("api/CargueBrazalete/GetTipoBrazalete")]
        public HttpResponseMessage GetTipoBrazalete()
        {
            var list = _service.ObtenerTipoBrazalete();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/CargueBrazalete/Insertar")]
        public HttpResponseMessage Insertar(CargueBrazalete modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
                     
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound,  strError  );
        }

        [HttpPost]
        [Route("api/CargueBrazalete/Actualizar")]
        public HttpResponseMessage Actualizar(CargueBrazalete modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);

            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
            
        }

    }
}
