using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class InventarioController : ApiController
    {
        private readonly IServicioMateriales _serviceMateriales;
        private readonly IServicioInventario _serviceInventario;


        public InventarioController(IServicioMateriales serviceMateriales, IServicioInventario serviceInventario)
        {
            _serviceMateriales = serviceMateriales;
            _serviceInventario = serviceInventario;
        }

        [HttpGet]
        [Route("api/Inventario/ObtenerMaterialesxPunto/{IdPunto}")]
        public HttpResponseMessage ObtenerMaterialesxPunto(int IdPunto)
        {
            var list = _serviceMateriales.ObtenerMaterialesxPuntos(IdPunto);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Inventario/ObtenerResumenCierre")]
        public HttpResponseMessage ObtenerResumenCierre()
        {
            var list = _serviceInventario.ObtenerResumenCierre(null);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/Inventario/ActualizarInventario")]
        public HttpResponseMessage ActualizarInventario(Inventario modelo)
        {
            var list = _serviceInventario.ActualizarInventario(modelo);
            return list == "" ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, list.ToString());
        }
        [HttpPost]
        [Route("api/Inventario/EntregaPedido")]
        public HttpResponseMessage EntregaPedido(IEnumerable<TransladoInventario> modelo)
        {
            var list = _serviceInventario.EntregaPedido(modelo);
            return list == "" ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, list.ToString());
        }

        [HttpPost]
        [Route("api/Inventario/ActualizarTransladoInventario")]
        public HttpResponseMessage ActualizarTransladoInventario(IEnumerable<TransladoInventario> modelo)
        {
            var list = _serviceInventario.ActualizarTransladoInventario(modelo);
            return list == "" ? Request.CreateResponse(HttpStatusCode.NotFound, list)
                            : Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost]
        [Route("api/Inventario/TrasladoPedido")]
        public HttpResponseMessage TrasladoPedido(IEnumerable<TransladoInventario> modelo)
        {
            var list = _serviceInventario.TrasladoPedido(modelo);
            return list != "" ? Request.CreateResponse(HttpStatusCode.NotFound, list)
                            : Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("api/Inventario/BuscarMotivosInventario")]
        public HttpResponseMessage BuscarMotivosInventario()
        {
            var _list = _serviceInventario.BuscarMotivosInventario();
            return _list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, _list);
        }

        [HttpPost]
        [Route("api/Inventario/ActualizarAjusteInventario")]
        public HttpResponseMessage ActualizarAjusteInventario(IEnumerable<AjustesInventario> modelo)
        {
            var list = _serviceInventario.ActualizarAjustesInventario(modelo);
            return list == "" ? Request.CreateResponse(HttpStatusCode.NotFound, list)
                            : Request.CreateResponse(HttpStatusCode.OK, "");
        }


        [HttpPost]
        [Route("api/Inventario/InsertarAjusteInventario")]
        public HttpResponseMessage InsertarAjusteInventario(IEnumerable<AjustesInventario> modelo)
        {
            var resp = _serviceInventario.InsertarAjusteInventario(modelo);
            return resp != "" ? Request.CreateResponse(HttpStatusCode.NotFound, resp)
                            : Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("api/Inventario/ObtenerTiposAjuste")]
        public HttpResponseMessage ObtenerTiposAjuste()
        {
            var list = _serviceInventario.ObtenerTiposAjuste();
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound, "")
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Inventario/ObtenerMotivosAjuste/{CodSapAjuste}")]
        public HttpResponseMessage ObtenerMotivosAjuste(string CodSapAjuste)
        {
            var list = _serviceInventario.ObtenerMotivosAjuste(CodSapAjuste);
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound, "")
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Inventario/ObtenerTodosMotivos")]
        public HttpResponseMessage ObtenerTodosMotivos()
        {
            var list = _serviceInventario.ObtenerTodosMotivos();
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound, "")
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Inventario/ObtenerPedidosTraslado")]
        public HttpResponseMessage ObtenerPedidosTraslado()
        {
            var list = _serviceInventario.ObtenerPedidosTraslado();
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound, "")
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Inventario/ObtenerPedidosEntregaAsesor/{idPunto}")]
        public HttpResponseMessage ObtenerPedidosEntregaAsesor(int idPunto)
        {
            var list = _serviceInventario.ObtenerPedidosEntregaAsesor(idPunto);
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound, "")
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Inventario/ObtenerSolicitudesDevolucion")]
        public HttpResponseMessage ObtenerSolicitudesDevolucion()
        {
            var list = _serviceInventario.ObtenerSolicitudesDevolucion();
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound, "")
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Inventario/ConsultarPedidoRetorno/{pedido}")]
        public HttpResponseMessage ConsultarPedidoRetorno(string pedido)
        {
            var list = _serviceInventario.ConsultarPedidoRetorno(pedido);
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound, "")
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Inventario/ObtenerMotivos")]
        public HttpResponseMessage ObtenerMotivos()
        {
            var list = _serviceInventario.ConsultarMotivosRetorno();
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound, "")
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/Inventario/CrearSolicitudRetorno")]
        public HttpResponseMessage CrearSolicitudRetorno(SolicitudRetorno modelo)
        {
            var resp = _serviceInventario.CrearSolicitudRetorno(modelo);
            return !string.IsNullOrEmpty(resp) ? Request.CreateResponse(HttpStatusCode.NotFound, resp)
                            : Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("api/Inventario/maill/{to}/{subject}/{mensaje}/{attachmentt}")]
        public string Maill(string to, string subject, string mensaje, string attachmentt)
        {
            string rutaReporte = string.Empty;
            List<string> attachment = new List<string>();

            EnvioMails enviarMails = new EnvioMails();
            rutaReporte = Utilidades.RutaReportes();
            string rutaFinal = rutaReporte + attachmentt + ".xlsx";
            attachment.Add(rutaFinal);
            bool envio = false;

            envio = _serviceInventario.enviarMail(to, subject, mensaje, System.Net.Mail.MailPriority.High, attachment);
            if (envio)
            {
                return rutaFinal;
            }
            else
            {
                return rutaFinal + ", no envio";
            }
        }


    }
}