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
    public class ReporteAprovechamientoFAController : ApiController
    {
        private readonly IServicioReporteAprovechamientoFA _service;

        public ReporteAprovechamientoFAController(IServicioReporteAprovechamientoFA service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteAprovechamientoFA/ObtenerReporte/{FechaInicial}/{FechaFinal}/{cliente}/{pedido}/{factura}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string cliente = null, string pedido = null, string factura = null)
        {
            cliente = cliente == "null" ? null : cliente;
            pedido = pedido == "null" ? null : pedido;
            factura = factura == "null" ? null : factura;
            var item = _service.ObtenerReporte(FechaInicial, FechaFinal, cliente, pedido , factura);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}