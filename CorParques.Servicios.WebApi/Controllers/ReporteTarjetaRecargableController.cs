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
    public class ReporteTarjetaRecargableController : ApiController
    {
        private readonly IServicioReporteTarjetaRecargable _service;

        public ReporteTarjetaRecargableController(IServicioReporteTarjetaRecargable service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteTarjetaRecargable/ObtenerReporte/{FechaInicial}/{FechaFinal}/{FechaCompra}/{FechaVencimiento}/{Cliente}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial, string FechaFinal,string FechaCompra, string FechaVencimiento, string Cliente)
        {
            string fIni = FechaInicial == "null" ? null : FechaInicial;
            string fFin = FechaFinal == "null" ? null : FechaFinal;
            string fcom = FechaCompra == "null" ? null : FechaCompra;
            string fven = FechaVencimiento == "null" ? null : FechaVencimiento;
            string cliente = Cliente == "null" ? null : Cliente;

            var item = _service.ObtenerReporteTarjetas(fIni, fFin,fcom,fven, cliente);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/ReporteTarjetaRecargable/ObtenerClientes")]
        public HttpResponseMessage ObtenerClientes()
        {

            var item = _service.ObtenerClientes();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }   
    }
}