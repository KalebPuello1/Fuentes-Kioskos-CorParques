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
    public class ReportePasajerosAtraccionesController : ApiController
    {
        private readonly IServicioReportePasajerosAtracciones _service;

        public ReportePasajerosAtraccionesController(IServicioReportePasajerosAtracciones service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReportePasajerosAtracciones/ObtenerReporte/{FechaInicial}/{FechaFinal}/{IdPunto}/{IdTipoProducto}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial, string FechaFinal, int IdPunto, int IdTipoProducto)
        {
            string fIni = FechaInicial == "null" ? null : FechaInicial;
            string fFin = FechaFinal == "null" ? null : FechaFinal;
            int idPunto = IdPunto;
            int idTipoProducto = IdTipoProducto;

            var item = _service.ObtenerReporte(fIni, fFin,idPunto, idTipoProducto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }        
    }
}