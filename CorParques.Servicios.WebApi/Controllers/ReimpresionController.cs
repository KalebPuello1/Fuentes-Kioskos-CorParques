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
    public class ReimpresionController : ApiController
    {
        private readonly IServicioReimpresion _service;
        private readonly IServicioControlParqueadero _serviceParqueadero;

        public ReimpresionController(IServicioReimpresion service, IServicioControlParqueadero serviceParqueadero)
        {
            _service = service;
            _serviceParqueadero = serviceParqueadero;
        }

        [HttpGet]
        [Route("api/Reimpresion/ObtenerReimpresion/{Punto}/{FechaInicial}/{FechaFinal}/{HoraInicial}/{HoraFinal}/{CodBrazalete}/{CodInicialFactura}/{CodFinalFactura}")]
        public HttpResponseMessage ObtenerReimpresion(string Punto = null, string FechaInicial = null, string FechaFinal = null, string HoraInicial = null, string HoraFinal = null, string CodBrazalete = null, string CodInicialFactura = null, string CodFinalFactura = null)
        {
            var item = _service.ObtenerReimpresion(Punto, FechaInicial, FechaFinal, HoraInicial, HoraFinal, CodBrazalete, CodInicialFactura, CodFinalFactura);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Reimpresion/GetReimpresion")]
        public HttpResponseMessage GetReimpresion(ReimpresionFiltros modelo)
        {
            var item = _service.GetReimpresion(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        [Route("api/Reimpresion/ParqueaderoReImprimir/{Placa}")]
        public HttpResponseMessage ParqueaderoReImprimir(string Placa)
        {
            var item = _serviceParqueadero.ObtenerPorPlaca(Placa);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

    }
}