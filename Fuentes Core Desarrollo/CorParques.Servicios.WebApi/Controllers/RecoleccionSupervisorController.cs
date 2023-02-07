using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class RecoleccionSupervisorController : ApiController
    {
        private readonly IServicioRecoleccionSupervisor _service;
        private readonly IServicioRecoleccion _serviceRecoleccion;

        public RecoleccionSupervisorController(IServicioRecoleccionSupervisor service, IServicioRecoleccion serviceRecoleccion)
        {
            _service = service;
            _serviceRecoleccion = serviceRecoleccion;
        }

        [HttpGet]
        [Route("api/RecoleccionSupervisor/ObtenerRecoleccionActiva/{IdUsuario}/{IdPunto}/{IdEstado}")]
        public HttpResponseMessage ObtenerRecoleccionActiva(int IdUsuario, int IdPunto, int IdEstado)
        {
            var item = _service.ObtenerRecoleccionActiva(IdUsuario, IdPunto, IdEstado);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/RecoleccionSupervisor/ObtenerRecoleccionesActivas/{IdUsuario}/{IdPunto}/{Cierre}/{IdEstado}")]
        public HttpResponseMessage ObtenerRecoleccionesActivas(int IdUsuario, int IdPunto, int Cierre, int IdEstado)
        {
            var item = _serviceRecoleccion.ObtenerRecoleccionesActivas(IdUsuario, IdPunto, (Cierre == 1), IdEstado);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]   
        [Route("api/RecoleccionSupervisor/Actualizar")]
        public HttpResponseMessage Actualizar(DetalleRecoleccionMonetaria modelo)
        {
            string strError;
            var item = _service.ActualizarRecoleccionMonetaria(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/RecoleccionSupervisor/ActualizarDocumentos")]
        public HttpResponseMessage ActualizarDocumentos(DetalleRecoleccionDocumento modelo)
        {
            string strError;
            var item = _service.ActualizarRecoleccionDocumentos(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/RecoleccionSupervisor/ActualizarNovedades")]
        public HttpResponseMessage ActualizarNovedades(DetalleRecoleccionNovedad modelo)
        {
            string strError;
            var item = _service.ActualizarRecoleccionNovedades(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/RecoleccionSupervisor/ActualizarRecoleccion")]
        public HttpResponseMessage ActualizarRecoleccion(Recoleccion modelo)
        {
            string strError;
            var item = _service.ActualizarRecoleccion(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPost]
        [Route("api/RecoleccionSupervisor/InsertaObservacion")]
        public HttpResponseMessage InsertaObservacion(ObservacionRecoleccion modelo)
        {
            string strError;
            int IdRecoleccion;
            var item = _service.InsertaObservacion(modelo, out strError, out IdRecoleccion);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/RecoleccionSupervisor/ObtenerDocumentos/{IdUsuario}")]
        public HttpResponseMessage ObtenerDocumentos(int IdUsuario)
        {
            var item = _service.ObtenerDocumentos(IdUsuario);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/RecoleccionSupervisor/ObtenerNovedadPorIdRecoleccion/{IdRecoleccion}")]
        public HttpResponseMessage ObtenerNovedadPorIdRecoleccion(int IdRecoleccion)
        {
            var item = _service.ObtenerNovedadPorIdRecoleccion(IdRecoleccion);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/RecoleccionSupervisor/ObtenerNovedadPorIdUsuario/{IdUsuario}")]
        public HttpResponseMessage ObtenerNovedadPorIdUsuario(int IdUsuario)
        {
            var item = _service.ObtenerNovedadPorIdUsuario(IdUsuario);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/RecoleccionSupervisor/ObtenerDetalleRecoleccion/{IdApertura}/{TipoConsulta}")]
        public HttpResponseMessage ObtenerDetalleRecoleccion(int IdApertura, int TipoConsulta)
        {
            var item = _service.ObtenerDetalleRecoleccion(IdApertura, TipoConsulta);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/RecoleccionSupervisor/RegresarEstado")]
        public HttpResponseMessage RegresarEstado(int IdApertura, int IdEstado)
        {
            var item = _service.RegresarEstado(IdEstado, IdApertura);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

    }
}