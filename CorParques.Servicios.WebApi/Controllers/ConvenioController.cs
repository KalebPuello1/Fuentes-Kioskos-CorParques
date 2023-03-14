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
    public class ConvenioController : ApiController
    {
        private readonly IServicioConvenio _service;

        public ConvenioController(IServicioConvenio service)
        {
            _service = service;
        }

        /// <summary>
        /// RDSH: Retorna la información de los pedidos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Convenio/ObtenerListaConvenios")]
        public HttpResponseMessage ObtenerListaConvenios()
        {
            var list = _service.ObtenerListaConvenios();
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        /// EDSP: Obtener todas las exclusiones
        /// </summary>
        [HttpGet]
        [Route("api/Convenio/ObtenerListaExclusion")]
        public HttpResponseMessage ObtenerListaExclusionesConvenios()
        {
            var list = _service.ObtenerExclusionesConvenio();
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        /// EDSP: Obtener exclusicones por id de convenio
        /// </summary>
        [HttpGet]
        [Route("api/Convenio/ObtenerExclusionesPorConvenio/{IdConvenio}")]
        public HttpResponseMessage ObtenerExclusionesPorConvenio(int IdConvenio)
        {
            var list = _service.ObtenerExclusionesPorIdConvenio(IdConvenio);
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        /// EDSP: Insertar exclusión 
        /// </summary>
        [HttpPost]
        [Route("api/Convenio/InsertarExclusion")]
        public HttpResponseMessage InsertarExclusionConveno(ExclusionConvenio modelo)
        {
            var rta = _service.InsertarExclusionConvenio(modelo);
            return rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        /// <summary>
        /// EDSP: Obtener productos por id convenio
        /// </summary>
        [HttpGet]
        [Route("api/Convenio/ObtenerProductoConvenio/{IdConvenio}")]
        public HttpResponseMessage ObtenerProductoConvenio(int IdConvenio)
        {
            var list = _service.ObtenerProductoConvenio(IdConvenio);
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        /// EDSP: Insertar exclusión 
        /// </summary>
        [HttpPost]
        [Route("api/Convenio/ActualizarProductosConvenio")]
        public HttpResponseMessage ActualizarProductosConvenio(IEnumerable<ConvenioProducto> modelo)
        {
            var rta = _service.ActualizarProductosConvenio(modelo);
            return rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, rta);
        }
        



        /// <summary>
        /// RDSH: Inserta un convenio. Retorna el id del convenio en la propiedad error
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Convenio/Insertar")]
        public HttpResponseMessage Insertar(Convenio modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        /// <summary>
        /// EDSP: Actualizar precios de los convenios
        /// </summary>
        [HttpPut, Route("api/Convenio/ActualizarPrecios")]
        public HttpResponseMessage ActualizarPrecios(ActualizarPrecios modelo)
        {
            var item = _service.ActualizarPreciosConvenios(modelo);
            return string.IsNullOrEmpty(item) ? Request.CreateResponse(HttpStatusCode.OK, "")
                        : Request.CreateResponse(HttpStatusCode.NotFound, item);
        }

        /// <summary>
        /// RDSH: Retorna la información de los pedidos.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Convenio/Actualizar")]
        public HttpResponseMessage Actualizar(Convenio modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        /// <summary>
        /// EDSP: Deshabilitar exclusión
        /// </summary>
        [HttpPut]
        [Route("api/Convenio/DeshabilitarExclusion")]
        public HttpResponseMessage DeshabilitarExclusion(ExclusionConvenio modelo)
        {
            var rta = _service.DeshabilitarExclusion(modelo);
            return rta == 0  ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("api/Convenio/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

    }
}