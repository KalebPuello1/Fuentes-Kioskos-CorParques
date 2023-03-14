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
    public class BitacoraDiaController : ApiController
    {
        private readonly IServicioBitacoraDia _serviceBitacora;
        private readonly IServicioClima _serviceClima;
        private readonly IServicioSegmentoDia _serviceSegmentoDia;

        public BitacoraDiaController(IServicioBitacoraDia serviceBitacora, IServicioClima serviceClima, IServicioSegmentoDia serviceSegmentoDia)
        {
            _serviceBitacora = serviceBitacora;
            _serviceClima = serviceClima;
            _serviceSegmentoDia = serviceSegmentoDia;
        }

        [HttpGet]
        [Route("api/BitacoraDia/GetAll")]
        public HttpResponseMessage Obtener()
        {
            IEnumerable<BitacoraDia> listaBitacoraDia = _serviceBitacora.ObtenerTodos();
            if (listaBitacoraDia.Count() > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, listaBitacoraDia);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("api/BitacoraDia/Get/{fecha}")]
        public HttpResponseMessage Obtener(string fecha)
        {
            IEnumerable<BitacoraDia> listaBitacoraDia = _serviceBitacora.Obtener(fecha);
            if (listaBitacoraDia.Count() > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, listaBitacoraDia);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("api/BitacoraDia/ObtenerCantidadPersonas")]
        public HttpResponseMessage ObtenerCantidadPersonas()
        {
            int CantidadPersonas = _serviceBitacora.ObtenerCantidadPersonas() ?? 0;
            return Request.CreateResponse(HttpStatusCode.OK, CantidadPersonas);
        }

        [HttpPost]
        [Route("api/BitacoraDia/Set")]
        public HttpResponseMessage Establecer(BitacoraDiaLista modelo)
        {
            var rta = _serviceBitacora.Asignar(modelo);
            return !string.IsNullOrEmpty(rta.Mensaje) ? Request.CreateResponse(HttpStatusCode.NotFound, rta.Mensaje)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }
    }

}