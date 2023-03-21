using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class UsuarioGrupoController : ApiController
    {
        private readonly IServicioUsuarioGrupo _service;

        public UsuarioGrupoController(IServicioUsuarioGrupo service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/UsuarioGrupo/GetAll")]
        public HttpResponseMessage GetAll()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/UsuarioGrupo/GetAllxGroup/{idGrupo}")]
        public HttpResponseMessage GetByxGroup(int idGrupo)
        {
            var list = _service.ObtenerxGrupo (idGrupo);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }


    }
}
