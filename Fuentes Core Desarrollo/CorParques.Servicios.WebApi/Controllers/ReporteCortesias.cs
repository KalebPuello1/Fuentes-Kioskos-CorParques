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
    public class ReporteCortesiasController : ApiController
    {
        private readonly IServicioReporteCortesias _service;

        public ReporteCortesiasController(IServicioReporteCortesias service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteCortesias/ObtenerReporte/{FechaInicial}/{FechaFinal}/{Documento}/{TarjetaFan}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial, string FechaFinal, string Documento, string TarjetaFan)
        {
            var item = _service.ObtenerReporte(FechaInicial, FechaFinal, Documento, TarjetaFan);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}