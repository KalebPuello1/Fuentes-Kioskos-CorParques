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
    public class AdicionPedidosController : ApiController
    {

        private readonly IServicioAdicionPedidos _service;

        public AdicionPedidosController(IServicioAdicionPedidos service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/AdicionPedidos/DetallePedido/{CodigoSapPedido}")]
        public HttpResponseMessage DetallePedido(string CodigoSapPedido)
        {
            var list = _service.DetallePedido(CodigoSapPedido);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/AdicionPedidos/ValidarRangoConsecutivos/{ConsecutivoInicial}/{ConsecutivoFinal}/{Cantidad}/{IdProducto}/{idUsuario}")]
        public HttpResponseMessage ValidarRangoConsecutivos(string ConsecutivoInicial, string ConsecutivoFinal, int Cantidad, int IdProducto, int idUsuario)
        {
            string strResultado = string.Empty;
            strResultado = _service.ValidarRangoConsecutivos(ConsecutivoInicial, ConsecutivoFinal, Cantidad, IdProducto,idUsuario);
            return Request.CreateResponse(HttpStatusCode.OK, strResultado);

        }

        [HttpPost]
        [Route("api/AdicionPedidos/Guardar")]
        public HttpResponseMessage Guardar(IEnumerable<AdicionPedidos> modelo)
        {
            var list = _service.Guardar(modelo);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}
