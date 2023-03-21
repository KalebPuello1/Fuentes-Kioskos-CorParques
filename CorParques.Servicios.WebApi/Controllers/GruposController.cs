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
    public class GruposController : ApiController
    {
        private readonly IServicioGruposNotificacion _service;

        public GruposController(IServicioGruposNotificacion service)
        {
            _service = service;
        }

        [Route("api/Grupo/GetAll")]
        public HttpResponseMessage GetAll()
        {
            var list = _service.ObtenerTodosGrupo();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
               
        [Route("api/Grupo/ObtenerGruposActivos")]
        [HttpGet]
        public HttpResponseMessage ObtenerGruposActivos()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Grupo/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Grupo/ObtenerIdGrupo/{id}")]
        public HttpResponseMessage ObtenerIdGrupo(int id)
        {
            var item = _service.ObtenerUsuarioxGrupo(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Grupo/Insert")]
        public HttpResponseMessage Crear(Grupo modelo)
        {
            var item = _service.Insertar(modelo);
            return item != null ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        [Route("api/Grupo/ActualizarGrupo")]
        public HttpResponseMessage ActualizarGrupo(Grupo modelo)
        {
            var item = _service.ActualizarGrupoUsuario(modelo);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
        }

        [HttpPut]
        [Route("api/Grupo/Update")]
        public HttpResponseMessage Actualizar(Grupo modelo)
        {
            var item = _service.Actualizar(modelo);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpDelete]
        [Route("api/Grupo/Delete/{id}")]
        public HttpResponseMessage Eliminar(int id)
        {
            //var modelo = _service.Obtener(id);
            var item = _service.Eliminar(new Grupo { Id = id });
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

    }
}
