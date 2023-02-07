using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using System.IO;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Threading;

namespace CorParques.Transversales.Util
{
    public static class Utilidades
    {
        

        #region Funciones

        /// <summary>
        /// convierte una coleccion de datos en un objeto DataTable
        /// </summary>
        /// <typeparam name="T">entidad o clase con propiedades</typeparam>
        /// <param name="data">coleccion de datos de la clase T</param>
        /// <returns>DataTable</returns>
        public static DataTable ConvertIneumerableToDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// RDSH: Retorna un arreglo de bytes de un texto.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StrToByteArray(string str)
        {
            System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
            return encoding.GetBytes(str);
        }              

        /// <summary>
        /// RDSH: Retorna la ruta de almacenamiento de los reportes.
        /// </summary>
        /// <returns></returns>
        public static string RutaReportes()
        {
            string strRuta = string.Empty; 

            try
            {
                strRuta = Path.GetTempPath();
            }
            catch (Exception ex)
            {
                strRuta = string.Concat("Error en RutaReportes: ", ex.Message);
            }

            return strRuta;
        }

        /// <summary>
        /// RDSH: Retorna fecha con minutos y segundos para nombrar un archivo.
        /// </summary>
        /// <returns></returns>
        public static string FechaString()
        {

            string strNombreArchivo = string.Empty;
            strNombreArchivo = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            strNombreArchivo = strNombreArchivo.Replace("/", "_");
            strNombreArchivo = strNombreArchivo.Replace(" ", "_");
            strNombreArchivo = strNombreArchivo.Replace(":", "");
            return strNombreArchivo;            

        }

        /// <summary>
        /// RDSH: Retorna la ruta completa de un archivo para su descarga.
        /// </summary>
        /// <param name="strNombreArchivo"></param>
        /// <returns></returns>
        public static string RutaArchivo(string strNombreArchivo)
        {
            string strRutaArchivo = string.Empty;
            strRutaArchivo = string.Concat(RutaReportes(), @"\", strNombreArchivo);
            return strRutaArchivo;

        }

        private static void LimpiarReporte(string strNombreReporte)
        {

            string strRutaArchivo = string.Empty;
            strRutaArchivo = RutaArchivo(strNombreReporte);

            if (File.Exists(strRutaArchivo))
            {
                File.Delete(strRutaArchivo);
            }
        }

        /// <summary>
        /// RDSH: Retorna un arreglo de bytes de un archivo.
        /// </summary>
        /// <param name="strRutaArchivo"></param>
        /// <returns></returns>
        public static byte[] ObtenerBytesArchivo(string strRutaArchivo)
        {
            byte[] objbytes;

            try
            {
                objbytes = System.IO.File.ReadAllBytes(Transversales.Util.Utilidades.RutaArchivo(strRutaArchivo));

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                LimpiarReporte(strRutaArchivo);
            }
            return objbytes;

        }

        /// <summary>
        /// RDSH: Recibe la fecha en formato dd/MM/yyyy HH:mm y retorna un objeto fecha, hora y minutos son opcionales.
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="strHora"></param>
        /// <returns></returns>
        public static DateTime FormatoFechaValido(string strFecha, string strHora = "00:00")
        {
            DateTime datFechaIncidente;
            string[] strSplit;
            string strFechaArmada = string.Empty;

            try
            {

                strSplit = strFecha.Split('/');
                strFechaArmada = string.Join("/", strSplit[2], strSplit[1], strSplit[0]);
                strFechaArmada = string.Concat(strFechaArmada, " ", strHora);

                datFechaIncidente = DateTime.Parse(strFechaArmada);

            }
            catch (Exception)
            {
                datFechaIncidente = DateTime.Now;
            }

            return datFechaIncidente;

        }

        /// <summary>
        /// RDSH: Convierte 22-09-2017 a 2017/09/22 00:00, o la hora que se le envie. 
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="strHora"></param>
        /// <param name="strSeparador"></param>
        /// <returns></returns>
        public static DateTime FormatoFechaSQLHora(string strFecha, string strHora = "00:00")
        {
            DateTime datFechaIncidente;
            string[] strSplit;
            string strFechaArmada = string.Empty;

            try
            {
                strSplit = strFecha.Split('-');
                strFechaArmada = string.Join("/", strSplit[2], strSplit[1], strSplit[0]);
                strFechaArmada = string.Concat(strFechaArmada, " ", strHora);
                datFechaIncidente = DateTime.Parse(strFechaArmada);

            }
            catch (Exception)
            {
                datFechaIncidente = DateTime.Now;
            }

            return datFechaIncidente;

        }

        /// <summary>
        /// EDSP: Covertir Ienumerable a DataTable - tabla general
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static DataTable convertTable(IEnumerable<TablaGeneral> lista)
        {
            var tabla = new DataTable();
            Type type = typeof(TablaGeneral);
            int attributeCount = 0;

            foreach (PropertyInfo property in type.GetProperties())
                tabla.Columns.Add(property.Name, typeof(string));

            foreach (var item in lista)
            {
                DataRow newCustomersRow = tabla.NewRow();

                newCustomersRow["col1"] = item.col1;
                newCustomersRow["col2"] = item.col2;
                newCustomersRow["col3"] = item.col3;
                newCustomersRow["col4"] = item.col4;
                newCustomersRow["col5"] = item.col5;
                newCustomersRow["col6"] = item.col6;
                newCustomersRow["col7"] = item.col7;
                newCustomersRow["col8"] = item.col8;
                newCustomersRow["col9"] = item.col9;

                tabla.Rows.Add(newCustomersRow);
            }
            return tabla;
        }


        //EDSP Convert Lista a Datatable
        public static DataTable Lista_DataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }


        /// <summary>
        /// RDSH: Retorna la fecha en formato DD/MM/YYYY
        /// </summary>
        /// <returns></returns>
        public static string ObtenerFechaActual()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// RDSH: Retorna la hora en formato 24 horas HH:mm
        /// </summary>
        /// <returns></returns>
        public static string ObtenerHoraActual()
        {
            return DateTime.Now.ToString("HH:mm");
        }

        /// <summary>
        /// RDSH: Retorna un objeto Image
        /// </summary>
        /// <param name="strRutaImagen"></param>
        /// <returns></returns>
        public static Image RetornarImagen(string strRutaImagen)
        {
            Image objImage;           

            try
            {
                
                using (Stream BitmapStream = System.IO.File.Open(strRutaImagen, System.IO.FileMode.Open))
                {
                    objImage = Image.FromStream(BitmapStream);                
                }
                
                return objImage;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Utilidades_RetornarImagen");
                return null;
            }            

        }

        /// <summary>
        /// RDSH: Retorna un valor con formato moneda.
        /// </summary>
        /// <param name="strValor"></param>
        /// <returns></returns>
        public static string FormatoMoneda(double dblValor)
        {

            string strRetorno = string.Empty;

            try
            {
                strRetorno = string.Format("{0:C0}", dblValor);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return strRetorno;
        }

        /// <summary>
        /// RDSH: Registra un error en un log txt.
        /// </summary>
        /// <param name="exc"></param>
        public static void RegistrarError(Exception exc, string strClaseMetodo)
        {
            try
            {
                string strRuta = System.Configuration.ConfigurationManager.AppSettings["RutaLogError"].ToString();
                string strNombre = DateTime.Now.ToString("dd_MM_yyyy") + ".txt";

                if (!System.IO.Directory.Exists(strRuta))
                    System.IO.Directory.CreateDirectory(strRuta);

                using (var sw = new System.IO.StreamWriter(System.IO.Path.Combine(strRuta, strNombre), true))
                {
                    sw.WriteLine("*********************************************************");
                    sw.WriteLine("Error :" + exc.Message);
                    sw.WriteLine("Hora :" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    sw.WriteLine("Traza: " + exc.StackTrace);
                    sw.WriteLine("Clase-Metodo: " + strClaseMetodo);
                    sw.Close();
                }
            }
            catch (Exception)
            {
                
            } 
        }

        /// <summary>
        /// RDSH: Despues de haber generado un codigo de barras lo borra de la carpeta de temporales.
        /// </summary>
        /// <param name="strRutaCodigoBarras"></param>
        public static void LimpiarTempCodigosBarra(string strRutaCodigoBarras)
        {

            try
            {
                File.Delete(strRutaCodigoBarras);
            }
            catch (Exception ex)
            {
                RegistrarError(ex, "Utilidades_LimpiarTempCodigosBarra");
            }

        }

        public static bool PingWeb(string host, int cantidad)
        {
            bool rta = false;
            try
            {
                var blnActivoContigencia = ConfigurationManager.AppSettings["Contingencia"] == null ? false
                    : Convert.ToBoolean(Convert.ToInt32(ConfigurationManager.AppSettings["Contingencia"].ToString()));

                if (!blnActivoContigencia)
                    rta = true;
                else
                {
                    Ping _ping = new Ping();
                    try
                    {
                        PingReply _reply = _ping.Send(host, 100);
                        rta = _reply.Status == IPStatus.Success;
                        
                    }
                    catch (PingException e) { }

                    if (!rta)
                        if (cantidad > 1)
                        {
                            Thread.Sleep(500);
                            return PingWeb(host, cantidad - 1);
                        }
                            

                }
            }
            catch { rta = true; }

            return rta;

        }


        /// <summary>
        /// Método que valida si el hay conexion con server principal contingencia
        /// </summary>
        public static bool PingContingencia(string host, int cantidad)
        {
            bool rta = false;
            try
            {
                Ping _ping = new Ping();
                try
                {
                    PingReply _reply = _ping.Send(host, 100);
                    rta = _reply.Status == IPStatus.Success;
                    
                }
                catch (PingException e) { }

                if (!rta)
                    if (cantidad > 1)
                    {
                        Thread.Sleep(500);
                        return PingContingencia(host, cantidad - 1);
                        
                    }
                        

            }
            catch { rta = true; }

            return rta;

        }

        /// <summary>
        /// GALD: Registra un error en un log txt en contingencia.
        /// </summary>
        /// <param name="exc"></param>
        public static void RegistrarErrorContingencia(Exception exc, string strClaseMetodo)
        {
            try
            {
                string strRuta = System.Configuration.ConfigurationManager.AppSettings["RutaLogError"].ToString();
                string strNombre = "Contigencia_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";

                if (!System.IO.Directory.Exists(strRuta))
                    System.IO.Directory.CreateDirectory(strRuta);

                using (var sw = new System.IO.StreamWriter(System.IO.Path.Combine(strRuta, strNombre), true))
                {
                    sw.WriteLine("*********************************************************");
                    sw.WriteLine("Error :" + exc.Message);
                    sw.WriteLine("Hora :" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    sw.WriteLine("Traza: " + exc.StackTrace);
                    sw.WriteLine("Clase-Metodo: " + strClaseMetodo);
                    sw.Close();
                }
            }
            catch (Exception)
            {

            }
        }


        public static void RegistrarDebug(string strClaseMetodo)
        {
            try
            {
                if (ConfigurationManager.AppSettings["guardarDebug"].ToString() == "1")
                {
                    string strRuta = System.Configuration.ConfigurationManager.AppSettings["RutaLogError"].ToString();
                    string strNombre = "Contigencia_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";

                    if (!System.IO.Directory.Exists(strRuta))
                        System.IO.Directory.CreateDirectory(strRuta);

                    using (var sw = new System.IO.StreamWriter(System.IO.Path.Combine(strRuta, strNombre), true))
                    {
                        sw.WriteLine("*********************************************************");
                        sw.WriteLine("accion :" + strClaseMetodo);
                        sw.WriteLine("Hora :" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        sw.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region Codigo Barras

        /// <summary>
        /// RDSH: Genera un codigo de 13 digitos.
        /// </summary>
        /// <param name="strIdentificador"></param>
        /// <param name="strConsecutivo"></param>
        /// <returns></returns>
        public static string CodigoBarrasEAN13(string strIdentificador, string strConsecutivo)
        {
            string strCodigoRetorno = string.Empty;
            int intCantidadCaracteres = 0;
            int intLongitudEAN13 = 13;

            try
            {
                intCantidadCaracteres = string.Concat(strIdentificador, strConsecutivo).Length;
                if (intCantidadCaracteres > intLongitudEAN13)
                {
                    throw new ArgumentException("Longitud de identificador y de consecutivo es mayor a 13 caracteres.");
                }

                strCodigoRetorno = string.Concat(strIdentificador, Replicar((intLongitudEAN13 - intCantidadCaracteres), "0"), strConsecutivo);

            }
            catch (Exception ex)
            {
                strCodigoRetorno = string.Concat("Error inesperado en CodigoBarrasEAN13: ", ex.Message);
            }

            return strCodigoRetorno;
        }


        public static char GenerarDigitoEAN13(string data)
        {
            char result = '?';

            int sum = 0;
            bool oddIndex = false;

            foreach (char c in data)
            {
                int number = c - '0';
                sum += number * (oddIndex ? 3 : 1);
                oddIndex = !oddIndex;
            }

            result = (char)(((10 - sum % 10) % 10) + '0'); // number - '0' convert integer into char code
                                                           /*
                                                           * sum = (isbn[0] * 1 + isbn[1] * 3 + isbn[2] * 1 + isbn[3] * 3 + ... + isbn[11] * 3)
                                                           * result = (10 - sum mod 10) mod 10
                                                           */
            return result;
        }



        /// <summary>
        /// RDSH: Replica un caractes las veces que se especifiquen en intNumeroVeces
        /// </summary>
        /// <param name="intNumeroVeces"></param>
        /// <param name="strCaracter"></param>
        /// <returns></returns>
        public static string Replicar(int intNumeroVeces, string strCaracter)
        {
            string strResultado = string.Empty;
            if (intNumeroVeces < 0)
                intNumeroVeces = 0;

            strResultado = new String(char.Parse(strCaracter), intNumeroVeces);

            return strResultado;

        }

        /// <summary>
        /// RDSH: Recorta una cadena de texto.
        /// </summary>
        /// <param name="strTexto"></param>
        /// <param name="intMaximoCaracteres"></param>
        /// <returns></returns>
        public static string RecortarTexto(string strTexto, int intMaximoCaracteres)
        {
            string strTextoRecortado = string.Empty;

            if (strTexto.Trim().Length > 0 && intMaximoCaracteres > 0)
            {
                if (strTexto.Trim().Length > intMaximoCaracteres)
                {
                    strTextoRecortado = strTexto.Remove(intMaximoCaracteres);
                }
                else
                {
                    strTextoRecortado = strTexto;
                }

            }

            return strTextoRecortado;

        }

        /// <summary>
        /// RDSH: Retorna un objeto de tipo general valor donde: Id = IdPunto, Nombre = NombrePunto, Valor = Codigo SAP.
        /// </summary>
        /// <param name="strCodigoSap"></param>
        /// <returns></returns>
        public static TipoGeneralValor ObtenerInformacionPunto(string strCodigoSap)
        {
            HttpClient client;
            TipoGeneralValor objTipoGeneral = null;
            client = new HttpClient();

            var _host = new Uri(ConfigurationManager.AppSettings["UrlService"]);

            if (ConfigurationManager.AppSettings["IntentosConsumoServicio"] != null)
            {

                if (Utilidades.PingWeb(_host.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString()))) //3 cantidad de intentos
                    client.BaseAddress = _host;
                else
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlServiceLocal"]);
            }else
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlService"]);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = AsyncHelpers.RunSync(() => client.GetAsync("Puntos/ObtenerPuntosCache"));

            if (response.IsSuccessStatusCode)
            {
                var objListaTipoGeneralValor = JsonConvert.DeserializeObject<IEnumerable<TipoGeneralValor>>(AsyncHelpers.RunSync<string>(() => response.Content.ReadAsStringAsync()));
                objTipoGeneral = objListaTipoGeneralValor.ToList().Where(x => x.Valor.ToString() == strCodigoSap).FirstOrDefault();

                if (objTipoGeneral == null)
                {
                    objTipoGeneral =  new TipoGeneralValor();
                    objTipoGeneral.Id = 0;
                    objTipoGeneral.Nombre = "";
                    objTipoGeneral.CodSAP = "";                   
                }
            }

            return objTipoGeneral;

        }

        /// <summary>
        /// Manuel Ochoa: Retorna la fecha para la zona horaria de colombia
        /// </summary>
        /// <returns></returns>
        public static DateTime FechaActualColombia
        {
            get
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                DateTime localNow = TimeZoneInfo.ConvertTime(DateTime.Now, tz);
                return localNow;
            }
        }


        #endregion

    }
}
