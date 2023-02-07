using CorParques.Negocio.Contratos;
using CorParques.Servicios.WebApi.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ProductosAppController : ApiController
    {
        private readonly IServicioProductosApp _service;

        public ProductosAppController(IServicioProductosApp service)
        {
            _service = service;
        }

        [HttpPost, Route("api/Insertar")]
        public HttpResponseMessage Insertar(Negocio.Entidades.Producto producto)
        {
            var item = _service.Insertar(ref producto);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                              : Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
        }

        [HttpPost, Route("api/ProductosApp/Crear")]
        public HttpResponseMessage Crear(Negocio.Entidades.Producto producto)
        {
            var item = _service.Crear(ref producto);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                              : Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
        }

        [HttpPut, Route("api/ProductosApp/Inactivar")]
        public HttpResponseMessage Inactivar(Producto producto)
        {

            var item = _service.Inactivar(producto.IdProducto, producto.ActivoApp);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                              : Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
        }

        [HttpGet]
        [Route("api/ProductosApp/Obtener")]
        public HttpResponseMessage Obtener()
        {
            var item = _service.Obtener();
            return item != null ? Request.CreateResponse(HttpStatusCode.OK, item)
                              : Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
        }

        [HttpGet]
        [Route("api/ProductosApp/ObtenerId/{IdProducto}")]
        public HttpResponseMessage ObtenerId(int IdProducto)
        {
            var item = _service.ObtenerId(IdProducto);
            return item != null ? Request.CreateResponse(HttpStatusCode.OK, item)
                              : Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
        }
    }
}
