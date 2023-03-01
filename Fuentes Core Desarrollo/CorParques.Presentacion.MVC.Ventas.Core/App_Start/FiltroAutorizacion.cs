using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

namespace CorParques.Presentacion.MVC.Ventas.Core.App_Start
{
    public class FiltroAutorizacion : AuthorizeAttribute
    {

        public int IdPunto
        {
            get
            {
                return Utilidades.ObtenerInformacionPunto(ConfigurationManager.AppSettings["IdPunto"].ToString()).Id;
            }
        }

        public static HttpClient client;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var usuario = AsyncHelpers.RunSync<Usuario>(() => new AuthenticationService().ObtenerCookie(httpContext));
            if (usuario == null)
            {
                httpContext.Session.Remove("UsuarioAutenticado");
                httpContext.Response.Redirect("~/Cuenta/Login", true);
                return false;
            }
            else
            {
                var IdMenus = new int[] { 27, 39, 42, 49, 55, 56, 60, 69, 77, 90, 91, 93, 106, 108, 117, 118, 128, 136, 137, 144, 145, 146, 149, 164, 84 };
                usuario.ListaMenu = usuario.ListaMenu.Where(s => IdMenus.Contains(s.IdMenu)).ToList();
                
                bool Redirect = false;
                Initializer(ref Redirect);

                //Inicio

                //client = new HttpClient();
                //var contigencia = System.Web.HttpContext.Current.Session["contingencia"] == null ? 0 : (int)System.Web.HttpContext.Current.Session["contingencia"];
                //var _host = new Uri(ConfigurationManager.AppSettings["UrlService"]);
                //bool blnRedirect = false;

                //if (Utilidades.PingWeb(_host.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString())))
                //{ //3 cantidad de intentos
                //    System.Web.HttpContext.Current.Session["contingencia"] = 0;
                //    if (contigencia == 1)
                //        blnRedirect = true;
                //    client.BaseAddress = _host;
                //}
                //else
                //{
                //    System.Web.HttpContext.Current.Session["contingencia"] = 1;
                //    if (contigencia == 0)
                //        blnRedirect = true;

                //    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlServiceLocal"]);
                //}

                //var login = HttpContext.Current.Session["LoginCont"] == null ? false : (bool)HttpContext.Current.Session["LoginCont"];
                //var control = HttpContext.Current.Session["PosCont"] == null ? false : (bool)HttpContext.Current.Session["PosCont"];

                //fin 


                if (Redirect)
                {
                    System.Web.HttpContext.Current.Session["redirectContingencia"] = true;
                    httpContext.Session.Remove("UsuarioAutenticado");
                    httpContext.Response.Redirect("~/Cuenta/Login", true);
                    return false;
                }
                else
                {
                    System.Web.HttpContext.Current.Session["redirectContingencia"] = false;
                    HttpContext.Current.Session["LoginCont"] = false;
                }

                httpContext.Session["UsuarioAutenticado"] = httpContext.Session["UsuarioAutenticado"];
                httpContext.Session.Timeout = int.Parse(((System.Web.Configuration.SessionStateSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState")).Timeout.TotalMinutes.ToString());
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));                

                HttpResponseMessage response = AsyncHelpers.RunSync(() => client.GetAsync($"Notificacion/Get/{IdPunto}"));
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<IEnumerable<Notificacion>>(AsyncHelpers.RunSync<string>(() => response.Content.ReadAsStringAsync()));
                    httpContext.Session.Add("Notificaciones", result);
                }
                //Manuel Ochoa - Valida si tiene acceso al formulario solicitado en la URL
                string ControladorActionValidar = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
                string ControladorValidar = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
                string ActionValidar = httpContext.Request.RequestContext.RouteData.GetRequiredString("action");
                if (!string.IsNullOrEmpty(ActionValidar) && ActionValidar != "Index")
                {
                    ControladorActionValidar += ("/" + ActionValidar);
                }

                if (!string.IsNullOrEmpty(ControladorValidar)
                    && ControladorValidar != "Home"
                    && ControladorValidar != "Cuenta"
                    && ActionValidar != "Redenciones"
                    && ControladorValidar != "Redeban")
                {
                    bool IsValid = false;
                    foreach (Menu itemMenu in usuario.ListaMenu)
                    {
                        if (itemMenu.Controlador != null)
                        {
                            if (itemMenu.Controlador.Contains(ControladorValidar) || itemMenu.Controlador == ControladorActionValidar)
                            {
                                IsValid = true;
                                break;
                            }
                        }
                    }

                    if (!IsValid)
                    {
                        httpContext.Response.Redirect("~/Home", true);
                        return false;
                    }
                }
            }
            new AuthenticationService().CrearCookie(usuario.Id.ToString());
            return true;
        }

        internal static void Initializer(ref bool redirect)
        {
            var _host = new Uri(ConfigurationManager.AppSettings["UrlService"]);

            if (Utilidades.PingWeb(_host.Host, Convert.ToInt32(ConfigurationManager.AppSettings["IntentosConsumoServicio"].ToString()))) //3 cantidad de intentos
            {

                System.Web.HttpContext.Current.Session["contingencia"] = 0;

                if (client == null)
                {
                    client = new HttpClient();
                    client.BaseAddress = _host;
                }
                else
                {
                    if (client.BaseAddress != _host)
                    {
                        redirect = true;
                        client = new HttpClient();
                        client.BaseAddress = _host;
                    }
                }
            }
            else
            {
                System.Web.HttpContext.Current.Session["contingencia"] = 1;

                _host = new Uri(ConfigurationManager.AppSettings["UrlServiceLocal"]);

                if (client == null)
                {
                    client = new HttpClient();
                    client.BaseAddress = _host;
                }
                else
                {
                    if (client.BaseAddress != _host)
                    {
                        redirect = true;
                        client = new HttpClient();
                        client.BaseAddress = _host;
                    }
                }
            }

            //System.Web.HttpContext.Current.Session["contingencia"] = Contingencia ? 1 : 0;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

    }
}