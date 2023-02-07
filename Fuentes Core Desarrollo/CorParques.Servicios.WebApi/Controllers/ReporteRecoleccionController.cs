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
    public class ReporteRecoleccionController : ApiController
    {
        private readonly IServicioReporteRecoleccion _service;

        public ReporteRecoleccionController(IServicioReporteRecoleccion service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteRecoleccion/ObtenerReporte/{Fecha}/{IdTaquillero}/{IdSupervisor}")]
        public HttpResponseMessage ObtenerReporte(string Fecha = null, int? IdTaquillero = null, int? IdSupervisor = null)
       {
            var item = _service.ObtenerReporte(Fecha, IdTaquillero , IdSupervisor);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}