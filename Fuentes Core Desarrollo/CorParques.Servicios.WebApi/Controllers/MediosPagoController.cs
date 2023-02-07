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
    public class MediosPagoController : ApiController
    {

        private readonly IServicioMediosPago _servicio;

        public MediosPagoController(IServicioMediosPago servicio)
        {
            this._servicio = servicio;
        }

        [Route("api/MediosPago/GetAll"), HttpGet]
        public HttpResponseMessage ObtenerTodos()
        {
            var list = Cache.GetCache<IEnumerable<MediosPago>>("MediosPago");
            if (list == null)
            {
                list = _servicio.ObtenerTodos();
                Cache.SetCache("MediosPago", list, Cache.Long);
            }
            return list.Count() > 0 ? Request.CreateResponse(HttpStatusCode.OK, list) 
                : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [Route("api/MediosPago/GetAllSimple"), HttpGet]
        public HttpResponseMessage ObtenerListaSimple()
        {
            var list = Cache.GetCache<IEnumerable<TipoGeneral>>("MediosPagoSimple");
            if (list == null)
            {
                list = _servicio.ObtenerListaSimple();
                Cache.SetCache("MediosPagoSimple", list, Cache.Long);
            }
            return list.Count() > 0 ? Request.CreateResponse(HttpStatusCode.OK, list)
               : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("api/MediosPago/Insert")]
        public HttpResponseMessage Crear(MediosPago modelo)
        {
            var item = _servicio.Crear(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Se presento un error creando un medio de pago. Por favor intentelo de nuevo")
                            : Request.CreateResponse(HttpStatusCode.OK, item.Id);
        }
        [HttpPut]
        [Route("api/MediosPago/Update")]
        public HttpResponseMessage Actualizar(MediosPago modelo)
        {
            var item = _servicio.Actualizar(modelo);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Se presento un error actualizando el medio de pago, Por favor intentelo de nuevo")
                            : Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}