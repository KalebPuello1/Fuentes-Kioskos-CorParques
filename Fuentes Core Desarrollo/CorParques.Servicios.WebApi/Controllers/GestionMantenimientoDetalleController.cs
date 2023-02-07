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
    public class GestionMantenimientoDetalleController : ApiController
    {
        private readonly IServicioGestionMantenimientoDetalle _servicio;

        public GestionMantenimientoDetalleController(IServicioGestionMantenimientoDetalle servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        [Route("api/GestionMantenimientoDetalle/GetByid/{id}")]
        public HttpResponseMessage GetByid(int id)
        {
            var item = _servicio.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/GestionMantenimientoDetalle/ObtenerListaSimple/{id}")]
        public HttpResponseMessage ObtenerListaSimple(int id)
        {
            var list = _servicio.ObtenerListaSimple(id);
            return list == null ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}
