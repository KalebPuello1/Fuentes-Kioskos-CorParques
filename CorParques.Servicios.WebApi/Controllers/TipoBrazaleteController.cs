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
    public class TipoBrazaleteController : ApiController
    {
        private readonly IServicioTipoBrazalete _service;

        public TipoBrazaleteController(IServicioTipoBrazalete service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/TipoBrazalete/ObtenerListaSimpleAtraccion")]
        public HttpResponseMessage ObtenerListaSimpleAtraccion()
        {
            var list = _service.ObtenerListaSimpleAtraccion();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }


        [HttpGet]
        [Route("api/TipoBrazalete/Obtener/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/TipoBrazalete/ObtenerTodosBrazalete")]
        public HttpResponseMessage ObtenerTodosBrazalete()
        {
            var list = _service.ObtenerTodosBrazalete();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/TipoBrazalete/ObtenerBrazaletesSupervisor/{supervisor}")]
        public HttpResponseMessage ObtenerBrazaletesSupervisor(int supervisor)
        {
            var list = _service.ObtenerBrazaletesSupervisor(supervisor);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/TipoBrazalete/ObtenerTodosBrazaleteInventario/{IdPunto}")]
        public HttpResponseMessage ObtenerTodosBrazaleteInventario(int IdPunto)
        {
            var list = _service.ObtenerTodosBrazaleteInventario(IdPunto);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }


        [HttpGet]
        [Route("api/TipoBrazalete/ObtenerAtraccionxBrazalete/{IdTipoBrazalete}")]
        public HttpResponseMessage ObtenerAtraccionxBrazalete(int IdTipoBrazalete)
        {
            var item = _service.ObtenerAtraccionxBrazalete(IdTipoBrazalete);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/TipoBrazalete/ActualizarTipoBrazalete")]
        public HttpResponseMessage ActualizarTipoBrazalete(TipoBrazalete modelo)
        {
            var item = _service.ActualizarBrazalete(modelo);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpDelete]
        [Route("api/TipoBrazalete/block/{id}")]
        public HttpResponseMessage DesactivarTipoBrazalete(int id)
        {
            var item = _service.Desactivar(id);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

    }
}