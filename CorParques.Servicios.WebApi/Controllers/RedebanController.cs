using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class RedebanController : ApiController
    {
        private readonly IServicioRedeban _service;
        private readonly IServicioFactura _serviceFactura;

        public RedebanController(IServicioRedeban service, IServicioFactura serviceFacura)
        {
            _service = service;
            _serviceFactura = serviceFacura;
        }


        [HttpPost]
        [Route("api/Redeban/InsertarLogRedebanSolicitud")]
        public HttpResponseMessage InsertarLogRedebanSolicitud(LogRedebanSolicitud logRedebanSolicitud)
        {

            var item = _service.InsertarLogRedebanSolicitud(ref logRedebanSolicitud);
            return item == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Redeban/InsertarLogRedebanSolicitudAnulacion")]
        public HttpResponseMessage InsertarLogRedebanSolicitudAnulacion(LogRedebanSolicitudAnulacion logRedebanSolicitudAnulacion)
        {

            var item = _service.InsertarLogRedebanSolicitudAnulacion(ref logRedebanSolicitudAnulacion);
            return item == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Redeban/InsertarLogRedebanRespuesta")]
        public HttpResponseMessage InsertarLogRedebanRespuesta(LogRedebanRespuesta logRedebanRespuesta)
        {

            var item = _service.InsertarLogRedebanRespuesta(ref logRedebanRespuesta);
            return item == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Redeban/ActualizarLogRedebanRespuesta")]
        public HttpResponseMessage ActualizarLogRedebanRespuesta(LogRedebanRespuesta logRedebanRespuesta)
        {

            var item = _service.ActualizarLogRedebanRespuesta(ref logRedebanRespuesta);
            return item == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Redeban/ObtenerNumeroFactura/{IdPunto}")]
        public HttpResponseMessage ObtenerNumeroFactura(int IdPunto)
        {

            var item = _serviceFactura.GenerarNumeroFactura(IdPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}