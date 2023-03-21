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
    public class ReporteMovimientoInventarioController : ApiController
    {
        private readonly IServicioReporteMovimientoInventario _servicio;

        public ReporteMovimientoInventarioController(IServicioReporteMovimientoInventario servicio)
        {
            _servicio = servicio;
        }


        [HttpGet, Route("api/ReporteMovimientoInventario/ObtenerUnidadMedida")]
        public HttpResponseMessage ObtenerUnidadMedida()
        {
            var rta = _servicio.ObtenerUnidadMedida();
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpGet, Route("api/ReporteMovimientoInventario/ObtenerTipoMovimiento")]
        public HttpResponseMessage ObtenerTipoMovimiento()
        {
            var rta = _servicio.ObtenerTiposMovimiento();
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpGet]
        [Route("api/ReporteMovimientoInventario/ObtenerReporte/{FechaIni}/{FechaFin}/{CodMaterial}/{IdMovimiento}/{IdPtoOrigen}/{IdPtoDestino}/{IdReponsable}/{UnidadMedida}")]
        public HttpResponseMessage ObtenerInformeMovimiento(string FechaIni, string FechaFin,
            string CodMaterial, int IdMovimiento, string IdPtoOrigen, string IdPtoDestino, int IdReponsable, string UnidadMedida)
        {
            var obj = new ReporteMovimientoInventario();

            obj.FechaInicial = FechaIni == "null" ? null : FechaIni;
            obj.FechaFinal = FechaFin == "null" ? null : FechaFin;
            obj.CodigoMaterial = CodMaterial == "null" ? null : CodMaterial;
            obj.IdTipoMovimiento = IdMovimiento;
            //obj.IdPuntoOrigen = IdPtoOrigen;
            // --------> obj.IdPuntoOrigen = 209;
            //obj.PuntoDestino = IdPtoDestino;
            // --------> obj.PuntoDestino = 209;
            obj.IdPersonaResponsable = IdReponsable;
            obj.CodSapAlmacenOrigen = IdPtoOrigen == "null" ? null: IdPtoOrigen;
            obj.CodSapAlmacenDestino = IdPtoDestino == "null"? null : IdPtoDestino;
            obj.UnidadMedida = UnidadMedida == "null" ? null : UnidadMedida;
            var rta = _servicio.ObtenerReporte(obj);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpGet]
        [Route("api/ReportePedidoRestaurante/ObtenerReporteRestaurante/{FechaIni}/{FechaFin}/{IdVendedor}/{CodSapAlmacenOrigen}/{IdZona}/{IdMesa}/{IdEstadoMesa}")]
        public HttpResponseMessage ObtenerReporteRestaurante(string FechaIni, string FechaFin,
         int IdVendedor, string CodSapAlmacenOrigen, int IdZona, int IdMesa, int IdEstadoMesa)
        {
            var obj = new ReportePedidoRestaurante();

            obj.FechaInicial = FechaIni == "null" ? null : FechaIni;
            obj.FechaFinal = FechaFin == "null" ? null : FechaFin;
            obj.CodSapAlmacenOrigen = CodSapAlmacenOrigen == "null" ? null : CodSapAlmacenOrigen;
            obj.Id_Vendedor = IdVendedor;
            //obj.IdPuntoOrigen = IdPtoOrigen;
            // --------> obj.IdPuntoOrigen = 209;
            //obj.PuntoDestino = IdPtoDestino;
            // --------> obj.PuntoDestino = 209;
            obj.IdZona = IdZona;
            obj.Id_Mesa = IdMesa;
            obj.IdEstadoMesa = IdEstadoMesa; 
            var rta = _servicio.ObtenerReporteRestaurante(obj);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpGet]
        [Route("api/ReporteBonoRegalo/ObtenerReporteBonoRegalo/{FechaInicialP}/{FechaFinalP}/{IdTipoPedido}/{CodCliente}/{CodVendedor}/{CodPedido}")]
        public HttpResponseMessage ObtenerReporteBonoRegalo(string FechaInicialP, string FechaFinalP,
        int? IdTipoPedido, string CodCliente, string CodVendedor, string CodPedido)
        {
            var obj = new ReporteBonoRegalo();

            obj.FechaInicialP = FechaInicialP == "null" ? null : FechaInicialP;
            obj.FechaFinalP = FechaFinalP == "null" ? null : FechaFinalP;
            obj.IdTipoPedido = IdTipoPedido;
            obj.CodCliente = CodCliente == "null" ? null : CodCliente;
            obj.CodVendedor = CodVendedor == "null" ? null : CodVendedor;
            obj.CodPedido = CodPedido == "null" ? null : CodPedido;
           
            var rta = _servicio.ObtenerReporteBonoRegalo(obj);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }

    }
}