using CorParques.Negocio.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace CorParques.Transversales.Util
{
    public class AuthenticationService
    {
        public int IdPunto
        {
            get
            {
                return Utilidades.ObtenerInformacionPunto(ConfigurationManager.AppSettings["IdPunto"].ToString()).Id;
            }
        }

        public void CrearCookie(string usuario)
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
        public async Task<Usuario> ObtenerCookie(HttpContextBase context)
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
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlService"]);

                var _host = new Uri(ConfigurationManager.AppSettings["UrlService"]);

                if (ConfigurationManager.AppSettings["IntentosConsumoServicio"] != null)
                {

                    if (Utilidades.PingWeb(_host.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString()))) //3 cantidad de intentos
                        client.BaseAddress = _host;
                    else
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlServiceLocal"]);
                }
                else
                    client.BaseAddress = _host;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"Usuario/GetById/{authTicket.Name}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<Usuario>(result);
                    context.Session["UsuarioAutenticado"] = user;

                    context.Session.Timeout = int.Parse(((System.Web.Configuration.SessionStateSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState")).Timeout.TotalMinutes.ToString());
                    HttpResponseMessage responseNotifications = await client.GetAsync($"Notificacion/Get/{IdPunto}");
                    if (responseNotifications.IsSuccessStatusCode)
                    {
                        var resultNotification = await responseNotifications.Content.ReadAsStringAsync();
                        context.Session["Notificaciones"] = JsonConvert.DeserializeObject<IEnumerable<Notificacion>>(resultNotification);
                    }

                }
            }
            return context.Session["UsuarioAutenticado"] as Usuario;
        }
    }
}
