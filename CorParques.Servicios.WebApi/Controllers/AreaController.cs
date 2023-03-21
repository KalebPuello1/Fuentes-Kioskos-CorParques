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
    public class AreaController : ApiController
    {

        private readonly IServicioArea _service;

        public AreaController(IServicioArea service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Area/ObtenerListaAreas")]
        public HttpResponseMessage ObtenerListaAreas()
        {
            var list = _service.ObtenerListaAreas();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Area/ObtenerTodos")]
        public HttpResponseMessage ObtenerTodos()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Area/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpPost]
        [Route("api/Area/Insertar")]
        public HttpResponseMessage Insertar(Area modelo)
        {
         
            var item = _service.Crear(modelo);
            return item != null ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, "Error insertando area.");
        }

        [HttpPut]
        [Route("api/Area/Actualizar")]
        public HttpResponseMessage Actualizar(Area modelo)
        {            
            var item = _service.Actualizar(modelo);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, "Error actualizando el area.");
        }

    }
}
