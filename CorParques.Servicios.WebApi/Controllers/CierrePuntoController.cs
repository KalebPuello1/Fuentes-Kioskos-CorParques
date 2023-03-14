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
    public class CierrePuntoController : ApiController
    {
        private readonly IServicioCierrePuntos _service;


        public CierrePuntoController(IServicioCierrePuntos service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("api/CierrePunto/ObtenerAperturaElemento/{idpunto}")]
        public HttpResponseMessage ObtenerAperturaElemento(int IdPunto)
        {
            var list = _service.ObtenerElementosCierre(IdPunto);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }


        [HttpPost]
        [Route("api/CierrePunto/ActaulizarCierre")]
        public HttpResponseMessage ObtenerAperturaElemento(IEnumerable<CierreElementos> modelo)
        {
            var item = _service.ActualizarCierre(modelo);
            return item == false ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [Route("api/CierrePunto/ObtenerAperturaElementoSupervisor/{Usuario}")]
        public HttpResponseMessage ObtenerAperturaElementoSupervisor(int Usuario)
        {
            var list = _service.ObtenerElementosCierreSupervisor(Usuario);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/CierrePunto/ObtenerAperturaElementoNido/{Usuario}")]
        public HttpResponseMessage ObtenerAperturaElementoNido(int Usuario)
        {
            var list = _service.ObtenerElementosCierreNido(Usuario);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
                
        /// <summary>
        /// RDSH: Actualizacion masiva de cierre de elementos para supervisor.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/CierrePunto/ActualizarMasivoSupervisor")]
        public HttpResponseMessage ActualizarMasivoSupervisor(IEnumerable<CierreElementos> modelo)
        {
            string strError = string.Empty;
            var item = _service.ActualizarMasivo(1, modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        /// <summary>
        /// RDSH: Actualizacion masiva de cierre de elementos para nido.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/CierrePunto/ActualizarMasivoNido")]
        public HttpResponseMessage ActualizarMasivoNido(IEnumerable<CierreElementos> modelo)
        {
            string strError = string.Empty;
            var item = _service.ActualizarMasivo(2, modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

    }
}