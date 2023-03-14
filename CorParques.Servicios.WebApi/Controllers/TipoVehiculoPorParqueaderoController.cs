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
    public class TipoVehiculoPorParqueaderoController : ApiController
    {

        #region Declaraciones

        private readonly IServicioTipoVehiculoPorParqueadero _service;

        #endregion
        
        #region Constructor

        public TipoVehiculoPorParqueaderoController(IServicioTipoVehiculoPorParqueadero service)
        {
            _service = service;
        }

        #endregion

        #region Metodos

        [HttpGet]
        [Route("api/TipoVehiculoPorParqueadero/ObtenerTipoVehiculoPorParqueadero")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerLista();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/TipoVehiculoPorParqueadero/ObtenerPorIdTipoVehiculo/{IdTipoVehiculo}")]
        public HttpResponseMessage ObtenerPorIdTipoVehiculo(int IdTipoVehiculo)
        {
            var list = _service.ObtenerPorIdTipoVehiculo(IdTipoVehiculo);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/TipoVehiculoPorParqueadero/Insertar")]
        public HttpResponseMessage Insertar(TipoVehiculoPorParqueadero modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/TipoVehiculoPorParqueadero/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/TipoVehiculoPorParqueadero/Actualizar")]
        public HttpResponseMessage Actualizar(TipoVehiculoPorParqueadero modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/TipoVehiculoPorParqueadero/Eliminar")]
        public HttpResponseMessage Eliminar(TipoVehiculoPorParqueadero modelo)
        {
            string strError;
            var item = _service.Eliminar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        #endregion
    }
}
