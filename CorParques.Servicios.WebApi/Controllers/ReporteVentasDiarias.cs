using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;


namespace CorParques.Servicios.WebApi.Controllers
{
    public class ReporteVentasDiariasController : ApiController
    {
        private readonly IServicioReporteVentasDiarias _service;

        public ReporteVentasDiariasController(IServicioReporteVentasDiarias service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteVentas/ObtenerReporteInventario/")]
        public HttpResponseMessage ObtenerReporteInventario()
        {
            var item = _service.ObtenerReporteInventario();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/ReporteVentas/ObtenerReporteDiario/{fecha}")]
        public HttpResponseMessage ObtenerReporteDiario(String fecha)
        {
            var item = _service.ObtenerReporteDiario(fecha);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}