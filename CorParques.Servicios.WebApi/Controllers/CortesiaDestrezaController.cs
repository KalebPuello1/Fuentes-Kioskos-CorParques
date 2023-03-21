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
    public class CortesiaDestrezaController : ApiController
    {

        private readonly IServicioCortesiaDestreza _service;

        public CortesiaDestrezaController(IServicioCortesiaDestreza service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/CortesiaDestreza/ObtenerPorDestrezaAtraccion/{IdDestreza}/{IdAtraccion}")]
        public HttpResponseMessage ObtenerLista(int IdDestreza, int IdAtraccion)
        {
            var list = _service.ObtenerPorDestrezaAtraccion(IdDestreza, IdAtraccion);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/CortesiaDestreza/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        

        [HttpPost]
        [Route("api/CortesiaDestreza/Insertar")]
        public HttpResponseMessage Insertar(CortesiaDestreza modelo)
        {
            string strError;
            string strCodigoBarras;
            var item = _service.Insertar(modelo, out strError, out strCodigoBarras);
            return item ? Request.CreateResponse(HttpStatusCode.OK, strCodigoBarras)
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/CortesiaDestreza/Actualizar")]
        public HttpResponseMessage Actualizar(CortesiaDestreza modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/CortesiaDestreza/ObtenerPorCodigoBarras/{CodigoBarras}")]
        public HttpResponseMessage ObtenerPorCodigoBarras(string CodigoBarras)
        {
            var item = _service.ObtenerPorCodigoBarras(CodigoBarras);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

    }
}
