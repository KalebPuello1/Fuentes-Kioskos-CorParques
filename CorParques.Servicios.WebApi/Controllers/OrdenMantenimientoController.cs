using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class OrdenMantenimientoController : ApiController
    {
        private readonly IServicioOrdenMantenimiento _service;

        public OrdenMantenimientoController(IServicioOrdenMantenimiento service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/OrdenMantenimiento/ObtenerOrdenesMantenimiento/{Punto}/{NumeroOrden}")]
        public HttpResponseMessage ObtenerOrdenesMantenimiento(string Punto, long NumeroOrden)
        {
            var list = _service.ObtenerOrdenesMantenimiento(Punto, NumeroOrden);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPut]
        [Route("api/OrdenMantenimiento/ActualizaHoraOrden")]
        public HttpResponseMessage ActualizaHoraOrden(string observaciones, int idUsuarioAprobador, long NumeroOrden, int Aprobado, int IdOperaciones, int Procesado, string CodSapPunto)
        {
            var item = _service.ActualizaHoraOrden(observaciones, idUsuarioAprobador, NumeroOrden, Aprobado, IdOperaciones, Procesado, CodSapPunto);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Se presento un error actualizando el parámetro. Por favor intentelo de nuevo")
                            : Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [HttpGet]
        [Route("api/OrdenMantenimiento/ObtenerRetornoPorNumeroOrden/{NumeroOrden}")]
        public HttpResponseMessage ObtenerRetornoPorNumeroOrden(long NumeroOrden)
        {
            var item = _service.ObtenerRetornoPorNumeroOrden(NumeroOrden);

            return item.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item.FirstOrDefault().NumeroOrden);
        }

        /// <summary>
        /// RDSH: Retorna una orden de mantenimiento por el numero de la orden.
        /// </summary>
        /// <param name="NumeroOrden"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/OrdenMantenimiento/ObtenerOrdenMantenimiento/{NumeroOrden}")]
        public HttpResponseMessage ObtenerOrdenMantenimiento(long NumeroOrden)
        {
            var item = _service.ObtenerOrdenMantenimiento(NumeroOrden);

            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

    }
}