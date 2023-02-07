using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class TipoPuntosController : ApiController
    {

        private readonly IServicioTipoPuntos _servicio;

        public TipoPuntosController(IServicioTipoPuntos TipoPunto)
        {
            this._servicio = TipoPunto;
        }

        [HttpGet]
        [Route("api/TipoPunto/ObtenerListaSimple")]        
        public HttpResponseMessage ObtenerListaSimple()
        {
            var list = _servicio.ObtenerListaSimple();
            bool any = list.Any();
            return !any ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        
    }
}