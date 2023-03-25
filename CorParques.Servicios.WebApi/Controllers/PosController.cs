using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class PosController : ApiController
    {
        private readonly IServicioPos _service;
        private readonly IServicioSerieCodBarra _serviceCodBarra;


        public PosController(IServicioPos service, IServicioSerieCodBarra serviceCodBarra)
        {
            _service = service;
            _serviceCodBarra = serviceCodBarra;
        }

        [HttpGet]
        [Route("api/Pos/ObtenerBrazaleteConsecutivo/{Consecutivo}/{taquillero}/{recarga}")]
        public HttpResponseMessage ObtenerBrazaleteConsecutivo(string Consecutivo,int taquillero, int recarga)
        {
            var item = _service.ObtenerBrazaleteConsecutivo(Consecutivo, taquillero,recarga);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerProducto/{idProducto}")]
        public HttpResponseMessage ObtenerProducto(int idProducto)
        {
            var item = _service.ObtenerProducto(idProducto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/Pos/ObtenerProductoPtoEntrega/{idProducto}")]
        public HttpResponseMessage ObtenerProductoPtoEntrega(int idProducto)
        {
            var item = _service.ObtenerProductoPtoEntrega(idProducto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        

        [HttpGet]
        [Route("api/Pos/ObtenerFactura/{codigoFactura}")]
        public HttpResponseMessage ObtenerFactura(string codigoFactura)
        {
            var item = _service.ObtenerFactura(codigoFactura);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/Pos/ObtenerFacturaById/{idFactura}")]
        public HttpResponseMessage ObtenerFacturaById(int idFactura)
        {
            var item = _service.ObtenerFactura(idFactura);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        [Route("api/Pos/ObtenerProductosDonacion")]
        public HttpResponseMessage ObtenerProductosDonacion()
        {
            var item = _service.ObtenerProductosDonacion();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet, Route("api/Pos/ObtenerProductos")]
        public HttpResponseMessage ObtenerProductos()
        {
            var item = _service.ObtenerProductos();
            return item.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet, Route("api/Pos/ObtenerProductosPtoEntrega")]
        public HttpResponseMessage ObtenerProductosPtoEntrega()
        {
            var item = _service.ObtenerProductosPtoEntrega();
            return item.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        
        [HttpGet, Route("api/Pos/ObtenerListaNotaCredito")]
        public HttpResponseMessage ObtenerListaNotaCredito(int Usuario)
        {
            var item = _service.ObtenerNotaCredito(Usuario);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet, Route("api/Pos/ObtenerListaAnulaciones")]
        public HttpResponseMessage ObtenerAnulaciones(int Usuario)
        {
            var item = _service.ObtenerAnulaciones(Usuario);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet, Route("api/Pos/ObtenerCodBarraId/{id}")]
        public HttpResponseMessage ObtenerSerieCodigoBarra(int id)
        {
            var _rta = _serviceCodBarra.ObtenerSerie(id);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                                 : Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpGet, Route("api/Pos/ObtenerCodBarraConsecutivo/{Consecutivo}")]
        public HttpResponseMessage ObtenerSerieCodigoBarra(string  Consecutivo)
        {
            var _rta = _serviceCodBarra.ObtenerSerie(Consecutivo);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                                 : Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpGet, Route("api/Pos/ObtenerListaProductoBoleta/{CodBarra}/{usuario}")]
        public HttpResponseMessage ObtenerListaProductoBoleta(string CodBarra, int usuario)
        {
            var _rta = _service.ObtenerProductosBoletaControl(CodBarra,usuario);
            return Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpGet, Route("api/Pos/ObtenerProductoInstitucional/{CodBarra}")]
        public HttpResponseMessage ObtenerProductoInstitucional(string CodBarra)
        {
            var _rta = _service.ObtenerProductoInstitucional(CodBarra);
            return Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpGet, Route("api/Pos/ObtenerProductosInstitucional/{CodBarra1}/{CodBarra2}")]
        public HttpResponseMessage ObtenerProductosInstitucional(string CodBarra1, string CodBarra2)
        {
            var _rta = _service.ObtenerProductosInstitucional(CodBarra1, CodBarra2);
            return Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpPost, Route("api/Pos/DescargarProductosInstitucional")]
        public HttpResponseMessage DescargarProductosInstitucional(ImprimirBoletaControl modelo)
        {
            var _rta = _service.DescargarProductosInstitucional(modelo);
            return Request.CreateResponse(HttpStatusCode.OK, _rta);
        }


        [HttpGet, Route("api/Pos/ObtenerListaProductoFactura/{CodBarra}")]
        public HttpResponseMessage ObtenerListaDescargue(string CodBarra)
        {
            var _rta = _service.ObtenerListaDescargue(CodBarra);
            return Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpGet, Route("api/Pos/ObtenerProductosPedidosDia")]
        public HttpResponseMessage ObtenerProductosPedidosDia()
        {
            var _rta = _service.ObtenerProductosPedidosDia();
            return Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpGet, Route("api/Pos/BuscarBoleta/{CodBarraInicio}/{CodBarraFinal}/{Codproducto}/{usuario}")]
        public HttpResponseMessage BuscarBoleta(string CodBarraInicio, string CodBarraFinal, string Codproducto, int usuario)
        {
            var _rta = _service.BuscarBoleta(CodBarraInicio, CodBarraFinal, Codproducto,usuario);
            return Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpGet, Route("api/Pos/ObtenerUltimaFactura/{IdPunto}")]
        public HttpResponseMessage ObtenerListaProductoBoleta(int IdPunto)
        {
            var _rta = _service.ObtenerUltimaFactura(IdPunto);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                                 : Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpGet, Route("api/Pos/ObtenerCortesiaEmpleado/{documento}")]
        public HttpResponseMessage ObtenerCortesiaEmpleado(string documento)
        {
            var _rta = _service.ObtenerCortesiaEmpleado(documento);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpPost, Route("api/Pos/DescargueBoletaFactura")]
        public HttpResponseMessage DescargueBoletaFactura(List<Producto> listaProductos)
        {
            var _rta = _service.DescargueBoletaFactura(listaProductos);
            return string.IsNullOrEmpty(_rta) ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, _rta);
        }


        [HttpPost]
        [Route("api/Pos/InsertarNotaCredito")]
        public HttpResponseMessage GuardarNotaCredito(NotaCredito modelo)
        {
            var item = _service.GuardarNotaCredito(modelo);
            return item == 0? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        //[HttpPost]
        //[Route("api/Pos/ActualizarBrazaleteEstado")]
        //public HttpResponseMessage ActualizarBrazaleteEstado(List<Producto> lista)
        //{
        //    var item = _service.ActualizarBrazaleteEstado(lista);
        //    return item == false ? Request.CreateResponse(HttpStatusCode.NotFound)
        //                    : Request.CreateResponse(HttpStatusCode.OK, item);
        //}

        [HttpPost]
        [Route("api/Pos/InsertarCompra")]
        public HttpResponseMessage InsertarFactura(PagoFactura PagoFactura)
        {
            string strError = string.Empty;

            var item = _service.InsertarCompra(PagoFactura, ref strError);

            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : string.IsNullOrEmpty(strError) ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, strError);
        }
        [HttpPost]
        [Route("api/Pos/UpdatePedidoRestFactura")]
        public HttpResponseMessage UpdatePedidoRestFactura(PagoFactura PagoFactura)
        {
            string strError = string.Empty;

            var item = _service.UpdatePedidoRestFactura(PagoFactura, ref strError);

            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : string.IsNullOrEmpty(strError) ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, strError);
        }

        [HttpPost]
        [Route("api/Pos/InsertarPedidoR")]
        public HttpResponseMessage InsertarPedidoR(PagoFactura PagoFactura)
        {
            string strError = string.Empty;

            var item = _service.InsertarPedidoR(PagoFactura, ref strError);

            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : string.IsNullOrEmpty(strError) ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, strError);
        }
        [HttpPost]
        [Route("api/Pos/AgregarProducAraza")]
        public HttpResponseMessage AgregarProducAraza(PagoFactura PagoFactura)
        {
            string strError = string.Empty;

            var item = _service.AgregarProducAraza(PagoFactura, ref strError);

            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : string.IsNullOrEmpty(strError) ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, strError);
        }
        
        [HttpPost]
        [Route("api/Pos/ActualizarTipoAcompaRestaurante")]
        public HttpResponseMessage ActualizarTipoAcompaRestaurante(TipoAcompanamiento modelo)
        {
            string strError = string.Empty;

            var item = _service.ActualizarTipoAcompaRestaurante(modelo, ref strError);

            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : string.IsNullOrEmpty(strError) ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, strError);
        }

        
        [HttpPost]
        [Route("api/Pos/GuardarAcompaProduAdmin")]
        public HttpResponseMessage GuardarAcompaProduAdmin(ProductoxAcompanamiento ProductoxAcompanamiento)
        {
            string strError = string.Empty;

            var item = _service.GuardarAcompaProduAdmin(ProductoxAcompanamiento, ref strError);

            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : string.IsNullOrEmpty(strError) ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, strError);
        }
        [HttpPost]
        [Route("api/Pos/ActualizarProductoAdminRestaurante")]
        public HttpResponseMessage ActualizarProductoAdminRestaurante(Producto producto)
        {
            string strError = string.Empty;

            var item = _service.ActualizarProductoAdminRestaurante(producto, ref strError);

            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : string.IsNullOrEmpty(strError) ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, strError);
        }
        
        [HttpPost]
        [Route("api/Pos/ValidarCompra")]
        public HttpResponseMessage ValidarCompra(PagoFactura PagoFactura)
        {
            var item = _service.ValidarCompra(PagoFactura);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerLineaProductos")]
        public HttpResponseMessage ObtenerLineaproductos()
        {
            var item = _service.ObtenerLineaproductos();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Pos/ActualizarProducto")]
        public HttpResponseMessage Actualizarproducto(Producto modelo)
        {
            var item = _service.ActualizarProducto(modelo);
            return item == false ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Pos/ActualizarProductoPuntosEntrega")]
        public HttpResponseMessage ActualizarProductoPuntosEntrega(Producto modelo)
        {
            var item = _service.ActualizarProductoPuntosEntrega(modelo);
            return item == false ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        
        [HttpPost]
        [Route("api/Pos/AnularFacturas")]
        public HttpResponseMessage AnularFacturas(IEnumerable<AnulacionFactura> modelo)
        {
            var item = _service.AnularFacturas(modelo);
            return item ? Request.CreateResponse(HttpStatusCode.OK, item) 
                            : Request.CreateResponse(HttpStatusCode.NotFound);
        }
        

        [HttpGet, Route("api/Pos/ValidarTipoFactura/{idFactura}")]
        public HttpResponseMessage ValidarTipoFactura(int idFactura)
        {
            var item = _service.ValidarTipoFactura(idFactura);
            return item.Count() !=0 ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpPost, Route("api/Pos/ObtenerCodBarrasBoletaControl")]
        public HttpResponseMessage ObtenerCodBarrasBoletaControl(ImprimirBoletaControl modelo)
        {
            string error = string.Empty;
            var item = _service.RedencionBoletaControl(modelo);
            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost, Route("api/Pos/GuardarLogVentaContingencia")]
        public HttpResponseMessage GuardarLogVentaContingencia(LogVentaPunto modelo)
        {
            var rta = _service.GuardarLogVenta(modelo);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        [HttpGet]
        [Route("api/Pos/ValidaPermiteAnular/{idPunto}")]
        public HttpResponseMessage ValidaPermiteAnular(int idPunto)
        {
            var item = _service.ValidaPermiteAnular(idPunto);
            return item != null ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("api/Pos/ValidaPermiteAnularUsuario/{IdUsuario}")]
        public HttpResponseMessage ValidaPermiteAnularUsuario(int IdUsuario)
        {
            var item = _service.ValidaPermiteAnularUsuario(IdUsuario);
            return item != null ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerFacturasAnular/{idPunto}")]
        public HttpResponseMessage ObtenerFacturasAnular(int idPunto)
        {
            var item = _service.ObtenerFacturasAnular(idPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerFacturasRedebanAnular/{idPunto}")]
        public HttpResponseMessage ObtenerFacturasRedebanAnular(int idPunto)
        {
            var item = _service.ObtenerFacturasRedebanAnular(idPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerProductoPorTipoProducto/{CodSapTipoProducto}")]
        public HttpResponseMessage ObtenerProductoPorTipoProducto(string CodSapTipoProducto)
        {
            var item = _service.ObtenerProductoPorTipoProducto(CodSapTipoProducto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerCodSapPorTipoProducto/{IdTipoProducto}")]
        public HttpResponseMessage ObtenerCodSapPorTipoProducto(int IdTipoProducto)
        {
            var item = _service.ObtenerCodSapPorTipoProducto(IdTipoProducto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }



        [HttpGet]
        [Route("api/Pos/ValidaReservaParqueadero/{CodigoBarrasBoletaControl}")]
        public HttpResponseMessage ValidaReservaParqueadero(string CodigoBarrasBoletaControl)
        {
            var item = _service.ValidaReservaParqueadero(CodigoBarrasBoletaControl);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerTodosProductos")]
        public HttpResponseMessage ObtenerTodosProductos()
        {
            var item = _service.ObtenerTodosProductos();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerProductosXPuntoSurtido")]
        public HttpResponseMessage ObtenerProductosXPuntoSurtido()
        {
            var item = _service.ObtenerProductosXPuntoSurtido();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerTodosProductosRestaurante")]
        public HttpResponseMessage ObtenerTodosProductosRestaurante()
        {
            var item = _service.ObtenerTodosProductosRestaurante();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerFacturaImprimir/{IdFactura}")]
        public HttpResponseMessage ObtenerFacturaImprimir(int IdFactura)
        {
            var item = _service.ObtenerFacturaImprimir(IdFactura);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerFacturaNotaCredImprimir/{IdFactura}/{IdNotaCredito}")]
        public HttpResponseMessage ObtenerFacturaNotaCredImprimir(int IdFactura, int IdNotaCredito)
        {
            var item = _service.ObtenerFacturaNotaCredImprimir(IdFactura, IdNotaCredito);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerEmpleadoPorConsecutivo/{Consecutivo}")]
        public HttpResponseMessage ObtenerEmpleadoPorConsecutivo(string Consecutivo)
        {
            var item = _service.ObtenerEmpleadoPorConsecutivo(Consecutivo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerPremiosDestrezas")]
        public HttpResponseMessage ObtenerPremiosDestrezas()
        {
            var item = _service.ObtenerPremiosDestrezas();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        [Route("api/Pos/ObtenerPasaportesActivos")]
        public HttpResponseMessage ObtenerPasaportesActivos()
        {
            var item = _service.ObtenerPasaportesActivos();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerPasaportesXPunto/{idPunto}")]
        public HttpResponseMessage ObtenerPasaportesXPunto(int idPunto)
        {
            var item = _service.ObtenerPasaporteXPunto(idPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerPasaporteXPuntoPOS/{idPunto}/{idUsuario}")]
        public HttpResponseMessage ObtenerPasaporteXPuntoPOS(int idPunto, int idUsuario)
        {
            var item = _service.ObtenerPasaporteXPuntoPOS(idPunto, idUsuario);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Pos/ObtenerBrazaleteReimpresion/{idPunto}")]
        public HttpResponseMessage ObtenerBrazaleteReimpresion(int idPunto)
        {
            var item = _service.ObtenerBrazaleteReimpresion(idPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Pos/ActualizarPasaporteXPunto")]
        public HttpResponseMessage ActualizarPasaporteXPunto(List<PuntoBrazalete> modelo)
        {
            var item = _service.ActualizarPasaporteXPunto(modelo);
            return item == false ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Pos/ActualizarArchivoBrazalete")]
        public HttpResponseMessage ActualizarArchivoBrazalete(ArchivoBrazalete modelo)
        {
            var item = _service.ActualizarArchivoBrazalete(modelo);
            return item == false ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpPost, Route("api/Pos/GuardarCortesiaEmpleado")]
        public HttpResponseMessage GuardarCortesiaEmpleado(GuardarCortesiaEmpleado modelo)
        {
            var item = _service.GuardarCortesiaEmpleado(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

          [HttpPost, Route("api/Pos/RegistrarCodigoBoleteriaImpresionLinea")]
        public HttpResponseMessage RegistrarCodigoBoleteriaImpresionLinea(Producto producto)
        {
            var _rta = _service.RegistrarCodigoBoleteria(producto.IdProducto, producto.Precio, producto.IdUsuarioModificacion);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpPost, Route("api/Pos/RegistroRolloImpresionLinea")]
        public HttpResponseMessage RegistroRolloImpresionLinea(Producto productoRollo)
        {
            var _rta = _service.RegistroRolloImpresionLinea(productoRollo);
            return _rta == "Error registrando el rollo" ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpPost, Route("api/Pos/RegistarCompraTienda")]
        public HttpResponseMessage RegistarCompraTienda(FacturaSolicitud facturaSolicitud)
        {
            var _rta = _service.RegistarCompraTienda(facturaSolicitud.productosTienda, facturaSolicitud.mediosPago, facturaSolicitud.IdUsuario, facturaSolicitud.idPunto);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpPost, Route("api/Pos/RegistarReagendamientoTienda")]
        public HttpResponseMessage RegistarReagendamientoTienda(FacturaSolicitud facturaSolicitud)
        {
            var _rta = _service.RegistarReagendamientoTienda(facturaSolicitud.productosTienda, facturaSolicitud.mediosPago, facturaSolicitud.IdUsuario, facturaSolicitud.idPunto);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, _rta);
        }


        [HttpPost]
        [Route("api/Pos/InsertarPlantilla")]
        public HttpResponseMessage InsertarPlantilla(PlantillaBrazalete plantilla)
        {
            string strError = string.Empty;

            var item = _service.InsertarPlantillaBrazalete(plantilla);

            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : string.IsNullOrEmpty(strError) ? Request.CreateResponse(HttpStatusCode.OK, item)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError, strError);
        }



        [HttpGet, Route("api/Pos/ConsultarUsoFactura/{codigoFactura}")]
        public HttpResponseMessage ConsultarUsoFactura(string codigoFactura)
        {
            var _rta = _service.ValidarUsoFactura(codigoFactura);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

        [HttpGet]
        [Route("api/Pos/ConsultarControlBoleteria/{idBoleta}")]
        public HttpResponseMessage ControlBoleteria(int idBoleta)
        { 
          var dato = _service.ControlBoleteria(idBoleta);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound, 0)
                : Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpGet]
        [Route("api/Pos/UpdateControlBoleteria/{idBoleta}/{NumBoletasRestantes}")]
        public HttpResponseMessage ControlBoleteria(int idBoleta, int NumBoletasRestantes)
        {
            var dato = _service.ModificarControlBoleteria( idBoleta, NumBoletasRestantes);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpGet]
        [Route("api/Pos/ValidarImpresionEnLinea/{idBoleteria}")]
        public HttpResponseMessage ValidarImpresionEnLinea(string idBoleteria)
        {
            int idBoleta = int.Parse(idBoleteria);
            var dato = _service.ValidarImpresionEnLinea(idBoleta);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpGet]
        [Route("api/Pos/NumBoletasControlBoleteria")]
        public HttpResponseMessage NumBoletasControlBoleteria()
        {
            var dato = _service.NumBoletasControlBoleteria();
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpGet]
        [Route("api/Pos/VerPasaportesCodigoPedido/{codigoPedido}")]
        public HttpResponseMessage VerPasaportesCodigoPedido(string codigoPedido)
        {
            var dato = _service.VerPasaportesCodigoPedido(codigoPedido);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, dato);
        }
    }
}