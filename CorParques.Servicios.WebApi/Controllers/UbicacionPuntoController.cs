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
    public class UbicacionPuntoController : ApiController
    {

        private readonly IServicioUbicacionPunto _service;

        public UbicacionPuntoController(IServicioUbicacionPunto service)
        {
            _service = service;
        }

        /// <summary>
        ///RDSH: Retorna todas las ubicaciones activas para un punto especifico.
        /// </summary>
        /// <param name="IdPunto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/UbicacionPunto/ObtenerPorPunto/{IdPunto}")]
        public HttpResponseMessage ObtenerPorPunto(int IdPunto)
        {
            var list = _service.ObtenerPorPunto(IdPunto);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        ///RDSH: Retorna una lista de tipo general para cargar un dropdown list.
        /// </summary>
        /// <param name="IdPunto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/UbicacionPunto/ObtenerListaSimplePorPunto/{IdPunto}")]
        public HttpResponseMessage ObtenerListaSimplePorPunto(int IdPunto)
        {
            var list = _service.ObtenerListaSimplePorPunto(IdPunto);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        /// RDSH: Inserta los datos de una ubicación punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UbicacionPunto/Insertar")]
        public HttpResponseMessage Insertar(UbicacionPunto modelo)
        {
            string strError = "";
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        /// <summary>
        /// RDSH: Actualiza los datos de una ubicación punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UbicacionPunto/Actualizar")]
        public HttpResponseMessage Actualizar(UbicacionPunto modelo)
        {
            string strError;
            var item = _service.ActualizarUbicacion(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }
        
        /// <summary>
        ///OEGA: Retorna una lista de todas las ubicaciones.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/UbicacionPunto/ObtenerUbicaciones")]
        public HttpResponseMessage ObtenerUbicaciones()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        ///OEGA: Retorna una lista de todas las ubicaciones.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/UbicacionPunto/ObtenerPorId/{IdUbicacion}")]
        public HttpResponseMessage ObtenerPorId(int IdUbicacion)
        {
            var list = _service.Obtener(IdUbicacion);
            return Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpDelete]
        [Route("api/UbicacionPunto/Delete/{id}")]
        public HttpResponseMessage Eliminar(int id)
        {
            var item = _service.Eliminar(id);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

    }
}
