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
    public class RetornoController : ApiController
    {

        private readonly IServicioRetorno _service;

        public RetornoController(IServicioRetorno service)
        {
            _service = service;
        }
        
        [HttpPost]
        [Route("api/Retorno/Insertar")]
        public HttpResponseMessage Insertar(Retorno modelo)
        {
            string strError = "";
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPost]
        [Route("api/Retorno/InsertarDetalle")]
        public HttpResponseMessage InsertarDetalle(RetornoDetalle modelo)
        {
            string strError = "";
            var item = _service.InsertarDetalle(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        //[HttpPut]
        //[Route("api/Retorno/Actualizar")]
        //public HttpResponseMessage Actualizar(Operaciones modelo)
        //{
        //    string strError;
        //    var item = _service.ActualizarOperacion(modelo, out strError);
        //    return item ? Request.CreateResponse(HttpStatusCode.OK, "")
        //                       : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        //}       
        

    }
}
