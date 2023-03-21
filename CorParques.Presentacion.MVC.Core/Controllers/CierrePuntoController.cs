//Cambioquitar: Este controlador usa el enumerador de perfiles.
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
    public class CierrePuntoController : ControladorBase
    {

        #region Declaraciones

        private readonly IServicioImprimir _service;

        public CierrePuntoController(IServicioImprimir service)
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


        public async Task<ActionResult> Index()
        {

            ViewBag.IsApertura = true;
            var aperturaelemento = await GetAsync<IEnumerable<CierreElementos>>($"CierrePunto/ObtenerAperturaElemento/{IdPunto}");
            if (aperturaelemento == null)
                ViewBag.IsApertura = false;

            return View(aperturaelemento);
        }

        public async Task<ActionResult> GuardarCierrePuntos(IEnumerable<CierreElementos> modelo)
        {
            foreach (var item in modelo)
                item.IdUsuarioPunto = (Session["UsuarioAutenticado"] as Usuario).Id;

            if (await PostAsync<IEnumerable<CierreElementos>, string>("CierrePunto/ActaulizarCierre", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el alistamiento. Por favor intentelo de nuevo" });

        }

        public async Task<ActionResult> ObtenerLogin()
        {
            return PartialView("_Login");
        }

        #region Metodos

        /// <summary>
        /// RDSH: Consulta los usuario con perfil taquillero.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> IndexSupervisor()
        {

            //string strPerfiles = ((int)Enumerador.Perfiles.Taquilla).ToString();
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesCierrePuntoSupervisor");
            strPerfiles = objParametro.Valor;

            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
            ViewBag.Usuarios = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });
            return View();

        }

        /// <summary>
        /// RDSH: Consulta los usuarios con perfil supervisor.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> IndexNido()
        {

            //string strPerfiles = ((int)Enumerador.Perfiles.Supervisor).ToString();
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesCierrePuntoNido");
            strPerfiles = objParametro.Valor;

            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
            ViewBag.Usuarios = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });
            return View();

        }

        [HttpGet]
        public async Task<ActionResult> ObtenerAperturaElementosUsuario(int IdUsuario, int Opcion)
        {
            IEnumerable<CierreElementos> objListaCierreElementos;

            if (Opcion == 1)
            {
                //Trae alistamiento de taquillero para supervisor
                objListaCierreElementos = await GetAsync<IEnumerable<CierreElementos>>($"CierrePunto/ObtenerAperturaElementoSupervisor/{IdUsuario}");
                return PartialView("_DetalleElementos_Supervisor", objListaCierreElementos);
            }
            else
            {
                //Trae revision de supervisor para nido.
                objListaCierreElementos = await GetAsync<IEnumerable<CierreElementos>>($"CierrePunto/ObtenerAperturaElementoNido/{IdUsuario}");
                return PartialView("_DetalleElementos_Nido", objListaCierreElementos);
            }


        }

        /// <summary>
        /// RDSH: Guarda el recibimiento del supervisor.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ActualizarElementosSupervisor(IEnumerable<CierreElementos> modelo)
        {
            string strResultado = string.Empty;

            try
            {
                modelo.ToList().ForEach(x => x.IdUsuarioSupervisor = IdUsuario);
                modelo.ToList().Where(x => x.ObservacionesSupervisor == null).ToList().ForEach(x => x.ObservacionesSupervisor = string.Empty);
                modelo.ToList().Where(x => x.Observacion == null).ToList().ForEach(x => x.Observacion = string.Empty);
                var resultado = await PostAsync<IEnumerable<CierreElementos>, string>("CierrePunto/ActualizarMasivoSupervisor", modelo);
                if (resultado.Correcto)
                {
                    strResultado = await GenerarRecibosCierrePunto(modelo, true);
                    if(strResultado.Trim().Length > 0)
                    {
                        Utilidades.RegistrarError(new Exception(strResultado), "CierrePuntoController_ActualizarElementosSupervisor");
                    }
                }

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Ha ocurrido un error mientras se procesaba la información. Por favor intentelo de nuevo." });
            }

        }

        /// <summary>
        /// RDSH: Guarda el recibimiento del nido.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ActualizarElementosNido(IEnumerable<CierreElementos> modelo)
        {
            string strResultado = string.Empty;

            try
            {
                modelo.ToList().ForEach(x => x.IdUsuarioNido = IdUsuario);
                modelo.ToList().Where(x => x.ObservacionesNido == null).ToList().ForEach(x => x.ObservacionesNido = string.Empty);
                modelo.ToList().Where(x => x.Observacion == null).ToList().ForEach(x => x.Observacion = string.Empty);
                var resultado = await PostAsync<IEnumerable<CierreElementos>, string>("CierrePunto/ActualizarMasivoNido", modelo);
                if (resultado.Correcto)
                {
                    strResultado = await GenerarRecibosCierrePunto(modelo, false);
                    if (strResultado.Trim().Length > 0)
                    {
                        Utilidades.RegistrarError(new Exception(strResultado), "CierrePuntoController_ActualizarElementosNido");
                    }                    
                }

                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Ha ocurrido un error mientras se procesaba la información. Por favor intentelo de nuevo" });
            }

        }


        /// <summary>
        /// RDSH: Genera los recibos para voucher.
        /// </summary>
        /// <param name="objRecoleccion"></param>
        /// <returns></returns>
        private async Task<string> GenerarRecibosCierrePunto(IEnumerable<CierreElementos> objCierreElementos, bool blnSupervisor)
        {

            TicketImprimir objTicketImprimir = new TicketImprimir();
            string strRetorno = string.Empty;
            DataTable objDataTable;
            DataRow objDataRow;
            string strObservaciones = "";
            int? intIdUsuarioFirma = 0;
            Usuario objUsuario = new Usuario();
            string strNombreFirma = string.Empty;

            try
            {

                if (blnSupervisor)
                {
                    intIdUsuarioFirma = objCierreElementos.Where(x => x.IdUsuarioPunto > 0).FirstOrDefault().IdUsuarioPunto;
                }
                else
                {
                    intIdUsuarioFirma = objCierreElementos.Where(x => x.IdUsuarioSupervisor > 0).FirstOrDefault().IdUsuarioSupervisor;
                }

                objUsuario = await GetAsync<Usuario>($"Usuario/GetById?id={intIdUsuarioFirma}&Punto={0}");
                strNombreFirma = string.Concat(objUsuario.Nombre, " ", objUsuario.Apellido);
                if (objCierreElementos.Where(x => x.Observacion.Trim().Length > 0).Count() > 0)
                {
                    strObservaciones = objCierreElementos.Where(x => x.Observacion.Trim().Length > 0).FirstOrDefault().Observacion;
                }               

                objDataTable = new DataTable();
                objDataTable.Columns.Add("Elemento");
                objDataTable.Columns.Add("Estado");

                objTicketImprimir.TituloRecibo = "ELEMENTOS";
                foreach (CierreElementos objCierreElementosFor in objCierreElementos)
                {
                    objDataRow = objDataTable.NewRow();
                    objDataRow["Elemento"] = objCierreElementosFor.NombreElemento;
                    objDataRow["Estado"] = objCierreElementosFor.Estado;
                    objDataTable.Rows.Add(objDataRow);
                    objDataRow = null;
                }
                objTicketImprimir.TablaDetalle = objDataTable;
                objTicketImprimir.Usuario = UsuarioLogueado;
                objTicketImprimir.Firma = string.Concat("Firma: ", NombreUsuarioLogueado, "|", "Firma: ", strNombreFirma); ;
                objTicketImprimir.PieDePagina = strObservaciones.Trim();
                strRetorno = _service.ImprimirTicketCierrePunto(objTicketImprimir);

            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en GenerarRecibosCierrePunto_CierrePuntoController: ", ex.Message);
            }
            finally
            {
                objTicketImprimir = null;
                objUsuario = null;
            }

            return strRetorno;
        }

        #endregion


    }
}