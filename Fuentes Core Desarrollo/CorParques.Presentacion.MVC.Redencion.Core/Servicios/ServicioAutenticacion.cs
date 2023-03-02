using CorParques.Negocio.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace CorParques.Presentacion.MVC.Redencion.Core.Servicios
{
    public static class ServicioAutenticacion
    {
        public static void CrearCookie(string usuario)
        {
            var tiempo = (System.Web.Configuration.SessionStateSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState");
            double _tiempo = tiempo == null ? 30 : tiempo.Timeout.TotalMinutes;

            var _tiket = new FormsAuthenticationTicket(1,
                    usuario,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(_tiempo),
                    true, "",
                    FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(_tiket);

            // Create the cookie.
            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            //FormsAuthentication.SetAuthCookie(usuario, true);
        }
        public static async  Task<Usuario> ObtenerCookie(HttpContextBase context)
        {
            HttpCookie authCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
                return null;
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket.Expired)
                return null;
            if (context.Session["UsuarioAutenticado"] == null)
            {
                HttpClient client;
                client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlService"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"Usuario/GetById/{authTicket.Name}");
                if (response.IsSuccessStatusCode)
                {
                    context.Session["UsuarioAutenticado"] = await response.Content.ReadAsAsync<Usuario>();
                    context.Session.Timeout = int.Parse(((System.Web.Configuration.SessionStateSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState")).Timeout.TotalMinutes.ToString());
                }
            }
            return context.Session["UsuarioAutenticado"] as Usuario; 

        }
    }
}