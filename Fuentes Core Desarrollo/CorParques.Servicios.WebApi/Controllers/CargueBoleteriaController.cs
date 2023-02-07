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
    public class CargueBoleteriaController : ApiController
    {

        #region Declaraciones

        private readonly IServicioCargueBoleteria _service;

        #endregion

        #region Constructor

        public CargueBoleteriaController(IServicioCargueBoleteria service)
        {
            _service = service;
        }

        #endregion

        #region Metodos        

        /// <summary>
        /// RDSH: Retorna los productos de tipo boleteria.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/CargueBoleteria/ObtenerListaCargueBoleteria")]
        public HttpResponseMessage ObtenerListaCargueBoleteria()
        {
            var list = _service.ObtenerListaCargueBoleteria();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        /// RDSH: Retorna los productos de tipo boleteria.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/CargueBoleteria/ObtenerTipoBoleteria")]
        public HttpResponseMessage ObtenerTipoBoleteria()
        {
            var list = _service.ObtenerTipoBoleteria();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        /// RDSH: Inserta un cargue masivo de boleteria.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/CargueBoleteria/Insertar")]
        public HttpResponseMessage Insertar(CargueBoleteria modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
                     
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound,  strError  );
        }

        /// <summary>
        /// RDSH: Retorna la lista de cargues realizados en la tabla TB_CargueBoleteria.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/CargueBoleteria/Actualizar")]
        public HttpResponseMessage Actualizar(CargueBoleteria modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);

            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
            
        }

        #endregion

    }
}
