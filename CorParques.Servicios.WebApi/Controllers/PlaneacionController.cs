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
    public class PlaneacionController : ApiController
    {

        private readonly IServicioPlaneacion _service;

        public PlaneacionController(IServicioPlaneacion service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Planeacion/ObtenerListaIndicadores")]
        public HttpResponseMessage ObtenerListaIndicadores()
        {
            var list = _service.ObtenerListaIndicadores();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Planeacion/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Planeacion/ConsultarPlaneacion/{IdIndicador}/{Fecha}")]
        public HttpResponseMessage ConsultarPlaneacion(int IdIndicador, string Fecha)
        {
            var item = _service.ConsultarPlaneacion(IdIndicador, Fecha);
            return item.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Planeacion/Insertar")]
        public HttpResponseMessage Insertar(Planeacion modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/Planeacion/Actualizar")]
        public HttpResponseMessage Actualizar(Planeacion modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }       

    }
}
