using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class CodigoDeBarrasController : ApiController
    {

        private readonly IServicioCodigodeBarras _service;

        public CodigoDeBarrasController(IServicioCodigodeBarras service)
        {
            _service = service;
        }

        /// <summary>
        /// RDSH: Valida los requisitos para que un punto pueda operar.
        /// </summary>
        /// <param name="IdPunto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/CodigoDeBarras/ValidarPermisoOperativoPunto/{IdPunto}")]
        public HttpResponseMessage ValidarPermisoOperativoPunto(int IdPunto)
        {

            string strError = string.Empty;

            var item = _service.ValidarPermisoOperativoPunto(IdPunto, out strError);
            return Request.CreateResponse(HttpStatusCode.OK, item);                   
        }

        /// <summary>
        /// RDSH: Valida si un codigo de barras es valido para ingresar a una atracción o destreza.
        /// </summary>
        /// <param name="CodigoBarras"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/CodigoDeBarras/ValidarCodigoBarras/{CodigoBarras}/{IdPunto}/{IdUsuario}/{ValorAcumulado}/{ValorAcumuladoConvenio}/{CodigoDeBarrasDescargar}")]
        public HttpResponseMessage ValidarCodigoBarras(string CodigoBarras, int IdPunto, int IdUsuario, long ValorAcumulado, long ValorAcumuladoConvenio, string CodigoDeBarrasDescargar)
        {
            string strError = string.Empty;
            var item = _service.ValidarCodigoBarras(CodigoBarras, IdPunto, IdUsuario, ValorAcumulado, ValorAcumuladoConvenio, CodigoDeBarrasDescargar, out strError);
            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Transfiere el dinero acumulado de varios codigos de barra a uno que haya sido leido dos veces.
        /// </summary>
        /// <param name="CodigoACargar"></param>
        /// <param name="CodigoDeBarrasDescargar"></param>
        /// <param name="ValorAcumulado"></param>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/CodigoDeBarras/TransferirSaldo/{CodigoACargar}/{CodigoDeBarrasDescargar}/{ValorAcumulado}/{IdUsuario}")]
        public HttpResponseMessage TransferirSaldo(string CodigoACargar, string CodigoDeBarrasDescargar, long ValorAcumulado, int IdUsuario)
        {
            string strError = string.Empty;
            var item = _service.TransferirSaldo(CodigoACargar, CodigoDeBarrasDescargar, ValorAcumulado, IdUsuario, out strError);
            return Request.CreateResponse(HttpStatusCode.OK, item);
        }



    }
}
