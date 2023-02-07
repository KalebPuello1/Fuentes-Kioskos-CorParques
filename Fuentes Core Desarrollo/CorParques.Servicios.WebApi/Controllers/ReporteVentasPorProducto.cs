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
    public class ReporteVentasPorProductoController : ApiController
    {
        private readonly IServicioReporteVentasPorProducto _service;

        public ReporteVentasPorProductoController(IServicioReporteVentasPorProducto service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteVentasPorProducto/ObtenerReporte/{FechaInicial}/{FechaFinal}/{CodigoProducto}/{CodigoPunto}/{CentroBeneficio}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string CodigoProducto = null, string CodigoPunto = null, string CentroBeneficio = null)
        {
            FechaInicial = FechaInicial == "null" ? "" : string.Concat(FechaInicial.Substring(0, 2), "/", FechaInicial.Substring(2, 2), "/", FechaInicial.Substring(4, 4)); ;
            FechaFinal = FechaFinal == "null" ? "" : string.Concat(FechaFinal.Substring(0, 2), "/", FechaFinal.Substring(2, 2), "/", FechaFinal.Substring(4, 4)); ;           
            CodigoProducto = CodigoProducto == "null" ? "" : CodigoProducto;
            CodigoPunto = CodigoPunto == "null" ? "" : CodigoPunto;
            CentroBeneficio = CentroBeneficio == "null" ? "" : CentroBeneficio;
             


            var item = _service.ObtenerReporte(FechaInicial, FechaFinal, CodigoProducto, CodigoPunto, CentroBeneficio);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}