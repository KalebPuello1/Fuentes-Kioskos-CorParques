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
    public class ReporteVentasDirectasController : ApiController
    {
        private readonly IServicioReporteVentasDirectas _service;

        public ReporteVentasDirectasController(IServicioReporteVentasDirectas service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteVentas/ObtenerReporte/{FechaInicial}/{FechaFinal}/{IdPunto}/{IdTaquillero}/{IdFormaPago}/{IdFranquicia}/{CentroBeneficio}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial = null, string FechaFinal = null, int? IdPunto = null, int? IdTaquillero = null, int? IdFormaPago = null, int? IdFranquicia = null, string CentroBeneficio = null)
        {
            CentroBeneficio = CentroBeneficio == "null" ? null : CentroBeneficio;
            var item = _service.ObtenerReporte(FechaInicial, FechaFinal, IdPunto, IdTaquillero , IdFormaPago , IdFranquicia, CentroBeneficio);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}