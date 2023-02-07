//Cambioquitar: Este controlador usa el enumerador de perfiles.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using CorParques.Transversales.Contratos;
using System.Data;
using System.Web.Script.Serialization;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class RecoleccionNidoController : ControladorBase
    {
        private readonly IServicioImprimir _service;

        public RecoleccionNidoController(IServicioImprimir service)
        {
            _service = service;
        }
        #region Propiedades


        public int Punto { get; set; }

        public int IdUsuario
        {
            get { return (Session["UsuarioAutenticado"] as Usuario).Id; }
        }

        #endregion

        #region Metodos

        public async Task<ActionResult> Index()
        {
            //string strPerfiles = (int)Enumerador.Perfiles.Supervisor + "," + (int)Enumerador.Perfiles.Recolector;
            Session["recoleccionesAlmacenarNido"] = null;
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesRecoleccionNido");
            strPerfiles = objParametro.Valor;

            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
            ViewBag.supervisores = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });

            var listaPuntos = await GetAsync<IEnumerable<TipoGeneralValor>>($"Recoleccion/ObtenerPuntosRecoleccion/{(int)Enumerador.Estados.EntregadoSupervisor}/{0}");
            ViewBag.Puntos = listaPuntos.Where(x => x.Valor != null);
            return View("_Preview");
        }

        public async Task<ActionResult> ObtenerDatosRecoleccion(int IdSupervisor, int IdPunto, int IdRecoleccion)
        {
            //-----------------------------------------------------------Mauricio Tellez--------------------------------------------------------------------------------
            var listaPuntos = await GetAsync<IEnumerable<TipoGeneralValor>>($"Recoleccion/ObtenerPuntosRecoleccion/{(int)Enumerador.Estados.EntregadoSupervisor}/{0}");
            ViewBag.Puntos = listaPuntos.Where(x => x.Valor != null);
            //----------------------------------------------------------------------------------------------------------------------------------------------------------

            ViewBag.IdUsuarioModificacion = IdSupervisor;
            List<Recoleccion> objRecoleccion = new List<Recoleccion>();
            Recoleccion _recoleccion = new Recoleccion();
            ObservacionRecoleccion objObservacion = new ObservacionRecoleccion();

            //var user = await GetAsync<Usuario>($"Usuario/GetById?id={IdSupervisor}&Punto={0}");
            var lista = await GetAsync<IEnumerable<Usuario>>("Usuario/GetAll");
            var usuario = lista.Where(x => x.Id == IdSupervisor).ToList().Select(x => x.NombreUsuario).SingleOrDefault();
            ViewBag.Usuario = usuario;
            var IdEstado = (int)Enumerador.Estados.EntregadoSupervisor;
            //objRecoleccion = await GetAsync<Recoleccion>($"RecoleccionSupervisor/ObtenerRecoleccionActiva/{IdSupervisor}/{IdPunto}/{IdEstado}");
            objRecoleccion = await GetAsync<List<Recoleccion>>($"RecoleccionSupervisor/ObtenerRecoleccionesActivas/{IdSupervisor}/{IdPunto}/{0}/{IdEstado}");
            IEnumerable<DetalleRecoleccionMonetaria> Base = null;
            IEnumerable<DetalleRecoleccionMonetaria> Corte = null;
            IEnumerable<DetalleRecoleccionDocumento> Voucher = null;
            IEnumerable<DetalleRecoleccionDocumento> Documento = null;
            int[] IdRecolecciones = null;
            string[] FechaRecolecciones = null;

            if (objRecoleccion != null)
            {
                IdRecolecciones = new int[objRecoleccion.Count()];
                FechaRecolecciones =  new string[objRecoleccion.Count()];
                var listaRecoleccion = objRecoleccion.OrderBy(xx => xx.IdRecoleccion).ToList();
                for (var i = 0; i < IdRecolecciones.Length; i += 1)
                {                    
                    IdRecolecciones[i] = listaRecoleccion[i].IdRecoleccion;
                    FechaRecolecciones[i] = listaRecoleccion[i].FechaCreacion.ToString("hh:mm tt");
                }

                foreach (var item in objRecoleccion)
                {

                    var listaTipoDenominacion = await TipoDenominacion();
                    item.TipoDenominacion = listaTipoDenominacion;
                    item.DocumentosRecoleccion = await DocumentosRecoleccion((int)item.IdUsuarioCreacion);
                    objObservacion.Observacion = "";
                    item.objObservaciones = objObservacion;
                    Base = item.RecoleccionBase.Where(x => x.IdEstado == (int)Enumerador.Estados.EntregadoSupervisor).ToList();
                    Corte = item.RecoleccionCorte.Where(x => x.IdEstado == (int)Enumerador.Estados.EntregadoSupervisor).ToList();
                    item.NovedadesArqueo = await NovedadesArqueo(IdSupervisor, item.IdRecoleccion);
                    //Voucher = objRecoleccion.RecoleccionVoucher.Where(x => x.IdEstado == (int)Enumerador.Estados.EntregadoSupervisor).ToList();
                    //Documento = objRecoleccion.RecoleccionDocumentos.Where(x => x.IdEstado == (int)Enumerador.Estados.EntregadoSupervisor).ToList();
                    if (item.DocumentosRecoleccion != null)
                    {
                        Voucher = item.RecoleccionVoucher.Where(x => x.IdEstado == IdEstado && x.RevisionSupervisor == true).ToList();
                    }

                    //foreach (var doc in item.DocumentosRecoleccion)
                    //{
                    //    if (item.RecoleccionVoucher.Where(x => x.IdEstado == IdEstado && x.IdMedioPagoFactura == doc.IdMedioPagoFactura).ToList().Count >0)
                    //    {
                    //        Voucher = item.RecoleccionVoucher.Where(x => x.IdEstado == IdEstado && x.IdMedioPagoFactura == doc.IdMedioPagoFactura).ToList();
                    //    }

                    //}
                    if (item.DocumentosRecoleccion != null)
                    {
                        Documento = item.RecoleccionDocumentos.Where(x => x.IdEstado == IdEstado && x.RevisionSupervisor == true).ToList();
                    }
                    //foreach (var doc in item.DocumentosRecoleccion)
                    //{
                    //    if (item.RecoleccionDocumentos.Where(x => x.IdEstado == IdEstado && x.IdMedioPagoFactura == doc.IdMedioPagoFactura).ToList().Count > 0)
                    //    {
                    //        Documento = item.RecoleccionDocumentos.Where(x => x.IdEstado == IdEstado && x.IdMedioPagoFactura == doc.IdMedioPagoFactura).ToList();                            
                    //    }                        
                    //}
                    item.RecoleccionBase = Base;
                    item.RecoleccionCorte = Corte;
                    item.RecoleccionVoucher = Voucher;
                    item.RecoleccionDocumentos = Documento;
                    item.IdUsuarioModificacion = IdUsuario;
                    item.IdUsuarioNido = IdUsuario;
                    //asigna el numero de cada sobre
                    if (item.RecoleccionBase != null && item.RecoleccionBase.Count() > 0)
                    {
                        item.SobreBilletesBase = item.RecoleccionBase.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base && xx.Tipo == Enumerador.TipoDenominacion.Billete.ToString()).FirstOrDefault() != null ?
                            item.RecoleccionBase.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base && xx.Tipo == Enumerador.TipoDenominacion.Billete.ToString()).FirstOrDefault().NumeroSobre : null;
                        item.SobreMonedasBase = item.RecoleccionBase.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base && xx.Tipo == Enumerador.TipoDenominacion.Moneda.ToString()).FirstOrDefault() != null ?
                            item.RecoleccionBase.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base && xx.Tipo == Enumerador.TipoDenominacion.Moneda.ToString()).FirstOrDefault().NumeroSobre : null;
                    }
                    if (item.RecoleccionCorte != null && item.RecoleccionCorte.Count() > 0)
                    {
                        item.SobreBilletesCorte = item.RecoleccionCorte.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte && xx.Tipo == Enumerador.TipoDenominacion.Billete.ToString()).FirstOrDefault() != null ?
                            item.RecoleccionCorte.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte && xx.Tipo == Enumerador.TipoDenominacion.Billete.ToString()).FirstOrDefault().NumeroSobre : null;
                        item.SobreMonedasCorte = item.RecoleccionCorte.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte && xx.Tipo == Enumerador.TipoDenominacion.Moneda.ToString()).FirstOrDefault() != null ?
                            item.RecoleccionCorte.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte && xx.Tipo == Enumerador.TipoDenominacion.Moneda.ToString()).FirstOrDefault().NumeroSobre : null;
                    }
                    if (item.RecoleccionVoucher != null && item.RecoleccionVoucher.Count() > 0)
                        item.SobreVoucher = item.RecoleccionVoucher.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Voucher).FirstOrDefault().NumeroSobre;
                    if (item.RecoleccionDocumentos != null && item.RecoleccionDocumentos.Count() > 0)
                        item.SobreDocumentos = item.RecoleccionDocumentos.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Documentos).FirstOrDefault().NumeroSobre;
                    if (item.RecoleccionNovedad != null && item.RecoleccionNovedad.Count() > 0)
                        item.SobreNovedad = item.RecoleccionNovedad.Where(xx => xx.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Novedad).FirstOrDefault().NumeroSobre;

                    if (item.RecoleccionBase.Count() == 0) { item.RecoleccionBase = null; }
                    if (item.RecoleccionCorte.Count() == 0) { item.RecoleccionCorte = null; }
                    if (item.RecoleccionBase == null && item.RecoleccionCorte == null && item.RecoleccionVoucher == null && item.RecoleccionDocumentos == null)
                    {
                        objRecoleccion = null;
                    }
                }
            }
            ////////////////////////// Quitar desde la izquierda las recolecciones almacenasas en la variable de sesion
            if (Session["recoleccionesAlmacenarNido"] != null)
            {
                List<Recoleccion> objListaRecoleccion = new List<Recoleccion>();

                foreach (var item in objRecoleccion)
                {
                    Recoleccion RecoleccionExistente = new Recoleccion();
                    RecoleccionExistente = ((List<Recoleccion>)Session["recoleccionesAlmacenarNido"]).Where(x => x.IdRecoleccion == item.IdRecoleccion).FirstOrDefault();
                    if (RecoleccionExistente == null)
                    {
                        objListaRecoleccion.Add(item);
                    }                    
                }
                objRecoleccion = objListaRecoleccion;

                IdRecolecciones = new int[objRecoleccion.Count()];
                FechaRecolecciones = new string[objRecoleccion.Count()];
                var listaRecoleccion = objRecoleccion.OrderBy(xx => xx.IdRecoleccion).ToList();
                for (var i = 0; i < IdRecolecciones.Length; i += 1)
                {
                    IdRecolecciones[i] = listaRecoleccion[i].IdRecoleccion;
                    FechaRecolecciones[i] = listaRecoleccion[i].FechaCreacion.ToString("hh:mm tt");
                }
            }
            ///////////////////////////
            if (IdRecoleccion == 0 || IdRecoleccion == objRecoleccion.FirstOrDefault().IdRecoleccion)
            {
                _recoleccion = objRecoleccion.FirstOrDefault();
                ViewBag.Inicio = true;
            }
            else
            {
                _recoleccion = objRecoleccion.Where(xx => xx.IdRecoleccion == IdRecoleccion).FirstOrDefault();
                ViewBag.Inicio = false;
            }
            ViewBag.TotalRecolecciones = objRecoleccion.Count();
            ViewBag.FechaRecolecciones = FechaRecolecciones;
            ViewBag.IdRecolecciones = IdRecolecciones;
            return View("_Wizard", _recoleccion);
        }

        public async Task<ActionResult> ObtenerDetalleRecoleccion(int IdApertura, string usuario, int IdUsuarioCreacion)
        {
            List<DetalleRecoleccion> modelo = null;
            List<CabeceraDetalleRecoleccion> _model = new List<CabeceraDetalleRecoleccion>();
            CabeceraDetalleRecoleccion _cabecera = null;
            DetalleRecoleccion objDetalle = null;
            int IdUsuario = 0;
            var detalle = await GetAsync<IEnumerable<DetalleRecoleccion>>($"RecoleccionSupervisor/ObtenerDetalleRecoleccion/{IdApertura}/{1}");
            var registros = detalle.Select(xx => xx.IdRecoleccion).Distinct();
            for (int i = 0; i < registros.Count(); i++)
            {
                modelo = new List<DetalleRecoleccion>();
                _cabecera = new CabeceraDetalleRecoleccion();
                var _idRecoleccion = registros.ToList()[i];
                _cabecera.IdApertura = detalle.FirstOrDefault().IdApertura;
                _cabecera.IdRecoleccion = _idRecoleccion;
                

                foreach (var item in detalle.Where(xx => xx.IdRecoleccion == _idRecoleccion))
                {
                    if (item.IdUsuarioEntrega != null)
                    {
                        IdUsuario = (int)item.IdUsuarioEntrega;
                        objDetalle = new DetalleRecoleccion();
                        objDetalle.IdApertura = item.IdApertura;
                        objDetalle.IdRecoleccion = item.IdRecoleccion;
                        objDetalle.NumeroSobre = item.NumeroSobre;
                        objDetalle.Tipo = item.Tipo;
                        objDetalle.TipoRecoleccion = item.TipoRecoleccion;
                        objDetalle.Total = item.Total;
                        modelo.Add(objDetalle);
                    }
                }
                _cabecera.objDetalle = modelo;
                _model.Add(_cabecera);
            }
            ViewBag.Usuario = usuario;
            ViewBag.IdApertura = IdApertura;
            ViewBag.IdUsuarioSupervisor = IdUsuario;
            ViewBag.IdRecolecciones = modelo.Select(xx => xx.IdRecoleccion).Distinct();
            return PartialView("_Detalle", _model);



        }

        public async Task<ActionResult> ObtenerDetalleRecoleccionSinGuardar(string Usuario, int IdApertura)
        {
            var recolecciones = ((List<Recoleccion>)Session["recoleccionesAlmacenarNido"]);

            List<DetalleRecoleccion> modelo = null;
            List<CabeceraDetalleRecoleccion> _model = new List<CabeceraDetalleRecoleccion>();
            CabeceraDetalleRecoleccion _cabecera = null;
            DetalleRecoleccion objDetalle = null;

            var Denominacion = await GetAsync<IEnumerable<TipoDenominacion>>("TipoDenominacion/Obtener");
            var detalle = await GetAsync<IEnumerable<DetalleRecoleccion>>($"RecoleccionSupervisor/ObtenerDetalleRecoleccion/{IdApertura}/{0}");

            foreach (var recoleccion in recolecciones)
            {
                modelo = new List<DetalleRecoleccion>();
                _cabecera = new CabeceraDetalleRecoleccion();
                _cabecera.IdApertura = recolecciones.FirstOrDefault().IdApertura;

                if (recoleccion.RecoleccionBase != null)
                {
                    var totalMonedas = 0;
                    var totalBilletes = 0;
                    foreach (var item in recoleccion.RecoleccionBase)
                    {
                        var Tipodenominacion = Denominacion.Where(x => x.IdTipoDenominacion == item.IdTipoDenominacion).FirstOrDefault();
                        if (Tipodenominacion.Tipo == Enumerador.TipoDenominacion.Moneda.ToString())
                        {
                            totalMonedas += (item.CantidadNido * int.Parse(Tipodenominacion.Denominacion));
                        }
                        else
                        {
                            totalBilletes += (item.CantidadNido * int.Parse(Tipodenominacion.Denominacion));
                        }
                    }

                    if (totalMonedas > 0)
                    {
                        objDetalle = new DetalleRecoleccion();
                        objDetalle.IdApertura = recoleccion.IdApertura;
                        objDetalle.IdRecoleccion = recoleccion.IdRecoleccion;
                        objDetalle.NumeroSobre = detalle.Where(x => x.IdRecoleccion == recoleccion.IdRecoleccion && x.TipoRecoleccion == Enumerador.TipoRecoleccion.Base.ToString() && x.Tipo == Enumerador.TipoDenominacion.Moneda.ToString()).FirstOrDefault().NumeroSobre;
                        objDetalle.Tipo = Enumerador.TipoDenominacion.Moneda.ToString();
                        objDetalle.TipoRecoleccion = Enumerador.TipoRecoleccion.Base.ToString();
                        objDetalle.Total = totalMonedas;
                        modelo.Add(objDetalle);
                    }
                    if (totalBilletes > 0)
                    {
                        objDetalle = new DetalleRecoleccion();
                        objDetalle.IdApertura = recoleccion.IdApertura;
                        objDetalle.IdRecoleccion = recoleccion.IdRecoleccion;
                        objDetalle.NumeroSobre = detalle.Where(x => x.IdRecoleccion == recoleccion.IdRecoleccion && x.TipoRecoleccion == Enumerador.TipoRecoleccion.Base.ToString() && x.Tipo == Enumerador.TipoDenominacion.Billete.ToString()).FirstOrDefault().NumeroSobre;
                        objDetalle.Tipo = Enumerador.TipoDenominacion.Billete.ToString();
                        objDetalle.TipoRecoleccion = Enumerador.TipoRecoleccion.Base.ToString();
                        objDetalle.Total = totalBilletes;
                        modelo.Add(objDetalle);
                    }
                }

                if (recoleccion.RecoleccionCorte != null)
                {
                    var totalMonedas = 0;
                    var totalBilletes = 0;
                    foreach (var item in recoleccion.RecoleccionCorte)
                    {
                        var Tipodenominacion = Denominacion.Where(x => x.IdTipoDenominacion == item.IdTipoDenominacion).FirstOrDefault();
                        if (Tipodenominacion.Tipo == Enumerador.TipoDenominacion.Moneda.ToString())
                        {
                            totalMonedas += (item.CantidadNido * int.Parse(Tipodenominacion.Denominacion)); ;
                        }
                        else
                        {
                            totalBilletes += (item.CantidadNido * int.Parse(Tipodenominacion.Denominacion)); ;
                        }
                    }

                    if (totalMonedas > 0)
                    {
                        objDetalle = new DetalleRecoleccion();
                        objDetalle.IdApertura = recoleccion.IdApertura;
                        objDetalle.IdRecoleccion = recoleccion.IdRecoleccion;
                        objDetalle.NumeroSobre = detalle.Where(x => x.IdRecoleccion == recoleccion.IdRecoleccion && x.TipoRecoleccion == Enumerador.TipoRecoleccion.Corte.ToString() && x.Tipo == Enumerador.TipoDenominacion.Moneda.ToString()).FirstOrDefault().NumeroSobre;
                        objDetalle.Tipo = Enumerador.TipoDenominacion.Moneda.ToString();
                        objDetalle.TipoRecoleccion = Enumerador.TipoRecoleccion.Corte.ToString();
                        objDetalle.Total = totalMonedas;
                        modelo.Add(objDetalle);
                    }
                    if (totalBilletes > 0)
                    {
                        objDetalle = new DetalleRecoleccion();
                        objDetalle.IdApertura = recoleccion.IdApertura;
                        objDetalle.IdRecoleccion = recoleccion.IdRecoleccion;
                        objDetalle.NumeroSobre = detalle.Where(x => x.IdRecoleccion == recoleccion.IdRecoleccion && x.TipoRecoleccion == Enumerador.TipoRecoleccion.Corte.ToString() && x.Tipo == Enumerador.TipoDenominacion.Billete.ToString()).FirstOrDefault().NumeroSobre;
                        objDetalle.Tipo = Enumerador.TipoDenominacion.Billete.ToString();
                        objDetalle.TipoRecoleccion = Enumerador.TipoRecoleccion.Corte.ToString();
                        objDetalle.Total = totalBilletes;
                        modelo.Add(objDetalle);
                    }
                }

                if (recoleccion.RecoleccionVoucher != null)
                {
                    long totalVoucher = 0;
                    string numeroSobre = "";
                    foreach (var item in recoleccion.RecoleccionVoucher)
                    {
                        totalVoucher += item.Valor;
                        numeroSobre = item.NumeroSobre;
                    }

                    if (totalVoucher > 0)
                    {
                        objDetalle = new DetalleRecoleccion();
                        objDetalle.IdApertura = recoleccion.IdApertura;
                        objDetalle.IdRecoleccion = recoleccion.IdRecoleccion;
                        objDetalle.NumeroSobre = numeroSobre;
                        objDetalle.Tipo = Enumerador.TipoRecoleccion.Voucher.ToString();
                        objDetalle.TipoRecoleccion = Enumerador.TipoRecoleccion.Voucher.ToString();
                        objDetalle.Total = (int)totalVoucher;
                        modelo.Add(objDetalle);
                    }
                }
                if (recoleccion.RecoleccionDocumentos != null)
                {
                    long totalDocumento = 0;
                    string numeroSobre = "";
                    foreach (var item in recoleccion.RecoleccionDocumentos)
                    {
                        totalDocumento += item.Valor;
                        numeroSobre = item.NumeroSobre;
                    }

                    if (totalDocumento > 0)
                    {
                        objDetalle = new DetalleRecoleccion();
                        objDetalle.IdApertura = recoleccion.IdApertura;
                        objDetalle.IdRecoleccion = recoleccion.IdRecoleccion;
                        objDetalle.NumeroSobre = numeroSobre;
                        objDetalle.Tipo = Enumerador.TipoRecoleccion.Documentos.ToString();
                        objDetalle.TipoRecoleccion = Enumerador.TipoRecoleccion.Documentos.ToString();
                        objDetalle.Total = (int)totalDocumento;
                        modelo.Add(objDetalle);
                    }
                }

                if (recoleccion.RecoleccionNovedad != null)
                {
                    long totalNovedad = 0;
                    string numeroSobre = "";
                    foreach (var item in recoleccion.RecoleccionNovedad)
                    {
                        numeroSobre = item.NumeroSobre;
                        totalNovedad += item.Valor;
                    }

                    if (totalNovedad > 0)
                    {
                        objDetalle = new DetalleRecoleccion();
                        objDetalle.IdApertura = recoleccion.IdApertura;
                        objDetalle.IdRecoleccion = recoleccion.IdRecoleccion;
                        objDetalle.NumeroSobre = numeroSobre;
                        objDetalle.Tipo = Enumerador.TipoRecoleccion.Novedad.ToString();
                        objDetalle.TipoRecoleccion = Enumerador.TipoRecoleccion.Novedad.ToString();
                        objDetalle.Total = (int)totalNovedad;
                        modelo.Add(objDetalle);
                    }
                }
                _cabecera.objDetalle = modelo;
                _model.Add(_cabecera);

                ViewBag.Usuario = Usuario;
                ViewBag.IdApertura = recoleccion.IdApertura;
                ViewBag.IdUsuarioSupervisor = recoleccion.IdUsuarioSupervisor;

            }

            return PartialView("_Detalle", _model);

        }
        public async Task<IEnumerable<NovedadArqueo>> NovedadesArqueo(int Id, int IdRecoleccion)
        {
            IEnumerable<NovedadArqueo> objNovedad = new List<NovedadArqueo>();
            var detalleNovedad = await GetAsync<IEnumerable<DetalleRecoleccionNovedad>>($"RecoleccionSupervisor/ObtenerNovedadPorIdRecoleccion/{IdRecoleccion}");
            //var objNovedadArqueo = await GetAsync<IEnumerable<NovedadArqueo>>($"RecoleccionSupervisor/ObtenerNovedadPorIdUsuario/{Id}");

            var _objNovedades = new List<NovedadArqueo>();
            foreach (var item in detalleNovedad)
            {
                var lista = new NovedadArqueo();
                lista.Id = item.IdDetalleRecoleccionNovedad;
                lista.IdEstado = item.IdEstado;
                lista.IdPunto = IdPunto;
                lista.IdTaquillero = item.IdUsuarioCreacion;
                lista.IdTipoNovedadArqueo = 1;
                lista.TipoNovedadNombre = item.TipoNovedadNombre;
                lista.Valor = item.Valor;
                lista.FechaCreado = item.FechaCreacion;
                if (lista != null) { _objNovedades.Add(lista); } else { _objNovedades = null; }
            }
            objNovedad = _objNovedades;
            return objNovedad;
        }

        [HttpPost]
        public async Task<ActionResult> Index(string Resultado)
        {
            ViewBag.Resultado = Resultado;
            return View();
        }

        public async Task<IEnumerable<TipoDenominacion>> TipoDenominacion()
        {
            var objTipoDenominacion = await GetAsync<IEnumerable<TipoDenominacion>>("TipoDenominacion/Obtener");
            return objTipoDenominacion;
        }

        public async Task<IEnumerable<MediosPagoFactura>> DocumentosRecoleccion(int Id)
        {
            var objDocumentos = await GetAsync<IEnumerable<MediosPagoFactura>>($"RecoleccionSupervisor/ObtenerDocumentos/{Id}");
            return objDocumentos;
        }

        [HttpPost]
        public async Task<ActionResult> Actualizar(Recoleccion modelo)
        {
            IEnumerable<DetalleRecoleccionMonetaria> Base = null;
            IEnumerable<DetalleRecoleccionMonetaria> Corte = null;
            IEnumerable<DetalleRecoleccionDocumento> Voucher = null;
            IEnumerable<DetalleRecoleccionDocumento> Documento = null;
            IEnumerable<DetalleRecoleccionNovedad> Novedad = null;

            if (Session["recoleccionesAlmacenarNido"] == null)
                Session["recoleccionesAlmacenarNido"] = new List<Recoleccion>();
            try
            {
                //if (modelo.RecoleccionBase != null)
                //{
                //    Base = modelo.RecoleccionBase.Where(b => b.CantidadNido > 0).ToList();
                //}
                //if (modelo.RecoleccionCorte != null)
                //{
                //    Corte = modelo.RecoleccionCorte.Where(c => c.CantidadNido > 0).ToList();
                //}
                if (modelo.RecoleccionVoucher != null)
                {
                    Voucher = modelo.RecoleccionVoucher.Where(v => v.RevisionNido == true).ToList();
                }
                if (modelo.RecoleccionDocumentos != null)
                {
                    Documento = modelo.RecoleccionDocumentos.Where(d => d.RevisionNido == true).ToList();
                }
                if (modelo.RecoleccionNovedad != null)
                {
                    Novedad = modelo.RecoleccionNovedad.Where(d => d.RevisionSupervisor == true).ToList();
                }

                //ObservacionRecoleccion objObservacion = new ObservacionRecoleccion();
                //objObservacion.IdRecoleccion = modelo.IdRecoleccion;
                //objObservacion.IdUsuarioCreacion = modelo.IdUsuarioModificacion;
                //objObservacion.FechaCreacion = DateTime.Now;
                //objObservacion.Observacion = modelo.objObservaciones.Observacion;
                //modelo.objObservaciones = objObservacion;

                //modelo.RecoleccionBase = Base;
                //modelo.RecoleccionCorte = Corte;
                modelo.RecoleccionVoucher = Voucher;
                modelo.RecoleccionDocumentos = Documento;
                modelo.IdUsuarioNido = modelo.IdUsuarioModificacion;
                ((List<Recoleccion>)Session["recoleccionesAlmacenarNido"]).Add(modelo);
                if (modelo.cierreRecoleccion == 1)
                {
                    var user = await GetAsync<Usuario>($"Usuario/GetById?id={modelo.IdUsuarioSupervisor}&Punto={IdPunto}");
                    return await ObtenerDetalleRecoleccionSinGuardar(user.NombreUsuario, modelo.IdApertura);
                    //return await ObtenerDetalleRecoleccion(modelo.IdApertura, user.NombreUsuario, modelo.IdUsuarioCreacion);
                }
                else
                {
                    return await ObtenerDatosRecoleccion(modelo.IdUsuarioSupervisor, modelo.IdPunto, 0);

                }

            }
            catch (Exception ex)
            {
                return await Index(string.Concat("Error: ", ex.Message));
            }
            finally
            {
                Base = null;
                Corte = null;
                Voucher = null;
                Documento = null;
            }
        }

        private async Task ActualizarTodasRecolecciones()
        {
            var recolecciones = ((List<Recoleccion>)Session["recoleccionesAlmacenarNido"]);
            foreach (var modelo in recolecciones)
            {

                if (modelo.RecoleccionBase != null)
                {
                    foreach (var item in modelo.RecoleccionBase)
                    {
                        DetalleRecoleccionMonetaria _recoleccionBase = new DetalleRecoleccionMonetaria();
                        _recoleccionBase.IdDetalleRecoleccionMonetaria = item.IdDetalleRecoleccionMonetaria;
                        _recoleccionBase.CantidadNido = item.CantidadNido;
                        _recoleccionBase.IdUsuarioNido = modelo.IdUsuarioNido;
                        _recoleccionBase.IdTipoRecoleccion = (int)Enumerador.TipoRecoleccion.Base;
                        _recoleccionBase.IdEstado = (int)Enumerador.Estados.EntregadoNido;
                        _recoleccionBase.FechaCreacion = DateTime.Now;
                        _recoleccionBase.FechaModificacion = DateTime.Now;
                        var resultado = await PutAsync<DetalleRecoleccionMonetaria, string>("RecoleccionSupervisor/Actualizar", _recoleccionBase);
                    }
                }
                if (modelo.RecoleccionCorte != null)
                {
                    foreach (var item in modelo.RecoleccionCorte)
                    {
                        DetalleRecoleccionMonetaria _recoleccionCorte = new DetalleRecoleccionMonetaria();
                        _recoleccionCorte.IdDetalleRecoleccionMonetaria = item.IdDetalleRecoleccionMonetaria;
                        _recoleccionCorte.CantidadNido = item.CantidadNido;
                        _recoleccionCorte.IdUsuarioNido = modelo.IdUsuarioNido;
                        _recoleccionCorte.IdTipoRecoleccion = (int)Enumerador.TipoRecoleccion.Corte;
                        _recoleccionCorte.IdEstado = (int)Enumerador.Estados.EntregadoNido;
                        _recoleccionCorte.FechaCreacion = DateTime.Now;
                        _recoleccionCorte.FechaModificacion = DateTime.Now;
                        var resultado = await PutAsync<DetalleRecoleccionMonetaria, string>("RecoleccionSupervisor/Actualizar", _recoleccionCorte);
                    }
                }
                if (modelo.RecoleccionVoucher != null)
                {
                    foreach (var item in modelo.RecoleccionVoucher)
                    {
                        DetalleRecoleccionDocumento _recoleccionVoucher = new DetalleRecoleccionDocumento();
                        _recoleccionVoucher.IdDetalleRecoleccionDocumento = item.IdDetalleRecoleccionDocumento;
                        _recoleccionVoucher.RevisionNido = item.RevisionNido;
                        _recoleccionVoucher.IdUsuarioNido = modelo.IdUsuarioNido;
                        _recoleccionVoucher.IdTipoRecoleccion = (int)Enumerador.TipoRecoleccion.Voucher;
                        _recoleccionVoucher.IdEstado = (int)Enumerador.Estados.EntregadoNido;
                        _recoleccionVoucher.FechaCreacion = DateTime.Now;
                        _recoleccionVoucher.FechaModificacion = DateTime.Now;
                        var resultado = await PutAsync<DetalleRecoleccionDocumento, string>("RecoleccionSupervisor/ActualizarDocumentos", _recoleccionVoucher);
                    }
                }
                if (modelo.RecoleccionDocumentos != null)
                {
                    foreach (var item in modelo.RecoleccionDocumentos)
                    {
                        DetalleRecoleccionDocumento _recoleccionDocumento = new DetalleRecoleccionDocumento();
                        _recoleccionDocumento.IdDetalleRecoleccionDocumento = item.IdDetalleRecoleccionDocumento;
                        _recoleccionDocumento.RevisionNido = item.RevisionNido;
                        _recoleccionDocumento.IdUsuarioNido = modelo.IdUsuarioNido;
                        _recoleccionDocumento.IdTipoRecoleccion = (int)Enumerador.TipoRecoleccion.Documentos;
                        _recoleccionDocumento.IdEstado = (int)Enumerador.Estados.EntregadoNido;
                        _recoleccionDocumento.FechaCreacion = DateTime.Now;
                        _recoleccionDocumento.FechaModificacion = DateTime.Now;
                        var resultado = await PutAsync<DetalleRecoleccionDocumento, string>("RecoleccionSupervisor/ActualizarDocumentos", _recoleccionDocumento);
                    }
                }
                if (modelo.RecoleccionNovedad != null)
                {
                    foreach (var item in modelo.RecoleccionNovedad)
                    {
                        DetalleRecoleccionNovedad _recoleccionNovedad = new DetalleRecoleccionNovedad();
                        _recoleccionNovedad.IdDetalleRecoleccionNovedad = item.IdDetalleRecoleccionNovedad;
                        _recoleccionNovedad.RevisionNido = item.RevisionNido;
                        _recoleccionNovedad.IdUsuarioNido = modelo.IdUsuarioModificacion;
                        _recoleccionNovedad.IdTipoRecoleccion = (int)Enumerador.TipoRecoleccion.Base;
                        _recoleccionNovedad.IdEstado = (int)Enumerador.Estados.EntregadoNido;
                        _recoleccionNovedad.FechaCreacion = DateTime.Now;
                        _recoleccionNovedad.FechaModificacion = DateTime.Now;
                        _recoleccionNovedad.IdNovedadArqueo = item.IdNovedadArqueo;
                        var resultado = await PutAsync<DetalleRecoleccionNovedad, string>("RecoleccionSupervisor/ActualizarNovedades", _recoleccionNovedad);
                    }
                }
                Recoleccion _recoleccion = new Recoleccion();
                _recoleccion.IdRecoleccion = modelo.IdRecoleccion;
                _recoleccion.IdEstado = (int)Enumerador.Estados.EntregadoNido;
                _recoleccion.FechaCreacion = DateTime.Now;
                _recoleccion.FechaModificacion = DateTime.Now;
                _recoleccion.IdUsuarioModificacion = modelo.IdUsuarioModificacion;
                _recoleccion.IdUsuarioNido = modelo.IdUsuarioNido;
                var resultadoRecoleccion = await PutAsync<Recoleccion, string>("RecoleccionSupervisor/ActualizarRecoleccion", _recoleccion);

                string json = new JavaScriptSerializer().Serialize(modelo);
                ViewBag.json = json;

            }
        }


        [HttpPost]
        public async Task<JsonResult> RegresarRecoleccion(int IdApertura)
        {
            string error = string.Empty;
            try
            {
                int estado = (int)Enumerador.Estados.EntregadoSupervisor;
                error = await PutAsync<Recoleccion, string>($"RecoleccionSupervisor/RegresarEstado?IdApertura={IdApertura}&IdEstado={estado}", null);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw;
            }
            return Json(error, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ValidaClave(string usuario, string clave, string observacion, Recoleccion modelo)
        {
            ViewBag.Validar = false;
            ViewBag.Observacion = observacion;
            var idPuntoConfigurado = IdPunto;
            var pwd = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            var user = await GetAsync<Usuario>($"Usuario/GetByUserPwd?pwd={Server.UrlEncode(pwd)}&user={usuario}&Punto={idPuntoConfigurado}");

            var pwd2 = Encripcion.Encriptar(clave, ConfigurationManager.AppSettings["llaveEncripcion"]);
            var user2 = (Usuario)Session["UsuarioAutenticadoTemp"];

            if (user != null && user.Password2 == pwd2)
            {
                await ActualizarTodasRecolecciones();
                Session["recoleccionesAlmacenarNido"] = null;
                return Json(new RespuestaViewModel { Correcto = true, Mensaje = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Usuario o contraseña incorrectos" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> GuardaObservacion(string observacion, int IdApertura, int IdUsuarioSupervisor)
        {
            ObservacionRecoleccion objObservacion = new ObservacionRecoleccion();
            objObservacion.IdRecoleccion = IdApertura;
            objObservacion.IdUsuarioCreacion = IdUsuarioLogueado;
            objObservacion.FechaCreacion = DateTime.Now;
            objObservacion.Observacion = observacion;

            var resultadoObservaciones = await PostAsync<ObservacionRecoleccion, string>("RecoleccionSupervisor/InsertaObservacion", objObservacion);
            if (resultadoObservaciones != null)
            {
                List<DetalleRecoleccion> modelo = null;
                List<CabeceraDetalleRecoleccion> _model = new List<CabeceraDetalleRecoleccion>();
                CabeceraDetalleRecoleccion _cabecera = null;
                DetalleRecoleccion objDetalle = null;
                var detalle = await GetAsync<IEnumerable<DetalleRecoleccion>>($"RecoleccionSupervisor/ObtenerDetalleRecoleccion/{IdApertura}/{1}");
                var registros = detalle.Select(xx => xx.IdRecoleccion).Distinct();
                for (int i = 0; i < registros.Count(); i++)
                {
                    modelo = new List<DetalleRecoleccion>();
                    _cabecera = new CabeceraDetalleRecoleccion();
                    var _idRecoleccion = registros.ToList()[i];
                    _cabecera.IdApertura = detalle.FirstOrDefault().IdApertura;
                    _cabecera.IdRecoleccion = _idRecoleccion;

                    foreach (var item in detalle.Where(xx => xx.IdRecoleccion == _idRecoleccion))
                    {
                        objDetalle = new DetalleRecoleccion();
                        objDetalle.IdApertura = item.IdApertura;
                        objDetalle.IdRecoleccion = item.IdRecoleccion;
                        objDetalle.NumeroSobre = item.NumeroSobre;
                        objDetalle.Tipo = item.Tipo;
                        objDetalle.TipoRecoleccion = item.TipoRecoleccion;
                        objDetalle.Total = item.Total;
                        modelo.Add(objDetalle);
                    }
                    _cabecera.objDetalle = modelo;
                    _model.Add(_cabecera);
                }
                await GenerarFactura(_model, observacion, IdUsuarioSupervisor);
                return Json(new RespuestaViewModel { Correcto = true, Mensaje = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Error al actualizar la observacion" }, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<string> GenerarFactura(List<CabeceraDetalleRecoleccion> _detalle, string Observaciones, int IdUsuario)
        {

            string strRetorno = string.Empty;
            string strNombreSupervisor = string.Empty;
            Usuario objUsuarioSupervisor = new Usuario();
            List<TicketImprimir> objListaTickets = new List<TicketImprimir>();
            TicketImprimir objTicketImprimir = new TicketImprimir();

            try
            {
                objUsuarioSupervisor = await GetAsync<Usuario>($"Usuario/GetById?id={IdUsuario}&Punto={0}");
                strNombreSupervisor = string.Concat(objUsuarioSupervisor.Nombre, " ", objUsuarioSupervisor.Apellido);

                var objTicket = new TicketImprimir();

                for (int i = 0; i < _detalle.Count(); i++)
                {
                    objTicket = await GenerarRecibos(_detalle[i], i);
                    if (objTicket != null)
                    {
                        if (objTicket.TituloRecibo.Length > 0)
                        {
                            objListaTickets.Add(objTicket);
                        }
                    }
                }

                objTicketImprimir.Usuario = NombreUsuarioLogueado;
                objTicketImprimir.Firma = string.Concat("Caja General: ", NombreUsuarioLogueado, "|", "Supervisor: ", strNombreSupervisor);
                objTicketImprimir.PieDePagina = Observaciones.Trim();
                objTicketImprimir.ListaTickets = objListaTickets;
                strRetorno = _service.ImprimirTicketMasivo(objTicketImprimir);
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RecoleccionNidoController-GenerarFactura");
                strRetorno = string.Concat("Error en GenerarFactura_RecoleccionNidoController ", ex.Message);
            }

            return strRetorno;
        }

        private async Task<TicketImprimir> GenerarRecibos(CabeceraDetalleRecoleccion _detalle, int numRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = null;
            List<Articulo> ListaArticulos = null;
            try
            {
                objTicketImprimir.TituloRecibo = string.Concat("RECOLECCION: ", (numRecoleccion + 1));
                objTicketImprimir.TituloColumnas = "Tipo Sobre||Total";
                ListaArticulos = new List<Articulo>();
                foreach (var itemDetalle in _detalle.objDetalle)
                {
                    objArticulo = new Articulo();
                    if (itemDetalle.TipoRecoleccion == "Base" || itemDetalle.TipoRecoleccion == "Corte")
                    { objArticulo.Nombre = string.Concat(itemDetalle.TipoRecoleccion, " ", itemDetalle.Tipo, "s ", itemDetalle.NumeroSobre); }
                    else { objArticulo.Nombre = string.Concat(itemDetalle.TipoRecoleccion, " ", itemDetalle.Tipo, " ", itemDetalle.NumeroSobre); }
                    objArticulo.Cantidad = 0;
                    objArticulo.Precio = double.Parse(itemDetalle.Total.ToString());
                    ListaArticulos.Add(objArticulo);
                    objArticulo = null;
                }
                objTicketImprimir.ListaArticulos = ListaArticulos;
                ListaArticulos = null;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RecoleccionNidoController-GenerarRecibos");
                return null;
            }
            finally
            {
                objArticulo = null;
                ListaArticulos = null;
            }

            return objTicketImprimir;
        }



        #endregion 
    }
}