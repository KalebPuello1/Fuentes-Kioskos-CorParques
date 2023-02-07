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
    public class ReporteDonacionesController : ApiController
    {
        private readonly IServicioReporteDonaciones _service;

        public ReporteDonacionesController(IServicioReporteDonaciones service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteDonaciones/ObtenerReporte/{FechaInicial}/{FechaFinal}/{producto}/{punto}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string producto = null, int? punto = null)
        {
            producto = producto== "null" ? null : producto;
            var item = _service.ObtenerReporte(FechaInicial, FechaFinal, producto, punto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/ReporteDonaciones/ObtenerProductos")]
        public HttpResponseMessage ObtenerProductos()
        {
            var item = _service.ObtenerProductos();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}