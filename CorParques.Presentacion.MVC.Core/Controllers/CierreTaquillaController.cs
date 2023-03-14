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
using System.Data;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class CierreTaquillaController : ControladorBase
    {

        #region Declaraciones

        private readonly IServicioImprimir _service;

        public CierreTaquillaController(IServicioImprimir service)
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
            bool blnHayRecoleccion = true;
            //RDSH: Se envia 1 en el ultimo parametro para indicar que es una preparacion para cierre de taquilla.
            objRecoleccion = await GetAsync<Recoleccion>($"Recoleccion/ObtenerRecoleccionActiva/{IdUsuario}/{IdPunto}/{1}/{(int)Enumerador.Estados.Activo}");
            if (objRecoleccion == null)
            {
                objRecoleccion = new Recoleccion();
                blnHayRecoleccion = false;
            }
            var listaTipoDenominacion = await TipoDenominacion();
            objRecoleccion.TipoDenominacion = listaTipoDenominacion;
            objRecoleccion.DocumentosRecoleccion = await DocumentosRecoleccion();
            objRecoleccion.objRecoleccionAuxliar = await ObtenerTopesRecoleccion();
            objRecoleccion.CierreBrazalete = await CierreBrazalete();
            objRecoleccion.NovedadesArqueo = await NovedadesArqueo();

            if (!blnHayRecoleccion)
            {
                return View(objRecoleccion);
            }
            else
            {
                return View("Index_Editar", objRecoleccion);
            }
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
            var objRecoleccionAuxliar = await GetAsync<RecoleccionAuxliar>($"Recoleccion/ObtenerTopesCierreTaquilla/{IdUsuario}/{IdPunto}");
            return objRecoleccionAuxliar;
        }

        public async Task<IEnumerable<NovedadArqueo>> NovedadesArqueo()
        {
            var objNovedadArqueo = await GetAsync<IEnumerable<NovedadArqueo>>($"Recoleccion/ObtenerNovedadesRecoleccion/{IdUsuario}");
            return objNovedadArqueo;
        }

        [HttpPost]
        public async Task<ActionResult> Insert(Recoleccion modelo)
        {

            IEnumerable<DetalleRecoleccionMonetaria> Efectivo = null;
            IEnumerable<DetalleRecoleccionDocumento> Voucher = null;
            IEnumerable<DetalleRecoleccionDocumento> Documento = null;
            //IEnumerable<DetalleRecoleccionBrazalete> Brazalete = null;
            IEnumerable<DetalleRecoleccionNovedad> Novedades = null;
            string strResultado = string.Empty;

            try
            {

                if (modelo.SobreBilletesBase == null)
                    modelo.SobreBilletesBase = string.Empty;

                if (modelo.SobreMonedasBase == null)
                    modelo.SobreMonedasBase = string.Empty;

                if (modelo.SobreVoucher == null)
                    modelo.SobreVoucher = string.Empty;

                if (modelo.SobreDocumentos == null)
                    modelo.SobreDocumentos = string.Empty;

                if (modelo.SobreNovedad == null)
                    modelo.SobreNovedad = string.Empty;

                if (modelo.RecoleccionBase != null)
                {
                    Efectivo = modelo.RecoleccionBase.Where(b => b.CantidadTaquillero > 0).ToList();
                    Efectivo.Where(x => x.Tipo == "Billete").ToList().ForEach(x => x.NumeroSobre = modelo.SobreBilletesBase);
                    Efectivo.Where(x => x.Tipo == "Moneda").ToList().ForEach(x => x.NumeroSobre = modelo.SobreMonedasBase);
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
                //if (modelo.RecoleccionBrazalete != null)
                //{
                //    modelo.RecoleccionBrazalete.ToList().ForEach(x => x.NumeroSobre = modelo.SobreBoleteria);
                //}                  
                
                //RDSH: Se debe permitir guardar las que estan en cero para cuando vaya a editar.
                //if (modelo.RecoleccionBrazalete != null)
                //    Brazalete = modelo.RecoleccionBrazalete.Where(b => b.CantidadTaquillero > 0).ToList();

                modelo.RecoleccionBase = Efectivo;
                modelo.RecoleccionVoucher = Voucher;
                modelo.RecoleccionDocumentos = Documento;
                //modelo.RecoleccionBrazalete = Brazalete;
                modelo.RecoleccionNovedad = Novedades;

                modelo.IdPunto = IdPunto;
                modelo.IdUsuarioCreacion = IdUsuario;
                modelo.FechaCreacion = DateTime.Now;
                modelo.IdEstado = (int)Enumerador.Estados.Activo;
                if (modelo.IdRecoleccion > 0)
                {
                    modelo.IdUsuarioModificacion = IdUsuario;
                    modelo.FechaModificacion = DateTime.Now;
                }
                modelo.Cierre = true;
                var resultado = await PostAsync<Recoleccion, string>("Recoleccion/Insertar", modelo);
                if (resultado.Correcto)
                {
                    strResultado = await GenerarReciboRetorno(modelo);
                    if (strResultado.Trim().Length > 0)
                    {
                        return await Index(string.Concat("No fue posible generar todas las impresiones debido a lo siguiente: ", strResultado));
                    }
                    else
                    {
                        return await Index("El proceso preparación de cierre de taquilla se realizó satisfactoriamente.");
                    }                    
                }
                else
                {
                    return await Index(string.Concat("Error: ", "Ocurrió un error en el proceso de preparación de cierre de taquilla, por favor inténtelo mas tarde."));
                }
                //return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return await Index(string.Concat("Error: ", ex.Message));
            }
            finally
            {
                Efectivo = null;
                Voucher = null;
                Documento = null;
                //Brazalete = null;
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
                if (objRecoleccion.RecoleccionVoucher != null)
                {
                    strRetorno = string.Concat(strRetorno, await GenerarRecibosVoucher(objRecoleccion));
                }
                if (objRecoleccion.RecoleccionDocumentos != null)
                {
                    strRetorno = string.Concat(strRetorno, await GenerarRecibosDocumentos(objRecoleccion));
                }
                if (objRecoleccion.RecoleccionBrazalete != null)
                {
                    strRetorno = await GenerarRecibosBrazalete(objRecoleccion);
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
                    objTicketImprimir.TituloRecibo = string.Concat("EFECTIVO - Sobre Billetes: ", objRecoleccion.SobreBilletesBase);
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
                    objTicketImprimir.TituloRecibo = string.Concat("EFECTIVO - Sobre Monedas: ", objRecoleccion.SobreMonedasBase);
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
                    objTicketImprimir.Usuario = UsuarioLogueado;
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
                    objTicketImprimir.Usuario = UsuarioLogueado;
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
        /// Genera los recibos para los brazaletes
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<string> GenerarRecibosBrazalete(Recoleccion objRecoleccion)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            Articulo objArticulo = null;
            List<Articulo> ListaArticulos = null;
            string strRetorno = string.Empty;

            try
            {
                                
                ListaArticulos = new List<Articulo>();
                objTicketImprimir.TituloRecibo = string.Concat("BOLETERÍA");
                objTicketImprimir.TituloColumnas = "Tipo|Cant";

                foreach (DetalleRecoleccionBrazalete objBrazalete in objRecoleccion.RecoleccionBrazalete)
                {
                    objArticulo = new Articulo();
                    objArticulo.Nombre = objBrazalete.TipoBrazalete;
                    objArticulo.Cantidad = objBrazalete.CantidadTaquillero;                    
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
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarRecibosBrazalete_RecoleccionController: ", ex.Message);
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
                    objTicketImprimir.Usuario = UsuarioLogueado;
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

    }
}