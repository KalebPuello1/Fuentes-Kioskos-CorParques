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
    public class SegmentoDiaController : ApiController
    {
        private readonly IServicioSegmentoDia _service;

        public SegmentoDiaController(IServicioSegmentoDia service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/SegmentoDia/GetAll")]
        public HttpResponseMessage Obtener()
        {
            IEnumerable<TipoGeneral> listaBitacoraDia = _service.ObtenerTodos();
            if (listaBitacoraDia.Count() > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, listaBitacoraDia);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }

}