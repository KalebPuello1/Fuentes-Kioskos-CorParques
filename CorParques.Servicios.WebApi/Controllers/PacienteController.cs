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
    public class PacienteController : ApiController
    {
        private readonly IServicioPaciente _service;
        
        public PacienteController(IServicioPaciente service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Paciente/ObtenerListPacientes")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerLista();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Paciente/ObtenerListaSimplePacientes")]
        public HttpResponseMessage ObtenerListaSimple()
        {
            var list = Cache.GetCache<IEnumerable<TipoGeneral>>("Pacientes");
            if (list == null)
            {
                list = _service.ObtenerListaSimple();
                Cache.SetCache("Pacientes", list, Cache.Medium);
            }
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Paciente/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Paciente/ObtenerPorTipoDocumento/{IdTipoDocumento}/{Documento}")]
        public HttpResponseMessage ObtenerPorTipoDocumento(int IdTipoDocumento, string Documento)
        {
            var item = _service.ObtenerPorTipoDocumento(IdTipoDocumento, Documento);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Paciente/Insertar")]
        public HttpResponseMessage Insertar(Paciente modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/Paciente/Actualizar")]
        public HttpResponseMessage Actualizar(Paciente modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/Paciente/Eliminar")]
        public HttpResponseMessage Eliminar(Paciente modelo)
        {
            string strError;
            var item = _service.Eliminar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

    }
}