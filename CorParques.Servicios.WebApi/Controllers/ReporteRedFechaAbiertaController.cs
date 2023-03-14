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
    public class ReporteRedFechaAbiertaController : ApiController
    {
        private readonly IServicioReporteRedFechaAbierta _service;

        public ReporteRedFechaAbiertaController(IServicioReporteRedFechaAbierta service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteRedFechaAbierta/set/{fechaInicial}/{fechaFinal}/{SapTipoProducto}/{SapAsesor}")]
        public HttpResponseMessage ReporteRedFechaAbiertaf(string fechaInicial, string fechaFinal, string SapTipoProducto, string SapAsesor)
        //public HttpResponseMessage ObtenerAlistamientoPendiente(ReporteFallaAtraccion modelo)
        {
            ReporteRedFechaAbierta modelo = new ReporteRedFechaAbierta();
             modelo.fechaInicial = fechaInicial=="0"?null: fechaInicial;
             modelo.fechaFinal = fechaFinal == "0" ? null : fechaFinal;
             modelo.SapAsesor = SapAsesor == "null" ? null : SapAsesor;
             modelo.SapTipoProducto = SapTipoProducto == "null" ? null : SapTipoProducto;
            
            var item = _service.ObtenerTodos(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/obtenerTiposProducto/set")]
        public HttpResponseMessage obtenerTiposProducto()
        {
            var item = _service.obtenerTiposProducto();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/obtenerTodosVendedores/set")]
        public HttpResponseMessage obtenerTodosVendedores()
        {
            var item = _service.obtenerTodosVendedores();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}