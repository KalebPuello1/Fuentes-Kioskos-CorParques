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
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
    public class PerfilController : ApiController
    {
        #region Declaraciones
        private readonly IServicioPerfil _service;
        #endregion

        #region Constructor
        public PerfilController(IServicioPerfil service)
        {
            _service = service;
        }
        #endregion

        #region Metodos

        [HttpGet]
        [Route("api/Perfil/ObtenerLista")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Perfil/ObtenerListaSimple")]
        public HttpResponseMessage ObtenerListaSimple()
        {
            var list = _service.ObtenerListaSimple();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/Perfil/Insertar")]
        public HttpResponseMessage Insertar(Perfil modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }
        [HttpGet]
        [Route("api/Perfil/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Perfil/Actualizar")]
        public HttpResponseMessage Actualizar(Perfil modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }
        [HttpPut]
        [Route("api/Perfil/Inactivar")]
        public HttpResponseMessage Inactivar(Perfil modelo)
        {
            var item = _service.Actualizar(modelo);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, "Se presento un error al desactivar el perfil.Por favor inténtelo de nuevo");
        }
        

        [HttpGet]
        [Route("api/Perfil/PerfilActivos/{IdPerfil}")]
        public HttpResponseMessage PerfilActivos(int IdPerfil)
        {
            var item = _service.PerfilActivos(IdPerfil);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Perfil/ActualizarSegregacion")]
        public HttpResponseMessage ActualizarSegregacion(SegregacionFunciones segregacionFunciones)
        {
            var item = _service.ActualizarSegregacion(segregacionFunciones);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Perfil/ConsultarSegregacion/{IdPerfil}")]
        public HttpResponseMessage ConsultarSegregacion(int idPerfil)
        {
            var item = _service.ConsultarSegregacion(idPerfil);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Perfil/ValidarSegurgacion")]
        public HttpResponseMessage ValidarSegregacion(IEnumerable<Perfil> modelo)
        {
            var item = _service.ValidarSegregacion(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
    #endregion
}