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
    public class BoleteriaController : ApiController
    {

        private readonly IServicioBoleteria _service;

        public BoleteriaController(IServicioBoleteria service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Boleteria/ObtenerBoleta/{Codigo}")]
        public HttpResponseMessage ObtenerBoleta(string Codigo)
        {
            var item = _service.ObtenerBoleta(Codigo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Boleteria/CambiarBoleta")]
        public HttpResponseMessage CambiarBoleta(List<Boleteria> modelo)
        {
            var item = _service.CambiarBoleta(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                           : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Boleteria/Cambioboleta/{codigo1}")]
        //public HttpResponseMessage Cambioboleta(string codigo1)
        public HttpResponseMessage Cambioboleta(string codigo1)
        {
            var item = _service.Cambioboleta(codigo1, "");
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                           : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Retorna una boleta por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Boleteria/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Retorna un objeto boleteria por su consecutivo (codigo de barras).
        /// </summary>
        /// <param name="Consecutivo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Boleteria/ObtenerPorConsecutivo/{Consecutivo}")]
        public HttpResponseMessage ObtenerPorConsecutivo(string Consecutivo)
        {
            var item = _service.ObtenerPorConsecutivo(Consecutivo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Boleteria/ConsultaMovimientoBoletaControl/{CodBarrasBoletaControl}")]
        public HttpResponseMessage ConsultaMovimientoBoletaControl(string CodBarrasBoletaControl)
        {
            var item = _service.ConsultaMovimientoBoletaControl(CodBarrasBoletaControl);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

       


        [HttpGet]
        [Route("api/Boleteria/NumeroUsos/{Consecutivos}")]
        public HttpResponseMessage NumeroUsos(string Consecutivos)
        {
            var item = _service.NumeroUsosBoleteria(Consecutivos);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        /// <summary>
        /// DANR: Retorna un objeto boleteria por su consecutivo con saldo tarjeta Recargable (codigo de barras).
        /// </summary>
        /// <param name="Consecutivo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Boleteria/ObtenerPorConsecutivoTarjetaRecargable/{Consecutivo}")]
        public HttpResponseMessage ObtenerPorConsecutivoTarjetaRecargable(string Consecutivo)
        {
            var item = _service.ObtenerPorConsecutivo(Consecutivo);
            if (item == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            if (Consecutivo.StartsWith("7"))
            {
                var saldo = _service.ObtenerSaldoTarjetaRecargable(Consecutivo);
                if (saldo.StartsWith("error:"))
                {
                    item.NombreProducto = saldo;
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }
                else
                {

                    item.Saldo = int.Parse(saldo.Split('|')[0]);
                    if (saldo.Split('|').Length > 1)
                    {
                        item.NombreProducto = saldo.Split('|')[2] + "|" + saldo.Split('|')[1];
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Inserta una boleta.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Boleteria/Insertar")]
        public HttpResponseMessage Insertar(Boleteria modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        /// <summary>
        /// RDSH: Inserta una boleta.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Boleteria/InsertarBrazalete")]
        public HttpResponseMessage InsertarBrazalete(string modelo)
        {
            var data = modelo.Split('|');
            var item = _service.InsertarBrazalete(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]));
            return item.StartsWith("Error") ? Request.CreateResponse(HttpStatusCode.InternalServerError, item)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// Bloquear boleta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Boleteria/Bloquear")]
        public HttpResponseMessage Bloquear(Boleteria modelo)
        {
            var item = _service.BloquearBoleta(modelo);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// RDSH: Actualiza la tabla de boleteria.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Boleteria/Actualizar")]
        public HttpResponseMessage Actualizar(Boleteria modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        /// <summary>
        /// RDSH: Retorna un objeto boleteria para el ajuste de medio de pago bono regalo.
        /// </summary>
        /// <param name="Codigo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Boleteria/ConsultarBonoRegalo/{Consecutivo}")]
        public HttpResponseMessage ConsultarBonoRegalo(string Consecutivo)
        {
            var item = _service.ConsultarBonoRegalo(Consecutivo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Route("api/Boleteria/ConsultarInventarioDia")]
        public HttpResponseMessage ConsultarInventarioDia()
        {
            var item = _service.ConsultarInventarioDia();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Boleteria/ConsultarInventarioProdDia/{producto}")]
        public HttpResponseMessage ConsultarInventarioProdDia(int producto)
        {
            var item = _service.ConsultarInventarioProdDia(producto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Route("api/Boleteria/CambioboletaDato/{codigo1}")]
        //public HttpResponseMessage Cambioboleta(string codigo1)
        public HttpResponseMessage CambioboletaDato(string codigo1)
        {
            var item = _service.CambioboletaDato(codigo1, "");
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                           : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Boleteria/UpdateEstadoCambioboleta/{codigo1}")]
        //public HttpResponseMessage Cambioboleta(string codigo1)
        public HttpResponseMessage UpdateEstadoCambioboleta(string codigo1)
        {
            var item = _service.UpdateEstadoCambioboleta(codigo1);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                           : Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}
