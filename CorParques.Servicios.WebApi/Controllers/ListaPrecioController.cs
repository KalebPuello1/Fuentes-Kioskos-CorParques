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
    public class ListaPrecioController : ApiController
    {
        private readonly IServicioListaPrecio _service;

        public ListaPrecioController(IServicioListaPrecio service)
        {
            _service = service;
        }

        [Route("api/ListaPrecio/GetAll")]
        public HttpResponseMessage GetAll()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/ListaPrecio/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/ListaPrecio/Insert")]
        public HttpResponseMessage Crear(ListaPrecio modelo)
        {
            var item = _service.Insertar(modelo);
            return item != null ? Request.CreateResponse(HttpStatusCode.OK, string.Empty)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpPut]
        [Route("api/ListaPrecio/Update")]
        public HttpResponseMessage Actualizar(ListaPrecio modelo)
        {
            var item = _service.Actualizar(modelo);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpDelete]
        [Route("api/ListaPrecio/Delete/{id}")]
        public HttpResponseMessage Eliminar(int id)
        {
            //var modelo = _service.Obtener(id);
            var item = _service.Eliminar(new ListaPrecio { Id = id });
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

    }
}
