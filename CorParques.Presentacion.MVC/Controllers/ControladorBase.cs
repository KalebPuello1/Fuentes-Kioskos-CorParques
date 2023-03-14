using CorParques.Presentacion.MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;

namespace CorParques.Presentacion.MVC.Controllers
{
    
    public class ControladorBase : Controller
    {
        public static HttpClient client;

        #region Propiedades

        public int IdUsuarioLogueado
        {
            get
            {
                return (Session["UsuarioAutenticado"] as Usuario).Id;
            }
        }

        #endregion

        internal static async Task<T> GetAsync<T>(string path) where T : class
        {
            Initializer();
            //HttpResponseMessage response = await client.GetAsync(path);
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<T>();
                return result;
            }
            return null;
        }

        internal static async Task<RespuestaViewModel> PostAsync<T,P>(string path, T element) where T : class where P : class
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

        internal static async Task<P> PutAsync<T,P>(string path, T element) where P : class where T : class
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
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlService"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
    }
}