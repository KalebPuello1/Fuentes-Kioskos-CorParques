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
    public class ReporteFANVendidasController : ApiController
    {
        private readonly IServicioReporteFANVendidas _service;

        public ReporteFANVendidasController(IServicioReporteFANVendidas service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteFANVendidas/ObtenerReporte/{FechaInicial}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial = null)
        {            
            var item = _service.ObtenerReporte(FechaInicial);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}