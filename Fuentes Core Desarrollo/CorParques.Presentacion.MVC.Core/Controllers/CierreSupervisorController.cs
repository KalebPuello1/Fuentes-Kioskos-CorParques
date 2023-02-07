using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Web.Mvc;
using CorParques.Transversales.Contratos;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class CierreSupervisorController : ControladorBase
    {

        #region Declaraciones

        private readonly IServicioImprimir _service;

        public CierreSupervisorController(IServicioImprimir service)
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
            var _listUsuarios = await GetAsync<IEnumerable<TipoGeneral>>($"Recoleccion/ObtenerTaquillerosConRecoleccion/{(int)Enumerador.Estados.Activo}/{1}");
            ViewBag.Taquilleros = _listUsuarios;
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> Index(string Resultado)
        {
            ViewBag.Resultado = Resultado;
            return View();
        }
        
        /// <summary>
        /// RDSH: Actualiza la información del cierre.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        //public async Task<ActionResult> Insert(Recoleccion modelo)//recoleccionbrazalete //recoleccionBase //RecoleccionBrazalete
        public async Task<ActionResult> Insert(Recoleccion modelo)//recoleccionbrazalete //recoleccionBase //RecoleccionBrazalete
        {
            string strResultado = string.Empty;

            try
            {
                
                modelo.IdPunto = IdPunto;
                //modelo.IdUsuarioCreacion = IdUsuario;
                //modelo.FechaCreacion = DateTime.Now;
                modelo.IdEstado = (int)Enumerador.Estados.EntregadoSupervisor;              
                modelo.IdUsuarioModificacion = IdUsuario;
                modelo.FechaModificacion = DateTime.Now;
                modelo.IdUsuarioSupervisor = IdUsuario;

                var id = modelo.IdUsuarioCreacion;
                var objRecoleccionn = await GetAsync<Recoleccion>($"Recoleccion/ObtenerRecoleccionActiva/{id}/{IdPunto}/{1}/{(int)Enumerador.Estados.Activo}");
                //modelo.RecoleccionBrazalete = (IEnumerable<DetalleRecoleccionBrazalete>)objRecoleccionn.CierreBrazalete;
                if(modelo.RecoleccionBrazalete == null)
                {
                    if (objRecoleccionn != null)
                    {
                        if (objRecoleccionn.RecoleccionBrazalete != null && objRecoleccionn.RecoleccionBrazalete.Count() > 0)
                        {
                            foreach (var item in objRecoleccionn.RecoleccionBrazalete)
                            {
                                item.CantidadSupervisor = item.CantidadTaquillero;
                            }
                            modelo.RecoleccionBrazalete = objRecoleccionn.RecoleccionBrazalete;
                        }
                    }
                }
                modelo.Cierre = true;
                var resultado = await PostAsync<Recoleccion, string>("Recoleccion/Actualizar", modelo);
                if (resultado.Correcto)
                {

                    strResultado = await GenerarReciboRetorno(modelo);
                    if (strResultado.Trim().Length > 0)
                    {
                        return await Index(string.Concat("No fue posible generar todas las impresiones debido a lo siguiente: ", strResultado));
                    }
                    else
                    {
                        return await Index("El proceso de cierre de taquilla se realizó satisfactoriamente.");
                    }
                    
                }
                else
                {
                    return await Index(string.Concat("Error: ", "Ocurrió un error en el proceso de cierre de taquilla, por favor inténtelo mas tarde."));
                }                
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CierreSupervisorController-Insert");
                return await Index(string.Concat("Error: ", ex.Message));
            }   
        }

        /// <summary>
        /// RDSH: Retorna la recolección del taquillero.
        /// </summary>
        /// <param name="IdTaquillero"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> BuscarRecoleccion(int IdTaquillero)
        {
            Recoleccion objRecoleccion;
            objRecoleccion = await GetAsync<Recoleccion>($"Recoleccion/ObtenerRecoleccionActiva/{IdTaquillero}/{IdPunto}/{1}/{(int)Enumerador.Estados.Activo}");
            if (objRecoleccion != null) //El cambio que se hizo fue este if -> objRecoleccion.RecoleccionVoucher = await DocumentosRecoleccion(IdTaquillero); va fuera de este if
            {
                objRecoleccion.RecoleccionVoucher = await DocumentosRecoleccion(IdTaquillero);
            }
            else
            {
                ViewBag.mensaje = "Se debe hacer alistamiento del taquillero";
            }
            return PartialView("_Wizard", objRecoleccion);
        }
        public async Task<IEnumerable<DetalleRecoleccionDocumento>> DocumentosRecoleccion(int IdTaquillero)
        {
            var objMediosPagoFactura = await GetAsync<IEnumerable<DetalleRecoleccionDocumento>>($"Recoleccion/ObtenerDocumentosRecoleccion/{IdTaquillero}");
            return objMediosPagoFactura;
        }

        /// <summary>
        /// RDSH: Levanta la pantalla contraseña y observaciones.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ObtenerLogin()
        {
            return PartialView("_Login");
        }

        /// <summary>
        /// RDSH: Guarda la observacion de la recoleccion.
        /// </summary>
        /// <param name="objObservacionRecoleccion"></param>
        /// <returns></returns>
        public async Task<JsonResult> GuardarObservaciones(ObservacionRecoleccion objObservacionRecoleccion)
        {

            RespuestaViewModel objRespuestaViewModel = new RespuestaViewModel();

            try
            {

                objObservacionRecoleccion.IdUsuarioCreacion = IdUsuario;
                objObservacionRecoleccion.FechaCreacion = DateTime.Now;
                await PostAsync<ObservacionRecoleccion, string>("ObservacionRecoleccion/Insert", objObservacionRecoleccion);


                objRespuestaViewModel.Correcto = true;
                objRespuestaViewModel.Elemento = "";
                objRespuestaViewModel.Mensaje = "Correcto";

            }
            catch (Exception ex)
            {
                objRespuestaViewModel.Correcto = false;
                objRespuestaViewModel.Elemento = "";
                objRespuestaViewModel.Mensaje = ex.Message;
            }

            return Json(objRespuestaViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// RDSH: Genera la impresión del recibo para cada una de las opciones.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GenerarReciboRetorno(Recoleccion objRecoleccion)
        {

            string strRetorno = string.Empty;
            string strNombreTaquillero = string.Empty;
            Usuario objUsuarioTaquillero = new Usuario();
            List<TicketImprimir> objListaTickets = new List<TicketImprimir>();
            TicketImprimir objTicketImprimir = new TicketImprimir();
            
            try
            {

                objUsuarioTaquillero = await GetAsync<Usuario>($"Usuario/GetById?id={objRecoleccion.IdUsuarioCreacion}&Punto={0}");
                strNombreTaquillero = string.Concat(objUsuarioTaquillero.Nombre, " ", objUsuarioTaquillero.Apellido);

                

                if (objRecoleccion.RecoleccionBase != null)
                {
                    var objTicket = await GenerarRecibosBilletes(objRecoleccion);
                    if (objTicket != null)
                    {
                        if (objTicket.TituloRecibo.Length > 0)
                        {
                            objListaTickets.Add(objTicket);
                        }
                    }
                    objTicket = null;
                    objTicket = await GenerarRecibosMonedas(objRecoleccion);
                    if (objTicket != null)
                    {
                        if (objTicket.TituloRecibo.Length > 0)
                        {
                            objListaTickets.Add(objTicket);
                        }
                    }
                }
                if (objRecoleccion.RecoleccionVoucher != null)
                {
                    if (objRecoleccion.RecoleccionVoucher.Where(x => x.RevisionSupervisor == true).Count() > 0)
                    {
                        var objTicket = await GenerarRecibosVoucher(objRecoleccion);
                        if (objTicket != null)
                        {
                            if (objTicket.TituloRecibo.Length > 0)
                            {
                                objListaTickets.Add(objTicket);
                            }
                        }                        
                    }                    
                }
                if (objRecoleccion.RecoleccionDocumentos != null)
                {
                    if (objRecoleccion.RecoleccionDocumentos.Where(x => x.RevisionSupervisor == true).Count() > 0)
                    {
                        var objTicket = await GenerarRecibosDocumentos(objRecoleccion);
                        if (objTicket != null)
                        {
                            if (objTicket.TituloRecibo.Length > 0)
                            {
                                objListaTickets.Add(objTicket);
                            }
                        }                        
                    }                        
                }
                if (objRecoleccion.RecoleccionBrazalete != null)
                {
                    var objTicket = await GenerarRecibosBrazalete(objRecoleccion);
                    if (objTicket != null)
                    {
                        if (objTicket.TituloRecibo.Length > 0)
                        {
                            objListaTickets.Add(objTicket);
                        }
                    }                    
                }
                if (objRecoleccion.RecoleccionNovedad != null)
                {
                    if (objRecoleccion.RecoleccionNovedad.Where(x => x.RevisionSupervisor == true).Count() > 0)
                    {
                        var objTicket = await GenerarRecibosNovedades(objRecoleccion);
                        if (objTicket != null)
                        {
                            if (objTicket.TituloRecibo.Length > 0)
                            {
                                objListaTickets.Add(objTicket);
                            }
                        }
                        
                    }                        
                }

                if (objRecoleccion.Observaciones == null)
                {
                    objRecoleccion.Observaciones = string.Empty;
                }
                objTicketImprimir.Usuario = NombreUsuarioLogueado;
                objTicketImprimir.Firma = string.Concat("Supervisor: ", NombreUsuarioLogueado, "|", "Taquillero: ", strNombreTaquillero);
                objTicketImprimir.PieDePagina = objRecoleccion.Observaciones.Trim();
                objTicketImprimir.ListaTickets = objListaTickets;
                strRetorno = _service.ImprimirTicketMasivo(objTicketImprimir);

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CierreSupervisorController-GenerarReciboRetorno");
                strRetorno = string.Concat("Error en GenerarReciboRetorno_CierreSupervisorController: ", ex.Message);
            }

            return strRetorno;
        }

        /// <summary>
        /// RDSH: Genera las impresion para billetes.
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<TicketImprimir> GenerarRecibosBilletes(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = null;
            List<Articulo> ListaArticulos = null;            

            try
            {
                if (objRecoleccion.SobreBilletesBase == null)
                    objRecoleccion.SobreBilletesBase = string.Empty;

                if (objRecoleccion.SobreBilletesBase.Trim().Length > 0 && objRecoleccion.RecoleccionBase.Where(x => x.Tipo == "Billete").Count() > 0)
                {
                    ListaArticulos = new List<Articulo>();
                    objTicketImprimir.TituloRecibo = string.Concat("EFECTIVO - Sobre Billetes: ", objRecoleccion.SobreBilletesBase);
                    objTicketImprimir.TituloColumnas = "Denominación|Cant|Total";

                    foreach (DetalleRecoleccionMonetaria objBilletes in objRecoleccion.RecoleccionBase.Where(x => x.Tipo == "Billete"))
                    {
                        objArticulo = new Articulo();
                        objArticulo.Nombre = Utilidades.FormatoMoneda(double.Parse(objBilletes.Denominacion));
                        objArticulo.Cantidad = objBilletes.CantidadSupervisor;
                        objArticulo.Precio = (objBilletes.CantidadSupervisor * double.Parse(objBilletes.Denominacion));
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }                   
                    objTicketImprimir.ListaArticulos = ListaArticulos;                    
                    ListaArticulos = null;
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CierreSupervisorController-GenerarRecibosBilletes");
                return null;
            }
            finally
            {                
                objArticulo = null;
                ListaArticulos = null;
            }

            return objTicketImprimir;
        }

        /// <summary>
        /// RDSH: Genera impresion para monedas
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <param name="strNombreTaquillero"></param>
        /// <returns></returns>
        private async Task<TicketImprimir> GenerarRecibosMonedas(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = null;
            List<Articulo> ListaArticulos = null;

            try
            {
                if (objRecoleccion.SobreMonedasBase == null)
                    objRecoleccion.SobreMonedasBase = string.Empty;

                if (objRecoleccion.SobreMonedasBase.Trim().Length > 0 && objRecoleccion.RecoleccionBase.Where(x => x.Tipo == "Moneda").Count() > 0)
                {
                    ListaArticulos = new List<Articulo>();
                    objTicketImprimir.TituloRecibo = string.Concat("EFECTIVO - Sobre Monedas: ", objRecoleccion.SobreMonedasBase);
                    objTicketImprimir.TituloColumnas = "Denominación|Cant|Total";

                    foreach (DetalleRecoleccionMonetaria objBilletes in objRecoleccion.RecoleccionBase.Where(x => x.Tipo == "Moneda"))
                    {
                        objArticulo = new Articulo();
                        objArticulo.Nombre = Utilidades.FormatoMoneda(double.Parse(objBilletes.Denominacion));
                        objArticulo.Cantidad = objBilletes.CantidadSupervisor;
                        objArticulo.Precio = (objBilletes.CantidadSupervisor * double.Parse(objBilletes.Denominacion));
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;
                    }

                    objTicketImprimir.Usuario = NombreUsuarioLogueado;
                    objTicketImprimir.ListaArticulos = ListaArticulos;
                    ListaArticulos = null;
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CierreSupervisorController-GenerarRecibosMonedas");
                return null;
            }
            finally
            {                
                objArticulo = null;
                ListaArticulos = null;
            }

            return objTicketImprimir;

        }

        /// <summary>
        /// RDSH: Genera los recibos para voucher.
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<TicketImprimir> GenerarRecibosVoucher(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();            
            DataTable objDataTable;
            DataRow objDataRow;

            try
            {
                if (objRecoleccion.SobreVoucher == null)
                    objRecoleccion.SobreVoucher = string.Empty;

                if (objRecoleccion.SobreVoucher.Trim().Length > 0)
                {
                    objDataTable = new DataTable();
                    objDataTable.Columns.Add("Franquicia");
                    objDataTable.Columns.Add("Referencia");

                    objTicketImprimir.TituloRecibo = string.Concat("VOUCHER - Sobre: ", objRecoleccion.SobreVoucher);
                    foreach (DetalleRecoleccionDocumento objVoucher in objRecoleccion.RecoleccionVoucher.Where(x => x.RevisionSupervisor == true))
                    {
                        objDataRow = objDataTable.NewRow();
                        objDataRow["Franquicia"] = objVoucher.Franquicia;
                        objDataRow["Referencia"] = string.Concat(objVoucher.NumReferencia);
                        objDataTable.Rows.Add(objDataRow);
                        objDataRow = null;
                    }
                    objTicketImprimir.EsDataTable = true;
                    objTicketImprimir.TablaDetalle = objDataTable;                    
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CierreSupervisorController-GenerarRecibosVoucher");
                return null;
            }

            return objTicketImprimir;

        }

        /// <summary>
        /// RDSH: Genera las impresiones para alistamiento recoleccion de documentos
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<TicketImprimir> GenerarRecibosDocumentos(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();            
            DataTable objDataTable;
            DataRow objDataRow;

            try
            {
                if (objRecoleccion.SobreDocumentos == null)
                    objRecoleccion.SobreDocumentos = string.Empty;

                if (objRecoleccion.SobreDocumentos.Trim().Length > 0)
                {
                    objDataTable = new DataTable();
                    objDataTable.Columns.Add("Fecha Hora");
                    objDataTable.Columns.Add("Código Documento");

                    objTicketImprimir.TituloRecibo = string.Concat("DOCUMENTOS - Sobre: ", objRecoleccion.SobreDocumentos);
                    foreach (DetalleRecoleccionDocumento objDocumento in objRecoleccion.RecoleccionDocumentos.Where(x => x.RevisionSupervisor == true))
                    {
                        objDataRow = objDataTable.NewRow();
                        objDataRow["Fecha Hora"] = objDocumento.Fecha;
                        objDataRow["Código Documento"] = string.Concat(objDocumento.NumReferencia);
                        objDataTable.Rows.Add(objDataRow);
                        objDataRow = null;
                    }
                    objTicketImprimir.EsDataTable = true;
                    objTicketImprimir.TablaDetalle = objDataTable;                    
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CierreSupervisorController-GenerarRecibosDocumentos");
                return null;
            }            

            return objTicketImprimir;
        }

        /// <summary>
        /// Genera los recibos para los brazaletes
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<TicketImprimir> GenerarRecibosBrazalete(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = null;
            List<Articulo> ListaArticulos = null;
            
            try
            {

                ListaArticulos = new List<Articulo>();
                objTicketImprimir.TituloRecibo = string.Concat("BOLETERÍA");
                objTicketImprimir.TituloColumnas = "Tipo|Cant";

                foreach (DetalleRecoleccionBrazalete objBrazalete in objRecoleccion.RecoleccionBrazalete)
                {
                    objArticulo = new Articulo();
                    objArticulo.Nombre = objBrazalete.TipoBrazalete;
                    objArticulo.Cantidad = objBrazalete.CantidadSupervisor;
                    ListaArticulos.Add(objArticulo);
                    objArticulo = null;
                }                
                objTicketImprimir.ListaArticulos = ListaArticulos;                
                ListaArticulos = null;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CierreSupervisorController-GenerarRecibosBrazaletes");
                return null;
            }
            finally
            {                
                objArticulo = null;
                ListaArticulos = null;
            }

            return objTicketImprimir;
        }

        /// <summary>
        /// RDSH: Genera la impresion de recibos para la recoleccion de novedades.
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<TicketImprimir> GenerarRecibosNovedades(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();            
            DataTable objDataTable;
            DataRow objDataRow;

            try
            {
                if (objRecoleccion.SobreNovedad == null)
                    objRecoleccion.SobreNovedad = string.Empty;

                if (objRecoleccion.SobreNovedad.Trim().Length > 0)
                {
                    objDataTable = new DataTable();
                    objDataTable.Columns.Add("Tipo Novedad");
                    objDataTable.Columns.Add("Valor");

                    objTicketImprimir.TituloRecibo = string.Concat("NOVEDADES - Sobre: ", objRecoleccion.SobreNovedad);
                    foreach (DetalleRecoleccionNovedad objNovedad in objRecoleccion.RecoleccionNovedad.Where(x => x.RevisionSupervisor == true))
                    {
                        objDataRow = objDataTable.NewRow();
                        objDataRow["Tipo Novedad"] = objNovedad.TipoNovedadNombre;
                        objDataRow["Valor"] = string.Format("{0:C0}", objNovedad.Valor);
                        objDataTable.Rows.Add(objDataRow);
                        objDataRow = null;
                    }
                    objTicketImprimir.EsDataTable = true;
                    objTicketImprimir.TablaDetalle = objDataTable;                    
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CierreSupervisorController-GenerarRecibosNovedades");
                return null;
            }           

            return objTicketImprimir;
        }

        #endregion

    }
}