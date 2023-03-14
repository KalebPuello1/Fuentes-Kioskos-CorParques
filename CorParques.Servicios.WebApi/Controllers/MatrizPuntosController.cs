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
    public class MatrizPuntosController : ApiController
    {

        private readonly IServicioMatrizPuntos _service;

        public MatrizPuntosController(IServicioMatrizPuntos service)
        {
            _service = service;
        }
        

        [HttpGet]
        [Route("api/MatrizPuntos/ObtenerTodos")]
        public HttpResponseMessage ObtenerTodos()
        {
            var list = _service.ObtenerTodos();
            return  Request.CreateResponse(HttpStatusCode.OK, list);
        }
        

        [HttpPost]
        [Route("api/MatrizPuntos/Insertar")]
        public HttpResponseMessage Insertar(TipoGeneral modelo)
        {
            var item = _service.InsertarMatriz(modelo);
            return string.IsNullOrEmpty(item) ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, item);
        }

        [HttpDelete]
        [Route("api/MatrizPuntos/Eliminar/{id}")]
        public HttpResponseMessage Eliminar(int id)
        {
            var item = _service.Eliminar(id);
            return string.IsNullOrEmpty(item) ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, item);
        }

    }
}
