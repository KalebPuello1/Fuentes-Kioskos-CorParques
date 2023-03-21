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
    public class ResolucionFacturaController : ApiController
    {

        private readonly IServicioResolucionFactura _service;

        public ResolucionFacturaController(IServicioResolucionFactura service)
        {
            _service = service;
        }
        

        [HttpGet]
        [Route("api/ResolucionFactura/ObtenerTodos/{aprovador}")]
        public HttpResponseMessage ObtenerTodos(int aprovador)
        {
            var list = _service.ObtenerResoluciones(aprovador);
            return  Request.CreateResponse(HttpStatusCode.OK, list);
        }
        

        [HttpPost]
        [Route("api/ResolucionFactura/Insertar")]
        public HttpResponseMessage Insertar(ResolucionFactura modelo)
        {
            var item = _service.InsertarResolucion(modelo);
            return string.IsNullOrEmpty(item) ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, item);
        }

        [HttpDelete]
        [Route("api/ResolucionFactura/Eliminar/{id}")]
        public HttpResponseMessage Eliminar(int id)
        {
            var item = _service.EliminarResolucion(id);
            return string.IsNullOrEmpty(item) ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, item);
        }
        [HttpDelete]
        [Route("api/ResolucionFactura/Aprobar/{id}")]
        public HttpResponseMessage Aprobar(int id)
        {
            var item = _service.AprobarResolucion(id);
            return string.IsNullOrEmpty(item) ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, item);
        }

        [HttpGet]
        [Route("api/ResolucionFactura/ObtenerPrefijoConsecutivo/{prefijo}/consecutivoInicial")]
        public HttpResponseMessage ObtenerPrefijoConsecutivo(string prefijo, int consecutivoInicial)
        {
            var list = _service.ObtenerPrefijoConsecutivo(prefijo, consecutivoInicial);
            return Request.CreateResponse(HttpStatusCode.OK, list);
        }

    }
}
