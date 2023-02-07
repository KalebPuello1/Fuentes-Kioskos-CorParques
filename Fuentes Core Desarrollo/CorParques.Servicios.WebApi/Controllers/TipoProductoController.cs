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
    public class TipoProductoController : ApiController
    {
        private readonly IServicioTipoProducto _service;

        public TipoProductoController(IServicioTipoProducto service)
        {
            _service = service;
        }

        /// <summary>
        /// RDSH: Retorna la información de los pedidos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TipoProducto/ObtenerListaTipoProduto")]
        public HttpResponseMessage ObtenerListaTipoProduto()
        {
            var list = _service.ObtenerListaTipoProduto();
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

    }
}