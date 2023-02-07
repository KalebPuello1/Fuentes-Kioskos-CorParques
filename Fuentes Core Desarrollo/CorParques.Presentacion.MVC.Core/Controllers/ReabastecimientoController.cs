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

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class ReabastecimientoController : ControladorBase
    {

        #region Declaraciones

        private readonly IServicioImprimir _service;

        public ReabastecimientoController(IServicioImprimir service)
        {
            _service = service;
        }

        #endregion

        // GET: Reabastecimiento
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Elementos()
        {
            var item = new Apertura();
            item.TipoElementos = await GetAsync<IEnumerable<TipoGeneral>>("Apertura/ObtenerTipoElementos");
            item.ListaPuntos = await GetAsync<List<Puntos>>("Apertura/ObtenerPuntosAperturaEnProceso");
            if (item.ListaPuntos == null)
                item.ListaPuntos = new List<Puntos>();

            return View(item);
        }

        public async Task<ActionResult> ElementosPorIdPunto(int IdPunto)
        {
            string Fecha = DateTime.Now.Date.ToString("dd/MM/yyyy");
            var resultado = await GetAsync<IEnumerable<AperturaElementos>>($"Apertura/ElementosPorIdPunto/{IdPunto}/{Fecha.Replace("/", "-")}");
            resultado = resultado.Where(l => l.EsReabastecimiento);
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
            modelo.Fecha = DateTime.Now.Date.ToString("dd/MM/yyyy");
            if (await PostAsync<AperturaElementosInsert, string>("Apertura/InsertElementos", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error. Por favor intentelo de nuevo" });
        }

        [HttpGet]
        public async Task<ActionResult> EntregaInventario()
        {
            //var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerTodosBrazalete");
            var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>($"TipoBrazalete/ObtenerTodosBrazaleteInventario/{IdPunto}");
            if (_listTipoBzalete == null){
                ViewBag.tipoBrazalete = new List<TipoBrazalete>();
            }
            else {

                ViewBag.tipoBrazalete = _listTipoBzalete.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo);
            }
            //string strPerfiles = (int)Enumerador.Perfiles.Supervisor + "," + (int)Enumerador.Perfiles.Recolector;
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilSupervisorReabastecimiento");
            strPerfiles = objParametro.Valor;

            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
            ViewBag.supervisor = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });

            var _listaPunto = await GetAsync<IEnumerable<Puntos>>("Apertura/ObtenerPuntosConElementosReabastecimiento");
            ViewBag.listaPunto = _listaPunto;
            ViewBag.Cantidad1 = "Cantidad Nido";
            ViewBag.Cantidad2 = "Cantidad Supervisor";
            ViewBag.issupervisor = true;
            ViewBag.listaPunto = _listaPunto ?? new List<Puntos>();

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> EntregaInventarioPuntos()
        {
            var _detalleInventario = await GetAsync<DetalleInventario>($"Apertura/DetalleInventario/{IdPunto.ToString()}");
            var detalleTmp = _detalleInventario;
            var apertura = detalleTmp.Apertura.FirstOrDefault();
            apertura.AperturaElemento = apertura.AperturaElemento.Where(l => l.EsReabastecimiento).ToList();
            //Se adiciona para consultar boletas rebastecimiento por supervisor GALD
            apertura.AperturaBrazalete = await GetAsync<IEnumerable<AperturaBrazalete>>($"Apertura/ObtenerApeturaBrazaleteSup/{(Session["UsuarioAutenticado"] as Usuario).Id}");
            //apertura.AperturaBrazalete = apertura.AperturaBrazalete.Where(l => l.CantidadFinal > 0).ToList();
            
            if (apertura == null)
            {
                apertura = new Apertura();
            }
            //Se comentarea para que cualquier supervisor pueda hacer rebastecimiento GALD
            //if (apertura.IdSupervisor != (Session["UsuarioAutenticado"] as Usuario).Id)
            //{
            //    apertura = new Apertura();
            //}
            var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerTodosBrazalete");
            //apertura.TiposDenominacion = _detalleInventario.TipoDenomicacionMoneda;
            //apertura.AperturaBrazalete = await GetAsync<IEnumerable<AperturaBrazalete>>($"Apertura/ObtenerListaAperturaBrazalete/{apertura.Id}");
            apertura.TiposBrazaletes = _listTipoBzalete.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo && apertura.AperturaBrazalete.Select(y=>y.CodigoSap).Contains(x.CodigoSap));
            ViewBag.tipoBrazalete = apertura.TiposBrazaletes;

            //string strPerfiles = ((int)Enumerador.Perfiles.Taquilla).ToString();
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesReabastecimiento");
            strPerfiles = objParametro.Valor;

            //var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");

            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosSinAperturaPorPefil?idsPerfil={strPerfiles}&Tipo=2");
            ViewBag.supervisor = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });

        

            ViewBag.AperturaBase = apertura;
            ViewBag.Cantidad1 = "Cantidad Supervisor";
            ViewBag.Cantidad2 = "Cantidad taquillero";
            ViewBag.issupervisor = false;

            return View(_detalleInventario);
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerDetalleInventario(DetalleInventario modelo)
        {
            DetalleInventario _detalleInventario = null;

            _detalleInventario = await GetAsync<DetalleInventario>($"Apertura/DetalleInventario/{modelo.hdPuntos}");

            if (_detalleInventario == null)
                _detalleInventario = new DetalleInventario();

            var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerTodosBrazalete");

            _detalleInventario.Observacion = modelo.Observacion;
            _detalleInventario.IdSupervisor = modelo.IdSupervisor;
            _detalleInventario.hdPuntos = modelo.hdPuntos;
            _detalleInventario.Brazaletes = modelo.Brazaletes == null ? new List<BrazaletesApertura>() : modelo.Brazaletes;

            foreach (var item in _detalleInventario.Brazaletes)
            {
                item.Nombre = _listTipoBzalete.Any(l => l.Id == item.Id) ? _listTipoBzalete.FirstOrDefault(l => l.Id == item.Id).Nombre : "";
            }

            ViewBag.Cantidad1 = "Cantidad Nido";
            ViewBag.Cantidad2 = "Cantidad Supervisor";


            ViewBag.issupervisor = true;

            if (_detalleInventario.Apertura == null)
                _detalleInventario.Apertura = new List<Apertura>().ToArray();

            return PartialView("_DetalleInventario", _detalleInventario);

        }

        //Valida si el supervisor tiene brazaletes asignados del día
        [HttpGet]
        public async Task<ActionResult> ObtenerBrazaletesPorSupervisor(int IdSupervisor)
        {
            var _detalleBrazalete = await GetAsync<List<AperturaBrazalete>>($"Apertura/ObtenerListaAperturaBrazalete/{IdSupervisor}");
            if (_detalleBrazalete == null)
                _detalleBrazalete = new List<AperturaBrazalete>();

            //_detalleBrazalete = _detalleBrazalete.Where(l => l.EsReabastecimiento && l.CantidadFinal > 0).ToList();
            return Json(_detalleBrazalete ?? new List<AperturaBrazalete>(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerBrazaletesPorTaquillero(int IdTaquillero)
        {
            var _detalleBrazalete = await GetAsync<IEnumerable<CierreBrazalete>>($"Apertura/ObtenerBoleteriaAsignada/{IdTaquillero}/{false}");
            
            if (_detalleBrazalete == null)
                _detalleBrazalete = new List<CierreBrazalete>();

            //_detalleBrazalete = _detalleBrazalete.Where(l => l.EsReabastecimiento && l.CantidadFinal > 0).ToList();
            return Json(_detalleBrazalete ?? new List<CierreBrazalete>(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ObtenerLogin()
        {
            return PartialView("_Login");
        }

        [HttpPost]
        public async Task<JsonResult> GuardarInformacionInventario(DetalleInventario modeloInventario)
        {

            RespuestaViewModel _rta = new RespuestaViewModel();
            TicketImprimir objTicketImprimir = new TicketImprimir();
            List<Articulo> ListaArticulos = new List<Articulo>();
            List<TicketImprimir> objListaTickets = new List<TicketImprimir>();

            try
            {
                if (modeloInventario.Apertura == null)
                    modeloInventario.Apertura = new List<Apertura>().ToArray();

                List<AperturaBrazalete> _list = new List<AperturaBrazalete>();

                if (modeloInventario.Brazaletes != null)
                {
                    var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerTodosBrazalete");
                    _listTipoBzalete = _listTipoBzalete.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo);

                    var BrazaletesAsignados = await GetAsync<IEnumerable<CierreBrazalete>>($"Apertura/ObtenerBoleteriaAsignada/{modeloInventario.IdSupervisor}/{true}");


                    foreach (var item in modeloInventario.Brazaletes)
                    {
                        var _brazalete = _listTipoBzalete.Where(x => x.Id == item.Id).FirstOrDefault();

                        _list.Add(new AperturaBrazalete
                        {
                            CantidadInicial = item.Cantidad,
                            CantidadFinal = item.Cantidad,
                            Fecha = DateTime.Now,
                            IdBrazalete = item.Id,
                            IdSupervisor = modeloInventario.IdSupervisor,
                            idUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id,
                            Observacion = modeloInventario.Observacion,
                            EsReabastecimiento = true
                        });

                        Articulo objArticulo = new Articulo();
                        var objBrazalete = BrazaletesAsignados.Where(x => x.IdTipoBrazalete == item.Id).FirstOrDefault();
                        if (objBrazalete != null)
                        {
                            //Para la columna Total.
                            objArticulo.Otro = (item.Cantidad + objBrazalete.Asignados).ToString();
                    }
                        else
                        {
                            objArticulo.Otro = item.Cantidad.ToString();
                }
                        //RDSH: Para impresión de boleteria.                        
                        objArticulo.Grupo = "REABASTECIMIENTO BOLETERIA";
                        objArticulo.Nombre = _brazalete.Nombre;
                        objArticulo.TituloColumnas = "TIPO     |CANT|TOTAL";
                        objArticulo.Cantidad = item.Cantidad;                        
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;


                    }
                }


                if (_list.Count > 0)
                    await PostAsync<List<AperturaBrazalete>, string>("Apertura/ActualizarAperturaBrazalete", _list);



                //foreach (Apertura item in modeloInventario.Apertura)
                //{
                //    item.IdEstado = (int)Enumerador.Estados.EnProceso;
                //    item.FechaModificado = DateTime.Now;
                //    item.IdSupervisor = modeloInventario.IdSupervisor;
                //    item.UsuarioModificado = (Session["UsuarioAutenticado"] as Usuario).Id;
                //    await PostAsync<Apertura, string>("AperturaBase/Actualizar", item);
                //}


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
                    

                //RDSH: Impresion de elementos
                var ItemApertura = new Apertura();
                ItemApertura.TipoElementos = await GetAsync<IEnumerable<TipoGeneral>>("Apertura/ObtenerTipoElementos");
                foreach (TipoGeneral TipoElemento in ItemApertura.TipoElementos)
                {
                    var aperturasElementos = modeloInventario.Apertura.Where(Y => Y.AperturaElemento != null).Select(X => X.AperturaElemento.Where(Z => Z.ValidSupervisor == true));
                    var elementos = new List<TipoElementos>();
                    aperturasElementos.ToList().ForEach(x => elementos.AddRange(x.Select(y => y.Elemento)));
                    var total = elementos.Where(x => x.Id.Equals(TipoElemento.Id)).ToList();

                    if (total.Count() > 0)
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

                var user = await GetAsync<Usuario>($"Usuario/GetById?id={modeloInventario.IdSupervisor}&Punto={0}");
                Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/FacturaPosTextoHead1");
                objTicketImprimir.AdicionarContenidoHeader = objParametro.Valor;                
                objTicketImprimir.ListaArticulos = ListaArticulos;
                if (string.IsNullOrEmpty(modeloInventario.Observacion))
                {
                    objTicketImprimir.PieDePagina = "";
                }
                else
                {
                    objTicketImprimir.PieDePagina = modeloInventario.Observacion.Trim();
                }
                objTicketImprimir.Firma = string.Concat("Caja General: ", NombreUsuarioLogueado, "|", "Supervisor: ", user.Nombre, " ", user.Apellido);
                _service.ImprimirTicketApertura(objTicketImprimir);

                _rta.Correcto = true;
                _rta.Elemento = "";
                _rta.Mensaje = "Correcto";
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReabastecimientoController_GuardarInformaciónInventario");
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
            TicketImprimir objTicketImprimir = new TicketImprimir();
            List<Articulo> ListaArticulos = new List<Articulo>();
            List<TicketImprimir> objListaTickets = new List<TicketImprimir>();

            try
            {

                //Actualizar Moneda
                //foreach (Apertura item in modeloInventario.Apertura)
                //{
                //    item.IdEstado = (int)Enumerador.Estados.Entregado;
                //    item.FechaModificado = DateTime.Now;
                //    item.UsuarioModificado = (Session["UsuarioAutenticado"] as Usuario).Id;
                //    await PostAsync<Apertura, string>("AperturaBase/Actualizar", item);
                //}

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

                //foreach (var item in modeloInventario.Apertura)
                //{
                //    foreach (var item2 in item.AperturaBrazalete)
                //    {
                //        item2.BrazaleteDetalle.Fecha = DateTime.Now;
                //        item2.BrazaleteDetalle.IdAperturaBrazalete = item2.Id;
                //        item2.BrazaleteDetalle.IdEstado = (int)Enumerador.Estados.Activo;
                //        await PostAsync<AperturaBrazaleteDetalle, string>("Apertura/InsertarBrazaleteDetalle", item2.BrazaleteDetalle);

                //    }
                //}
                //Insertar AperturaBrazaleteDetalle

                List<TransladoInventario> _TransladoInventario = new List<TransladoInventario>();
                var _listTipoBzalete = await GetAsync<IEnumerable<TipoBrazalete>>("TipoBrazalete/ObtenerTodosBrazalete");
                _listTipoBzalete = _listTipoBzalete.Where(x => x.IdEstado == (int)Enumerador.Estados.Activo);
                var BrazaletesAsignados = await GetAsync<IEnumerable<CierreBrazalete>>($"Apertura/ObtenerBoleteriaAsignada/{modeloInventario.IdTaquillero}/{false}");

                foreach (var item in modeloInventario.Apertura)
                {
                    if (item.AperturaBrazalete != null)
                    {
                    foreach (var item2 in item.AperturaBrazalete.Where(x=>x.BrazaleteDetalle.Cantidad!=0))
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

                        //RDSH: Impresion de brazaletes
                        var _brazalete = _listTipoBzalete.Where(x => x.Id == item2.IdBrazalete).FirstOrDefault();
                        Articulo objArticulo = new Articulo();
                        var objBrazalete = BrazaletesAsignados.Where(x => x.IdTipoBrazalete == item2.IdBrazalete).FirstOrDefault();
                        if (objBrazalete != null)
                        {
                            //Para la columna Total.
                            objArticulo.Otro = (item2.BrazaleteDetalle.Cantidad + objBrazalete.EnCaja).ToString();
                        }
                        else
                        {
                            objArticulo.Otro = item2.BrazaleteDetalle.Cantidad.ToString();
                        }
                        //RDSH: Para impresión de boleteria.                        
                        objArticulo.Grupo = "REABASTECIMIENTO BOLETERIA";
                        objArticulo.Nombre = _brazalete.Nombre;
                        objArticulo.TituloColumnas = "TIPO     |CANT|TOTAL";
                        objArticulo.Cantidad = item2.BrazaleteDetalle.Cantidad;
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;

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


                //    }

                if (_listElementos.Count > 0)
                {
                    //RDSH: Impresion de elementos
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
                }

                var user = await GetAsync<Usuario>($"Usuario/GetById?id={modeloInventario.IdTaquillero}&Punto={0}");
                Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/FacturaPosTextoHead1");
                objTicketImprimir.NombrePunto = NombrePunto;
                objTicketImprimir.AdicionarContenidoHeader = objParametro.Valor;
                objTicketImprimir.ListaArticulos = ListaArticulos;
                if (string.IsNullOrEmpty(modeloInventario.Apertura.FirstOrDefault().ObservacionPunto))
                {
                    objTicketImprimir.PieDePagina = "";
                }
                else
                {
                    objTicketImprimir.PieDePagina = modeloInventario.Apertura.FirstOrDefault().ObservacionPunto.Trim();
                }
                objTicketImprimir.Firma = string.Concat("Supervisor: ", NombreUsuarioLogueado, "|", "Taquillero: ", user.Nombre, " ", user.Apellido);
                _service.ImprimirTicketApertura(objTicketImprimir);


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
    }
}