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
    public class ReporteCostoProductoController : ApiController
    {
        private readonly IServicioReporteCostoProducto _service;

        public ReporteCostoProductoController(IServicioReporteCostoProducto service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteCostoProductoController/getRep/{fechaInicial}/{fechaFinal}/{CodSap}")]
        public HttpResponseMessage ReporteRedFechaAbiertaf(string fechaInicial, string fechaFinal, string CodSap)
        {
            ReporteCostoProducto modelo = new ReporteCostoProducto();
            modelo.fechaInicial = fechaInicial == "null" ? null : fechaInicial;
            modelo.fechaFinal = fechaFinal == "null" ? null : fechaFinal;
            modelo.CodSap = CodSap == "null" ? null : CodSap;

            var item = _service.Obtener(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/ReporteCostoProductoController/getProd")]
        public HttpResponseMessage obtenerProductos()
        {
            var item = _service.obtenerProductos();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}