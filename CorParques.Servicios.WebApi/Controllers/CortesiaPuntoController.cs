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
    public class CortesiaPuntoController : ApiController
    {

        private readonly IServicioCortesiaPunto _service;

        public CortesiaPuntoController(IServicioCortesiaPunto service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/CortesiaPunto/ObtenerPorDestrezaAtraccion/{IdDestreza}/{IdAtraccion}")]
        public HttpResponseMessage ObtenerLista(int IdDestreza, int IdAtraccion)
        {
            var list = _service.ObtenerPorDestrezaAtraccion(IdDestreza, IdAtraccion);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/CortesiaPunto/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        

        [HttpPost]
        [Route("api/CortesiaPunto/Insertar")]
        public HttpResponseMessage Insertar(CortesiaPunto modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/CortesiaPunto/Actualizar")]
        public HttpResponseMessage Actualizar(CortesiaPunto modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/CortesiaPunto/Eliminar")]
        public HttpResponseMessage Eliminar(CortesiaPunto modelo)
        {
            string strError;
            var item = _service.Eliminar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/CortesiaPunto/ObtenerProductos/{CodTipoProducto}")]
        public HttpResponseMessage ObtenerProductos(string CodTipoProducto)
        {
            var item = _service.ObtenerProductos(CodTipoProducto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}
