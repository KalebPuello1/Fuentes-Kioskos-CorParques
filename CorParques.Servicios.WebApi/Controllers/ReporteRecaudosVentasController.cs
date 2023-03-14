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
    public class ReporteRecaudosVentasController : ApiController
    {
        private readonly IServicioReporteRecaudosVentas _service;

        public ReporteRecaudosVentasController(IServicioReporteRecaudosVentas service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteRecaudosVentas/set/{_FechaInicial}/{_FechaFinal}/{_Consecutivo}/{_Cliente}/{_FormaPago}/{_Entidad}")]
        public HttpResponseMessage ReporteFallaAtraccion(string _FechaInicial, string _FechaFinal, int _Consecutivo, string _Cliente, int _FormaPago, int _Entidad)
        {
            ReporteRecaudosVentas modelo = new ReporteRecaudosVentas();
            modelo._FechaInicial = _FechaInicial == "nulo" ? null : _FechaInicial;
            modelo._FechaFinal = _FechaFinal == "nulo" ? null : _FechaFinal;
            modelo._Consecutivo = _Consecutivo;
            modelo._Cliente = _Cliente == "nulo" ? null : _Cliente;
            modelo._FormaPago = _FormaPago;
            modelo._Entidad = _Entidad;

            var item = _service.ObtenerTodos(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}