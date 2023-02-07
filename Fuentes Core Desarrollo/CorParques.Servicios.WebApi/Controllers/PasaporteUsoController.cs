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
    public class PasaporteUsoController : ApiController
    {
        private readonly IServicioPasaporteUso _service;

        public PasaporteUsoController(IServicioPasaporteUso service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/PasaporteUso/ObtenerListaSimpleAtraccion")]
        public HttpResponseMessage ObtenerListaSimpleAtraccion()
        {
            var list = Cache.GetCache<IEnumerable<TipoGeneral>>("AtraccionSimple");
            if (list == null)
            {
                list = _service.ObtenerListaSimpleAtraccion();
                Cache.SetCache("AtraccionSimple", list, Cache.Medium);
            }
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }


        [HttpGet]
        [Route("api/PasaporteUso/Obtener/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        //[HttpGet]
        //[Route("api/PasaporteUso/ObtenerTodosPasaporte")]
        //public HttpResponseMessage ObtenerTodosPasaporte()
        //{
        //    var list = _service.ObtenerTodosPasaporteUso();
        //    return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
        //                    : Request.CreateResponse(HttpStatusCode.OK, list);
        //}


        [HttpPost]
        [Route("api/PasaporteUso/ActualizarPasaporteUso")]
        public HttpResponseMessage ActualizarPasaporteUso(PasaporteUso modelo)
        {
            var item = _service.ActualizarPasaporteUso(modelo);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }        

    }
}