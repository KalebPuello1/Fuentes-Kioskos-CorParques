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
    public class ObservacionRecoleccionController : ApiController
    {

        private readonly IServicioObservacionRecoleccion _service;

        public ObservacionRecoleccionController(IServicioObservacionRecoleccion service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/ObservacionRecoleccion/Insert")]
        public HttpResponseMessage Crear(ObservacionRecoleccion modelo)
        {
            HttpResponseMessage returnObject = null;
            modelo.Observacion = modelo.Observacion == null ? "" : modelo.Observacion;
            var item = _service.Crear(modelo);
            if (item != null)
            {                
                returnObject = Request.CreateResponse(HttpStatusCode.OK, item.IdObservacionRecoleccion.ToString());
            }
            else
            {
                returnObject = Request.CreateResponse(HttpStatusCode.InternalServerError, "No fue posible guardar la observación.");
            }

            return returnObject;
        }

        [HttpGet]
        [Route("api/ObservacionRecoleccion/ObtenerPorIdRecoleccionUsuario/{IdRecoleccion}/{IdUsuario}")]
        public HttpResponseMessage ObtenerPorIdRecoleccionUsuario(int IdRecoleccion, int IdUsuario)
        {
            var list = _service.ObtenerPorIdRecoleccionUsuario(IdRecoleccion, IdUsuario);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}
