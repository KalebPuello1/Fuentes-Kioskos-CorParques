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
    public class FacturaController : ApiController
    {

        private readonly IServicioFactura _service;

        public FacturaController(IServicioFactura service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Factura/ObtenerFacturaContingencia")]
        public HttpResponseMessage ObtenerFacturaContingencia()
        {
            var list = _service.ObtenerFacturaContingencia();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/Factura/ProcesaFacturaContingencia")]
        public HttpResponseMessage ProcesaFacturaContingencia(IEnumerable<Factura> _factura)
        {
            var item = _service.ProcesaFacturaContingencia (_factura);
            return item.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Factura/BorrarFacturaContingencia")]
        public HttpResponseMessage BorrarFacturaContingencia(List<Factura> _factura)
        {
            var item = _service.BorrarFacturaContingencia(_factura);
            return item != "" ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// Obtiene los datos relacionados perfiles, menu y puntos usuario
        /// </summary>
        [HttpGet]
        [Route("api/Factura/ObtenerDiccionarioContigencia")]
        public HttpResponseMessage ObtenerDicionarioContigencia()
        {
            var item = _service.ObtenerDiccionarioContigencia();
            return item == null? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Factura/ObtenerUltimaFactura/{CodSapPunto}")]
        public HttpResponseMessage ObtenerUltimaFactura(string CodSapPunto)
        {
            var item = _service.ObtenerUltimaFactura(CodSapPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                           : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/Factura/ObtenerIdFranquiciaRedeban/{CodFranquicia}")]
        public HttpResponseMessage ObtenerIdFranquiciaRedeban(string CodFranquicia)
        {
            var item = _service.ObtenerIdFranquiciaRedeban(CodFranquicia);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                           : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        
    }
}
