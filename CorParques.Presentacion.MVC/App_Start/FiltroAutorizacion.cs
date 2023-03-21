using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CorParques.Presentacion.MVC.App_Start
{
    public class FiltroAutorizacion : AuthorizeAttribute
    {
        
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
                httpContext.Session["UsuarioAutenticado"] = httpContext.Session["UsuarioAutenticado"];
                httpContext.Session.Timeout = int.Parse(((System.Web.Configuration.SessionStateSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState")).Timeout.TotalMinutes.ToString());

                //Manuel Ochoa - Valida si tiene acceso al formulario solicitado en la URL
                string ControladorActionValidar = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
                string ControladorValidar = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
                string ActionValidar = httpContext.Request.RequestContext.RouteData.GetRequiredString("action");
                if (!string.IsNullOrEmpty(ActionValidar) && ActionValidar != "Index")
                {
                    ControladorValidar += ("/" + ActionValidar);
                }

                if (!string.IsNullOrEmpty(ControladorValidar)
                    && ControladorValidar != "Home"
                    && ControladorValidar != "Cuenta")
                {
                    bool IsValid = false;
                    foreach (Menu itemMenu in usuario.ListaMenu)
                    {
                        if (itemMenu.Controlador == ControladorValidar || itemMenu.Controlador == ControladorActionValidar)
                        {
                            IsValid = true;
                            break;
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

    }
}