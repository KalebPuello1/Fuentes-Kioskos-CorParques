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
    public class TarifasParqueaderoController : ApiController
    {

        #region Declaraciones

        private readonly IServicioTarifasParqueadero _service;

        #endregion
        
        #region Constructor

        public TarifasParqueaderoController(IServicioTarifasParqueadero service)
        {
            _service = service;
        }

        #endregion

        #region Metodos

        [HttpGet]
        [Route("api/TarifasParqueadero/ObtenerTarifasParqueadero")]
        public HttpResponseMessage ObtenerTarifasParqueadero()
        {
            var list = _service.ObtenerListaSimple();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/TarifasParqueadero/ObtenerLista")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/TarifasParqueadero/Insertar")]
        public HttpResponseMessage Insertar(TarifasParqueadero modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/TarifasParqueadero/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/TarifasParqueadero/Actualizar")]
        public HttpResponseMessage Actualizar(TarifasParqueadero modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/TarifasParqueadero/ObtenerTarifaPorIdTipoVehiculo/{id}")]
        public HttpResponseMessage ObtenerTarifaPorIdTipoVehiculo(int id)
        {
            var list = _service.ObtenerTarifaPorIdTipoVehiculo(id);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        #endregion
    }
}
