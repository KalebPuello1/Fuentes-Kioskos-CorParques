using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class HistoricoBoletaController : ApiController
    {
        private readonly IServicioHistoricoBoleta _servicio;

        public HistoricoBoletaController(IServicioHistoricoBoleta servicio)
        {
            _servicio = servicio;
        }

        [HttpGet, Route("api/HistoricoBoleta/ObtenerHistoricoBoleta/{consecutivo}")]
        public HttpResponseMessage ObtenerHistoricoBoleta(string consecutivo)
        {
            var rta = _servicio.ObtenerHistoricoBoleta(consecutivo);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }
    }
}