//Cambioquitar: Este controlador usa el enumerador de perfiles.
using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Transversales.Contratos;
using System.Configuration;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class AperturaController : ControladorBase
    {

        #region Declaraciones

        private readonly IServicioImprimir _service;

        public AperturaController(IServicioImprimir service)
        {
            _service = service;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var item = new Apertura();
            var Denominacion = await GetAsync<IEnumerable<TipoDenominacion>>("TipoDenominacion/Obtener");
            item.ListaPuntos = await GetAsync<List<Puntos>>("Apertura/ObtenerPuntosSinApertura");
            item.FechaString = Utilidades.FormatoFechaValido(Utilidades.ObtenerFechaActual()).ToString();
            //EDSP INICIO
            //item.FechaString = DateTime.Now.ToString("dd/MM/yyyy");
            //EDSP FIN
            item.TiposDenominacion = Denominacion;
            return View(item);
        }

        public async Task<ActionResult> PuntosxFecha(string Fecha)
        {
            var item = new Apertura();
            var FechaString = Fecha.Replace("/", "-");
            var Denominacion = await GetAsync<IEnumerable<TipoDenominacion>>("TipoDenominacion/Obtener");
            item.ListaPuntos = await GetAsync<List<Puntos>>($"Apertura/ObtenerPuntosSinAperturaFecha/{FechaString}");
            item.TiposDenominacion = Denominacion;
            item.FechaString = Utilidades.FormatoFechaValido(Fecha).ToString();
            //EDSP INICIO
            //item.FechaString = DateTime.Now.ToString("dd/MM/yyyy");
            //EDSP FIN
            return Json(item.ListaPuntos.Select(x => new { label = x.Nombre, value = x.Id, category = x.TipoPunto }), JsonRequestBehavior.AllowGet);// PartialView("_AperturaBase", item);
        }

        public async Task<ActionResult> EditAperturaBase()
        {
            var item = new Apertura();
            var Denominacion = await GetAsync<IEnumerable<TipoDenominacion>>("TipoDenominacion/Obtener");
            item.FechaString = Utilidades.FormatoFechaValido(Utilidades.ObtenerFechaActual()).ToString();
            //EDSP INICIO
            //item.FechaString = DateTime.Now.ToString("dd/MM/yyyy");
            //EDSP FIN
            item.ListaPuntos = await GetAsync<List<Puntos>>("Apertura/ObtenerPuntosConApertura");
            if (item.ListaPuntos == null)
                item.ListaPuntos = new List<Puntos>();

            item.TiposDenominacion = Denominacion;
            return View(item);
        }

        public async Task<ActionResult> ObtenerAperturaBase(int IdPunto, string Fecha)
        {
            var apertura = new Apertura();
            var FechaString = Fecha.Replace("/", "-");
            IEnumerable<AperturaBase> aperturabase = await GetAsync<IEnumerable<AperturaBase>>($"AperturaBase/ObtenerAperturaBaseFecha/{IdPunto}/{FechaString}");
            var Idapertura = aperturabase.Select(x => x.IdApertura).FirstOrDefault();
            apertura = await GetAsync<Apertura>($"Apertura/Obtener/{Idapertura}");
            if (apertura == null)
                apertura = new Apertura();

            apertura.AperturaBase = aperturabase;
            var Denominacion = await GetAsync<IEnumerable<TipoDenominacion>>("TipoDenominacion/Obtener");
            apertura.ListaPuntos = await GetAsync<List<Puntos>>($"Apertura/ObtenerPuntosConAperturaFecha/{FechaString}"); /*await GetAsync<List<Puntos>>("Apertura/ObtenerPuntosConApertura");*/
            if (apertura.ListaPuntos == null)
                apertura.ListaPuntos = new List<Puntos>();

            apertura.TiposDenominacion = Denominacion;
            apertura.FechaString = Utilidades.FormatoFechaValido(Fecha).ToString();
            //return Json(apertura, JsonRequestBehavior.AllowGet); //
            return PartialView("_EditAperturaBaseDenominacion", apertura);
        }

        public async Task<ActionResult> PuntosEditxFecha(string Fecha)
        {
            var item = new Apertura();
            var FechaString = Fecha.Replace("/", "-");
            var Denominacion = await GetAsync<IEnumerable<TipoDenominacion>>("TipoDenominacion/Obtener");
            item.ListaPuntos = await GetAsync<List<Puntos>>($"Apertura/ObtenerPuntosConAperturaFecha/{FechaString}");
            if (item.ListaPuntos == null)
                item.ListaPuntos = new List<Puntos>();

            item.TiposDenominacion = Denominacion;
            item.FechaString = Utilidades.FormatoFechaValido(Fecha).ToString();
            return Json(item.ListaPuntos.Select(x => new { label = x.Nombre, value = x.Id, category = x.TipoPunto }), JsonRequestBehavior.AllowGet); //return PartialView("_EditAperturaBase", item);
        }

        public async Task<ActionResult> Insert(Apertura modelo, string hdListPuntos)
        {

            modelo.ListaPuntos = new List<Puntos>();
            modelo.FechaCreado = DateTime.Now;
            modelo.UsuarioCreado = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Abierto;
            modelo.Fecha = DateTime.Parse(modelo.FechaString);
            modelo.IdPuntoCreado = IdPunto;
            if (!string.IsNullOrEmpty(hdListPuntos))
                foreach (var item in hdListPuntos.Split(','))
                    modelo.ListaPuntos.Add(new Puntos { Id = Convert.ToInt32(item) });

            if (await PostAsync<Apertura, string>("AperturaBase/Insertar", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el alistamiento. Por favor intentelo de nuevo" });

        }

        public async Task<ActionResult> Update(Apertura modelo)
        {

            modelo.FechaModificado = DateTime.Now;
            modelo.UsuarioModificado = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Abierto;

            if (await PostAsync<Apertura, string>("AperturaBase/Actualizar", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el alistamiento. Por favor intentelo de nuevo" });
        }


        //EDSP Entrega de inventario operacional - Nido-Supervisor 


        [HttpGet]
        public async Task<ActionResult> EntregaInventario()
        {
            var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>($"TipoBrazalete/ObtenerTodosBrazaleteInventario/{IdPunto}");

            if (_listTipoBzalete == null)
            {
                ViewBag.tipoBrazalete = new List<TipoBrazalete>();
            }
            else
            {
                ViewBag.tipoBrazalete = _listTipoBzalete;
            }
            //string strPerfiles = (int)Enumerador.Perfiles.Supervisor + "," + (int)Enumerador.Perfiles.Recolector;
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesAperturaEntregaInventario");
            strPerfiles = objParametro.Valor;

            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
            ViewBag.supervisor = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });

            var _listaPunto = await GetAsync<IEnumerable<Puntos>>("Apertura/ObtenerPuntosConApertura");
            ViewBag.listaPunto = _listaPunto;
            ViewBag.Cantidad1 = "Cantidad nido";
            ViewBag.Cantidad2 = "Cantidad supervisor";
            ViewBag.issupervisor = true;
            ViewBag.listaPunto = _listaPunto ?? new List<Puntos>();
            ViewBag.listaPunto = _listaPunto ?? new List<Puntos>();

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerDetalleInventario(DetalleInventario modelo)
        {
            var _detalleInventario = await GetAsync<DetalleInventario>($"Apertura/DetalleInventario/{modelo.hdPuntos}");
            _detalleInventario.Observacion = modelo.Observacion;
            _detalleInventario.IdSupervisor = modelo.IdSupervisor;
            _detalleInventario.hdPuntos = modelo.hdPuntos;
            _detalleInventario.Brazaletes = new List<BrazaletesApertura>();
            ViewBag.Cantidad1 = "Cantidad nido";
            ViewBag.Cantidad2 = "Cantidad supervisor";
            ViewBag.issupervisor = true;
            return PartialView("_DetalleInventario", _detalleInventario);

        }

        //Valida si el supervisor tiene brazaletes asignados del día
        [HttpGet]
        public async Task<ActionResult> ObtenerBrazaletesPorSupervisor(int IdSupervisor)
        {
            var _detalleBrazalete = await GetAsync<List<AperturaBrazalete>>($"Apertura/ObtenerListaAperturaBrazalete/{IdSupervisor}");
            return Json(_detalleBrazalete ?? new List<AperturaBrazalete>(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerDetalleInventarioPuntos(DetalleInventario modelo)
        {
            if (modelo.Apertura.FirstOrDefault().AperturaElemento == null)
            {
                modelo.Apertura.FirstOrDefault().AperturaElemento = new List<AperturaElementos>();
            }
            if (modelo.Apertura.FirstOrDefault().AperturaBrazalete == null)
            {
                modelo.Apertura.FirstOrDefault().AperturaBrazalete = new List<AperturaBrazalete>();
            }
            return PartialView("_DetalleSupervisorTaquillero", modelo);

        }


        [HttpPost]
        public async Task<JsonResult> GuardarInformacionInventario(DetalleInventario modeloInventario)
        {

            RespuestaViewModel _rta = new RespuestaViewModel();

            try
            {

                List<AperturaBrazalete> _list = new List<AperturaBrazalete>();

                if (modeloInventario.Brazaletes != null)
                {
                    foreach (var item in modeloInventario.Brazaletes)
                    {
                        _list.Add(new AperturaBrazalete
                        {
                            CantidadInicial = item.Cantidad,
                            CantidadFinal = item.Cantidad,
                            CodigoSap = item.CodigoSap,
                            Fecha = DateTime.Now,
                            IdBrazalete = item.Id,
                            IdSupervisor = modeloInventario.IdSupervisor,
                            idUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id,
                            Observacion = modeloInventario.Observacion
                        });
                    }
                }


                if (_list.Count > 0)
                {
                    await PostAsync<List<AperturaBrazalete>, string>("Apertura/InsertarBrazaletes", _list);
                }


                foreach (Apertura item in modeloInventario.Apertura)
                {
                    item.IdEstado = (int)Enumerador.Estados.EnProceso;
                    item.FechaModificado = DateTime.Now;
                    item.IdSupervisor = modeloInventario.IdSupervisor;
                    item.UsuarioModificado = (Session["UsuarioAutenticado"] as Usuario).Id;
                    await PostAsync<Apertura, string>("AperturaBase/Actualizar", item);
                }


                List<AperturaElementos> _listElementos = new List<AperturaElementos>();
                foreach (var item in modeloInventario.Apertura)
                {
                    if (item.AperturaElemento != null)
                    {
                        foreach (var item2 in item.AperturaElemento)
                        {
                            if (item2 != null)
                                _listElementos.Add(item2);
                        }
                    }
                }

                if (_listElementos.Count > 0)
                {
                    await PostAsync<List<AperturaElementos>, string>("AperturaBase/ValidSupervisorElemento", _listElementos);
                    var obj = await GetAsync<AperturaElementosHeader>($"Apertura/ObtenerAperturaElementoHeader/{_listElementos.First().IdAperturaElementosHeader}");
                    obj.IdEstado = (int)Enumerador.Estados.EnProceso;
                    obj.IdSupervisor = modeloInventario.IdSupervisor;
                    await PostAsync<AperturaElementosHeader, string>("Apertura/ActualizarAperturaElementoHeader", obj);
                }


                _rta.Correcto = true;
                _rta.Elemento = "";
                _rta.Mensaje = "Correcto";

                //GALD metodo para imprimir recibo de recibido de nido a supervisor 
                #region Imprimir

                TicketImprimir objTicketImprimir = new TicketImprimir();
                List<Articulo> ListaArticulos = new List<Articulo>();
                var user = new Usuario();
                string Observaciones = string.Empty;

                foreach (Apertura item in modeloInventario.Apertura)
                {
                    user = await GetAsync<Usuario>($"Usuario/GetById?id={item.IdSupervisor}&Punto={0}");
                    var Punto = await GetAsync<Puntos>($"Puntos/GetById/{item.IdPunto}");
                    Observaciones = item.ObservacionSupervisor;
                    double CantidadTotal = 0;
                    Articulo objArticulo = new Articulo();
                    objArticulo.Grupo = "ALISTAMIENTO EFECTIVO BASE";
                    objArticulo.TituloColumnas = "PUNTO||VALOR";
                    objArticulo.Nombre = Punto.Nombre;

                    foreach (AperturaBase Apertura in item.AperturaBase)
                    {
                        if (Apertura.CantidadSupervisor != 0)
                        {
                            //Articulo objArticulo = new Articulo();
                            //objArticulo.Grupo = Punto.Nombre;
                            //objArticulo.subGrupo = "DINERO";
                            //objArticulo.Nombre = (Apertura.TotalNido / Apertura.CantidadNido).ToString("C0");
                            //objArticulo.Cantidad = int.Parse(Apertura.CantidadSupervisor.ToString());
                            //objArticulo.TituloColumnas = "Denominación|Cant|Total";
                            CantidadTotal = CantidadTotal + Apertura.TotalSupervisor;
                            //objArticulo.Otro = "";
                            //ListaArticulos.Add(objArticulo);
                            //objArticulo = null;
                        }
                    }

                    objArticulo.Precio = int.Parse(CantidadTotal.ToString());
                    ListaArticulos.Add(objArticulo);
                    objArticulo = null;
                }
                if (modeloInventario.Brazaletes != null)
                {
                    var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerTodosBrazalete");
                    _listTipoBzalete = _listTipoBzalete.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo);

                    foreach (var Brazalete in modeloInventario.Brazaletes.Where(x=>x.Cantidad != 0))
                    {
                        var _brazalete = _listTipoBzalete.Where(x => x.Id == Brazalete.Id).FirstOrDefault();

                        Articulo objArticulo = new Articulo();
                        objArticulo.Grupo = "ALISTAMIENTO BOLETERIA";
                        objArticulo.Nombre = _brazalete.Nombre;                        
                        objArticulo.TituloColumnas = "TIPO|CANTIDAD";
                        objArticulo.Cantidad = Brazalete.Cantidad;
                        objArticulo.Otro = "";
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;

                    }
                }

                var ItemApertura = new Apertura();
                ItemApertura.TipoElementos = await GetAsync<IEnumerable<TipoGeneral>>("Apertura/ObtenerTipoElementos");
                foreach (TipoGeneral TipoElemento in ItemApertura.TipoElementos)
                { 
                    var aperturasElementos = modeloInventario.Apertura.Where(Y => Y.AperturaElemento != null).Select(X => X.AperturaElemento.Where(Z => Z.ValidSupervisor == true) );
                    var elementos = new List<TipoElementos>();
                    aperturasElementos.ToList().ForEach(x => elementos.AddRange(x.Select(y => y.Elemento)));
                    var total = elementos.Where(x => x.Id.Equals(TipoElemento.Id)).ToList();
                                       
                    if (total.Count() > 0 )
                    { 
                        Articulo objArticulo = new Articulo();
                        objArticulo.Grupo = "ALISTAMIENTO ELEMENTOS";
                        objArticulo.Nombre = TipoElemento.Nombre;
                        objArticulo.TituloColumnas = "TIPO|CANTIDAD";
                        objArticulo.Cantidad = total.Count();
                        objArticulo.Precio = 0;
                        objArticulo.Otro = "";
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }
                }
  
                
                Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/FacturaPosTextoHead1");
                objTicketImprimir.AdicionarContenidoHeader = objParametro.Valor;
                
                objTicketImprimir.TituloRecibo = "Apertura de Nido-Supervisor";
                objTicketImprimir.ListaArticulos = ListaArticulos;
                if (string.IsNullOrEmpty(Observaciones))
                {
                    objTicketImprimir.PieDePagina = "";
                }
                else
                {
                    objTicketImprimir.PieDePagina = Observaciones.Trim();
                }
                objTicketImprimir.Firma = string.Concat("Caja General: ", NombreUsuarioLogueado, "|", "Supervisor: ", user.Nombre, " ", user.Apellido);
                _service.ImprimirTicketApertura(objTicketImprimir);

                #endregion

            }
            catch (Exception ex)
            {
                _rta.Correcto = false;
                _rta.Elemento = "";
                _rta.Mensaje = ex.Message;

            }
            return Json(_rta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ActualizarInformacionInventario(DetalleInventario modeloInventario)
        {
            RespuestaViewModel _rta = new RespuestaViewModel();
            try
            {

                //Actualizar Moneda
                foreach (Apertura item in modeloInventario.Apertura)
                {
                    item.IdEstado = (int)Enumerador.Estados.Entregado;
                    item.FechaModificado = DateTime.Now;
                    item.UsuarioModificado = (Session["UsuarioAutenticado"] as Usuario).Id;
                    await PostAsync<Apertura, string>("AperturaBase/Actualizar", item);
                }

                //Actualizar Elementos 

                List<AperturaElementos> _listElementos = new List<AperturaElementos>();
                foreach (var item in modeloInventario.Apertura)
                {
                    if (item.AperturaElemento != null)
                    {
                        foreach (var item2 in item.AperturaElemento)
                        {
                            if (item2 != null)
                                _listElementos.Add(item2);
                        }
                    }
                }

                if (_listElementos.Count > 0)
                {
                    await PostAsync<List<AperturaElementos>, string>("AperturaBase/ValidTaquilleroElemento", _listElementos);
                    var obj = await GetAsync<AperturaElementosHeader>($"Apertura/ObtenerAperturaElementoHeader/{_listElementos.First().IdAperturaElementosHeader}");
                    obj.IdEstado = (int)Enumerador.Estados.Entregado;
                    obj.IdTaquillero = modeloInventario.Apertura.First().IdTaquillero;
                    await PostAsync<AperturaElementosHeader, string>("Apertura/ActualizarAperturaElementoHeader", obj);

                }


                //Insertar AperturaBrazaleteDetalle

                List<TransladoInventario> _TransladoInventario = new List<TransladoInventario>();

                foreach (var item in modeloInventario.Apertura)
                {
                    if (item.AperturaBrazalete != null)
                    {
                        foreach (var item2 in item.AperturaBrazalete)
                        {
                            item2.BrazaleteDetalle.Fecha = DateTime.Now;
                            item2.BrazaleteDetalle.IdAperturaBrazalete = item2.Id;
                            item2.BrazaleteDetalle.IdEstado = (int)Enumerador.Estados.Activo;
                            await PostAsync<AperturaBrazaleteDetalle, string>("Apertura/InsertarBrazaleteDetalle", item2.BrazaleteDetalle);

                            TransladoInventario _Translado = new TransladoInventario();

                            _Translado.Cantidad = item2.BrazaleteDetalle.Cantidad;
                            _Translado.CodSapMaterial = item2.CodigoSap;
                            _Translado.Fecha = DateTime.Now;
                            _Translado.IdPuntoDestino = IdPunto;
                            _Translado.IdPuntoOrigen = item.IdPuntoCreado;
                            _Translado.idUsuario = (Session["UsuarioAutenticado"] as Usuario).Id;
                            _Translado.Procesado = false;
                            _Translado.UnidadMedida = item2.Unidad;
                            _TransladoInventario.Add(_Translado);

                        }
                    }
                }


                //if (_TransladoInventario.Count > 0)
                //{
                //    try
                //    {

                //        var respuesta = await PostAsync<IEnumerable<TransladoInventario>, string>("Inventario/ActualizarTransladoInventario", _TransladoInventario);
                //        if (respuesta.Mensaje != "")
                //            throw new ArgumentException(respuesta.Mensaje);

                //    }
                //    catch (Exception ex)
                //    {
                //        Utilidades.RegistrarError(ex, "Error de TransladoInventario en Apertura");

                //    }


                //}


                //GALD metodo para imprimir recibo de recibido
                #region Imprimir

                TicketImprimir objTicketImprimir = new TicketImprimir();
                List<Articulo> ListaArticulos = new List<Articulo>();
                var user = new Usuario();
                string Observaciones = string.Empty;

                foreach (Apertura item in modeloInventario.Apertura)
                {
                    foreach (AperturaBase Apertura in item.AperturaBase)
                    {
                        if (Apertura.CantidadPunto != 0)
                        {
                            Articulo objArticulo = new Articulo();
                            objArticulo.Grupo = NombrePunto;
                            objArticulo.subGrupo = "ALISTAMIENTO EFECTIVO BASE";
                            objArticulo.Nombre = (Apertura.TotalPunto / Apertura.CantidadPunto).ToString("C0");
                            objArticulo.Cantidad = int.Parse(Apertura.CantidadPunto.ToString());
                            objArticulo.TituloColumnas = "Denominación|Cant|Valor";
                            objArticulo.Precio = Apertura.TotalPunto;
                            objArticulo.Otro = "";
                            ListaArticulos.Add(objArticulo);
                            objArticulo = null;
                        }
                    }

                    //if (item.AperturaElemento != null)
                    //{
                    //    foreach (var elemento in item.AperturaElemento)
                    //    {
                    //        if (elemento != null)
                    //        {
                    //            if (elemento.ValidSupervisor)
                    //            {
                    //                Articulo objArticulo = new Articulo();
                    //                objArticulo.Grupo = NombrePunto;
                    //                objArticulo.subGrupo = "Elementos";
                    //                objArticulo.Nombre = elemento.Elemento.Nombre;
                    //                objArticulo.TituloColumnas = "Nombre";
                    //                objArticulo.Cantidad = 0;
                    //                objArticulo.Precio = 0;
                    //                objArticulo.Otro = "";
                    //                ListaArticulos.Add(objArticulo);
                    //                objArticulo = null;
                    //            }
                    //        }
                    //    }
                    //}

                    //Insertar AperturaBrazaleteDetalle

                    var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerTodosBrazalete");
                    //_listTipoBzalete = _listTipoBzalete.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo);
                    if (item.AperturaBrazalete != null)
                    {
                        foreach (var item2 in item.AperturaBrazalete)
                        {
                            var _brazalete = _listTipoBzalete.Where(x => x.Id == item2.IdBrazalete).FirstOrDefault();

                            Articulo objArticulo = new Articulo();
                            objArticulo.Grupo = NombrePunto;
                            objArticulo.subGrupo = "ALISTAMIENTO BOLETERIA"; 
                            objArticulo.Nombre = _brazalete.Nombre;
                            objArticulo.Cantidad = item2.BrazaleteDetalle.Cantidad;
                            objArticulo.TituloColumnas = "TIPO|CANTIDAD";
                            objArticulo.Precio = 0;
                            objArticulo.Otro = "";
                            ListaArticulos.Add(objArticulo);
                            objArticulo = null;

                        }
                    }

                    user = await GetAsync<Usuario>($"Usuario/GetById?id={item.IdTaquillero}&Punto={0}");
                    Observaciones = item.ObservacionPunto;
                }

                var ItemApertura = new Apertura();
                ItemApertura.TipoElementos = await GetAsync<IEnumerable<TipoGeneral>>("Apertura/ObtenerTipoElementos");
                foreach (TipoGeneral TipoElemento in ItemApertura.TipoElementos)
                {
                    var aperturasElementos = modeloInventario.Apertura.Where(Y => Y.AperturaElemento != null).Select(X => X.AperturaElemento.Where(Z => Z.ValidTaquilla == true));
                    var elementos = new List<TipoElementos>();
                    aperturasElementos.ToList().ForEach(x => elementos.AddRange(x.Select(y => y.Elemento)));
                    var total = elementos.Where(x => x.Id.Equals(TipoElemento.Id)).ToList();

                    if (total.Count() > 0)
                    {
                        Articulo objArticulo = new Articulo();
                        objArticulo.Grupo = NombrePunto;
                        objArticulo.subGrupo = "ALISTAMIENTO ELEMENTOS";
                        objArticulo.Nombre = TipoElemento.Nombre;
                        objArticulo.TituloColumnas = "TIPO|CANTIDAD";
                        objArticulo.Cantidad = total.Count();
                        objArticulo.Precio = 0;
                        objArticulo.Otro = "";
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }
                }

                Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/FacturaPosTextoHead1");
                objTicketImprimir.AdicionarContenidoHeader = objParametro.Valor;

                objTicketImprimir.TituloRecibo = "Apertura de Supervisor-Taquillero";
                objTicketImprimir.ListaArticulos = ListaArticulos;

                if (string.IsNullOrEmpty(Observaciones))
                {
                    objTicketImprimir.PieDePagina = "";
                }
                else
                {
                    objTicketImprimir.PieDePagina = Observaciones.Trim();
                }
                //objTicketImprimir.PieDePagina = Observaciones.Trim();
                objTicketImprimir.Firma = string.Concat("Supervisor: ", NombreUsuarioLogueado, "|", "Taquillero: ", user.Nombre, " ", user.Apellido);

                _service.ImprimirTicketAperturaTaquillero(objTicketImprimir);

                #endregion

                _rta.Correcto = true;
                _rta.Elemento = "";
                _rta.Mensaje = "Correcto";

            }
            catch (Exception ex)
            {
                _rta.Correcto = false;
                _rta.Elemento = "";
                _rta.Mensaje = ex.Message;
            }

            return Json(_rta, JsonRequestBehavior.AllowGet);
        }

        //FIN 
        /**INI - Apertura Elementos***/
        public async Task<ActionResult> Elementos()
        {
            //string Fecha = Utilidades.FormatoFechaValido(Utilidades.ObtenerFechaActual()).ToString();
            string Fecha = Utilidades.ObtenerFechaActual();
            ViewBag.FechaActual = Fecha;

            var item = new Apertura();
            item.TipoElementos = await GetAsync<IEnumerable<TipoGeneral>>("Apertura/ObtenerTipoElementos");
            //item.ListaPuntos = await GetAsync<List<Puntos>>("Apertura/ObtenerPuntosConApertura");
            //item.ListaPuntos = await GetAsync<List<Puntos>>("Apertura/ObtenerPuntosParaAperturaElementos");
            item.ListaPuntos = await GetAsync<List<Puntos>>($"Apertura/ObtenerPuntosConAperturaFecha/{Fecha.Replace("/", "-")}");
            if (item.ListaPuntos == null)
                item.ListaPuntos = new List<Puntos>();

            return View(item);
        }

        public async Task<ActionResult> PuntosxFechaConApertura(string Fecha)
        {
            var ListaPuntos = await GetAsync<List<Puntos>>($"Apertura/ObtenerPuntosConAperturaFecha/{Fecha.Replace("/", "-")}");
            if (ListaPuntos == null)
                ListaPuntos = new List<Puntos>();

            return Json(ListaPuntos, JsonRequestBehavior.AllowGet); ;
        }


        public async Task<ActionResult> ElementosPorIdPunto(int IdPunto, string Fecha)
        {
            var resultado = await GetAsync<IEnumerable<AperturaElementos>>($"Apertura/ElementosPorIdPunto/{IdPunto}/{Fecha.Replace("/", "-")}");
            resultado = resultado.Where(l => !l.EsReabastecimiento);
            if (resultado != null)
            {
                return Json(new RespuestaViewModel { Correcto = true, Elemento = resultado }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error. Por favor intentelo de nuevo" });
            }

        }

        public async Task<ActionResult> InsertElementos(AperturaElementosInsert modelo)
        {
            modelo.IdUsuario = (Session["UsuarioAutenticado"] as Usuario).Id;
            if (await PostAsync<AperturaElementosInsert, string>("Apertura/InsertElementos", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error. Por favor intentelo de nuevo" });
        }

        public async Task<ActionResult> EliminarElementoPorIdAperturaElemento(AperturaElementos objModel)
        {
            if (await PostAsync<AperturaElementos, string>("Apertura/EliminarElementoPorIdAperturaElemento", objModel) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error. Por favor intentelo de nuevo" });
        }

        /**FIN - Apertura Elementos***/

        //GALD1 Entrega de inventario operacional - Supervisor-Punto

        [HttpGet]
        public async Task<ActionResult> EntregaInventarioPuntos()
        {
            var _detalleInventario = await GetAsync<DetalleInventario>($"Apertura/DetalleInventario/{IdPunto.ToString()}");
            var apertura = _detalleInventario == null ? null : _detalleInventario.Apertura.FirstOrDefault();

            ViewBag.IsApertura = true;
            if (apertura == null || apertura.AperturaBase.Count() <= 0)
            {
                apertura = new Apertura();
                ViewBag.IsApertura = false;
            }
            if (apertura.IdSupervisor != (Session["UsuarioAutenticado"] as Usuario).Id)
            {
                apertura = new Apertura();
                ViewBag.IsApertura = false;
            }

            //if(apertura.AperturaBase == null)
            //    return View(_detalleInventario);


            apertura.AperturaElemento = apertura.AperturaElemento != null ? apertura.AperturaElemento.Where(l => !l.EsReabastecimiento) : new List<AperturaElementos>();
            //apertura.AperturaBrazalete = apertura.AperturaBrazalete != null ? apertura.AperturaBrazalete.Where(l => !l.EsReabastecimiento) : new List<AperturaBrazalete>();


            var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerBrazaletesSupervisor/"+ (Session["UsuarioAutenticado"] as Usuario).Id);
            //apertura.TiposDenominacion = _detalleInventario.TipoDenomicacionMoneda;
            //apertura.AperturaBrazalete = await GetAsync<IEnumerable<AperturaBrazalete>>($"Apertura/ObtenerListaAperturaBrazalete/{apertura.Id}");
            apertura.TiposBrazaletes = _listTipoBzalete; //.Where(x => x.Estado == Enumerador.Estados.Activo.ToString());

            if (_listTipoBzalete == null)
            {
                ViewBag.tipoBrazalete = new List<TipoBrazalete>();
            }
            else
            {
                ViewBag.tipoBrazalete = _listTipoBzalete.Where(x => x.Id == (int)Enumerador.Estados.Activo);
            }
            //string strPerfiles = ((int)Enumerador.Perfiles.Taquilla).ToString();
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesAperturaEntregaInventarioPuntos");
            strPerfiles = objParametro.Valor;
            
            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosSinAperturaPorPefil?idsPerfil={strPerfiles}&Tipo=0");
            ViewBag.supervisor = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });

            ViewBag.AperturaBase = apertura;
            ViewBag.Cantidad1 = "Cantidad supervisor";
            ViewBag.Cantidad2 = "Cantidad taquillero";
            ViewBag.issupervisor = false;

            return View(_detalleInventario);
        }

        //Bitacora Elemento EDSP
        [HttpGet]
        public async Task<ActionResult> BitacoraElementos()
        {
            ViewBag.Title = "Entrega elementos";
            ViewBag.Apertura = true;
            var _Apertura = new Apertura();

            var _detalleInventario = await GetAsync<DetalleInventario>($"Apertura/DetalleInventario/{IdPunto.ToString()}");

            if (_detalleInventario == null
                || _detalleInventario.Apertura.Count() == 0
                || _detalleInventario.Apertura.First().IdEstado != (int)Enumerador.Estados.Entregado)
                ViewBag.Apertura = false;
            else
                _Apertura = _detalleInventario.Apertura.Single();


            ViewBag.modeloApertura = _Apertura;


            return View();
        }


        //Fin Bitacora elemento

    }


}