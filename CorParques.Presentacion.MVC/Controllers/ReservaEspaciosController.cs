using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReservaEspaciosController : ControladorBase
    {

        #region Consultar

        public async Task<ActionResult> Index()
        {
            ViewBag.FechaActual = DateTime.Now.ToString("dd-MM-yyyy");
            var modelo = await GetReservas(DateTime.Now.ToString("yyyy-MM-dd"));
            return View(modelo);
        }

        [HttpGet]
        public async Task<ActionResult> BuscarReservas(string FechaReserva)
        {
            return PartialView("_List", await GetReservas(FechaReserva));
        }

        private async Task<IEnumerable<ReservaEspacios>> GetReservas(string FechaReserva)
        {
            IEnumerable<ReservaEspacios> objReservaEspacios = null;

            try
            {
                objReservaEspacios = await GetAsync<IEnumerable<ReservaEspacios>>($"ReservaEspacios/ObtenerReservaEspacios/{0}/{FechaReserva}");
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_BuscarReservas");
            }
            return objReservaEspacios.Count() > 0 ? objReservaEspacios : null;
        }

        #endregion

        #region Crear

        public string FormatoFechaReserva(string strFecha)
        {
            string strFechaArmada = string.Empty;
            string[] strSplit;

            try
            {
                strSplit = strFecha.Split('/');
                strFechaArmada = string.Join("-", strSplit[2], strSplit[1], strSplit[0]);
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_FormatoFechaReserva");                
            }

            return strFechaArmada;
        }
        
        public async Task<ActionResult> Crear()
        {

            ReservaEspacios objReservaEspacios = new ReservaEspacios();

            try
            {

                var objListaTipoEspacio = await GetAsync<IEnumerable<TipoGeneral>>("ReservaEspacios/ObtenerTipoEspacios");
                var objListaTipoReserva = await GetAsync<IEnumerable<TipoReserva>>("ReservaEspacios/ObtenerTiposReserva");
                
                objReservaEspacios.ListaTipoEspacio = objListaTipoEspacio;
                objReservaEspacios.ListaTipoReserva = objListaTipoReserva;
                objReservaEspacios.ListaEspacios = new List<TipoGeneral>();
                objReservaEspacios.HoraInicio = string.Empty;
                objReservaEspacios.HoraFin = string.Empty;
                objReservaEspacios.FechaReserva = DateTime.Now.ToString("dd-MM-yyyy");

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_Crear");
            }

            return View(objReservaEspacios);
        }

        [HttpGet]
        public async Task<ActionResult> ConsultarEspacios(int IdTipoEspacio)
        {

            IEnumerable<TipoGeneral> objListaTipoGeneral = null;

            try
            {
                

                objListaTipoGeneral = await GetAsync<IEnumerable<TipoGeneral>>($"ReservaEspacios/ObtenerEspaciosxTipo/{IdTipoEspacio}");

                if(objListaTipoGeneral==null)
                    objListaTipoGeneral = new List<TipoGeneral>();

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_ConsultarEspacios");
            }

            return Json(objListaTipoGeneral, JsonRequestBehavior.AllowGet);

        }
        
        [HttpGet]
        public async Task<ActionResult> ConsultarDetallePedido(string CodigoSap, string Fecha)
        {

            IEnumerable<ReservaEspaciosAuxiliar> objReservaEspaciosAuxiliar = null;

            try
            {
                Fecha = FormatoFechaReserva(Fecha);
                objReservaEspaciosAuxiliar = await GetAsync<IEnumerable<ReservaEspaciosAuxiliar>>($"ReservaEspacios/ObtenerDetallePedido/{CodigoSap}/{Fecha}");
                if (objReservaEspaciosAuxiliar.Count() == 0)
                    objReservaEspaciosAuxiliar = null;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_ConsultarDetallePedido");
            }
            
            return PartialView("_DetallePedido", objReservaEspaciosAuxiliar);
        }


        [HttpGet]
        public async Task<ActionResult> Insertar(ReservaEspacios modelo)
        {
            string strResultado = string.Empty;

            try
            {

                modelo.IdUsuarioCreacion = IdUsuarioLogueado;
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaReserva = FormatoFechaReserva(modelo.FechaReserva);
                if (modelo.Observaciones == null)
                    modelo.Observaciones = string.Empty;

                var resultado = await PostAsync<ReservaEspacios, string>("ReservaEspacios/Insertar", modelo);           

                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_Insertar");
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Ha ocurrido un error mientras se procesaba la información. Por favor intentelo de nuevo" });
            }

        }

        #endregion

        #region Editar

        public string FormatoFechaCalendar(string strFecha)
        {
            string strFechaArmada = string.Empty;
            string[] strSplit;

            try
            {
                strSplit = strFecha.Split('-');
                strFechaArmada = string.Join("/", strSplit[2], strSplit[1], strSplit[0]);
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_FormatoFechaCalendar");
            }

            return strFechaArmada;
        }

        public ActionResult Editar()
        {
            ViewBag.Titulo = "Editar reserva de espacio";
            ViewBag.Editar = 1;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerReservaEdicion(int NumeroReserva, int Editar)
        {

            IEnumerable<ReservaEspacios> objReservaEspacios = null;
            ReservaEspacios objReservaEspacioRetorno = null;
            string strDummy = "2017-09-21";

            try
            {
                objReservaEspacios = await GetAsync<IEnumerable<ReservaEspacios>>($"ReservaEspacios/ObtenerReservaEspacios/{NumeroReserva}/{strDummy}");

                if (objReservaEspacios.Count() > 0)
                {
                    objReservaEspacioRetorno = objReservaEspacios.FirstOrDefault();

                    var objListaTipoEspacio = await GetAsync<IEnumerable<TipoGeneral>>("ReservaEspacios/ObtenerTipoEspacios");
                    var objListaTipoReserva = await GetAsync<IEnumerable<TipoReserva>>("ReservaEspacios/ObtenerTiposReserva");
                    var objListaEspacios =  await GetAsync<IEnumerable<TipoGeneral>>($"ReservaEspacios/ObtenerEspaciosxTipo/{objReservaEspacioRetorno.IdTipoEspacio}");
                    var objReservaEspaciosAuxiliar = await GetAsync<IEnumerable<ReservaEspaciosAuxiliar>>($"ReservaEspacios/ObtenerDetallePedido/{objReservaEspacioRetorno.CodigoSapPedido}/{objReservaEspacioRetorno.FechaReserva}");
                    if (objReservaEspaciosAuxiliar.Count() == 0)
                        objReservaEspaciosAuxiliar = null;


                    objReservaEspacioRetorno.ListaTipoEspacio = objListaTipoEspacio;
                    objReservaEspacioRetorno.ListaTipoReserva = objListaTipoReserva;
                    objReservaEspacioRetorno.ListaEspacios = objListaEspacios;
                    objReservaEspacioRetorno.DetallePedido = objReservaEspaciosAuxiliar;
                    objReservaEspacioRetorno.FechaReserva = FormatoFechaCalendar(objReservaEspacioRetorno.FechaReserva);
                }
                else
                {
                    objReservaEspacioRetorno = null;
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_ObtenerReservaEdicion");
            }

            if (Editar == 1)
                ViewBag.Accion = "Editar";
            else
                ViewBag.Accion = "Eliminar";

            return PartialView("_Editar", objReservaEspacioRetorno);
        }

        [HttpGet]
        public async Task<ActionResult> Actualizar(ReservaEspacios modelo)
        {
            string strResultado = string.Empty;

            try
            {

                modelo.IdUsuarioModificacion = IdUsuarioLogueado;
                modelo.FechaModificacion = DateTime.Now;
                modelo.FechaReserva = FormatoFechaReserva(modelo.FechaReserva);
                if (modelo.Observaciones == null)
                    modelo.Observaciones = string.Empty;

                var resultado = await PutAsync<ReservaEspacios, string>("ReservaEspacios/Actualizar", modelo);
                if (string.IsNullOrEmpty(resultado))
                {
                    return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
                }
                else if (resultado.IndexOf("[S]") >= 0)
                {
                    return Json(new RespuestaViewModel { Correcto = true, Elemento = resultado.Replace("[S]", "")}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_Actualizar");
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Ha ocurrido un error mientras se procesaba la información. Por favor intentelo de nuevo" });
            }

        }

        #endregion

        #region DetalleReserva

        [HttpGet]
        public async Task<ActionResult> Detalle(int NumeroReserva)
        {

            IEnumerable<ReservaEspacios> objReservaEspacios = null;
            ReservaEspacios objReservaEspacioRetorno = null;
            string strDummy = "2017-09-21";

            try
            {
                objReservaEspacios = await GetAsync<IEnumerable<ReservaEspacios>>($"ReservaEspacios/ObtenerReservaEspacios/{NumeroReserva}/{strDummy}");

                if (objReservaEspacios.Count() > 0)
                {
                    objReservaEspacioRetorno = objReservaEspacios.FirstOrDefault();

                    var objListaTipoEspacio = await GetAsync<IEnumerable<TipoGeneral>>("ReservaEspacios/ObtenerTipoEspacios");                    
                    var objListaEspacios = await GetAsync<IEnumerable<TipoGeneral>>($"ReservaEspacios/ObtenerEspaciosxTipo/{objReservaEspacioRetorno.IdTipoEspacio}");
                    var objReservaEspaciosAuxiliar = await GetAsync<IEnumerable<ReservaEspaciosAuxiliar>>($"ReservaEspacios/ObtenerDetallePedido/{objReservaEspacioRetorno.CodigoSapPedido}/{objReservaEspacioRetorno.FechaReserva}");
                    if (objReservaEspaciosAuxiliar.Count() == 0)
                        objReservaEspaciosAuxiliar = null;


                    objReservaEspacioRetorno.ListaTipoEspacio = objListaTipoEspacio;                    
                    objReservaEspacioRetorno.ListaEspacios = objListaEspacios;
                    objReservaEspacioRetorno.DetallePedido = objReservaEspaciosAuxiliar;
                    objReservaEspacioRetorno.FechaReserva = FormatoFechaCalendar(objReservaEspacioRetorno.FechaReserva);
                }
                else
                {
                    objReservaEspacioRetorno = null;
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_ObtenerReservaEdicion");
            }

            return PartialView("_Detalle", objReservaEspacioRetorno);
        }

        #endregion

        #region Eliminar

        public ActionResult Eliminar()
        {
            ViewBag.Titulo = "Eliminar reserva de espacio";
            ViewBag.Editar = 0;
            return View("Editar");
        }

        [HttpGet]
        public async Task<ActionResult> EliminarReserva(int IdReserva)
        {
            string strResultado = string.Empty;

            try
            {
                var resultado = await GetAsync<string>($"ReservaEspacios/Eliminar/{IdReserva}/{IdUsuarioLogueado}");                
                if (string.IsNullOrEmpty(resultado))
                {
                    return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ReservaEspaciosController_EliminarReserva");
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Ha ocurrido un error mientras se procesaba la información. Por favor intentelo de nuevo" });
            }

        }

        #endregion


    }
}