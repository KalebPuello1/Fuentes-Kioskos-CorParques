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
    public class FidelizacionController : ApiController
    {

        private readonly IServicioFidelizacion _service;

        public FidelizacionController(IServicioFidelizacion service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Fidelizacion/Buscar/{doc}")]
        public HttpResponseMessage Buscar(string doc)
        {
            var rta = _service.Obtener(doc);
            return rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpGet]
        [Route("api/Fidelizacion/ObtenerClienteTarjeta/{doc}")]
        public HttpResponseMessage ObtenerClienteTarjeta(string doc)
        {
            var rta = _service.ObtenerClienteTarjeta(doc);
            return rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpGet]
        [Route("api/Fidelizacion/ObtenerTarjetaSaldoPuntos/{consecutivo}")]
        public HttpResponseMessage ObtenerTarjetaSaldoPuntos(string consecutivo)
        {
            var rta = _service.ObtenerTarjetaSaldoPuntos(consecutivo);
            return rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }


        [HttpGet]
        [Route("api/Fidelizacion/ObtenerProductosRedimibles/{puntos}")]
        public HttpResponseMessage ObtenerProductosRedimibles(int puntos)
        {
            var rta = _service.ObtenerProductosRedimibles(puntos);
            return rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }
        [HttpPost]
        [Route("api/Fidelizacion/Guardar")]
        public HttpResponseMessage Guardar(ClienteFideliacion model)
        {
            var rta = _service.Actualizar(model);
            return !rta ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }
        [HttpGet]
        [Route("api/Fidelizacion/BloqueoTarjeta/{datos}")]
        public HttpResponseMessage BloqueoTarjeta(string datos)
        {
            var data = datos.Split('|');
            var rta = _service.BloquearTarjeta(data[0],int.Parse(data[1]),int.Parse(data[2]));
            return !rta ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpGet]
        [Route("api/Fidelizacion/RedimirProducto/{datos}")]
        public HttpResponseMessage RedimirProducto(string datos)
        {
            var data = datos.Split('|');
            var rta = _service.RedimirProduto(data[0], int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]));
            return !rta ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }

    }
}
