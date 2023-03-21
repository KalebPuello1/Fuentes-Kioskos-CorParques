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
    public class ReporteCuadreDiarioFlujoCajasTaqController : ApiController
    {
        private readonly IServicioReporteCuadreDiarioFlujoCajasTaq _service;

        public ReporteCuadreDiarioFlujoCajasTaqController(IServicioReporteCuadreDiarioFlujoCajasTaq service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/ReporteCuadreDiarioFlujoCajasTaq/get/{fechaInicial}/{fechaFinal}/{idTipIngreso}/{TipNovedad}/{TipConsnumo}")]
        public HttpResponseMessage ReporteCuadreDiarioFlujoCajasTaq(string fechaInicial, string fechaFinal, int idTipIngreso, string TipNovedad, string TipConsnumo)
        {
            ReporteCuadreDiarioFlujoCajasTaq modelo = new ReporteCuadreDiarioFlujoCajasTaq();
            modelo.fechaInicial = fechaInicial == "null" ? null : fechaInicial;
            modelo.fechaFinal = fechaFinal == "null" ? null : fechaFinal;

            modelo.TipNovedad = TipNovedad == "null" ? null : TipNovedad;
            modelo.TipConsnumo = TipConsnumo == "null" ? null : TipConsnumo;

            modelo.idTipIngreso = idTipIngreso;

            var item = _service.ObtenerTodos(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}