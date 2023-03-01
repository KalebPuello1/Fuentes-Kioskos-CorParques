using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Ventas.Core.Models;
using CorParques.Transversales.Util;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Net.NetworkInformation;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    /// <summary>
    /// Redeaban API Datafono
    /// </summary>
    public class RedebanController : ControladorBase
    {
        //public RedebanController()
        //{
        //}

        public string RutaInstaladorRedeban = ConfigurationManager.AppSettings["RutaInstaladorRedeban"].ToString();
        public string NombreArchivoSolicitudRedeban = ConfigurationManager.AppSettings["NombreArchivoSolicitudRedeban"].ToString();
        public string NombreArchivoRespuestaRedeban = ConfigurationManager.AppSettings["NombreArchivoRespuestaRedeban"].ToString();
        public string RutaEjecutableCajasRedeban = ConfigurationManager.AppSettings["RutaEjecutableCajasRedeban"].ToString();
        public string FlujoRedeban = "0";

        public string OperacionPagoRedeban = ConfigurationManager.AppSettings["OperacionPagoRedeban"].ToString();
        public string OperacionAnulacionRedeban = ConfigurationManager.AppSettings["OperacionAnulacionRedeban"].ToString();
        public string CodigoErrorConfiguracionRedeban = ConfigurationManager.AppSettings["CodigoErrorConfiguracionRedeban"].ToString();
        public string CodigoCompraAprobadaRedeban = ConfigurationManager.AppSettings["CodigoCompraAprobadaRedeban"].ToString();
        public string CodigoCompraDeclinadaRedeban = ConfigurationManager.AppSettings["CodigoCompraDeclinadaRedeban"].ToString();
        public string CodigoCompraPinIncorrectoRedeban = ConfigurationManager.AppSettings["CodigoCompraPinIncorrectoRedeban"].ToString();
        public string CodigoCompraEntidadNoRespondeRedeban = ConfigurationManager.AppSettings["CodigoCompraEntidadNoRespondeRedeban"].ToString();
        public string ClaveSupervisorAnulacionRedeban = ConfigurationManager.AppSettings["ClaveSupervisorAnulacionRedeban"].ToString();
        public string ClaveAdministradorAnulacionRedeban = ConfigurationManager.AppSettings["ClaveAdministradorAnulacionRedeban"].ToString();
        public double BaseDevolucionRedeban = Convert.ToDouble(ConfigurationManager.AppSettings["BaseDevolucionRedeban"].ToString());
        public double ImpuestoConsumoRedeban = Convert.ToDouble(ConfigurationManager.AppSettings["ImpuestoConsumoRedeban"].ToString());
        public double IvaRedeban = Convert.ToDouble(ConfigurationManager.AppSettings["IvaRedeban"].ToString());
        public string CodigoCajeroRedeban = ConfigurationManager.AppSettings["CodigoCajeroRedeban"].ToString();
        public bool PropinaHabilitadaRedeban = Convert.ToBoolean(ConfigurationManager.AppSettings["PropinaHabilitadaRedeban"].ToString());
        public double PropinaRedeban = Convert.ToDouble(ConfigurationManager.AppSettings["PropinaRedeban"].ToString());
        public int IntentosBusquedaLogsRedeban = Convert.ToInt32(ConfigurationManager.AppSettings["IntentosBusquedaLogsRedeban"].ToString());
        public int ThreadSleepRedeban = Convert.ToInt32(ConfigurationManager.AppSettings["ThreadSleepRedeban"].ToString());
        public string EventoInicioRedeban = ConfigurationManager.AppSettings["EventoInicioRedeban"].ToString();
        public string EventoInicioAnulacionRedeban = ConfigurationManager.AppSettings["EventoInicioAnulacionRedeban"].ToString();
        public string EventoFinalizacionRedeban = ConfigurationManager.AppSettings["EventoFinalizacionRedeban"].ToString();
        public string EventoFinalizacionAnulacionRedeban = ConfigurationManager.AppSettings["EventoFinalizacionAnulacionRedeban"].ToString();
        public int IdProcesoRedeban = 0;
        public void Index()
        {
        }


        [HttpGet]
        public async Task<JsonResult> IniciarTransaccionRedeban(string montoPago)
        {
             FlujoRedeban = await GetAsync<string>($"Redeban/FlujoRedebanXPunto/{IdPunto}");
            RespuestaTransaccionRedaban resultadoTransaccion = new RespuestaTransaccionRedaban();
            var resultadoOperecion = "";
            string resultadoValidacionPath;
            int IdSolicitud = 0;
            try
            {
                LogRedebanSolicitud model = new LogRedebanSolicitud();
                model.DireccionMAC = ObtenerDireccionMACEquipo();
                model.IP = ObtenerIPEquipo();
                model.IdUsuarioCreacion = IdUsuarioLogueado;
                model.BaseDevolucion = BaseDevolucionRedeban;
                model.CodigoCajero = CodigoCajeroRedeban;
                model.Evento = EventoInicioRedeban;
                model.Fecha = DateTime.Now;
                model.ImpuestoConsumo = ImpuestoConsumoRedeban;
                model.Iva = IvaRedeban;
                model.Localizacion = ObtenerUbicacionPorIp(model.IP);
                model.NumeroFactura = await ObtenerNumeroFactura(IdPunto);
                model.PropinaHabilitada = PropinaHabilitadaRedeban;
                model.Propina = PropinaRedeban;
                model.Operacion = OperacionPagoRedeban;
                model.Monto = Convert.ToDouble(montoPago);
                string jsonObjectoSolicitud = new JavaScriptSerializer().Serialize(model);
                model.MensajeEnvio = jsonObjectoSolicitud;

                resultadoValidacionPath = ValidarExistenciaPath();
                if (resultadoValidacionPath == "OK")
                {
                    var resultadoCreacionArchivoSolicitud = await GuardarInformacionArchivoSolicitud(model);
                    if (resultadoCreacionArchivoSolicitud.ToString() == "OK")
                    {
                        bool resultadoEjecucionApp = await EjecutarCajasRedeban();
                        if (resultadoEjecucionApp)
                        {
                            var rtaLog = await InsertarLogRedebanSolicitud(model);

                            var data = rtaLog.GetType().GetProperty("Data");
                            if (data != null)
                            {
                                var dataValor = data.GetValue(rtaLog, null);
                                if (dataValor != null)
                                {
                                    var elemento = dataValor.GetType().GetProperty("Elemento");
                                    if (elemento != null)
                                    {
                                        var elementoValor = elemento.GetValue(dataValor, null);
                                        IdSolicitud = Convert.ToInt32(elementoValor);
                                    }
                                }
                            }
                            Thread.Sleep(ThreadSleepRedeban);
                            LogRedebanRespuesta resultadoLogRespuesta = await ObtenerInformacionArchivoRespuesta();
                            if (resultadoLogRespuesta != null)
                            {                                

                                if (string.IsNullOrWhiteSpace(resultadoLogRespuesta.CodigoRespuesta) ||  
                                    resultadoLogRespuesta.CodigoRespuesta == CodigoErrorConfiguracionRedeban ||
                                    resultadoLogRespuesta.CodigoRespuesta == CodigoCompraDeclinadaRedeban ||
                                    resultadoLogRespuesta.CodigoRespuesta == CodigoCompraEntidadNoRespondeRedeban ||
                                    resultadoLogRespuesta.CodigoRespuesta == CodigoCompraPinIncorrectoRedeban)
                                {
                                    resultadoTransaccion.CodigoRespuesta = resultadoLogRespuesta.CodigoRespuesta;

                                    if (string.IsNullOrWhiteSpace(resultadoLogRespuesta.CodigoRespuesta))
                                    {
                                        resultadoOperecion = "Error conectando con Redeban.";
                                    }
                                    else if(resultadoLogRespuesta.CodigoRespuesta == CodigoErrorConfiguracionRedeban)
                                    {
                                        resultadoOperecion = resultadoLogRespuesta.MensajeRespuesta;
                                    }
                                    else if (resultadoLogRespuesta.CodigoRespuesta == CodigoCompraDeclinadaRedeban)
                                    {
                                        resultadoOperecion = "Compra declinada, intente nuevamente por favor";
                                    }
                                    else if (resultadoLogRespuesta.CodigoRespuesta == CodigoCompraEntidadNoRespondeRedeban)
                                    {
                                        resultadoOperecion = "La entidad financiera no responde, intente nuevamente por favor";
                                    }
                                    else if (resultadoLogRespuesta.CodigoRespuesta == CodigoCompraPinIncorrectoRedeban)
                                    {
                                        resultadoOperecion = "Pin incorrecto, intente nuevamente por favor";
                                    }

                                }
                                else
                                {
                                    resultadoOperecion = "OK";
                                    resultadoTransaccion.TipoTarjeta = resultadoLogRespuesta.TipoTarjeta;
                                    resultadoTransaccion.CodigoRespuesta = resultadoLogRespuesta.CodigoRespuesta;
                                    resultadoTransaccion.Franquicia = resultadoLogRespuesta.Franquicia;
                                    resultadoTransaccion.Monto = resultadoLogRespuesta.Monto;
                                    resultadoTransaccion.NumeroAprobacion = resultadoLogRespuesta.NumeroAprobacion;
                                    resultadoTransaccion.NumeroFactura = resultadoLogRespuesta.NumeroFactura;
                                    resultadoTransaccion.NumeroTarjeta = resultadoLogRespuesta.NumeroTarjeta;
                                    resultadoTransaccion.NumeroRecibo = resultadoLogRespuesta.NumeroRecibo;


                                    //Para pruebas funcionales sin datafono
                                    //resultadoOperecion = "OK";
                                    //resultadoTransaccion.MensajeRespuesta = resultadoOperecion;
                                    //resultadoTransaccion.TipoTarjeta = "VISA";
                                    //resultadoTransaccion.CodigoRespuesta = "00";
                                    //resultadoTransaccion.Franquicia = "VISA";
                                    //resultadoTransaccion.Monto = 10000;
                                    //resultadoTransaccion.NumeroAprobacion = "HOME_01";
                                    //resultadoTransaccion.NumeroFactura = "WERT_12455";
                                    //resultadoTransaccion.NumeroTarjeta = "12144343*34343";
                                    //resultadoTransaccion.NumeroRecibo = "000036";
                                }
                                resultadoLogRespuesta.IdSolicitud = IdSolicitud;
                                resultadoTransaccion.MensajeRespuesta = resultadoOperecion;
                                await InsertarLogRedebanRespuesta(resultadoLogRespuesta, EventoFinalizacionRedeban);
                                if (resultadoLogRespuesta.Franquicia != null)
                                {
                                    var CodFranquicia = resultadoLogRespuesta.Franquicia.Trim(' ');
                                    var rta = await GetAsync<RespuestaTransaccionRedaban>($"Factura/ObtenerIdFranquiciaRedeban/{CodFranquicia}");
                                    if (rta != null)
                                    {
                                        if (rta.IdFranquicia != 0)
                                        {
                                            resultadoTransaccion.IdFranquicia = rta.IdFranquicia;
                                        }
                                    }
                                }

                                
                                
                            }
                        }
                    }
                }
                else
                {
                    resultadoOperecion = "ERROR";
                    resultadoTransaccion.MensajeRespuesta = resultadoValidacionPath;
                }

                await EliminarProcesoAdministradorTareas();
                return Json(resultadoTransaccion, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                await EliminarProcesoAdministradorTareas();
                throw new Exception(". HTTP status code from response was not expected. " + ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }

        }

        public string ValidarExistenciaPath()
        {
            string ValidacionPath = "";
            bool InstaladorRedeban = System.IO.File.Exists(RutaEjecutableCajasRedeban);
            bool ArchivoSolicitudRedeban = System.IO.File.Exists(RutaInstaladorRedeban + NombreArchivoSolicitudRedeban);
            bool ArchivoRespuestaRedeban = System.IO.File.Exists(RutaInstaladorRedeban + NombreArchivoSolicitudRedeban);
            bool EjecutableCajasRedeban = System.IO.File.Exists(RutaEjecutableCajasRedeban);
            
            if (FlujoRedeban == "1")
            {
                if (InstaladorRedeban == true && EjecutableCajasRedeban == true)
                {
                    ValidacionPath = "OK";
                }
                else if (InstaladorRedeban == false)
                {
                    ValidacionPath = "La variable RutaInstaladorRedeban no tiene asociado valor";
                }
                else if (EjecutableCajasRedeban == false)
                {
                    ValidacionPath = "La ruta de la variable RutaInstaladorRedeban no existe";
                }
            }

            else
            {
                ValidacionPath = "OK";
            }


            return ValidacionPath;
        }

        [HttpGet]
        public async Task<JsonResult> IniciarAnulacionRedeban(string numeroRecibo, string numeroFactura)
        {
            RespuestaTransaccionRedaban resultadoTransaccion = new RespuestaTransaccionRedaban();
            var resultadoOperecion = "";
            int IdSolicitud = 0;
            try
            {
                LogRedebanSolicitudAnulacion model = new LogRedebanSolicitudAnulacion();
                model.DireccionMAC = ObtenerDireccionMACEquipo();
                model.IP = ObtenerIPEquipo();
                model.IdUsuarioCreacion = IdUsuarioLogueado;
                model.CodigoCajero = CodigoCajeroRedeban;
                model.Evento = EventoInicioAnulacionRedeban;
                model.Fecha = DateTime.Now;
                model.NumeroRecibo = numeroRecibo;
                model.Clave = ClaveSupervisorAnulacionRedeban;
                model.Localizacion = ObtenerUbicacionPorIp(model.IP);
                model.NumeroFactura = numeroFactura;
                model.Operacion = OperacionAnulacionRedeban;

                string jsonObjectoSolicitud = new JavaScriptSerializer().Serialize(model);
                model.MensajeEnvio = jsonObjectoSolicitud;
                // await EliminarArchivoSolicitudRedeban();
                var resultadoCreacionArchivoSolicitud = await GuardarInformacionArchivoSolicitudAnulacion(model);
                if (resultadoCreacionArchivoSolicitud.ToString() == "OK")
                {
                    bool resultadoEjecucionApp = await EjecutarCajasRedeban();
                    if (resultadoEjecucionApp)
                    {
                        var rtaLog = await InsertarLogRedebanSolicitudAnulacion(model);
                        var data = rtaLog.GetType().GetProperty("Data");
                        if (data != null)
                        {
                            var dataValor = data.GetValue(rtaLog, null);
                            if (dataValor != null)
                            {
                                var elemento = dataValor.GetType().GetProperty("Elemento");
                                if (elemento != null)
                                {
                                    var elementoValor = elemento.GetValue(dataValor, null);
                                    IdSolicitud = Convert.ToInt32(elementoValor);
                                }
                            }
                        }
                        Thread.Sleep(ThreadSleepRedeban);
                        LogRedebanRespuesta resultadoLogRespuesta = await ObtenerInformacionArchivoRespuesta();

                        //Prueba funcional sin datafono
                        //resultadoLogRespuesta.CodigoRespuesta = CodigoErrorConfiguracionRedeban;
                        if (resultadoLogRespuesta != null)
                        {
                            if (string.IsNullOrWhiteSpace(resultadoLogRespuesta.CodigoRespuesta) || 
                                resultadoLogRespuesta.CodigoRespuesta == CodigoErrorConfiguracionRedeban ||
                                resultadoLogRespuesta.CodigoRespuesta == CodigoCompraDeclinadaRedeban ||
                                resultadoLogRespuesta.CodigoRespuesta == CodigoCompraEntidadNoRespondeRedeban ||
                                resultadoLogRespuesta.CodigoRespuesta == CodigoCompraPinIncorrectoRedeban)
                            {
                                resultadoTransaccion.CodigoRespuesta = resultadoLogRespuesta.CodigoRespuesta;

                                if (string.IsNullOrWhiteSpace(resultadoLogRespuesta.CodigoRespuesta))
                                {
                                    resultadoOperecion = "Error conectando con Redeban.";
                                }
                                else if (resultadoLogRespuesta.CodigoRespuesta == CodigoErrorConfiguracionRedeban)
                                {
                                    resultadoOperecion = resultadoLogRespuesta.MensajeRespuesta;
                                }
                                else if (resultadoLogRespuesta.CodigoRespuesta == CodigoCompraDeclinadaRedeban)
                                {
                                    resultadoOperecion = "Compra declinada, intente nuevamente por favor";
                                }
                                else if (resultadoLogRespuesta.CodigoRespuesta == CodigoCompraEntidadNoRespondeRedeban)
                                {
                                    resultadoOperecion = "La entidad financiera no responde, intente nuevamente por favor";
                                }
                                else if (resultadoLogRespuesta.CodigoRespuesta == CodigoCompraPinIncorrectoRedeban)
                                {
                                    resultadoOperecion = "Pin incorrecto, intente nuevamente por favor";
                                }
                            }
                            else
                            {
                                resultadoOperecion = "OK";
                                resultadoTransaccion.TipoTarjeta = resultadoLogRespuesta.TipoTarjeta;
                                resultadoTransaccion.CodigoRespuesta = resultadoLogRespuesta.CodigoRespuesta;
                                resultadoTransaccion.Franquicia = resultadoLogRespuesta.Franquicia;
                                resultadoTransaccion.Monto = resultadoLogRespuesta.Monto;
                                resultadoTransaccion.NumeroAprobacion = resultadoLogRespuesta.NumeroAprobacion;
                                resultadoTransaccion.NumeroFactura = resultadoLogRespuesta.NumeroFactura;
                                resultadoTransaccion.NumeroTarjeta = resultadoLogRespuesta.NumeroTarjeta;
                            }
                            resultadoLogRespuesta.IdSolicitud = IdSolicitud;
                            resultadoTransaccion.MensajeRespuesta = resultadoOperecion;
                            await InsertarLogRedebanRespuesta(resultadoLogRespuesta, EventoFinalizacionAnulacionRedeban);
                        }
                    }
                }
                await EliminarProcesoAdministradorTareas();
                return Json(resultadoTransaccion, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                await EliminarProcesoAdministradorTareas();
                throw new Exception(". HTTP status code from response was not expected. " + ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }

        }

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
                strPerfiles = objParametro.Valor;

                var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");

                foreach (var item in _listUsuarios)
                {
                    if (rta.Id == item.Id)
                    {
                        isSupervisor = true;
                        break;
                    }
                }

                if (!isSupervisor)
                {
                    resp = new RespuestaViewModel { Correcto = false, Elemento = null, Mensaje = "El usuario no esta autorizado." };
                    //Para pruebas funcionales sin datafono
                    //resp = new RespuestaViewModel { Correcto = true, Elemento = null, Mensaje = "El usuario no esta autorizado." };
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

        public async Task<object> GuardarInformacionArchivoSolicitud(LogRedebanSolicitud model)
        {

            try
            {
                model.DireccionMAC = ObtenerDireccionMACEquipo();
                model.IP = ObtenerIPEquipo();
                string folder = RutaInstaladorRedeban;
                string fileName = NombreArchivoSolicitudRedeban;
                string pathTxt = folder + fileName;
                string text = model.Operacion.ToString() + "," + model.Monto.ToString() + "," + model.Iva.ToString() + "," + model.NumeroFactura.ToString() + ","
                    + model.BaseDevolucion.ToString() + "," + model.ImpuestoConsumo.ToString();
                System.IO.File.WriteAllText(pathTxt, text);
            }

            catch (Exception ex)
            {
                throw new Exception(". HTTP status code from response was not expected. " + ex.Message);

            }
            return System.Net.HttpStatusCode.OK;
        }

        public async Task<object> GuardarInformacionArchivoSolicitudAnulacion(LogRedebanSolicitudAnulacion model)
        {

            try
            {
                model.DireccionMAC = ObtenerDireccionMACEquipo();
                model.IP = ObtenerIPEquipo();
                string folder = RutaInstaladorRedeban;
                string fileName = NombreArchivoSolicitudRedeban;
                string pathTxt = folder + fileName;
                string text = model.Operacion.ToString() + "," + model.NumeroRecibo.ToString() + "," + model.NumeroFactura.ToString() + "," + model.Clave.ToString() + ","
                    + model.CodigoCajero.ToString();
                System.IO.File.WriteAllText(pathTxt, text);
            }

            catch (Exception ex)
            {
                throw new Exception(". HTTP status code from response was not expected. " + ex.Message);

            }
            return System.Net.HttpStatusCode.OK;
        }

        public async Task<LogRedebanRespuesta> ObtenerInformacionArchivoRespuesta()
        {
            LogRedebanRespuesta logRedebanRespuesta = new LogRedebanRespuesta();

            try
            {
                bool resultadoExistenciaArchivo = false;
                for (int i = 0; i < IntentosBusquedaLogsRedeban; i++)
                {

                    resultadoExistenciaArchivo = await ComprobarExistenciaArchivoRespuestaRedeban();
                    if (resultadoExistenciaArchivo == true)
                    {
                        i = IntentosBusquedaLogsRedeban + 1;
                    }
                    Thread.Sleep(ThreadSleepRedeban);
                }

                if (resultadoExistenciaArchivo == true)
                {
                    string folder = RutaInstaladorRedeban;
                    string fileName = NombreArchivoRespuestaRedeban;
                    string pathTxt = folder + fileName;
                    string text = System.IO.File.ReadAllText(@pathTxt);

                    char[] delimiterChars = { ',', '\t' };
                    string[] words = text.Split(delimiterChars);

                    if (words.Count() > 0 && words.Count() > 2)
                    {
                        logRedebanRespuesta.MensajeRespuesta = text;
                        logRedebanRespuesta.CodigoRespuesta = words[0].ToString();
                        logRedebanRespuesta.NumeroAprobacion = words[1].ToString();
                        logRedebanRespuesta.NumeroTarjeta = words[2].ToString();
                        logRedebanRespuesta.TipoTarjeta = words[3].ToString();
                        if (logRedebanRespuesta.CodigoRespuesta != CodigoCompraAprobadaRedeban)
                        {
                            logRedebanRespuesta.Franquicia = "0";
                            logRedebanRespuesta.Cuotas = 0;
                        }
                        else
                        {
                            logRedebanRespuesta.Franquicia = words[4].ToString();
                            logRedebanRespuesta.Cuotas = Convert.ToInt32(words[8].ToString());
                        }

                        logRedebanRespuesta.Monto = Convert.ToDouble(words[5].ToString());
                        logRedebanRespuesta.Iva = Convert.ToDouble(words[6].ToString());
                        logRedebanRespuesta.NumeroRecibo = words[7].ToString();

                        logRedebanRespuesta.RRN = words[9].ToString();
                    }
                    else
                    {
                        logRedebanRespuesta.MensajeRespuesta = text;
                        logRedebanRespuesta.CodigoRespuesta = words[0].ToString();

                        //Paras pruebas funcionales
                        //logRedebanRespuesta.CodigoRespuesta = "00";
                    }
                }


            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return logRedebanRespuesta;
        }

        public async Task<bool> EjecutarCajasRedeban()
        {
            bool respuestaExitosa = false;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = RutaEjecutableCajasRedeban;
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            LogRedebanRespuesta logRedebanRespuesta = null;

            try
            {
                await EliminarArchivoRespuestaRedeban();

                using (Process exeProcess = Process.Start(startInfo))
                {
                    IdProcesoRedeban = exeProcess.Id;
                    respuestaExitosa = true;

                }
                return respuestaExitosa;
            }
            catch (Exception ex)
            {
                return respuestaExitosa;
            }
        }

        public async Task<bool> EliminarProcesoAdministradorTareas()
        {
            bool ejecucionOperacion = false;
            try
            {
                if (IdProcesoRedeban != 0)
                {
                    string command = "taskkill /f /PID" + " " + IdProcesoRedeban.ToString();
                    ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);
                    // Indicamos que la salida del proceso se redireccione en un Stream
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.UseShellExecute = false;
                    //Indica que el proceso no despliegue una pantalla negra (El proceso se ejecuta en background)
                    procStartInfo.CreateNoWindow = false;
                    //Inicializa el proceso
                    Process proc = new Process();
                    proc.StartInfo = procStartInfo;
                    proc.Start();
                    //Consigue la salida de la Consola(Stream) y devuelve una cadena de texto
                    string result = proc.StandardOutput.ReadToEnd();
                    ejecucionOperacion = true;
                }
            }
            catch (Exception ex)
            {
                ejecucionOperacion = false;
            }
            return ejecucionOperacion;
        }
        public async Task<object> InsertarLogRedebanSolicitud(LogRedebanSolicitud model)
        {

            try
            {
                model.IdUsuarioCreacion = IdUsuarioLogueado;
                var rta = await PostAsync<LogRedebanSolicitud, string>("Redeban/InsertarLogRedebanSolicitud", model);
                return Json(rta, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw new Exception(". HTTP status code from response was not expected. " + ex.Message);

            }
            return System.Net.HttpStatusCode.OK;
        }

        public async Task<object> InsertarLogRedebanSolicitudAnulacion(LogRedebanSolicitudAnulacion model)
        {

            try
            {
                model.IdUsuarioCreacion = IdUsuarioLogueado;
                var rta = await PostAsync<LogRedebanSolicitudAnulacion, string>("Redeban/InsertarLogRedebanSolicitudAnulacion", model);
                return Json(rta, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw new Exception(". HTTP status code from response was not expected. " + ex.Message);

            }
            return System.Net.HttpStatusCode.OK;
        }
        public async Task<object> InsertarLogRedebanRespuesta(LogRedebanRespuesta model, string tipoEvento)
        {

            try
            {
                model.DireccionMAC = ObtenerDireccionMACEquipo();
                model.IP = ObtenerIPEquipo();
                model.IdUsuarioCreacion = IdUsuarioLogueado;
                string jsonObjectoRespuesta = new JavaScriptSerializer().Serialize(model);
                model.MensajeRespuesta = jsonObjectoRespuesta;
                model.Evento = tipoEvento;
                model.Fecha = DateTime.Now;
                model.Localizacion = ObtenerUbicacionPorIp(model.IP);
                var rta = await PostAsync<LogRedebanRespuesta, string>("Redeban/InsertarLogRedebanRespuesta", model);
                return Json(rta, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw new Exception(". HTTP status code from response was not expected. " + ex.Message);

            }
            return System.Net.HttpStatusCode.OK;
        }

        public async Task<string> ObtenerNumeroFactura(int IdPunto)
        {
            try
            {
                // var rta = "1615|C42-6344";
                var rta = await GetAsync<string>($"Redeban/ObtenerNumeroFactura/{IdPunto}");
                string rtaCorta = rta.ToString();
                string[] authorsList = rtaCorta.Split('|');
                return authorsList[1];
            }

            catch (Exception ex)
            {
                throw new Exception(". HTTP status code from response was not expected. " + ex.Message);

            }
        }
        public async Task<bool> EliminarArchivoSolicitudRedeban()
        {
            bool respuestaExitosa = false;
            string pathArchivoSolicitud = RutaInstaladorRedeban + NombreArchivoSolicitudRedeban;
            try
            {
                bool resultArchivoSolicitud = System.IO.File.Exists(pathArchivoSolicitud);
                if (resultArchivoSolicitud == true)
                {
                    System.IO.File.Delete(pathArchivoSolicitud);
                    respuestaExitosa = true;
                }
                respuestaExitosa = true;
                return respuestaExitosa;
            }
            catch (Exception ex)
            {
                return respuestaExitosa;
            }
        }

        public async Task<bool> EliminarArchivoRespuestaRedeban()
        {
            bool respuestaExitosa = false;
            string pathArchivoRespuesta = RutaInstaladorRedeban + NombreArchivoRespuestaRedeban;
            try
            {
                bool resultArchivoRespuesta = System.IO.File.Exists(pathArchivoRespuesta);
                if (resultArchivoRespuesta == true)
                {
                    System.IO.File.Delete(pathArchivoRespuesta);
                    respuestaExitosa = true;
                }
                resultArchivoRespuesta = true;

                return respuestaExitosa;
            }
            catch (Exception ex)
            {
                return respuestaExitosa;
            }
        }

        public async Task<bool> ComprobarExistenciaArchivoRespuestaRedeban()
        {
            bool respuestaExitosa = false;
            string pathArchivoRespuesta = RutaInstaladorRedeban + NombreArchivoRespuestaRedeban;
            try
            {
                bool resultArchivoRespuesta = System.IO.File.Exists(pathArchivoRespuesta);
                if (resultArchivoRespuesta == true)
                {
                    respuestaExitosa = true;
                }

                return respuestaExitosa;
            }
            catch (Exception ex)
            {
                return respuestaExitosa;
            }
        }

        public string ObtenerIPEquipo()
        {
            try
            {
                String address = "";
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    address = stream.ReadToEnd();
                }

                int first = address.IndexOf("Address: ") + 9;
                int last = address.LastIndexOf("</body>");
                address = address.Substring(first, last - first);

                return address;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string ObtenerDireccionMACEquipo()
        {
            try
            {
                var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                               where nic.OperationalStatus == OperationalStatus.Up
                               select nic.GetPhysicalAddress().ToString()).FirstOrDefault();

                return macAddr;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string ObtenerUbicacionPorIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName + "-" + ipInfo.City;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return ipInfo.Country;
        }

    }
}
