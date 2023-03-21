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
    public class ReporteSoporteITController : ApiController
    {
        private readonly IServicioReporteSoporteIT _service;

        public ReporteSoporteITController(IServicioReporteSoporteIT service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteSoporteIT/ObtenerSoporteIT/{Fecha}")]
        public HttpResponseMessage ObtenerSoporteIT(string Fecha)
       {
            var item = _service.ObtenerReporte(Fecha);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}