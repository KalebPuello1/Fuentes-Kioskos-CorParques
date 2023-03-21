using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class VyucoomController : ApiController
    {
        #region properties
        public readonly IServicioVuycoom serviVuycoom;
        #endregion

        #region constructor
        public VyucoomController(IServicioVuycoom servi)
        {
            serviVuycoom = servi;
        }
        #endregion

        [HttpGet]
        [Route("api/vuycoom/factura")]
        public HttpResponseMessage factura()
        {
            IEnumerable<Factura> _list = serviVuycoom.facturas();
            return _list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, _list);
        }

        //Inserta el dato segun el IdFactura de TB_Factura
        [HttpPost]
        [Route("api/vuycoom/InsertaDato")]
        public string InsertaDato(Vyucoom vyucoom)
        {
            var data = serviVuycoom.InsertarDato(vyucoom);
            return data;
        }

        //Inserta en TB_Factura y trae el idFactura
        /*[HttpGet]
        [Route("api/vuycoom/InsertarFacturaVer/{lol}")]
        public HttpResponseMessage insertarFacturaVer(string lol)
        {
            var data = serviVuycoom.InsertarFactura(lol);
            return data == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, data); ;
        }*/

        [HttpPost]
        [Route("api/vuycoom/InsertarFacturaVer")]
        public HttpResponseMessage insertarFacturaVer(Vyucoom lol)
        {
            var data = serviVuycoom.InsertarFactura(lol);
            return data == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, data); ;
        }


        //getNumeroFactuar
        [HttpGet]
        [Route("api/vuycoom/getNumeroFactura")]
        public int getNumeroFactura()
        {
            var dataNum = serviVuycoom.getNumeroFactuar();
            return dataNum;
        }

        [HttpGet]
        [Route("api/vuycoom/PrecioFijo")]
        public HttpResponseMessage BuscarPrecioFijo()
        {
            Parametro precioFijo = serviVuycoom.BuscarPrecioFijo();
            return precioFijo.Nombre == "" ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, precioFijo);
        }


        [HttpPut]
        [Route("api/vyucoom/EditarPagoReciboCaja")]
        public HttpResponseMessage ModificarPagoReciboCaja(Vyucoom cliente)
        {
            var dato = serviVuycoom.ModificarPagoReciboCaja(cliente);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpGet]
        [Route("api/vuycoom/Pedido/{dato}")]
        public HttpResponseMessage Pedido(string dato)
        {
            int pedido = serviVuycoom.pedido(dato);
            return pedido == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, pedido);
        }

        [HttpGet]
        [Route("api/vuycoom/PedidoCliente/{dato}")]
        public HttpResponseMessage getClienteNuevo(string dato)
        {
            var dat = serviVuycoom.getClienteNuevo(dato);
            return dat == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, dat);
        }

        [HttpGet]
        [Route("api/vuycoom/NCliPedido/{dato}")]
        public HttpResponseMessage NCliPedido(string dato)
        {
            string pedido = serviVuycoom.NCliPedido(dato);
            return pedido == "" ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, pedido);
        }
        
        [HttpPost]
        [Route("api/vuycoom/InsertarRecCaja")]
        public HttpResponseMessage InsertarRecCaja(Vyucoom datoReciboCaja)
        {
            string pedido = serviVuycoom.InsertarRecCaja(datoReciboCaja);
            return pedido == "" ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, pedido);
        }

        /*[HttpGet]
        [Route("api/vyucoom/PedidoCliente/{dato}")]
        public HttpResponseMessage PedidoCliente()
        {
            string dato = "";
            return dato == "" ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, dato);
        }*/

        [HttpGet]
        [Route("api/vuycoom/Reimprimir/{Npedido}")]
        public HttpResponseMessage Reimprimir(string Npedido)
        {
            IEnumerable<Vyucoom>[] pedido = serviVuycoom.Reimprimir(Npedido);
            return pedido.Length == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, pedido);
        }
        
        [HttpGet]
        [Route("api/vuycoom/UsuarioReimprimir/{dato}")]
        public HttpResponseMessage UsuarioReciboCajaVyucoom(string dato)
        {
            //SP_ObtenerUsuarioReciboCajaVyucoom
            Vyucoom pedido = serviVuycoom.UsuarioReciboCajaVyucoom(dato);
            return pedido.Cliente == "" ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, pedido);
        }

        [HttpGet]
        [Route("api/vuycoom/UsuarioPorId/{id}")]
        public HttpResponseMessage UsuarioPorId(string id)
        {
            var datoUsu = serviVuycoom.UsuarioPorId(id);
            return datoUsu == "" ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, datoUsu);
        }
        
        [HttpGet]
        [Route("api/vuycoom/PuntoPorId/{id}")]
        public HttpResponseMessage PuntoPorId(string id)
        {
            var datoPunto = serviVuycoom.PuntoPorId(id);
            return datoPunto == "" ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, datoPunto);
        }

        //Pedido
    }
}
