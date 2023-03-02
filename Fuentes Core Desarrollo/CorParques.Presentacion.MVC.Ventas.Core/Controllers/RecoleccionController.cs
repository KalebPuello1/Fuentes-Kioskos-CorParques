

using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Ventas.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Transversales.Contratos;
using System.Data;
using System.Configuration;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class RecoleccionController : ControladorBase
    {

        #region Declaraciones

        private readonly IServicioImprimir _service;


        public RecoleccionController(IServicioImprimir service)
        {
            _service = service;
        }

        #endregion

        #region Propiedades

        public int IdUsuario
        {
            get { return (Session["UsuarioAutenticado"] as Usuario).Id; }
        }

        #endregion

        #region Metodos

        public async Task<ActionResult> Index()
        {

            Recoleccion objRecoleccion;
            //bool blnHayRecoleccion = true;

            //objRecoleccion = await GetAsync<Recoleccion>($"Recoleccion/ObtenerRecoleccionActiva/{IdUsuario}/{IdPunto}/{0}/{(int)Enumerador.Estados.Activo}");
            //if (objRecoleccion == null)
            //{
            //    objRecoleccion = new Recoleccion();
            //    blnHayRecoleccion = false;
            //}
            objRecoleccion = new Recoleccion();
            var listaTipoDenominacion = await TipoDenominacion();
            objRecoleccion.TipoDenominacion = listaTipoDenominacion;
            objRecoleccion.DocumentosRecoleccion = await DocumentosRecoleccion();
            objRecoleccion.objRecoleccionAuxliar = await ObtenerTopesRecoleccion();
            //objRecoleccion.CierreBrazalete = await CierreBrazalete();
            objRecoleccion.NovedadesArqueo = await NovedadesArqueo();

            //if (!blnHayRecoleccion)
            //{
            //    return View(objRecoleccion);
            //}
            //else
            //{
            //    return View("Index_Editar", objRecoleccion);
            //}

            return View(objRecoleccion);

        }


        public async Task<ActionResult> RecoleccionGeneral()
        {

            Recoleccion objRecoleccion;
            
            objRecoleccion = new Recoleccion();
            var listaTipoDenominacion = await TipoDenominacion();
            objRecoleccion.TipoDenominacion = listaTipoDenominacion;
            objRecoleccion.DocumentosRecoleccion = await DocumentosRecoleccion();
            objRecoleccion.objRecoleccionAuxliar = await ObtenerTopesRecoleccion();
            //objRecoleccion.CierreBrazalete = await CierreBrazalete();
            objRecoleccion.NovedadesArqueo = await NovedadesArqueo();

            //if (!blnHayRecoleccion)
            //{
            //    return View(objRecoleccion);
            //}
            //else
            //{
            //    return View("Index_Editar", objRecoleccion);
            //}

            return View(objRecoleccion);

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

        public async Task<IEnumerable<MediosPagoFactura>> DocumentosRecoleccion()
        {
            var objMediosPagoFactura = await GetAsync<IEnumerable<MediosPagoFactura>>($"Recoleccion/ObtenerDocumentosRecoleccion/{IdUsuario}");
            return objMediosPagoFactura;
        }

        public async Task<IEnumerable<CierreBrazalete>> CierreBrazalete()
        {
            var objCierreBrazalete = await GetAsync<IEnumerable<CierreBrazalete>>($"Recoleccion/ObtenerBrazaletesRestantes/{IdUsuario}/{IdPunto}");
            return objCierreBrazalete;
            //return null;
        }

        public async Task<RecoleccionAuxliar> ObtenerTopesRecoleccion()
        {
            var objRecoleccionAuxliar = await GetAsync<RecoleccionAuxliar>($"Recoleccion/ObtenerTopesRecoleccion/{IdUsuario}/{IdPunto}");
            return objRecoleccionAuxliar;
        }

        public async Task<IEnumerable<NovedadArqueo>> NovedadesArqueo()
        {
            var objNovedadArqueo = await GetAsync<IEnumerable<NovedadArqueo>>($"Recoleccion/ObtenerNovedadesRecoleccion/{IdUsuario}");
            return objNovedadArqueo;
        }
        
        public async Task<ActionResult> Insert(Recoleccion modelo)
        {

            IEnumerable<DetalleRecoleccionMonetaria> Base = null;
            IEnumerable<DetalleRecoleccionMonetaria> Corte = null;
            IEnumerable<DetalleRecoleccionDocumento> Voucher = null;
            IEnumerable<DetalleRecoleccionDocumento> Documento = null;
            IEnumerable<CierreBrazalete> Brazalete = null;
            IEnumerable<DetalleRecoleccionNovedad> Novedades = null;
            string strResultado = string.Empty;

            try
            {

                if (modelo.SobreBilletesBase == null)
                    modelo.SobreBilletesBase = string.Empty;

                if (modelo.SobreMonedasBase == null)
                    modelo.SobreMonedasBase = string.Empty;


                if (modelo.SobreBilletesCorte == null)
                    modelo.SobreBilletesCorte = string.Empty;

                if (modelo.SobreMonedasCorte == null)
                    modelo.SobreMonedasCorte = string.Empty;

                if (modelo.SobreVoucher == null)
                    modelo.SobreVoucher = string.Empty;

                if (modelo.SobreDocumentos == null)
                    modelo.SobreDocumentos = string.Empty;

                if (modelo.SobreNovedad == null)
                    modelo.SobreNovedad = string.Empty;


                if (modelo.RecoleccionBase != null)
                {
                    Base = modelo.RecoleccionBase.Where(b => b.CantidadTaquillero > 0).ToList();
                    Base.Where(x => x.Tipo == "Billete").ToList().ForEach(x => x.NumeroSobre = modelo.SobreBilletesBase);
                    Base.Where(x => x.Tipo == "Moneda").ToList().ForEach(x => x.NumeroSobre = modelo.SobreMonedasBase);
                }
                if (modelo.RecoleccionCorte != null)
                {
                    Corte = modelo.RecoleccionCorte.Where(c => c.CantidadTaquillero > 0).ToList();
                    Corte.Where(x => x.Tipo == "Billete").ToList().ForEach(x => x.NumeroSobre = modelo.SobreBilletesCorte);
                    Corte.Where(x => x.Tipo == "Moneda").ToList().ForEach(x => x.NumeroSobre = modelo.SobreMonedasCorte);
                }
                if (modelo.RecoleccionVoucher != null)
                {
                    Voucher = modelo.RecoleccionVoucher.Where(v => v.RevisionTaquillero == true).ToList();
                    Voucher.ToList().ForEach(x => x.NumeroSobre = modelo.SobreVoucher);
                }
                if (modelo.RecoleccionDocumentos != null)
                {
                    Documento = modelo.RecoleccionDocumentos.Where(d => d.RevisionTaquillero == true).ToList();
                    Documento.ToList().ForEach(x => x.NumeroSobre = modelo.SobreDocumentos);
                }
                if (modelo.RecoleccionNovedad != null)
                {
                    Novedades = modelo.RecoleccionNovedad.Where(n => n.RevisionTaquillero == true).ToList();
                    Novedades.ToList().ForEach(x => x.NumeroSobre = modelo.SobreNovedad);
                }
                if (modelo.CierreBrazalete != null)
                {
                    Brazalete = modelo.CierreBrazalete.ToList();
                }

                modelo.RecoleccionBase = Base;
                modelo.RecoleccionCorte = Corte;
                modelo.RecoleccionVoucher = Voucher;
                modelo.RecoleccionDocumentos = Documento;
                modelo.RecoleccionNovedad = Novedades;
                modelo.CierreBrazalete = Brazalete;

                modelo.IdPunto = IdPunto;
                modelo.IdUsuarioCreacion = IdUsuario;
                modelo.FechaCreacion = DateTime.Now;
                modelo.IdEstado = (int)Enumerador.Estados.Activo;
                if (modelo.IdRecoleccion > 0)
                {
                    modelo.IdUsuarioModificacion = IdUsuario;
                    modelo.FechaModificacion = DateTime.Now;
                }
                modelo.Cierre = false;
                var resultado = await PostAsync<Recoleccion, string>("Recoleccion/Insertar", modelo);
                if (resultado.Correcto)
                {
                    strResultado = await GenerarReciboRetorno(modelo);
                    if (strResultado.Trim().Length > 0)
                    {
                        return Json(new RespuestaViewModel { Correcto = true, Mensaje = string.Concat("No fue posible generar todas las impresiones debido a lo siguiente: ", strResultado) }, JsonRequestBehavior.AllowGet);

                        //return await Index(string.Concat("No fue posible generar todas las impresiones debido a lo siguiente: ", strResultado));
                    }
                    else
                    {
                        return Json(new RespuestaViewModel { Correcto = true, Mensaje = string.Empty }, JsonRequestBehavior.AllowGet);

                        //return await Index("El proceso de recolección de taquilla se realizó satisfactoriamente.");
                    }
                }
                else
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = string.Concat("Error: ", "Ocurrió un error en el proceso de recolección de taquilla por favor inténtelo mas tarde.") }, JsonRequestBehavior.AllowGet);
                    //return await Index(string.Concat("Error: ", "Ocurrió un error en el proceso de recolección de taquilla por favor inténtelo mas tarde."));
                }
                //return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = string.Concat("Error: ", ex.Message) }, JsonRequestBehavior.AllowGet);

                //return await Index();
            }
            finally
            {
                Base = null;
                Corte = null;
                Voucher = null;
                Documento = null;
                Novedades = null;
            }
        }

        /// <summary>
        /// RDSH: Genera la impresión del recibo para cada una de las opciones.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GenerarReciboRetorno(Recoleccion objRecoleccion)
        {

            string strRetorno = string.Empty;

            try
            {

                if (objRecoleccion.RecoleccionBase != null)
                {
                    strRetorno = await GenerarRecibosBase(objRecoleccion);
                }
                if (objRecoleccion.RecoleccionCorte != null)
                {
                    strRetorno = string.Concat(strRetorno, await GenerarRecibosCorte(objRecoleccion));
                }
                if (objRecoleccion.RecoleccionVoucher != null)
                {
                    strRetorno = string.Concat(strRetorno, await GenerarRecibosVoucher(objRecoleccion));
                }
                if (objRecoleccion.RecoleccionDocumentos != null)
                {
                    strRetorno = string.Concat(strRetorno, await GenerarRecibosDocumentos(objRecoleccion));
                }
                if (objRecoleccion.RecoleccionNovedad != null)
                {
                    strRetorno = string.Concat(strRetorno, await GenerarRecibosNovedades(objRecoleccion));
                }

            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarReciboRetorno_RecoleccionController: ", ex.Message);
            }

            return strRetorno;
        }

        /// <summary>
        /// RDSH: Genera las impresiones para base.
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<string> GenerarRecibosBase(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = null;
            List<Articulo> ListaArticulos = null;
            string strRetorno = string.Empty;

            try
            {


                if (objRecoleccion.SobreBilletesBase.Trim().Length > 0 && objRecoleccion.RecoleccionBase.Where(x => x.Tipo == "Billete").Count() > 0)
                {
                    ListaArticulos = new List<Articulo>();
                    objTicketImprimir.TituloRecibo = string.Concat("BASE - Sobre Billetes: ", objRecoleccion.SobreBilletesBase);
                    objTicketImprimir.TituloColumnas = "Denominación|Cant|Total";

                    foreach (DetalleRecoleccionMonetaria objBilletes in objRecoleccion.RecoleccionBase.Where(x => x.Tipo == "Billete"))
                    {
                        objArticulo = new Articulo();
                        objArticulo.Nombre = Utilidades.FormatoMoneda(double.Parse(objBilletes.Denominacion));
                        objArticulo.Cantidad = objBilletes.CantidadTaquillero;
                        objArticulo.Precio = (objBilletes.CantidadTaquillero * double.Parse(objBilletes.Denominacion));
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }

                    objTicketImprimir.Usuario = NombreUsuarioLogueado;
                    objTicketImprimir.ListaArticulos = ListaArticulos;
                    objTicketImprimir.PieDePagina = "";
                    //strRetorno = _service.ImprimirTicketCortesias(objTicketImprimir);
                    strRetorno = _service.ImprimirTicketRecoleccion(objTicketImprimir, true, false);
                    ListaArticulos = null;
                }
                if (objRecoleccion.SobreMonedasBase.Trim().Length > 0 && objRecoleccion.RecoleccionBase.Where(x => x.Tipo == "Moneda").Count() > 0)
                {
                    ListaArticulos = new List<Articulo>();
                    objTicketImprimir.TituloRecibo = string.Concat("BASE - Sobre Monedas: ", objRecoleccion.SobreMonedasBase);
                    objTicketImprimir.TituloColumnas = "Denominación|Cant|Total";

                    foreach (DetalleRecoleccionMonetaria objBilletes in objRecoleccion.RecoleccionBase.Where(x => x.Tipo == "Moneda"))
                    {
                        objArticulo = new Articulo();
                        objArticulo.Nombre = Utilidades.FormatoMoneda(double.Parse(objBilletes.Denominacion));
                        objArticulo.Cantidad = objBilletes.CantidadTaquillero;
                        objArticulo.Precio = (objBilletes.CantidadTaquillero * double.Parse(objBilletes.Denominacion));
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }

                    objTicketImprimir.Usuario = NombreUsuarioLogueado;
                    objTicketImprimir.ListaArticulos = ListaArticulos;
                    objTicketImprimir.PieDePagina = "";
                    //strRetorno = _service.ImprimirTicketCortesias(objTicketImprimir);
                    strRetorno = _service.ImprimirTicketRecoleccion(objTicketImprimir, true, false);
                }

            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarRecibosBase_RecoleccionController: ", ex.Message);
            }
            finally
            {
                objTicketImprimir = null;
                objArticulo = null;
                ListaArticulos = null;
            }

            return strRetorno;
        }

        /// <summary>
        /// RDSH: Genera las impresiones para corte.
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<string> GenerarRecibosCorte(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = null;
            List<Articulo> ListaArticulos = null;
            string strRetorno = string.Empty;

            try
            {

                if (objRecoleccion.SobreBilletesCorte.Trim().Length > 0 && objRecoleccion.RecoleccionCorte.Where(x => x.Tipo == "Billete").Count() > 0)
                {
                    ListaArticulos = new List<Articulo>();
                    objTicketImprimir.TituloRecibo = string.Concat("CORTE - Sobre Billetes: ", objRecoleccion.SobreBilletesCorte);
                    objTicketImprimir.TituloColumnas = "Denominación|Cant|Total";

                    foreach (DetalleRecoleccionMonetaria objBilletes in objRecoleccion.RecoleccionCorte.Where(x => x.Tipo == "Billete"))
                    {
                        objArticulo = new Articulo();
                        objArticulo.Nombre = Utilidades.FormatoMoneda(double.Parse(objBilletes.Denominacion));
                        objArticulo.Cantidad = objBilletes.CantidadTaquillero;
                        objArticulo.Precio = (objBilletes.CantidadTaquillero * double.Parse(objBilletes.Denominacion));
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }

                    objTicketImprimir.Usuario = NombreUsuarioLogueado;
                    objTicketImprimir.ListaArticulos = ListaArticulos;
                    objTicketImprimir.PieDePagina = "";
                    //strRetorno = _service.ImprimirTicketCortesias(objTicketImprimir);
                    strRetorno = _service.ImprimirTicketRecoleccion(objTicketImprimir, true, false);
                    ListaArticulos = null;
                }
                if (objRecoleccion.SobreMonedasCorte.Trim().Length > 0 && objRecoleccion.RecoleccionCorte.Where(x => x.Tipo == "Moneda").Count() > 0)
                {
                    ListaArticulos = new List<Articulo>();
                    objTicketImprimir.TituloRecibo = string.Concat("CORTE - Sobre Monedas: ", objRecoleccion.SobreMonedasCorte);
                    objTicketImprimir.TituloColumnas = "Denominación|Cant|Total";

                    foreach (DetalleRecoleccionMonetaria objBilletes in objRecoleccion.RecoleccionCorte.Where(x => x.Tipo == "Moneda"))
                    {
                        objArticulo = new Articulo();
                        objArticulo.Nombre = Utilidades.FormatoMoneda(double.Parse(objBilletes.Denominacion));
                        objArticulo.Cantidad = objBilletes.CantidadTaquillero;
                        objArticulo.Precio = (objBilletes.CantidadTaquillero * double.Parse(objBilletes.Denominacion));
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }

                    objTicketImprimir.Usuario = NombreUsuarioLogueado;
                    objTicketImprimir.ListaArticulos = ListaArticulos;
                    objTicketImprimir.PieDePagina = "";
                    //strRetorno = _service.ImprimirTicketCortesias(objTicketImprimir);
                    strRetorno = _service.ImprimirTicketRecoleccion(objTicketImprimir, true, false);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarRecibosCorte_RecoleccionController: ", ex.Message);
            }
            finally
            {
                objTicketImprimir = null;
                objArticulo = null;
                ListaArticulos = null;
            }

            return strRetorno;
        }

        /// <summary>
        /// RDSH: Genera los recibos para voucher.
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<string> GenerarRecibosVoucher(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            string strRetorno = string.Empty;
            DataTable objDataTable;
            DataRow objDataRow;

            try
            {

                if (objRecoleccion.SobreVoucher.Trim().Length > 0)
                {
                    objDataTable = new DataTable();
                    objDataTable.Columns.Add("Franquicia");
                    objDataTable.Columns.Add("Referencia");

                    objTicketImprimir.TituloRecibo = string.Concat("VOUCHER - Sobre: ", objRecoleccion.SobreVoucher);
                    foreach (DetalleRecoleccionDocumento objVoucher in objRecoleccion.RecoleccionVoucher)
                    {
                        objDataRow = objDataTable.NewRow();
                        objDataRow["Franquicia"] = objVoucher.Franquicia;
                        objDataRow["Referencia"] = string.Concat(objVoucher.NumReferencia);
                        objDataTable.Rows.Add(objDataRow);
                        objDataRow = null;
                    }
                    objTicketImprimir.TablaDetalle = objDataTable;
                    objTicketImprimir.Usuario = NombreUsuarioLogueado;
                    objTicketImprimir.PieDePagina = "";
                    strRetorno = _service.ImprimirTicketRecoleccion(objTicketImprimir, true, true);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarRecibosVoucher_RecoleccionController: ", ex.Message);
            }
            finally
            {
                objTicketImprimir = null;
            }

            return strRetorno;
        }

        /// <summary>
        /// RDSH: Genera las impresiones para alistamiento recoleccion de documentos
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<string> GenerarRecibosDocumentos(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            string strRetorno = string.Empty;
            DataTable objDataTable;
            DataRow objDataRow;

            try
            {

                if (objRecoleccion.SobreDocumentos.Trim().Length > 0)
                {
                    objDataTable = new DataTable();
                    objDataTable.Columns.Add("Fecha Hora");
                    objDataTable.Columns.Add("Código Documento");

                    objTicketImprimir.TituloRecibo = string.Concat("DOCUMENTOS - Sobre: ", objRecoleccion.SobreDocumentos);
                    foreach (DetalleRecoleccionDocumento objDocumento in objRecoleccion.RecoleccionDocumentos)
                    {
                        objDataRow = objDataTable.NewRow();
                        objDataRow["Fecha Hora"] = objDocumento.Fecha;
                        objDataRow["Código Documento"] = string.Concat(objDocumento.NumReferencia);
                        objDataTable.Rows.Add(objDataRow);
                        objDataRow = null;
                    }
                    objTicketImprimir.TablaDetalle = objDataTable;
                    objTicketImprimir.Usuario = NombreUsuarioLogueado;
                    objTicketImprimir.PieDePagina = "";
                    strRetorno = _service.ImprimirTicketRecoleccion(objTicketImprimir, true, true);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarRecibosDocumentos_RecoleccionController: ", ex.Message);
            }
            finally
            {
                objTicketImprimir = null;
            }

            return strRetorno;
        }

        /// <summary>
        /// RDSH: Genera la impresion de recibos para la recoleccion de novedades.
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<string> GenerarRecibosNovedades(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            string strRetorno = string.Empty;
            DataTable objDataTable;
            DataRow objDataRow;

            try
            {

                if (objRecoleccion.SobreNovedad.Trim().Length > 0)
                {
                    objDataTable = new DataTable();
                    objDataTable.Columns.Add("Tipo Novedad");
                    objDataTable.Columns.Add("Valor");

                    objTicketImprimir.TituloRecibo = string.Concat("NOVEDADES - Sobre: ", objRecoleccion.SobreNovedad);
                    foreach (DetalleRecoleccionNovedad objNovedad in objRecoleccion.RecoleccionNovedad)
                    {
                        objDataRow = objDataTable.NewRow();
                        objDataRow["Tipo Novedad"] = objNovedad.TipoNovedadNombre;
                        objDataRow["Valor"] = string.Format("{0:C0}", objNovedad.Valor);
                        objDataTable.Rows.Add(objDataRow);
                        objDataRow = null;
                    }
                    objTicketImprimir.TablaDetalle = objDataTable;
                    objTicketImprimir.Usuario = NombreUsuarioLogueado;
                    objTicketImprimir.PieDePagina = "";
                    strRetorno = _service.ImprimirTicketRecoleccion(objTicketImprimir, true, true);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarRecibosNovedades_RecoleccionController: ", ex.Message);
            }
            finally
            {
                objTicketImprimir = null;
            }

            return strRetorno;
        }

        #endregion

        #region RecoleccionGeneral


        public async Task<ActionResult> LoginSupervisor(string user, string pwd)
        {

            var Contrasena = Encripcion.Encriptar(pwd, ConfigurationManager.AppSettings["llaveEncripcion"]);
            var rta = await GetAsync<Usuario>($"Usuario/GetByUserPwd2?user={user}&pwd={Server.UrlEncode(Contrasena)}");
            var resp = new RespuestaViewModel();
            bool isSupervisor = false;

            if (rta != null)
            {
                string strPerfiles = string.Empty;
                Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilSupervisor");
                Parametro objParametroRecolector = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilRecolector");
                strPerfiles = String.Concat(objParametro.Valor,  "," , objParametroRecolector.Valor);


                var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");

                foreach (var item in _listUsuarios)
                {
                    if(rta.Id == item.Id)
                    {
                        isSupervisor = true;
                        break;
                    }
                }
                
                if (!isSupervisor)
                {
                    resp = new RespuestaViewModel { Correcto = false, Elemento = null, Mensaje = "El usuario no esta autorizado." };
                }
                else
                {
                    resp = new RespuestaViewModel { Correcto = true, Elemento = rta, Mensaje = "Contraseña correcta!" };
                }                
            }

            else
            {
                resp = new RespuestaViewModel { Correcto = false, Elemento = null, Mensaje = "Usuario o Contraseña incorrecta!" };
            }
                

            return Json(resp, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<ActionResult> InsertarRecoleccion(Recoleccion modelo)
        {

            IEnumerable<DetalleRecoleccionMonetaria> Base = null;
            IEnumerable<DetalleRecoleccionMonetaria> Corte = null;
            IEnumerable<DetalleRecoleccionDocumento> Voucher = null;
            IEnumerable<DetalleRecoleccionDocumento> Documento = null;
            IEnumerable<CierreBrazalete> Brazalete = null;
            IEnumerable<DetalleRecoleccionNovedad> Novedades = null;
            string strResultado = string.Empty;

            try
            {

                if (modelo.SobreBilletesBase == null)
                    modelo.SobreBilletesBase = string.Empty;

                if (modelo.SobreMonedasBase == null)
                    modelo.SobreMonedasBase = string.Empty;


                if (modelo.SobreBilletesCorte == null)
                    modelo.SobreBilletesCorte = string.Empty;

                if (modelo.SobreMonedasCorte == null)
                    modelo.SobreMonedasCorte = string.Empty;

                if (modelo.SobreVoucher == null)
                    modelo.SobreVoucher = string.Empty;

                if (modelo.SobreDocumentos == null)
                    modelo.SobreDocumentos = string.Empty;

                if (modelo.SobreNovedad == null)
                    modelo.SobreNovedad = string.Empty;


                if (modelo.RecoleccionBase != null)
                {
                    Base = modelo.RecoleccionBase.Where(b => b.CantidadTaquillero > 0).ToList();
                    Base.Where(x => x.Tipo == "Billete").ToList().ForEach(x => x.NumeroSobre = modelo.SobreBilletesBase);
                    Base.Where(x => x.Tipo == "Moneda").ToList().ForEach(x => x.NumeroSobre = modelo.SobreMonedasBase);
                }
                if (modelo.RecoleccionCorte != null)
                {
                    Corte = modelo.RecoleccionCorte.Where(c => c.CantidadTaquillero > 0).ToList();
                    Corte.Where(x => x.Tipo == "Billete").ToList().ForEach(x => x.NumeroSobre = modelo.SobreBilletesCorte);
                    Corte.Where(x => x.Tipo == "Moneda").ToList().ForEach(x => x.NumeroSobre = modelo.SobreMonedasCorte);
                }
                if (modelo.RecoleccionVoucher != null)
                {
                    Voucher = modelo.RecoleccionVoucher.Where(v => v.RevisionTaquillero == true).ToList();
                    Voucher.ToList().ForEach(x => x.NumeroSobre = modelo.SobreVoucher);
                }
                if (modelo.RecoleccionDocumentos != null)
                {
                    Documento = modelo.RecoleccionDocumentos.Where(d => d.RevisionTaquillero == true).ToList();
                    Documento.ToList().ForEach(x => x.NumeroSobre = modelo.SobreDocumentos);
                }
                if (modelo.RecoleccionNovedad != null)
                {
                    Novedades = modelo.RecoleccionNovedad.Where(n => n.RevisionTaquillero == true).ToList();
                    Novedades.ToList().ForEach(x => x.NumeroSobre = modelo.SobreNovedad);
                }
                if (modelo.CierreBrazalete != null)
                {
                    Brazalete = modelo.CierreBrazalete.ToList();
                }

                if (modelo.ValorRecoleccionBase != null)
                {
                    modelo.ValorRecoleccionBase = modelo.ValorRecoleccionBase.Replace(".","");
                }else
                {
                    modelo.ValorRecoleccionBase = "0";
                }
                if (modelo.ValorRecoleccionCorte != null)
                {
                    modelo.ValorRecoleccionCorte = modelo.ValorRecoleccionCorte.Replace(".", "");
                }
                else
                {
                    modelo.ValorRecoleccionCorte = "0";
                }

                if (modelo.CierreBrazalete != null)
                {
                    Brazalete = modelo.CierreBrazalete.ToList();
                }



                modelo.RecoleccionBase = Base;
                modelo.RecoleccionCorte = Corte;
                modelo.RecoleccionVoucher = Voucher;
                modelo.RecoleccionDocumentos = Documento;
                modelo.RecoleccionNovedad = Novedades;
                modelo.CierreBrazalete = Brazalete;

                modelo.IdPunto = IdPunto;
                modelo.IdUsuarioCreacion = IdUsuario;
                modelo.FechaCreacion = DateTime.Now;
                modelo.IdEstado = (int)Enumerador.Estados.EntregadoSupervisor;
                if (modelo.IdRecoleccion > 0)
                {
                    modelo.IdUsuarioModificacion = IdUsuario;
                    modelo.FechaModificacion = DateTime.Now;
                }
                modelo.Cierre = false;
                var resultado = await PostAsync<Recoleccion, string>("Recoleccion/InsertarGeneral", modelo);
                if (resultado.Correcto)
                {
                    modelo.IdRecoleccion = int.Parse(resultado.Elemento.ToString());
                    strResultado = await GenerarReciboRetornoGeneral(modelo);
                    strResultado = await GenerarReciboRetornoGeneral(modelo);
                    if (strResultado.Trim().Length > 0)
                    {
                        return Json(new RespuestaViewModel { Correcto = true, Mensaje = string.Concat("No fue posible generar todas las impresiones debido a lo siguiente: ", strResultado) }, JsonRequestBehavior.AllowGet);

                        //return await Index(string.Concat("No fue posible generar todas las impresiones debido a lo siguiente: ", strResultado));
                    }
                    else
                    {
                        return Json(new RespuestaViewModel { Correcto = true, Mensaje = string.Empty }, JsonRequestBehavior.AllowGet);

                        //return await Index("El proceso de recolección de taquilla se realizó satisfactoriamente.");
                    }
                }
                else
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = string.Concat("Error: ", "Ocurrió un error en el proceso de recolección de taquilla por favor inténtelo mas tarde.") }, JsonRequestBehavior.AllowGet);
                    //return await Index(string.Concat("Error: ", "Ocurrió un error en el proceso de recolección de taquilla por favor inténtelo mas tarde."));
                }
                //return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = string.Concat("Error: ", ex.Message) }, JsonRequestBehavior.AllowGet);

                //return await Index();
            }
            finally
            {
                Base = null;
                Corte = null;
                Voucher = null;
                Documento = null;
                Novedades = null;
            }
        }

        private async Task<string> GenerarReciboRetornoGeneral(Recoleccion objRecoleccion)
        {

            string strRetorno = string.Empty;

            try
            {
                TicketImprimir objTicketImprimir = new TicketImprimir();
                List<Articulo> ListaArticulos = new List<Articulo>();
                
                if (objRecoleccion.ValorRecoleccionBase != null && objRecoleccion.ValorRecoleccionBase != "0")
                {
                    Articulo objArticulo = new Articulo();
                    objArticulo.Grupo = "Recoleccion N° " + objRecoleccion.IdRecoleccion;
                    objArticulo.Nombre = "Recoleccion Base";
                    objArticulo.TituloColumnas = "||";
                    objArticulo.Precio = double.Parse(objRecoleccion.ValorRecoleccionBase);
                    objArticulo.Otro = "";
                    ListaArticulos.Add(objArticulo);
                    objArticulo = null;
                    
                }

                if (objRecoleccion.ValorRecoleccionCorte != null && objRecoleccion.ValorRecoleccionCorte != "0")
                {
                    Articulo objArticulo = new Articulo();
                    objArticulo.Grupo = "Recoleccion N° " + objRecoleccion.IdRecoleccion;
                    objArticulo.Nombre = "Recoleccion Corte";
                    objArticulo.TituloColumnas = "||";
                    objArticulo.Precio = double.Parse(objRecoleccion.ValorRecoleccionCorte);
                    objArticulo.Otro = "";
                    ListaArticulos.Add(objArticulo);
                    objArticulo = null;

                }

                if (objRecoleccion.RecoleccionVoucher != null)
                {
                    foreach (var item in objRecoleccion.RecoleccionVoucher)
                    {
                        Articulo objArticulo = new Articulo();
                        objArticulo.Grupo = "Recoleccion N° " + objRecoleccion.IdRecoleccion;
                        objArticulo.subGrupo = "Recoleccion Vaucher";
                        objArticulo.Nombre = item.NumReferencia;
                        objArticulo.TituloColumnas = "Referencia||Valor";
                        objArticulo.Precio = double.Parse(item.Valor.ToString());
                        objArticulo.Otro = "";
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }
                }
                if (objRecoleccion.RecoleccionDocumentos != null)
                {
                    foreach (var item in objRecoleccion.RecoleccionDocumentos)
                    {
                        Articulo objArticulo = new Articulo();
                        objArticulo.Grupo = "Recoleccion N° " + objRecoleccion.IdRecoleccion;
                        objArticulo.subGrupo = "Recoleccion Documentos";
                        objArticulo.Nombre = item.NumReferencia;
                        objArticulo.TituloColumnas = "Referencia||Valor";
                        objArticulo.Precio = double.Parse(item.Valor.ToString());
                        objArticulo.Otro = "";
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }
                }
                if (objRecoleccion.RecoleccionNovedad != null)
                {
                    foreach (var item in objRecoleccion.RecoleccionNovedad)
                    {
                        Articulo objArticulo = new Articulo();
                        objArticulo.Grupo = "Recoleccion N° " + objRecoleccion.IdRecoleccion;
                        objArticulo.subGrupo = "Recoleccion Novedades";
                        objArticulo.Nombre = item.TipoNovedadNombre;
                        objArticulo.TituloColumnas = "Referencia||Valor";
                        objArticulo.Precio = double.Parse(item.Valor.ToString());
                        objArticulo.Otro = "";
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }
                }

                Articulo objArticuloFinal = new Articulo();
                objArticuloFinal.Grupo = "Recoleccion N° " + objRecoleccion.IdRecoleccion;
                objArticuloFinal.Nombre = "";
                objArticuloFinal.TituloColumnas = "";
                objArticuloFinal.Precio = 0;
                objArticuloFinal.Otro = "";
                ListaArticulos.Add(objArticuloFinal);
                objArticuloFinal = null;

                var user = await GetAsync<Usuario>($"Usuario/GetById?id={objRecoleccion.IdUsuarioSupervisor}&Punto={0}");
                Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/FacturaPosTextoHead1");
                objTicketImprimir.AdicionarContenidoHeader = objParametro.Valor;

                objTicketImprimir.TituloRecibo = "Recoleccion Punto";
                objTicketImprimir.ListaArticulos = ListaArticulos;
                objTicketImprimir.PieDePagina = NombrePunto;
                
                objTicketImprimir.Firma = string.Concat("Supervisor: ", user.Nombre, " ", user.Apellido, "|", "Taquillero: ", NombreUsuarioLogueado);
                _service.ImprimirTicketApertura(objTicketImprimir);                


            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarReciboRetorno_RecoleccionController: ", ex.Message);
            }

            return strRetorno;
        }


        #endregion


        #region CodigoComentareado

        //private async Task<string> GenerarRecibosVoucher1(Recoleccion objRecoleccion)
        //{

        //    TicketImprimir objTicketImprimir = new TicketImprimir();
        //    Articulo objArticulo = null;
        //    List<Articulo> ListaArticulos = null;
        //    string strRetorno = string.Empty;


        //    try
        //    {

        //        if (objRecoleccion.SobreVoucher.Trim().Length > 0)
        //        {

        //            ListaArticulos = new List<Articulo>();
        //            objTicketImprimir.TituloRecibo = string.Concat("VOUCHER - Sobre: ", objRecoleccion.SobreVoucher);
        //            objTicketImprimir.TituloColumnas = "Franquicia|Referencia";

        //            foreach (DetalleRecoleccionDocumento objVoucher in objRecoleccion.RecoleccionVoucher)
        //            {
        //                objArticulo = new Articulo();
        //                objArticulo.Nombre = objVoucher.Franquicia;
        //                objArticulo.Cantidad = 0;
        //                objArticulo.Otro = objVoucher.NumReferencia;
        //                ListaArticulos.Add(objArticulo);
        //                objArticulo = null;
        //            }

        //            objTicketImprimir.Usuario = UsuarioLogueado;
        //            objTicketImprimir.ListaArticulos = ListaArticulos;
        //            objTicketImprimir.PieDePagina = "";
        //            strRetorno = _service.ImprimirTicketCortesias(objTicketImprimir);
        //            ListaArticulos = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        strRetorno = string.Concat("Error en GenerarRecibosVoucher_RecoleccionController: ", ex.Message);
        //    }
        //    finally
        //    {
        //        objTicketImprimir = null;
        //        objArticulo = null;
        //        ListaArticulos = null;
        //    }

        //    return strRetorno;
        //}




        //public async Task<ActionResult> GetList()
        //{
        //    var lista = await GetAsync<IEnumerable<CortesiaDestreza>>($"CortesiaDestreza/ObtenerPorDestrezaAtraccion/{IdPunto}/0");
        //    return PartialView("_List", lista);
        //}

        //public async Task<ActionResult> GetPartial()
        //{
        //    var modelo = new CortesiaDestreza();
        //    var listaDestrezas = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Destreza}");
        //    var listaAtracciones = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
        //    var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");            
        //    modelo.ListaDestrezas = listaDestrezas;
        //    modelo.ListaAtracciones = listaAtracciones;
        //   // modelo.ListaEstados = listaEstado;            
        //    return PartialView("_Create", modelo);
        //}

        //public async Task<ActionResult> Update(CortesiaDestreza modelo)
        //{
        //    modelo.FechaModificacion = DateTime.Now;
        //    modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
        //    var resultado = await PutAsync<CortesiaDestreza, string>("CortesiaDestreza/Actualizar", modelo);
        //    if (string.IsNullOrEmpty(resultado))
        //    {
        //        return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public async Task<ActionResult> Obtener(int Id)
        //{
        //    var item = await GetAsync<CortesiaDestreza>($"CortesiaDestreza/GetById/{Id}");
        //    var listaDestrezas = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Destreza}");
        //    var listaAtracciones = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
        //    var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");
        //    item.ListaDestrezas = listaDestrezas;
        //    item.ListaAtracciones = listaAtracciones;
        //   // item.ListaEstados = listaEstado;
        //    return PartialView("_Edit", item);
        //}

        //public async Task<ActionResult> UpdateEstado(int Id)
        //{
        //    var modelo = await GetAsync<CortesiaDestreza>($"CortesiaDestreza/GetById/{Id}");
        //    modelo.FechaModificacion = DateTime.Now;
        //    modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
        //    modelo.IdEstado = (int)Enumerador.Estados.Inactivo;
        //    var resultado = await PutAsync<CortesiaDestreza, string>("CortesiaDestreza/Eliminar", modelo);
        //    if (string.IsNullOrEmpty(resultado))
        //    {
        //        return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public async Task<ActionResult> Detalle(int Id)
        //{
        //    var item = await GetAsync<CortesiaDestreza>($"CortesiaDestreza/GetById/{Id}");
        //    var listaDestrezas = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Destreza}");
        //    var listaAtracciones = await GetAsync<IEnumerable<TipoGeneral>>($"Puntos/ObtenerxIdTipoPunto/{(int)Enumerador.TiposPuntos.Atraccion}");
        //    var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");
        //    item.ListaDestrezas = listaDestrezas;
        //    item.ListaAtracciones = listaAtracciones;
        //    //item.ListaEstados = listaEstado;
        //    return PartialView("_Detail", item);
        //}

        //public async Task<ActionResult> GenerarCortesia(int IdAtraccion)
        //{

        //    CortesiaDestreza modelo = new CortesiaDestreza();
        //    modelo.IdPuntoDestreza = IdPunto;
        //    modelo.IdPuntoAtraccion = IdAtraccion;
        //    modelo.CodigoBarras = string.Empty;
        //    modelo.Cantidad = 1;
        //    modelo.IdEstado = (int)Enumerador.Estados.Activo;
        //    modelo.FechaCreacion = DateTime.Now;
        //    modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;

        //    var resultado = await PostAsync<CortesiaDestreza, string>("CortesiaDestreza/Insertar", modelo);
        //    if (resultado.Correcto)
        //    {
        //        await GenerarRecibo(resultado.Elemento.ToString());
        //    }
        //    return Json(resultado, JsonRequestBehavior.AllowGet);
        //}

        //private async Task<ActionResult> GenerarRecibo(string strCodigoBarras)
        //{

        //    TicketImprimir objTicketImprimir = new TicketImprimir();
        //    Articulo objArticulo = new Articulo();
        //    List<Articulo> ListaArticulos = new List<Articulo>();

        //    try
        //    {

        //        var objCortesiaDestreza = await GetAsync<CortesiaDestreza>($"CortesiaDestreza/ObtenerPorCodigoBarras/{strCodigoBarras}"); 
        //        objArticulo.Nombre = objCortesiaDestreza.Atraccion.ToUpper();
        //        objArticulo.Cantidad = 1;
        //        objArticulo.Precio = 0;
        //        objArticulo.Otro = "";
        //        ListaArticulos.Add(objArticulo);

        //        objTicketImprimir.CodigoBarrasProp = strCodigoBarras;
        //        objTicketImprimir.TituloRecibo = string.Concat("CORTESIA - ", objCortesiaDestreza.Destreza.ToUpper());
        //        objTicketImprimir.TituloColumnas = "Valido para|Cant";
        //        objTicketImprimir.ListaArticulos = ListaArticulos;
        //        objTicketImprimir.PieDePagina = "Cortesia valida para un solo ingreso en la atracción seleccionada.";
        //        objTicketImprimir.Usuario = objCortesiaDestreza.Usuario;

        //        //_service.ImprimirTicketCortesias(objTicketImprimir);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException(string.Concat("Error en GenerarRecibo_CortesiaDestrezaController: ", ex.Message));
        //    }
        //    finally
        //    {
        //        objTicketImprimir = null;
        //        objArticulo = null;
        //        ListaArticulos = null;
        //    }

        //    return null;
        //}

        #endregion

    }
}