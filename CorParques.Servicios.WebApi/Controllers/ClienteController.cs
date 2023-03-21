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
    public class ClienteController : ApiController
    {
        #region Declaraciones

        private readonly IServicioCliente _service;

        #endregion

        #region Constructor

        public ClienteController(IServicioCliente service)
        {
            _service = service;
        }

        #endregion

        #region Metodos
        [HttpGet]
        [Route("api/Cliente/ObtenerTodos")]
        public HttpResponseMessage ObtenerTodos()
        {
            IEnumerable<TipoGeneral> lista = _service.ObtenerTodos().Select(m => new TipoGeneral {CodSAP = m.CodSapCliente, Nombre = string.Concat(m.Documento, "-", m.Nombres) });
            if (lista.Count() > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        #endregion
    }

}