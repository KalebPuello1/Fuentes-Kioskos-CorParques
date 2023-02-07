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
    public class ReporteVentasPorHoraController : ApiController
    {
        private readonly IServicioReporteVentasPorHora _service;

        public ReporteVentasPorHoraController(IServicioReporteVentasPorHora service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteVentasPorHora/ObtenerReporte/{FechaInicial}/{FechaFinal}/{HoraInicial}/{HoraFinal}/{CodigoProducto}/{NombreProducto}/{CodigoPunto}/{CB}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string HoraInicial = null, string HoraFinal = null, 
            string CodigoProducto = null, string NombreProducto = null, string CodigoPunto = null, string CB = null)
        {
            FechaInicial = FechaInicial == "null" ? null : FechaInicial;
            FechaFinal = FechaFinal == "null" ? null : FechaFinal;
            HoraInicial = HoraInicial == "null" ? null : string.Format("{0}:{1}", HoraInicial.Substring(0,2), HoraInicial.Substring(2, 2));
            HoraFinal = HoraFinal == "null" ? null : string.Format("{0}:{1}", HoraFinal.Substring(0, 2), HoraFinal.Substring(2, 2));
            CodigoProducto = CodigoProducto == "null" ? null : CodigoProducto;
            NombreProducto = NombreProducto == "null" ? null : NombreProducto;
            CodigoPunto = CodigoPunto == "null" ? null : CodigoPunto;
            CB = CB == "null" ? null : CB;

            var item = _service.ObtenerReporte(FechaInicial, FechaFinal, HoraInicial, HoraFinal, CodigoProducto, NombreProducto, CodigoPunto,CB);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}