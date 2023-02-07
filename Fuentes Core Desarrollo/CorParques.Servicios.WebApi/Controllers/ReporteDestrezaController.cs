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
    public class ReporteDestrezaController : ApiController
    {
        private readonly IServicioPos _service;
        private readonly IServicioReporteDestreza _serviceReporte;

        public ReporteDestrezaController(IServicioPos service, IServicioReporteDestreza serviceReporte)
        {
            _service = service;
            _serviceReporte = serviceReporte;
        }

        [HttpGet]
        [Route("api/ReporteDestreza/GetProductosDestrezasPremio")]
        public HttpResponseMessage GetProductosDestrezasPremio()
        {
            var item = _service.ObtenerPremiosDestrezas();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/ReporteDestreza/ObtenerReporte/{FechaInicial}/{FechaFinal}/{CodigoPunto}/{NombreTipoBoleta}/{NombreCliente}/{tipoVenta}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string CodigoPunto = null, string NombreTipoBoleta = null, string NombreCliente = null, string tipoVenta = null)
        {
            FechaInicial = FechaInicial == "null" ? null : FechaInicial;
            FechaFinal = FechaFinal == "null" ? null : FechaFinal;
            CodigoPunto = CodigoPunto == "null" ? null : CodigoPunto;            
            NombreTipoBoleta = NombreTipoBoleta == "null" ? null : NombreTipoBoleta;
            NombreCliente = NombreCliente == "null" ? null : NombreCliente;
            bool? bnlTipoVenta = null;
            if (tipoVenta != "null")
            {
                bnlTipoVenta = (tipoVenta == "1" ? true : false);
            }
            //var listaRet = _serviceReporte.ObtenerReporte(FechaInicial, FechaFinal, CodigoPunto, CodigoSeries, NombreTipoBoleta, NombreCliente,tipoVenta);
            //RDSH: Se implementa nuevo metodo para traer dos listas y hacer un reporte con dos hojas.
            var listaRet = _serviceReporte.ObtenerReporte(FechaInicial, FechaFinal, CodigoPunto, NombreTipoBoleta, NombreCliente, bnlTipoVenta);
            return listaRet == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, listaRet);
        }
    }
}