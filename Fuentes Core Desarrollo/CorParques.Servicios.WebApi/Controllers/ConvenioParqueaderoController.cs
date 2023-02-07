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
    public class ConvenioParqueaderoController : ApiController
    {

        private readonly IServicioConvenioParqueadero _service;

        public ConvenioParqueaderoController(IServicioConvenioParqueadero service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ConvenioParqueadero/ObtenerConvenioParqueadero")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerLista();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/ConvenioParqueadero/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/ConvenioParqueadero/ObtenerPorPlaca/{Placa}")]
        public HttpResponseMessage ObtenerPorPlaca(string Placa)
        {
            var item = _service.ObtenerPorPlaca(Placa);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/ConvenioParqueadero/Insertar")]
        public HttpResponseMessage Insertar(ConvenioParqueadero modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/ConvenioParqueadero/Actualizar")]
        public HttpResponseMessage Actualizar(ConvenioParqueadero modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/ConvenioParqueadero/Eliminar")]
        public HttpResponseMessage Eliminar(ConvenioParqueadero modelo)
        {
            string strError;
            var item = _service.Eliminar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/ConvenioParqueadero/ObtenerListaEmpleados")]
        public HttpResponseMessage ObtenerListaEmpleados()
        {
            var list = _service.ObtenerListaEmpleados();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

    }
}
