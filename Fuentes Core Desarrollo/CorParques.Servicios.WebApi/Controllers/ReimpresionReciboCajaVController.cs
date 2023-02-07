using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System.Net;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ReimpresionReciboCajaVController : ApiController
    {
        private readonly IServicioReimpresionReciboCajaV servicioReimpresion;
        public ReimpresionReciboCajaVController(IServicioReimpresionReciboCajaV servi)
        {
            servicioReimpresion = servi;
        }
        // GET: ReimpresionReciboCajaV
        [HttpGet]
        [Route("api/ReimpresionReciboCajaV/datosReimpresion/{datoI}/{datoF}")]
        public HttpResponseMessage datosReimpresion(string datoI, string datoF)
        {
            IEnumerable<ReimpresionReciboCajaV> dato = servicioReimpresion.datosReimpresion(datoI, datoF);
            return dato.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound) : 
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        
        [HttpGet]
        [Route("api/ReimpresionReciboCajaV/datoReimpresion/{datoI}/{datoF}")]
        public HttpResponseMessage datoReimpresion()
        {
            ReimpresionReciboCajaV dato = servicioReimpresion.datoReimpresion();
            return dato.IdPunto == 0 ? Request.CreateResponse(HttpStatusCode.NotFound) : 
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }


       
    }
}
