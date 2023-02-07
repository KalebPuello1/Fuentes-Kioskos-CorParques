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
    /// <summary>
    /// KADM Configuración Cargos
    /// </summary>
    public class CargosController : ApiController
    {
        #region Declaraciones
        private readonly IServicioCargos _service;
        #endregion

        #region Constructor
        public CargosController(IServicioCargos service)
        {
            _service = service;
        }
        #endregion

        #region Metodos

        [HttpGet]
        [Route("api/Cargos/ConsultarCargo")]
        public HttpResponseMessage ConsultarCargo(Cargos modelo)
        {
            var list = _service.ConsultarCargo(modelo);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Cargos/ObtenerListaSimple")]
        public HttpResponseMessage ObtenerListaSimple()
        {
            var list = _service.ObtenerListaSimple();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Cargos/ObtenerLista")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/Cargos/Insertar")]
        public HttpResponseMessage Insertar(Cargos modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/Cargos/GuardarCargoPerfil")]
        public HttpResponseMessage GuardarCargoPerfil(Cargos modelo)
        {
            var item = _service.GuardarCargoPerfil(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Cargos/ObtenerxId/{IdCargo}")]
        public HttpResponseMessage ObtenerxId(int IdCargo)
        {
            var item = _service.ObtenerxId(IdCargo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Cargos/Actualizar")]
        public HttpResponseMessage Actualizar(Cargos modelo)
        {            
            var item = _service.Actualizar(modelo);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, item);
        }

        [HttpPut]
        [Route("api/Cargos/ActualizarEmail")]
        public HttpResponseMessage ActualizarEmail(Cargos modelo)
        {
            var item = _service.ActualizarEmail(modelo);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, item);
        }

        [HttpGet]
        [Route("api/Cargos/ObtenerListaCargoPerfil/{IdCargo}")]
        public HttpResponseMessage ObtenerListaCargoPerfil(int IdCargo)
        {
            var list = _service.ObtenerCargosPerfil(IdCargo);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }


    }
    #endregion
}