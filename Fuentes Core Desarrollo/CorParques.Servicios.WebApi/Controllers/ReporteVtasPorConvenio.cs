using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ReporteVentasPorConvenioController : ApiController
    {
        #region Declaraciones

        private readonly IServicioReporteVentasPorConvenio _service;

        #endregion Declaraciones

        #region Constructor

        public ReporteVentasPorConvenioController(IServicioReporteVentasPorConvenio service)
        {
            _service = service;
        }

        #endregion Constructor

        #region Metodos

        

        [HttpGet]
        [Route("api/ReporteVentasPorConvenio/ObtenerReporte/{FechaInicial}/{FechaFinal}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial, string FechaFinal)
        {
           
            var item = _service.ObtenerReporte(FechaInicial, FechaFinal);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        #endregion Metodos
    }
}