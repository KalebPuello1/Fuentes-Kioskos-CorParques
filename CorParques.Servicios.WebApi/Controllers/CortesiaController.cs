using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;
using CorParques.Negocio.Entidades;
using System.Configuration;
using System.IO;
using CorParques.Datos.Contratos;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class CortesiaController : ApiController
    {
        private readonly IServicioCodigoFechaAbierta _serviceCorreo;
        private readonly IServicioCortesia _service;
        public string RutaSoporteCorreos = ConfigurationManager.AppSettings["RutaSoporteCorreos"].ToString();
        IRepositorioParametros _repositorioParametro;

        public CortesiaController(IServicioCortesia service, IRepositorioParametros repositorioParametro, IServicioCodigoFechaAbierta serviceCorreo)
        {
            _service = service;
            _repositorioParametro = repositorioParametro;
            _serviceCorreo = serviceCorreo;
        }


        [HttpPost]
        [Route("api/Cortesia/InsertarCortesia")]
        public HttpResponseMessage InsertarCortesia(Cortesia Cortesia)
        {

            var item = _service.InsertarCortesia(ref Cortesia);
            return item == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Cortesia/InsertarUsuarioVisitante")]
        public HttpResponseMessage InsertarUsuarioVisitante(UsuarioVisitanteViewModel usuarioVisitante)
        {
            usuarioVisitante.FechaCreacion = DateTime.Now;
            usuarioVisitante.FechaActualizacion = DateTime.Now;
            if (usuarioVisitante.RutaSoporte!=null) 
            {
                string pathDestino = RutaSoporteCorreos;
                string pathOrigen = usuarioVisitante.RutaSoporte;
                bool resultadoDestino = System.IO.Directory.Exists(pathDestino);
                bool resultadoOrigen = System.IO.File.Exists(pathOrigen);
                if (resultadoDestino == false)
                {
                    System.IO.Directory.CreateDirectory(RutaSoporteCorreos);
                }
                else
                {
                    if (resultadoOrigen == true)
                    {
                        System.IO.File.Copy(usuarioVisitante.RutaSoporte, RutaSoporteCorreos + usuarioVisitante.Archivo, true);
                    }
                }
            }

            var item = _service.InsertarUsuarioVisitante(ref usuarioVisitante);
            return item == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpPost]
        [Route("api/Cortesia/InsertarCortesiasClienteFanAPP")]
        public HttpResponseMessage InsertarCortesiasClienteFanAPP(ParamCortesiasAPP usuarioVisitante)
        {
            
            

            UsuarioVisitanteViewModel usuario = new UsuarioVisitanteViewModel();
            List<Producto> listaProductosAgregar = new List<Producto>();
            Producto productoagregar = new Producto();
            var ParametrosTipoProdCortesia = _repositorioParametro.ObtenerParametroPorNombre("CodSapproductoCortesiasFan");         
            Parametro objparametro = new Parametro();
            if (ParametrosTipoProdCortesia != null)
            {
                objparametro = ParametrosTipoProdCortesia;

            }


            productoagregar.CodigoSap = objparametro.Valor;
            productoagregar.Cantidad = 1;
            listaProductosAgregar.Add(productoagregar);

            usuario.Correo = usuarioVisitante.CorreoApp;
            usuario.Activo = true;
            usuario.Aprobacion = false;
            usuario.FechaCreacion = System.DateTime.Now;
            usuario.listaProductosAgregar = listaProductosAgregar;
            usuario.Cantidad = 1;
            usuario.BanderaFan = 1;
            usuario.IdTipoCortesia = 3;
            DateTime fechaInicialSet = new DateTime();
            DateTime fechaFinalSet = new DateTime();
            var anomas = System.DateTime.Now.Year + 1;
            var mesActual = System.DateTime.Now.Month;
            if (usuarioVisitante.FechaNaicimientoAPP != null)
            {
                var MesCumple = Convert.ToDateTime(usuarioVisitante.FechaNaicimientoAPP).Month;

                var mesmas = MesCumple + 1;

                if (MesCumple > mesActual)
                {
                    fechaInicialSet = Convert.ToDateTime("01-" + "- " + MesCumple + "- " + System.DateTime.Now.Year);
                    fechaFinalSet = Convert.ToDateTime("01-" + "- " + mesmas + "- " + System.DateTime.Now.Year).AddDays(-1);
                }
                else
                {
                    fechaInicialSet = Convert.ToDateTime("01" + "- " + MesCumple + "- " + anomas);
                    fechaFinalSet = Convert.ToDateTime("01" + "- " + mesmas + "- " + anomas).AddDays(-1);
                }
            }
            usuario.DescripcionBeneficioFAN = "Cortesía cumpleaños";
            usuario.FechaInicialFan = fechaInicialSet;
            usuario.FechaFinalFan = fechaFinalSet;
            var respuestaCortesiaCumple = _service.InsertarUsuarioVisitante(ref usuario);
            usuario.BanderaFan = 2;
            var mesnino = 4;
            if (mesnino > System.DateTime.Now.Month)
            {
                fechaInicialSet = Convert.ToDateTime("01" + "-04-" + System.DateTime.Now.Year);
                fechaFinalSet = Convert.ToDateTime("30" + "-04-" + System.DateTime.Now.Year);
            }
            else
            {
                fechaInicialSet = Convert.ToDateTime("01" + "-04-" + anomas);
                fechaFinalSet = Convert.ToDateTime("30" + "-04-" + anomas);
            }
            usuario.DescripcionBeneficioFAN = "Cortesia día del niño";
            usuario.FechaInicialFan = fechaInicialSet;
            usuario.FechaFinalFan = fechaFinalSet;
            var respuestaCortesiaDianino = _service.InsertarUsuarioVisitante(ref usuario);
            usuario.BanderaFan = 3;
            var meshall = 10;
            if (meshall > System.DateTime.Now.Month)
            {
                fechaInicialSet = Convert.ToDateTime("01" + "-10-" + System.DateTime.Now.Year);
                fechaFinalSet = Convert.ToDateTime("31" + "-10-" + System.DateTime.Now.Year);
            }
            else
            {
                fechaInicialSet = Convert.ToDateTime("01" + "-10-" + anomas);
                fechaFinalSet = Convert.ToDateTime("31" + "-10-" + anomas);
            }
            usuario.DescripcionBeneficioFAN = "Cortesia Halloween";
            usuario.FechaInicialFan = fechaInicialSet;
            usuario.FechaFinalFan = fechaFinalSet;
            var item = _service.InsertarUsuarioVisitante(ref usuario);

            return item == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Cortesia/ObtenerUsuarioVisitante")]
        public HttpResponseMessage ObtenerUsuarioVisitante()
        {

            var item = _service.ObtenerTodosUsuarioVisitante();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/Cortesia/ObtenerComplejidadCortesia")]
        public HttpResponseMessage ObtenerComplejidadCortesia()
        {

            var item = _service.ObtenerComplejidadCortesia();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/Cortesia/ObtenerPlazoCortesia")]
        public HttpResponseMessage ObtenerPlazoCortesia()
        {

            var item = _service.ObtenerPlazoCortesia();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/Cortesia/ObtenerTipoRedencionCortesia")]
        public HttpResponseMessage ObtenerTipoRedencionCortesia()
        {

            var item = _service.ObtenerTipoRedencionCortesia();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Cortesia/ListarCortesias")]
        public HttpResponseMessage ListarCortesias()
        {

            var item = _service.ListarCortesias();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Cortesia/ListarObtenerUsuarioVisitanteCortesias/{Idtipocortesia}")]
        public HttpResponseMessage ListarObtenerUsuarioVisitanteCortesias(int Idtipocortesia)
        {

            var item = _service.ListarObtenerUsuarioVisitanteCortesias(Idtipocortesia);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/Cortesia/AprobacionCortesia/{IdCortesia}")]
        public HttpResponseMessage AprobacionCortesia(int IdCortesia)
        {
            var item = _service.AprobacionCortesia(IdCortesia);
            return item == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                          : Request.CreateResponse(HttpStatusCode.OK, item.ToString());
           

        }


        [HttpPost]
        public HttpResponseMessage ObtenerCortesiaUsuarioVisitante(CortesiaViewModel cortesia)
        {
            var _rta = _service.ObtenerCortesiaUsuarioVisitante(cortesia.documento, cortesia.numTarjeta, cortesia.correoApp, cortesia.documentoEjecutivo);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpGet]
        [Route("api/Cortesia/ObtenerCortesiaUsuarioVisitante2/{documento}")]
        public HttpResponseMessage ObtenerCortesiaUsuarioVisitante2(string documento)
        {
            var _rta = _service.ObtenerCortesiaUsuarioVisitante(documento, "", "", "");
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpGet]
        [Route("api/Cortesia/ListarCategoriaImagenes")]
        public HttpResponseMessage ListarCategoriaImagenes()
        {
            var _rta = _service.ListarCategoriaImagenes();
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpPost, Route("api/Cortesia/SolicitudCodConfirmacion")]
        public HttpResponseMessage SolicitudCodConfirmacion(SolicitudCodigo modelo)
        {
            var guid = Guid.NewGuid();
            var justNumbers = new String(guid.ToString().Where(Char.IsDigit).ToArray());
            var seed = int.Parse(justNumbers.Substring(0, 5));
            modelo.Consecutivo = seed;
            var item = _service.SolicitudCodConfirmacion(modelo);

            if (item != null)
            {
                var probando = _serviceCorreo.EnviarCorreoCodConfirmacion(modelo.Correo, modelo.Consecutivo.ToString());
            }
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Cortesia/ConsultarCodConfirmacion/{Consecutivo}")]
        public HttpResponseMessage ConsultarCodConfirmacion(int Consecutivo)
        {
            var tiempoVigenciaCodigo = _repositorioParametro.ObtenerParametroPorNombre("PartiempoVigenciaCodigo");
            var _rta = _service.ConsultarCodConfirmacion(Consecutivo, tiempoVigenciaCodigo.Valor);

            if (_rta != null )
            {
                DateTime fechaInicialSet = new DateTime();
                fechaInicialSet = System.DateTime.Now;
                double minutos = 0;
                if (tiempoVigenciaCodigo != null)
                {
                    minutos = Convert.ToInt32(tiempoVigenciaCodigo.Valor);
                }
                fechaInicialSet = fechaInicialSet.AddMinutes(-minutos);
                if (_rta.FechaGeneracion > fechaInicialSet)
                {
                    _rta.RespuestaPeticion = "Codigo valido";
                }
                else
                {
                    _rta = new SolicitudCodigo();
                    _rta.RespuestaPeticion = "Codigo invalido";
                }
                string docu = "";

            }
            else
            {
                _rta = new SolicitudCodigo();
                _rta.RespuestaPeticion = "Codigo invalido";
            }
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpGet]
        [Route("api/Cortesia/ListarMensajesVisual")]
        public HttpResponseMessage ListarMensajesVisual()
        {
            var _rta = _service.ListarMensajesVisual();
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpGet]
        [Route("api/Cortesia/ObtenerMensajesVisualXCod/{Codigo}")]
        public HttpResponseMessage ObtenerMensajesVisualXCod(string Codigo)
        {
            var _rta = _service.ObtenerMensajesVisualXCod(Codigo);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpGet]
        [Route("api/Cortesia/ObtenerInfoMensajeXCod/{Codigo}")]
        public HttpResponseMessage ObtenerInfoMensajeXCod(string Codigo)
        {
            var _rta1 = _service.ObtenerMensajesVisualXCod(Codigo);
            var _rta = new MensajesVisual();
            if (_rta != null)
            {
                _rta = _rta1.FirstOrDefault();
            }
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpPost, Route("api/Cortesia/AgregarMensajesVisual")]
        public HttpResponseMessage AgregarMensajesVisual(MensajesVisual modelo)
        {
            var item = _service.AgregarMensajesVisual(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost, Route("api/Cortesia/ActualizarMensajesVisual")]
        public HttpResponseMessage ActualizarMensajesVisual(MensajesVisual modelo)
        {
            var item = _service.ActualizarMensajesVisual(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Cortesia/EliminarMensajesVisual/{Codigo}")]
        public HttpResponseMessage EliminarMensajesVisual(string Codigo)
        {
            var _rta = _service.EliminarMensajesVisual(Codigo);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpGet]
        [Route("api/Cortesia/ListarVisualPantallas/{IdCategoria}")]
        public HttpResponseMessage ListarVisualPantallas(int IdCategoria)
        {
            var _rta = _service.ListarVisualPantallas(IdCategoria);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpGet]
        [Route("api/Cortesia/ObtenerImagenAdminXCodpantalla/{CodPantalla}")]
        public HttpResponseMessage ObtenerImagenAdminXCodpantalla(string CodPantalla)
        {
            var _rta = _service.ObtenerImagenAdminXCodpantalla(CodPantalla);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpGet]
        [Route("api/Cortesia/ObtenerImagenXCodpantalla/{CodPantalla}")]
        public HttpResponseMessage ObtenerImagenXCodpantalla(string CodPantalla)
        {
            var _rta1 = _service.ObtenerImagenAdminXCodpantalla(CodPantalla);

            var _rta = new ImagenAdmin();
            if (_rta != null)
            {
                _rta = _rta1.FirstOrDefault();
            }
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpPost]
        [Route("api/Cortesia/ObtenerCortesiasClienteFanAPP")]
        public HttpResponseMessage ObtenerCortesiasClienteFanAPP(CortesiaViewModel cortesia)
        {
            var _rta = _service.ObtenerCortesiaUsuarioVisitante("", "", cortesia.correoApp, "");
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta.ListDetalleCortesia);

        }
        [HttpGet]
        [Route("api/Cortesia/ObtenerCortesiaUsuarioVisitanteEjecutivo/{documento}")]
        public HttpResponseMessage ObtenerCortesiaUsuarioVisitanteEjecutivo(string documento)
        {
            var _rta = _service.ObtenerCortesiaUsuarioVisitante("", "", "",documento);
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }

        [HttpGet]
        [Route("api/Cortesia/ObtenerCortesiaTarjetaFAN/{numTarjeta}")]
        public HttpResponseMessage ObtenerCortesiaTarjetaFAN(string numTarjeta)
        {
            var _rta = _service.ObtenerCortesiaUsuarioVisitante("", numTarjeta, "", "");
            return _rta == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                     : Request.CreateResponse(HttpStatusCode.OK, _rta);

        }


        [HttpPost, Route("api/Cortesia/GuardarCortesiaUsuarioVisitante")]
        public HttpResponseMessage GuardarCortesiaUsuarioVisitante(GuardarCortesiaUsuarioVisitante modelo)
        {
            var item = _service.GuardarCortesiaUsuarioVisitante(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost, Route("api/Cortesia/ActualizarAdminImagenes")]
        public HttpResponseMessage ActualizarAdminImagenes(ImagenAdmin modelo)
        {
            var item = _service.ActualizarAdminImagenes(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost, Route("api/Cortesia/GuardarCortesiaClienteFanAPP")]
        public HttpResponseMessage GuardarCortesiaClienteFanAPP(List<DetalleCortesia>  productos)
        {
            var obj = new GuardarCortesiaUsuarioVisitante()
            {
                ListaProductosAPP = productos,
                TipoCortesia = 3
            };

            var item = _service.GuardarCortesiaUsuarioVisitante(obj);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [AllowAnonymous]
        [HttpPost, Route("api/Cortesia/GuardarArchivoTemporal")]
        public void GuardarArchivoTemporal(System.Web.HttpPostedFileWrapper Files)
        {
            var fileName = Path.GetFileName(Files.FileName);
            var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Archivos/tempSoportes/" + fileName);
            Files.SaveAs(path);
        }

        [AllowAnonymous]
        [HttpGet, Route("api/Cortesia/RemoverArchivoTemporal/{name}")]
        public void RemoverArchivoTemporal(string name)
        {

            var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Archivos/tempSoportes/" + name);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

    }
}