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
    public class ReporteNotificacionesController : ApiController
    {

        #region Declaraciones
        private readonly IServicioReporteNotificaciones _service;
        #endregion

        #region Constructor
        public ReporteNotificacionesController(IServicioReporteNotificaciones service)
        {
            _service = service;
        }
        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: retorna la informacion de las notificaciones enviadas por rango de fecha.
        /// </summary>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ReporteNotificaciones/ObtenerReporte/{FechaInicial}/{FechaFinal}")]
        public HttpResponseMessage ObtenerReporte(string FechaInicial = null, string FechaFinal = null)
        {
            FechaInicial = FechaInicial == "null" ? null : FechaInicial;
            FechaFinal = FechaFinal == "null" ? null : FechaFinal;            
            var item = _service.ObtenerReporte(FechaInicial, FechaFinal);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        #endregion
    }
}