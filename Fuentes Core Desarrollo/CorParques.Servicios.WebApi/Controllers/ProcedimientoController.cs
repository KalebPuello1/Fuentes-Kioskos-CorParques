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
    public class ProcedimientoController : ApiController
    {
        private readonly IServicioProcedimiento _service;

        public ProcedimientoController(IServicioProcedimiento service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Procedimiento/ObtenerListaProcedimientos")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerLista();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Procedimiento/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
       }


        [HttpPost]
        [Route("api/Procedimiento/Insertar")]
        public HttpResponseMessage Insertar(Procedimiento modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/Procedimiento/Actualizar")]
        public HttpResponseMessage Actualizar(Procedimiento modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/Procedimiento/Eliminar")]
        public HttpResponseMessage Eliminar(Procedimiento modelo)
        {
            string strError;
            var item = _service.Eliminar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }
        
        [HttpGet]
        [Route("api/Procedimiento/ObtenerCategoriaAtencion")]
        public HttpResponseMessage ObtenerCategoriaAtencion()
        {
            var list = _service.ObtenerCategoriaAtencion();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Procedimiento/ObtenerTipoPaciente")]
        public HttpResponseMessage ObtenerTipoPaciente()
        {
            var list = _service.ObtenerTipoPaciente();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        #region Reporte

        [HttpGet]
        [Route("api/Procedimiento/ReporteAtenciones/{IdTipoDocumentoPaciente}/{IdCategoriaAtencion}/{IdTipoPaciente}/{FechaInicial}/{FechaFinal}/{IdProcedimiento}/{IdZonaArea}/{IdUbicacion}")]
        public HttpResponseMessage ReporteAtenciones(int IdTipoDocumentoPaciente, int IdCategoriaAtencion, int IdTipoPaciente, string FechaInicial, string FechaFinal, int IdProcedimiento, int IdZonaArea, int IdUbicacion)
        {
            var list = _service.ReporteAtenciones(IdTipoDocumentoPaciente, IdCategoriaAtencion, IdTipoPaciente, FechaInicial, FechaFinal, IdProcedimiento, IdZonaArea, IdUbicacion);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        #endregion
    }
}