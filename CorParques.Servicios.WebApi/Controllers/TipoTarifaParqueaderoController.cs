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
    public class TipoTarifaParqueaderoController : ApiController
    {

        #region Declaraciones

        private readonly IServicioTipoTarifaParqueadero _service;

        #endregion
        
        #region Constructor

        public TipoTarifaParqueaderoController(IServicioTipoTarifaParqueadero service)
        {
            _service = service;
        }

        #endregion

        #region Metodos

        [HttpGet]
        [Route("api/TipoTarifaParqueadero/ObtenerLista")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/TipoTarifaParqueadero/Insertar")]
        public HttpResponseMessage Insertar(TipoTarifaParqueadero modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);

            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/TipoTarifaParqueadero/GetById/{id}")]
        public HttpResponseMessage GetById(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/TipoTarifaParqueadero/Actualizar")]
        public HttpResponseMessage Actualizar(TipoTarifaParqueadero modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/TipoTarifaParqueadero/ObtenerTiposTarifasParqueadero")]
        public HttpResponseMessage ObtnerTiposTarifasParqueadero()
        {
            var list = _service.ObtenerListaSimple();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        #endregion

    }
}
