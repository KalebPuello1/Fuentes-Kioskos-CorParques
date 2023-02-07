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
    public class ReporteReservaEspacioController : ApiController
    {
        private readonly IServicioReporteReservaEspacio _service;

        public ReporteReservaEspacioController(IServicioReporteReservaEspacio service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteReservaEspacio/get/{fechaInicial}/{fechaFinal}/{HoraInicial}/{HoraFinal}/{TipoEspacio}/{Espacio}/{NumeroPedido}")]
        public HttpResponseMessage ReporteFallaAtraccion(string fechaInicial, string fechaFinal, string HoraInicial, string HoraFinal, int TipoEspacio, int Espacio, string NumeroPedido)
        {
            ReporteReservaEspacio modelo = new ReporteReservaEspacio();
            modelo.fechaInicialGet = fechaInicial == "null" ? null : fechaInicial;
            modelo.fechaFinalGet = fechaFinal == "null" ? null : fechaFinal;
            modelo.horaIniGet = HoraInicial == "null" ? null : string.Format("{0}:{1}", HoraInicial.Substring(0, 2), HoraInicial.Substring(2, 2)) ;
            modelo.horaFinGet = HoraFinal == "null" ? null : string.Format("{0}:{1}", HoraFinal.Substring(0, 2), HoraFinal.Substring(2, 2));
            modelo.idEspGet = Espacio;
            modelo.idTipEpsGet = TipoEspacio;
            modelo.txtNPedidoGet = NumeroPedido == "null" ? null : NumeroPedido;

            var item = _service.ObtenerTodos(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}