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
    public class CentroImpresionController : ApiController
    {
        private readonly IServicioCentroImpresion _service;

        public CentroImpresionController(IServicioCentroImpresion servicio)
        {
            _service = servicio;
        }

        [HttpGet]
        [Route("api/CentroImpresion/ObtenerTodasSolicitudesBoleteria")]
        public HttpResponseMessage ObtenerTodasSolicitudesBoleteria()
        {

            var list = _service.ObtenerTodasSolicitudes();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/CentroImpresion/EliminarSolicitud")]
        public HttpResponseMessage EliminarSolicitudImpresion(SolicitudBoleteria modelo)
        {
            var rta = _service.EliminarSolicitudImpresion(modelo);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpGet]
        [Route("api/CentroImpresion/ObtenerListSolicitudBoleteria/{idUsuario}")]
        public HttpResponseMessage ObtenerListSolicitudBoleteria(int idUsuario)
        {
            var list = _service.ObtenerListSolicitudBoleteria(idUsuario);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/CentroImpresion/AdicionarSolicitudImpresion")]
        public HttpResponseMessage AdicionarSolicitudImpresion(SolicitudBoleteria modelo)
        {
            var rta = _service.InsertarSolicitudImpresion(modelo);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpPost]
        [Route("api/CentroImpresion/GestionarCentroImpresion")]
        public HttpResponseMessage GestionarCentroImpresion(SolicitudBoleteria modelo)
        {
            var rta = _service.GestionarCentroImpresion(modelo);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }
    }
}