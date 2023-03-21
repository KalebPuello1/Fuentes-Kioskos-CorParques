using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class CuentaController : ControladorBase
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
             SetViewBag();
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(string user1, string pwd)
        {
            ViewBag.Validar = false;

            pwd = Encripcion.Encriptar(pwd, ConfigurationManager.AppSettings["llaveEncripcion"]);

            var user = await GetAsync<Usuario>($"Usuario/GetByUserPwd?pwd={Server.UrlEncode(pwd)}&user={user1}&Punto=0");

            if (user != null)
            {
                if (user.IdEstado != (int)Enumerador.Estados.Activo)
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Usuario se encuentra inactivo." }, JsonRequestBehavior.AllowGet);
                }


                new AuthenticationService().CrearCookie(user.Id.ToString());
                
                Session["UsuarioAutenticado"] = user;
                Session.Timeout = int.Parse(((System.Web.Configuration.SessionStateSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState")).Timeout.TotalMinutes.ToString());
                if (user.CambioPassword)
                    return Json(new RespuestaViewModel { Correcto = true, Mensaje = "Cambiopwd" }, JsonRequestBehavior.AllowGet);
                else
                {
                    return Json(new RespuestaViewModel { Correcto = true, Mensaje = "OK", Elemento = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Usuario o contraseña incorrectos" }, JsonRequestBehavior.AllowGet);

            }
        }


        [HttpGet]
        public void ActualizarCookie()
        {
            var user = (Usuario)Session["UsuarioAutenticado"];
            new AuthenticationService().CrearCookie(user.Id.ToString());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session["UsuarioAutenticado"] = null;
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        private void SetViewBag()
        {
            ViewBag.Error = string.Empty;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CambioClave()
        {

            return PartialView("_CambioClave");

        }

        [AllowAnonymous]
        public ActionResult ActualizarCambioClave(Usuario modelo)
        {
            var user = (Usuario)Session["UsuarioAutenticado"];
            var pdw = Encripcion.Encriptar(modelo.Password, ConfigurationManager.AppSettings["llaveEncripcion"]);

            if (pdw == user.Password)
            {
                if (modelo.Password != modelo.Password2)
                {
                    if (modelo.Password2 == modelo.ConfirmPassword2)
                    {
                        user.CambioPassword = false;
                        user.Password = Encripcion.Encriptar(modelo.Password2, ConfigurationManager.AppSettings["llaveEncripcion"]);
                        if (PutAsync<Usuario, string>("Usuario/ActualizarUsuario", user) != null)
                            return Json(new RespuestaViewModel { Correcto = true, Mensaje = "Su contraseña se ha cambiado exitosamente.", Elemento=Url.Action("Index","Home") }, JsonRequestBehavior.AllowGet);
                        return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al cambiar la contraseña. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new RespuestaViewModel { Correcto = false, Mensaje = "La nueva contraseña debe ser igual a la confirmación." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = "La nueva contraseña no puede ser igual a la anterior." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "La contraseña actual no es correcta." }, JsonRequestBehavior.AllowGet);
            }

            //return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Error al cambiar la contraseña." }, JsonRequestBehavior.AllowGet);

        }

    }
}