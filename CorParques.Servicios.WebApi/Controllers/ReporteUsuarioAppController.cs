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
    public class ReporteUsuarioAppController : ApiController
    {
        private readonly IServicioReporteUsuarioApp _service;

        public ReporteUsuarioAppController(IServicioReporteUsuarioApp service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteReporteUsuarioApp/ObtenerReporte/{Correoelectronico}")]
        public HttpResponseMessage ObtenerReporte(string Correoelectronico = null)
        {
            Correoelectronico = Correoelectronico.Replace("|",".");
            Correoelectronico = Correoelectronico == "null" ? null : Correoelectronico ;          
            var item = _service.ObtenerReporte(Correoelectronico);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}