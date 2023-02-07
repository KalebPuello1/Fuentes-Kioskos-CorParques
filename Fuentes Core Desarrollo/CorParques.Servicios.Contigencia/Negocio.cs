using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace CorParques.Servicios.Contigencia
{
    public class Negocio
    {
        protected SqlConnection _cnn;
        private SqlCommand _cmd;


        public Negocio()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Con_local"].ConnectionString);
        }

        public void SincronizarLocalPrincipal()
        {
            try
            {
                HttpClient client;
                client = new HttpClient();

                var _host = new Uri(ConfigurationManager.AppSettings["UrlService"]);

                HttpClient clientLocal;                
                clientLocal = new HttpClient();

                var _hostLocal = new Uri(ConfigurationManager.AppSettings["UrlServiceLocal"]);

                if (ConfigurationManager.AppSettings["IntentosConsumoServicio"] != null)
                {

                    if (Utilidades.PingContingencia(_host.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString()))) //3 cantidad de intentos
                        client.BaseAddress = _host;                        
                    else
                    {
                        Utilidades.RegistrarErrorContingencia(new Exception("No existe conexion para central"), "SincronizarLocalPrincipal");
                        client.BaseAddress = null;
                    }

                    if (Utilidades.PingContingencia(_hostLocal.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString()))) //3 cantidad de intentos
                        clientLocal.BaseAddress = _hostLocal;
                    else
                    {
                        Utilidades.RegistrarErrorContingencia(new Exception("No existe conexion para Local"), "SincronizarLocalPrincipal");
                        clientLocal.BaseAddress = null;
                    }
                }
                else
                {
                    if (Utilidades.PingContingencia(_host.Host, 3)) //3 cantidad de intentos
                        client.BaseAddress = _host;
                    else
                    {
                        Utilidades.RegistrarErrorContingencia(new Exception("No existe conexion para central"), "SincronizarLocalPrincipal");
                        client.BaseAddress = null;
                    }

                    if (Utilidades.PingContingencia(_hostLocal.Host, 3)) //3 cantidad de intentos
                        clientLocal.BaseAddress = _hostLocal;
                    else
                    {
                        Utilidades.RegistrarErrorContingencia(new Exception("No existe conexion para local"), "SincronizarLocalPrincipal");
                        clientLocal.BaseAddress = null;
                    }
                }
                if (client.BaseAddress != null && clientLocal.BaseAddress != null)
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    clientLocal.DefaultRequestHeaders.Accept.Clear();
                    clientLocal.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = AsyncHelpers.RunSync(() => clientLocal.GetAsync("Factura/ObtenerFacturaContingencia"));

                    if (response.IsSuccessStatusCode)
                    {
                        var objFacturasaProcesar = JsonConvert.DeserializeObject<IEnumerable<Factura>>(AsyncHelpers.RunSync<string>(() => response.Content.ReadAsStringAsync()));
                        HttpResponseMessage responseLocal = AsyncHelpers.RunSync(() => client.PostAsJsonAsync("Factura/ProcesaFacturaContingencia", objFacturasaProcesar));
                        
                        if (responseLocal.IsSuccessStatusCode)
                        {
                            var objFacturasaBorrar = JsonConvert.DeserializeObject<List<Factura>>(AsyncHelpers.RunSync<string>(() => response.Content.ReadAsStringAsync()));
                            HttpResponseMessage responseBorrar = AsyncHelpers.RunSync(() => clientLocal.PostAsJsonAsync("Factura/BorrarFacturaContingencia", objFacturasaBorrar));

                            // var objRespuestaaBorrar = JsonConvert.DeserializeObject<string>(AsyncHelpers.RunSync<string>(() => response.Content.ReadAsStringAsync()));
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            
        }

        public void SincronizarTablas()
        {
            try
            {
                string strRuta = ConfigurationManager.AppSettings["RutaFileSql"];

                if (strRuta == null || !ValidarRuta(strRuta))
                {

                    Utilidades.RegistrarErrorContingencia(new Exception("no hay acceso a ruta o no existe archivo - "+ strRuta), "SincronizarTablas");
                    return;
                }

                strRuta = Path.GetFullPath(strRuta);

                string strRutaTemporal = ConfigurationManager.AppSettings["RutaTemporalSql"];

                if (strRutaTemporal == null)
                    return;

                strRutaTemporal = Path.GetFullPath(strRutaTemporal);

                ValidarRutaTemporal(Path.GetDirectoryName(strRutaTemporal));
                CopiarArchivo(strRuta, strRutaTemporal);


                string script = File.ReadAllText(strRutaTemporal);

                //Valida que el sql tenga tenga contenido para evitar excepciones
                if (string.IsNullOrEmpty(script))
                    return;

                if (_cnn.State != System.Data.ConnectionState.Open)
                    _cnn.Open();
                _cmd = new SqlCommand(script, _cnn);
                _cmd.ExecuteNonQuery();
                _cnn.Close();

            }
            catch(Exception ex)
            {
                if (_cnn.State == System.Data.ConnectionState.Open)
                    _cnn.Close();

                Utilidades.RegistrarErrorContingencia(ex, "SincronizarTablas");
                throw;
            }
        }

        /// <summary>
        /// Actualizar tablas restantes
        /// </summary>
        public void  ActualizarDiccionarios()
        {
            try
            {

                Utilidades.RegistrarErrorContingencia(new Exception("ingreso actualizacion diccionarios"), "ActualizarDiccionarios");
                HttpClient client;
                client = new HttpClient();
                var _host = new Uri(ConfigurationManager.AppSettings["UrlService"]);

                client.BaseAddress = _host;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (ConfigurationManager.AppSettings["IntentosConsumoServicio"] != null)
                {

                    if (Utilidades.PingContingencia(_host.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString()))) //3 cantidad de intentos
                    {

                        HttpResponseMessage response = AsyncHelpers.RunSync(() => client.GetAsync("Factura/ObtenerDiccionarioContigencia"));

                        if (response.IsSuccessStatusCode)
                        {
                            var asd = response.Content.ReadAsAsync<CorParques.Negocio.Entidades.DiccionarioContigencia>();
                            DiccionarioContigencia _con = asd.Result;

                            Utilidades.RegistrarErrorContingencia(new Exception($"Franquicias: {_con.ListFranquicia.Count()} - ListMenuPerfil: {_con.ListMenuPerfil.Count()} - ListPerfilUsuario: {_con.ListPerfilUsuario.Count()} - ListPtoUsuario: {_con.ListPtoUsuario.Count()}"), "ActualizarDiccionarios");
                            if (_con != null)
                            {
                                _cnn.Query("TRUNCATE TABLE sec.TB_MenuPerfil TRUNCATE TABLE sec.TB_PerfilUsuario TRUNCATE TABLE sec.TB_PuntoUsuario TRUNCATE TABLE TB_CampanaDonacion TRUNCATE TABLE TB_ProductosCampanaDonacion");

                                foreach (var item in _con.ListMenuPerfil)
                                    _cnn.Query("INSERT INTO sec.TB_MenuPerfil VALUES (" + item.IdMenu + "," + item.IdPerfil + ")");


                                Utilidades.RegistrarErrorContingencia(new Exception("inserto menuperfil"), "ActualizarDiccionarios");

                                foreach (var item in _con.ListPerfilUsuario)
                                    _cnn.Query("INSERT INTO sec.TB_PerfilUsuario VALUES (" + item.IdPerfil + "," + item.IdUsuario + ")");


                                Utilidades.RegistrarErrorContingencia(new Exception("inserto perfilusuario"), "ActualizarDiccionarios");
                                foreach (var item in _con.ListPtoUsuario)
                                    _cnn.Query("INSERT INTO sec.TB_PuntoUsuario VALUES (" + item.IdPunto + "," + item.IdUsuario + ")");


                                Utilidades.RegistrarErrorContingencia(new Exception("inserto campanadonacion"), "ActualizarDiccionarios");
                                if (_con.ListCampanaDocacion != null && _con.ListCampanaDocacion.Count() > 0)
                                {
                                    foreach (var item in _con.ListCampanaDocacion)
                                        _cnn.Query("INSERT INTO TB_CampanaDonacion VALUES (" + item.IdCampanaDonacion + ",'" + item.Nombre + "'," + item.Activo + ",'" + item.Texto + "')");
                                }

                                Utilidades.RegistrarErrorContingencia(new Exception("inserto productocampanadonacion"), "ActualizarDiccionarios");
                                if (_con.ListProductoCampanaDocanion != null && _con.ListProductoCampanaDocanion.Count() > 0)
                                {
                                    foreach (var item in _con.ListProductoCampanaDocanion)
                                        _cnn.Query("INSERT INTO TB_ProductosCampanaDonacion VALUES (" + item.IdCampanaDonacion + ",'" + item.CodSapProducto+ "')");
                                }

                                Utilidades.RegistrarErrorContingencia(new Exception("inserto puntousuario"), "ActualizarDiccionarios");

                                if (_con.ListFranquicia != null && _con.ListFranquicia.Count() > 0)
                                {
                                    //Tabla franquicias no tiene identity
                                    _cnn.Query("TRUNCATE TABLE dbo.TB_Franquicias");
                                    foreach (var item in _con.ListFranquicia)
                                        _cnn.Query($"INSERT INTO dbo.TB_Franquicias(IdFranquicia,NomFranquicia,IdGrupoFranquiciaSap) VALUES ({item.Id},'{item.Nombre}',0)");

                                    Utilidades.RegistrarErrorContingencia(new Exception("inserto franquicias"), "ActualizarDiccionarios");
                                }
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                if (_cnn.State == System.Data.ConnectionState.Open)
                    _cnn.Close();

                Utilidades.RegistrarErrorContingencia(ex, "ActualizarDiccionarios");
                throw;
            }
        }

        /// <summary>
        /// Obtiene la última factura del punto server principal
        /// </summary>
        public void ObtenerUltimaFacturaPunto()
        {
            try
            {
                var filePath = Path.GetFullPath(ConfigurationManager.AppSettings["RutaWebConfigAppCore"].ToString());
                var map = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
                var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                var cod = configFile.AppSettings.Settings["IdPunto"].Value;

                HttpClient client;
                client = new HttpClient();
                var _host = new Uri(ConfigurationManager.AppSettings["UrlService"]);

                client.BaseAddress = _host;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                if (ConfigurationManager.AppSettings["IntentosConsumoServicio"] != null && (cod != null && cod.Length > 0 ))
                {

                    if (Utilidades.PingContingencia(_host.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString()))) //3 cantidad de intentos
                    {
                        HttpResponseMessage response = AsyncHelpers.RunSync(() => client.GetAsync($"Factura/ObtenerUltimaFactura/{cod}"));

                        if (response.IsSuccessStatusCode)
                        {
                            var asd = response.Content.ReadAsAsync<Factura>();
                            var _con = asd.Result;

                            if (_con != null && _con.IdPunto.Length > 0)
                            {
                                var buscar = _cnn.Query<string>($"SELECT Idpunto FROM TB_LogVentasPunto WHERE RTRIM(LTRIM(ISNULL(CodigoFactura, ''))) = '{_con.CodigoFactura}'");
                                if (buscar == null || buscar.Count() == 0)
                                    _cnn.Query($"INSERT INTO TB_LogVentasPunto VALUES({_con.IdPunto}, null, GETDATE(), '{_con.CodigoFactura}')");
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "ObtenerUltimaFacturaPunto");
            }
        }

        private bool ValidarRuta(string ruta)
        {
            return File.Exists(@ruta);
        }

        private void ValidarRutaTemporal(string ruta)
        {
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
        }

        private void CopiarArchivo(string Origen, string Destino)
        {
            File.Copy(Origen, Destino,true);
        }
    }
}
