using System.Linq;
using CorParques.Negocio.Contratos;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using CorParques.Transversales.Util;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class CodigoFechaAbiertaController : ApiController
    {
        private readonly IServicioCodigoFechaAbierta _service;
        private readonly IServicioCortesiaPunto serv;

        public CodigoFechaAbiertaController(IServicioCodigoFechaAbierta service, IServicioCortesiaPunto servi)
        {
            _service = service;
            serv = servi;
        }

        //NO ESTAN LLEGANDO LOS DATOS
        [HttpGet]
        [Route("api/CodigoFechaAbierta/VerFacturas/{IdDestreza}/{IdAtraccion}")]
        public HttpResponseMessage VerFacturas(string IdDestreza, int? IdAtraccion)
        {
            int? nm = null;
            nm = IdAtraccion == 1 ? nm : 0;
            var list = _service.VerFacturas(IdDestreza, nm);
            System.Console.WriteLine(list);
            /**
             * 
             */
            //return list;
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                             : Request.CreateResponse(HttpStatusCode.OK, list);
        }



        [HttpGet]
        [Route("api/CodigoFechaAbierta/ObtenerTodo/{CodSapPedido}")]
        public HttpResponseMessage ObtenerTodos(string CodSapPedido)
        {
            var _list = _service.ObtenerTodos(CodSapPedido);
            return _list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, _list);
        }

        [HttpGet]
        [Route("api/CodigoFechaAbierta/ObtenerId/{id}")]
        public HttpResponseMessage ObtenerId(int id)
        {
            var list = _service.ObtenerPorId(id);
            return list.IdPedidoBoletaControl == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                 : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/CodigoFechaAbierta/Editar")]
        public HttpResponseMessage editar(CodigoFechaAbierta c)
        {
            string datoo;
            var dato = _service.Actualizar(c, out datoo);
            return dato ? Request.CreateResponse(HttpStatusCode.OK, "")
                : Request.CreateResponse(HttpStatusCode.NotFound, datoo);
        }

        [HttpGet]
        [Route("api/CodigoFechaAbierta/EnviarQR/{envQR}/{pathQR}")]
        public HttpResponseMessage EnviarQR(string envQR, string pathQR)
        {
            string qr = envQR.Replace("|", ".");
            string VpathQR = $"D:{pathQR.Replace("||", "\\")}.Jpeg";
            string dato = _service.EnviarQRCorreo(qr, VpathQR);
            return string.IsNullOrEmpty(dato) ? Request.CreateResponse(HttpStatusCode.NotFound) : 
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpGet]
        [Route("api/CodigoFechaAbierta/EnviarCorreoCodConfirmacion/{envQR}/{CodConfirma}")]
        public HttpResponseMessage EnviarCorreoCodConfirmacion(string envQR, string CodConfirma)
        {
            string qr = envQR.Replace("|", ".");
            string dato = _service.EnviarCorreoCodConfirmacion(qr, CodConfirma);
            return string.IsNullOrEmpty(dato) ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpPost]
        [Route("api/CodigoFechaAbierta/EnviarUsuario")]
        public HttpResponseMessage EnviarUsuario(CodigoFechaAbierta datosAventurita)
        {
            ReportePDF pdfEnviar = new ReportePDF();
            pdfEnviar.MuestraDato("entro CodigoFechaAbierta/EnviarUsuario");
            string dato = _service.EnviarUsuario(datosAventurita);
            pdfEnviar.MuestraDato("Paso por el CodigoFechaAbierta/EnviarUsuario");   
            return string.IsNullOrEmpty(dato) ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        #region Test
        [HttpGet]
        [Route("api/CodigoFechaAbierta/testt/{IdDestreza}/{IdAtraccion}")]
        public HttpResponseMessage test(int IdDestreza, int IdAtraccion)
        {
            var list = _service.test(IdDestreza, IdAtraccion);
            /**
             * 
             */
            //return list;
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                             : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /* [HttpGet]
       [Route("api/CodigoFechaAbierta/ObtenerPorDestrezaAtraccion/{IdDestreza}/{IdAtraccion}")]
       public HttpResponseMessage ObtenerLista(int IdDestreza, int IdAtraccion)
       {
           var list = _service.ObtenerPorDestrezaAtraccion(IdDestreza, IdAtraccion);
           return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
       }*/
        #endregion

        [HttpGet]
        [Route("api/CodigoFechaAbierta/VerPedidosClienteExterno/{pedido}/")]
        public HttpResponseMessage VerPedidosClienteExterno(string pedido)
        {
            var dato = _service.VerPedidosClienteExterno(pedido);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        [HttpGet]
        [Route("api/CodigoFechaAbierta/VerPedidosClienteExternoMultiple/{pedido}/")]
        public HttpResponseMessage VerPedidosClienteExternoMultiple(string pedido)
        {
            var dato = _service.VerPedidosClienteExternoMultiple(pedido);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        
        [HttpGet]
        [Route("api/CodigoFechaAbierta/verPedidosPorIdCliente/{IdCliente}")]
        public HttpResponseMessage verPedidosPorIdCliente(string IdCliente)
        {
            var dato = _service.verPedidosPorIdCliente(IdCliente);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        [HttpGet]
        //[Route("api/CodigoFechaAbierta/UpdatePedidosClienteExterno/{pedido}/{numProductos}/{codSapProducto}")]
        //[Route("api/CodigoFechaAbierta/UpdatePedidosClienteExterno/{pedido}/{numProductos}")]
        [Route("api/CodigoFechaAbierta/UpdatePedidosClienteExterno/{pedidoo}/{numProductoss}")]
        //public HttpResponseMessage UpdatePedidosClienteExterno(string pedido, int numProductos, string codSapProducto)
        public HttpResponseMessage UpdatePedidosClienteExterno(string pedidoo, int numProductoss)
        {
            var dato = _service.UpdatePedidosClienteExterno(pedidoo, numProductoss);
            ReportePDF Rpdf = new ReportePDF();
            Rpdf.log("entro al api UpdatePedidosClienteExterno");
            Rpdf.repositorio("finalizo modificar ****** " + pedidoo + " " + numProductoss);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato); 
        }

        [HttpPost]
        [Route("api/CodigoFechaAbierta/InsertarAsigancionBoleta")]
        public HttpResponseMessage InsertarAsigancionBoleta(IEnumerable<CodigoFechaAbierta> FechaAbierta)
        {
            var dato = _service.InsertarAsignacionBoleta(FechaAbierta);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpGet]
        [Route("api/CodigoFechaAbierta/CorreoAsigancionBoleta/{pedido}/{numProductos}/{codSapProducto}")]
        public HttpResponseMessage CorreoAsigancionBoleta(string pedido, int numProductos, string codSapProducto)
        {
            var dato = _service.CorreoAsigancionBoleta(pedido, numProductos, codSapProducto);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpPost]
        [Route("api/CodigoFechaAbierta/UpdateFechaBoletas")]
        public HttpResponseMessage UpdateFechaBoletas(CodigoFechaAbierta item)
        {
            var cod = item.CodSapPedido;
            var dato = _service.UpdateFechaBoletas(item.Consecutivo, cod, item.Valor ,item.FechaInicial, item.FechaFinal);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        [HttpPost]
        [Route("api/CodigoFechaAbierta/InsertarBoleteriaCodigosExternos")]
        public HttpResponseMessage InsertarBoleteriaCodigosExternos(CodigoFechaAbierta item)
        {
            var cod = item.CodSapPedido;
            var dato = _service.InsertarBoleteriaCodigosExternos(item);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }

        [HttpGet]
        [Route("api/CodigoFechaAbierta/ObteneridCliente/{codPedido}")]
        public HttpResponseMessage ObteneridCliente(string codPedido)
        {
            var dato = _service.ObteneridCliente(codPedido);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        //ObteneridCliente

        [HttpGet]
        [Route("api/CodigoFechaAbierta/ObtenerPedidosPorUsuairo/{idUsuario}/{NomUsuario}")]
        public HttpResponseMessage ObtenerPedidosPorUsuairo(string idUsuario, string NomUsuario)
        {
            var dato = _service.ObtenerPedidosPorUsuario(idUsuario, NomUsuario);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        [HttpPost]
        [Route("api/CodigoFechaAbierta/InsertarPedidoUsuarioExterno")]
        public HttpResponseMessage InsertarPedidoUsuarioExterno(Negocio.Entidades.PedidosPorCliente usu)
        {
            var dato = _service.updatePedidoUsuarioExterno(usu.Pedido, usu.IdUsuario, usu.IdCliente);
            return dato == "fallo" ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        [HttpPost]
        [Route("api/CodigoFechaAbierta/intentoImgBD")]
        public HttpResponseMessage intentoImgBD(CodigoFechaAbierta cod)
        {
            var dato = _service.intentoImgBD(cod.rtaLogo, cod.CodCliente);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        //[HttpPost]
        [HttpGet]
        //[Route("api/CodigoFechaAbierta/ObtenerLogoCliente")]
        [Route("api/CodigoFechaAbierta/ObtenerLogoCliente/{IdSAPCliente}")]
        //public HttpResponseMessage ObtenerLogoCliente(CodigoFechaAbierta cod)
        public HttpResponseMessage ObtenerLogoCliente(string IdSAPCliente)
        {
            ReportePDF Rpdf = new ReportePDF();
            Rpdf.log("ObtenerLogoCliente");
            var dato = _service.ObtenerLogoCliente(IdSAPCliente);
            return dato == null ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }
    }
}