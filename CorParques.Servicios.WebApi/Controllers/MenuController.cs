using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    /// <summary>
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
    public class MenuController : ApiController
    {
        private readonly IServicioMenu _service;

        public MenuController(IServicioMenu service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Menu/ObtenerListaActivos")]
        public HttpResponseMessage ObtenerListaActivos()
        {
            var list = _service.ObtenerListaActivos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}