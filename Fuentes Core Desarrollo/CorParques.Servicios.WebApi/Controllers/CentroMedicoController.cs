using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    /// <summary>
    /// NMSR CU006-DE-1CI-ARS-001-Configuración centro médico
    /// </summary>
    public class CentroMedicoController : ApiController
    {

        #region Declaraciones

        private readonly IServicioCentroMedico _service;

        #endregion

        #region Constructor

        public CentroMedicoController(IServicioCentroMedico service)
        {
            _service = service;
        }

        #endregion

        #region Metodos

        [HttpGet]
        [Route("api/CentroMedico/ObtenerLista")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/CentroMedico/Insertar")]
        public HttpResponseMessage Insertar(CentroMedico modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/CentroMedico/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/CentroMedico/Actualizar")]
        public HttpResponseMessage Actualizar(CentroMedico modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/CentroMedico/ObtenerListaCentroMedico")]
        public HttpResponseMessage ObtenerListaCentroMedico()
        {
            //var list = Cache.GetCache<IEnumerable<TipoGeneral>>("UbicacionesCM");
            //if (list == null)
            //{
            //    list = _service.ObtenerListaCentroMedico();
            //    Cache.SetCache("UbicacionesCM", list, Cache.Long);
            //}
            var list = _service.ObtenerListaCentroMedico();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }


        [HttpGet]
        [Route("api/CentroMedico/ObtenerListaZonaAreaUbicacion/{IdCentroMedico}")]
        public HttpResponseMessage ObtenerListaZonaAreaUbicacion(int IdCentroMedico)
        {
            var list = _service.ObtenerListaZonaAreaUbicacion(IdCentroMedico);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/CentroMedico/Zonas")]
        public HttpResponseMessage ObtenerZonas()
        {
            var list = _service.ObtenerZonas();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        #endregion
    }
}