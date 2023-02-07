using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class ControladorBase : Controller
    {
        public static HttpClient client;

        #region Propiedades

        public int IdPunto
        {
            get
            {
                return Utilidades.ObtenerInformacionPunto(ConfigurationManager.AppSettings["IdPunto"].ToString()).Id;
            }
        }

        public static bool Contingencia { get; set; }

        public string NombrePunto
        {
            get
            {
                return Utilidades.ObtenerInformacionPunto(ConfigurationManager.AppSettings["IdPunto"].ToString()).Nombre;
            }
        }

        public string CodigoSapPunto
        {
            get
            {
                return ConfigurationManager.AppSettings["IdPunto"].ToString();
            }
        }

        public int IdUsuarioLogueado
        {
            get
            {
                return (Session["UsuarioAutenticado"] as Usuario).Id;
            }
        }

        public string UsuarioLogueado
        {
            get
            {
                return (Session["UsuarioAutenticado"] as Usuario).NombreUsuario;
            }
        }

        public string NombreUsuarioLogueado
        {
            get
            {
                return string.Concat((Session["UsuarioAutenticado"] as Usuario).Nombre, " ", (Session["UsuarioAutenticado"] as Usuario).Apellido);
            }
        }

        #endregion

        internal static async Task<T> GetAsync<T>(string path) where T : class
        {
            Initializer();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<T>();
                return result;
            }
            return null;
        }

        internal static async Task<RespuestaViewModel> PostAsync<T, P>(string path, T element) where T : class where P : class
        {
            Initializer();
            HttpResponseMessage response = await client.PostAsJsonAsync(path, element);
            var rta = new RespuestaViewModel { Correcto = response.IsSuccessStatusCode };
            if (rta.Correcto)
                rta.Elemento = await response.Content.ReadAsAsync<P>();
            else
                rta.Mensaje = await response.Content.ReadAsAsync<string>();
            return rta;


        }

        internal static async Task<RespuestaViewModel> PostAsyncLocal<T,P>(string path,T element) where T: class where P: class
        {
            var rta = new RespuestaViewModel();
            var url = ConfigurationManager.AppSettings["UrlServiceLocal"].ToString();
            Uri _host = new Uri(url);

            if (Utilidades.PingWeb(_host.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString()))) //3 cantidad de intentos
            {
                var _client = new HttpClient();
                _client.BaseAddress = _host;
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _client.PostAsJsonAsync(path, element);
                rta = new RespuestaViewModel { Correcto = response.IsSuccessStatusCode };

                if (rta.Correcto)
                    rta.Elemento = await response.Content.ReadAsAsync<P>();
                else
                    rta.Mensaje = await response.Content.ReadAsAsync<string>();
            }
            return rta;
        }

        internal static async Task<P> PutAsync<T, P>(string path, T element) where P : class where T : class
        {
            Initializer();
            HttpResponseMessage response = await client.PutAsJsonAsync(path, element);

            var result = await response.Content.ReadAsAsync<P>();
            return result;


        }
        internal static async Task<bool> DeleteAsync(string path)
        {
            Initializer();
            HttpResponseMessage response = await client.DeleteAsync(path);
            return response.IsSuccessStatusCode;
        }

        internal static void Initializer()
        {
            var _host = new Uri(ConfigurationManager.AppSettings["UrlService"]);

            if (Utilidades.PingWeb(_host.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString()))) //3 cantidad de intentos
            {

                Contingencia = false;

                if (client == null)
                {
                    client = new HttpClient();
                    client.BaseAddress = _host;
                }else
                {
                    if (client.BaseAddress != _host)
                    {
                        client = new HttpClient();
                        client.BaseAddress = _host;
                    }
                }
            }
            else
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlServiceLocal"]);
                Contingencia = true;
            }

            System.Web.HttpContext.Current.Session["contingencia"] = Contingencia ? 1 : 0;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        }

    }
}