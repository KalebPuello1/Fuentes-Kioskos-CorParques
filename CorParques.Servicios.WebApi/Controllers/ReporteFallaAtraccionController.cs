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
    public class ReporteFallaAtraccionController : ApiController
    {
        private readonly IServicioReporteFallaAtraccion _service;

        public ReporteFallaAtraccionController(IServicioReporteFallaAtraccion service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteFallaAtraccion/set/{fechaInicial}/{fechaFinal}/{idAtraccion}/{idArea}")]
        public HttpResponseMessage ReporteFallaAtraccion(string fechaInicial, string fechaFinal, int idAtraccion, int idArea)
        {
            ReporteFallaAtraccion modelo = new ReporteFallaAtraccion();
            modelo.fechaInicial = fechaInicial == "0" ? null : fechaInicial;
            modelo.fechaFinal = fechaFinal == "0" ? null : fechaFinal;
            modelo.idAtraccion = idAtraccion;
            modelo.idArea = idArea;

            var item = _service.ObtenerTodos(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}