using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using CorParques.Negocio.Contratos;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class Reporte_SI_Consumo_SF_FechaAbiertaControllerController : ApiController
    {

        private readonly IServicioReporte_SI_Consumo_SF_FechaAbierta servi;
        public Reporte_SI_Consumo_SF_FechaAbiertaControllerController(IServicioReporte_SI_Consumo_SF_FechaAbierta servicio)
        {
            servi = servicio;
        }

        // GET: Reporte_SI_Consumo_SF_FechaAbiertaController
        [HttpGet]
        [Route("api/Reporte_SI_Consumo_SF_FechaAbierta/getDato/{fechaI}/{fechaF}/{Npedido}/{redencion}")]
        public HttpResponseMessage getDato(string fechaI, string fechaF, string Npedido, string redencion)
        {
            var dato = servi.getSI_SF(fechaI, fechaF, Npedido, redencion);
            return dato == null ? Request.CreateResponse(System.Net.HttpStatusCode.NotFound) 
                : Request.CreateResponse(System.Net.HttpStatusCode.OK, dato);
        }

        [HttpGet]
        [Route("api/Reporte_SI_Consumo_SF_FechaAbierta/getDatos/{dat}")]
        public HttpResponseMessage getDatos(string dat)
        {
            var dato = servi.getSI_SFF(dat);
            return dato == null ? Request.CreateResponse(System.Net.HttpStatusCode.NotFound)
                : Request.CreateResponse(System.Net.HttpStatusCode.OK, dato);
        }
    }
}
