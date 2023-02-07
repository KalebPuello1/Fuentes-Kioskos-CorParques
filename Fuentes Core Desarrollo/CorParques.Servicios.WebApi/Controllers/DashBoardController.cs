using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class DashBoardController : ApiController
    {

        private readonly IServicioDashBoard _service;

        public DashBoardController(IServicioDashBoard service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/DashBoard/ObtenerInformacionDashBoard/{FechaInicial}/{FechaFinal}")]
        public HttpResponseMessage ObtenerInformacionDashBoard(string FechaInicial, string FechaFinal)
        {
            var list = _service.ObtenerInformacionDashBoard(FechaInicial, FechaFinal);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}
