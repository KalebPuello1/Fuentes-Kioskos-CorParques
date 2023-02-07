using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorParques.Transversales.Util;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ParametrosController : ApiController
    {
        private readonly IServicioParametros _servicio;

        public ParametrosController(IServicioParametros servicio)
        {
            _servicio = servicio;
        }
        [HttpGet]
        [Route("api/Parameters/GetAll")]
        public HttpResponseMessage GetAll()
        {
            var list = _servicio.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Parameters/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _servicio.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpPost]
        [Route("api/Parameters/Insert")]
        public HttpResponseMessage Crear(Parametro modelo)
        {
            var item = _servicio.Crear(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [HttpPut]
        [Route("api/Parameters/Update")]
        public HttpResponseMessage Actualizar(Parametro modelo)
        {
            var item = _servicio.Actualizar(modelo);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpDelete]
        [Route("api/Parameters/Delete/{id}")]
        public HttpResponseMessage Eliminar(int id)
        {
            var item = _servicio.Eliminar(id);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Parameters/ObtenerParametroPorNombre/{Nombre}")]
        public HttpResponseMessage ObtenerParametroPorNombre(string Nombre)
        {
            //Valida si tiene la lista de parametros en cache.
            var list = Cache.GetCache<IEnumerable<Parametro>>("ParametrosGlobales");
            if (list == null)
            {
                //Si no la tiene consulta los parametros en base de datos y setea la cache.
                list = _servicio.ObtenerParametrosGlobales();
                Cache.SetCache("ParametrosGlobales", list, Cache.Long);
            }
            var objParametro = list.Where(x => x.Nombre.Equals(Nombre)).FirstOrDefault();            
            //var item = _servicio.ObtenerParametroPorNombre(Nombre);
            return objParametro == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, objParametro);
        }

        /// <summary>
        /// RDSH: Consulta todos los parametros de la aplicacion para almacenarlos en cache.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Parameters/ObtenerParametrosGlobales")]
        public HttpResponseMessage ObtenerParametrosGlobales()
        {
            var list = Cache.GetCache<IEnumerable<Parametro>>("ParametrosGlobales");
            if (list == null)
            {
                list = _servicio.ObtenerParametrosGlobales();
                Cache.SetCache("ParametrosGlobales", list, Cache.Long);
            }
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}
