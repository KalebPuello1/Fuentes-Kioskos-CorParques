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
    public class ReporteControlCajaController : ApiController
    {
        private readonly IServicioReporteControlCaja _service;

        public ReporteControlCajaController(IServicioReporteControlCaja service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteControlCaja/get/{fechaInicial}/{fechaFinal}/{idPerfil}/{idTaquillero}")]
        public HttpResponseMessage ReporteControlCaja(DateTime fechaInicial, DateTime fechaFinal, int idPerfil, int idTaquillero)
        {
            ReporteControlCaja modelo = new ReporteControlCaja();
            modelo.fechaInicial = fechaInicial;
            modelo.fechaFinal = fechaFinal;
            modelo.idPerfil = idPerfil;
            modelo.idTaquillero = idTaquillero;
            
            object item = _service.ObtenerTodos(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}