//Cambioquitar: Este controlador usa el enumerador de perfiles.
using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Redencion.Core.Models;
using CorParques.Transversales.Util;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CorParques.Presentacion.MVC.Redencion.Core.Controllers
{
    public class CuentaController : ControladorBase
    {

        [HttpGet]
        public void ActualizarCookie()
        {
            var user = (Usuario)Session["UsuarioAutenticado"];
            new AuthenticationService().CrearCookie(user.Id.ToString());
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login()
        {
            string strNombrePunto = string.Empty;
            Session["LoginCont"] = true;
            Session["PosCont"] = false;


            ActionResult rta = null;
            if (Session["UsuarioAutenticado"] != null)
            {
                var _listPuntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
                ViewBag.PuntosLogin = _listPuntos.Where(x => x.IdTipoPunto == (int)Enumerador.TiposPuntos.Atraccion && x.EstadoId.Equals((int)Enumerador.Estados.Activo)).ToList();
                rta = await ValidaIngreso(Session["UsuarioAutenticado"] as Usuario, false);
            }

            try
            {
                strNombrePunto = NombrePunto;
                if (strNombrePunto.Trim().Length <= 0)
                    strNombrePunto = "Código SAP Punto Invalido.";
                ViewBag.NombrePunto = strNombrePunto;

            }
            catch (System.Exception)
            {
                ViewBag.NombrePunto = "No Identificado";
            }

            return rta == null ? View() : rta;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(string user1, int IdPuntoLogin = 0)
        {
            ViewBag.Validar = false;
            var idPuntoConfigurado = IdPuntoLogin;



            if (IdPuntoLogin > 0)
            {


                //RDSH: Valida que el punto este en estado activo para continuar con el proceso de login.
                var objPunto = await GetAsync<Puntos>($"Puntos/GetById/{IdPuntoLogin}");
                if (objPunto != null)
                {
                    if (objPunto.EstadoId.Equals((int)Enumerador.Estados.Mantenimiento))
                    {
                        //[Route("api/Puntos/ObservacionesMantenimiento/{IdPunto}")]

                        var strObservacionMto = await GetAsync<string>($"Puntos/ObservacionesMantenimiento/{IdPuntoLogin}");

                        if (!string.IsNullOrEmpty(strObservacionMto))
                        {
                            return Json(new RespuestaViewModel { Correcto = false, Mensaje = string.Concat("[M]", strObservacionMto.Trim()) }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "El punto se encuentra en mantenimiento." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (objPunto.EstadoId != ((int)Enumerador.Estados.Activo))
                    {
                        return Json(new RespuestaViewModel { Correcto = false, Mensaje = "El punto no esta disponible para iniciar sesión." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = string.Concat("Punto con el código: ", IdPuntoLogin.ToString(), ", no se encuentra en la base de datos.") }, JsonRequestBehavior.AllowGet);
                }
            }
            var pwd = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            var user = await GetAsync<Usuario>($"Usuario/GetByUserPwd?pwd={Server.UrlEncode(pwd)}&user={user1}&Punto={idPuntoConfigurado}");

            if (user != null)
            {
                if (user.IdEstado != (int)Enumerador.Estados.Activo)
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Usuario se encuentra inactivo." }, JsonRequestBehavior.AllowGet);
                }

                //Geyner Lopez - Valida que el usuario no se encuentre logueado
                if ((user.Logueado) && (user.IdPuntoLogueado != idPuntoConfigurado) && IdPuntoLogin > 0)
                {
                    string NombreAtraccion;
                    var item = await GetAsync<Puntos>($"Puntos/GetById/{user.IdPuntoLogueado}");
                    NombreAtraccion = item.Nombre;
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Usuario tiene sesion activa en " + NombreAtraccion }, JsonRequestBehavior.AllowGet);
                }

                return await ValidaIngreso(user, true, IdPuntoLogin);

            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Usuario o contraseña incorrectos" }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            var user = (Usuario)Session["UsuarioAutenticado"];
            user.Logueado = false;
            user.IdPuntoLogueado = 0;
            var userLogueado = PutAsync<Usuario, string>("Usuario/ActualizarUsuario", user);
            Session["UsuarioAutenticado"] = null;
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult CambioClave()
        {

            return PartialView("_CambioClave");

        }


        public ActionResult ActualizarCambioClave(Usuario modelo)
        {
            var user = (Usuario)Session["UsuarioAutenticado"];
            var pdw = Encripcion.Encriptar(modelo.Password, ConfigurationManager.AppSettings["llaveEncripcion"]);

            if (pdw == user.Password2)
            {
                if (modelo.Password != modelo.Password2)
                {
                    if (modelo.Password2 == modelo.ConfirmPassword2)
                    {
                        user.Password2 = Encripcion.Encriptar(modelo.Password2, ConfigurationManager.AppSettings["llaveEncripcion"]);
                        if (PutAsync<Usuario, string>("Usuario/ActualizarUsuario", user) != null)
                            return Json(new RespuestaViewModel { Correcto = true, Mensaje = "Su contraseña se ha cambiado exitosamente." }, JsonRequestBehavior.AllowGet);
                        return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al cambiar la contraseña. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new RespuestaViewModel { Correcto = false, Mensaje = "La nueva contraseña debe ser igual a la confirmación." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = "La nueva contraseña no puede ser igual a la anterior." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "La contraseña actual no es correcta." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Error al cambiar la contraseña." }, JsonRequestBehavior.AllowGet);

        }

        private async Task<ActionResult> ValidaIngreso(Usuario user, bool validadpwd2, int IdPuntoLogin = 0)
        {
            ViewBag.Validar = false;
            var idPuntoConfigurado = IdPuntoLogin;

            bool _isPuntoValid = false;
            int _IdTipoPunto = 0;
            var item = user.ListaPuntos.FirstOrDefault(x => x.Id == idPuntoConfigurado);
            if (item != null)
            {
                _isPuntoValid = true;
                _IdTipoPunto = item.IdTipoPunto;
            }


            if ((user.Logueado) && (user.IdPuntoLogueado != idPuntoConfigurado) && IdPuntoLogin > 0)
            {
                string NombreAtraccion;
                var item2 = await GetAsync<Puntos>($"Puntos/GetById/{user.IdPuntoLogueado}");
                NombreAtraccion = item2.Nombre;
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Usuario tiene sesion activa en " + NombreAtraccion }, JsonRequestBehavior.AllowGet);
            }

            if (_isPuntoValid == true || IdPuntoLogin == 0)
            {
                string RedirectController = "";
                switch (_IdTipoPunto)
                {
                    case 1:
                        RedirectController = "Home";
                        break;
                    case 3:
                        RedirectController = "Home";
                        break;
                    case 4:
                        RedirectController = "Home";
                        break;
                    case 5:
                        RedirectController = "Home";
                        break;
                    case 6:
                        RedirectController = "Parqueadero";
                        break;
                    default:
                    case 0:
                        RedirectController = "Home";
                        break;
                }


                if ((validadpwd2 && (_IdTipoPunto != (int)Enumerador.TiposPuntos.Atraccion) && (_IdTipoPunto != (int)Enumerador.TiposPuntos.Parqueadero)) || IdPuntoLogin == 0)
                {
                    var _listPuntos = await GetAsync<IEnumerable<Puntos>>($"Puntos/ObtenerPuntosXusuario/{user.Id}");

                    ViewBag.PuntosLogin = _listPuntos.Where(x => x.EstadoId.Equals((int)Enumerador.Estados.Activo)).ToList();
                    Session["UsuarioAutenticadoTemp"] = user;
                    return Json(new RespuestaViewModel { Correcto = true, Puntos = ViewBag.PuntosLogin });
                }
                if (IdPuntoLogin > 0)
                {
                    Session["UsuarioAutenticadoTemp"] = null;
                    new AuthenticationService().CrearCookie(user.Id.ToString());
                    user.Logueado = true;
                    user.IdPuntoLogueado = idPuntoConfigurado;
                    var userLogueado = PutAsync<Usuario, string>("Usuario/ActualizarUsuario", user);
                    //var Punto = await GetAsync<Puntos>($"Puntos/GetById/{idPuntoConfigurado}");
                    user.IdTipoPuntoLogueado = _IdTipoPunto;
                    Session["UsuarioAutenticado"] = user;
                    Session["UsuarioAutenticadoJSON"] = new { id = user.Id };
                    Session["InfoPunto"] = item.Nombre;
                    Session["IdPuntoSeleccionado"] = item.Id;

                    Session.Timeout = int.Parse(((System.Web.Configuration.SessionStateSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState")).Timeout.TotalMinutes.ToString());

                    //RDSH: Hace la consulta de los parametros de la aplicacion para mantenerlos en cache.
                    await GetAsync<IEnumerable<Parametro>>("Parameters/ObtenerParametrosGlobales");


                }
                return Json(new RespuestaViewModel { Correcto = true, Elemento = Url.Action("Index", RedirectController), Mensaje = "OK" });
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Usted no cuenta con acceso a este punto" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ValidaContrasena2(string Pwd2, string ChangePwd2, string ConfirmPwd2, int IdPuntoLogin = 0)
        {
            var pwd = Encripcion.Encriptar(Pwd2, ConfigurationManager.AppSettings["llaveEncripcion"]);
            var user = (Usuario)Session["UsuarioAutenticadoTemp"];

            if (user.Password2 == pwd)
            {
                if (user.CambioPassword)
                {
                    if (ChangePwd2.Length > 0 && ConfirmPwd2.Length > 0)
                    {
                        if (ChangePwd2 == ConfirmPwd2)
                        {
                            user.CambioPassword = false;
                            user.Password2 = Encripcion.Encriptar(ChangePwd2, ConfigurationManager.AppSettings["llaveEncripcion"]);
                            var userLogueado = PutAsync<Usuario, string>("Usuario/ActualizarUsuario", user);
                            return await ValidaIngreso(user, false, IdPuntoLogin);
                        }
                        else
                        {
                            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "El cambio de contraseña no es correcto" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new RespuestaViewModel { Correcto = true, Mensaje = "Cambiopwd" }, JsonRequestBehavior.AllowGet);
                }
                return await ValidaIngreso(user, false, IdPuntoLogin);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Contraseña incorrecta" }, JsonRequestBehavior.AllowGet);
            }
        }

        //EDSP Validacion vista parcial
        public async Task<ActionResult> ValidarPassword(int idUsuario, string password)
        {
            var pwd = Encripcion.Encriptar(password, ConfigurationManager.AppSettings["llaveEncripcion"]);
            var usuario = await GetAsync<Usuario>($"Usuario/GetById?id={idUsuario}&Punto={0}");
            var rta = await GetAsync<Usuario>($"Usuario/GetByUserPwd2?user={usuario.NombreUsuario}&pwd={Server.UrlEncode(pwd)}");
            var resp = new RespuestaViewModel();

            if (rta == null)
                resp = new RespuestaViewModel { Correcto = false, Elemento = null, Mensaje = "Contraseña incorrecta!" };
            else
                resp = new RespuestaViewModel { Correcto = true, Elemento = null, Mensaje = "Contraseña correcta!" };

            return Json(resp, JsonRequestBehavior.AllowGet);

        }
        //FIN EDSP Validacion vista parcial

        //EDSP Obtener Vista Parcial Supervisor
        public async Task<ActionResult> ObtenerLoginSupervisor()
        {
            //string strPerfiles = ((int)Enumerador.Perfiles.Supervisor).ToString();// Perfiles
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilSupervisor");
            strPerfiles = objParametro.Valor;

            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
            IEnumerable<TipoGeneral> Usuarios = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });
            return PartialView("_LoginSupervisor", Usuarios);

        }

        //FIN EDSP Obtener Vista Parcial Supervisor

        //INICIO GALD Obtener Vista Parcial Login con Confirmacion
        public async Task<ActionResult> ObtenerLoginCormfirmacion(string Mensaje)
        {
            ViewBag.Mensaje = Mensaje;
            return PartialView("_LoginConfirm");

        }

        //FIN GALD Obtener Vista Parcial Login con Confirmacion

        //EDSP Obtener Login
        public ActionResult ObtenerLogin()
        {
            return PartialView("_Login");
        }
        //FIN EDSP  Obtener Login

    }
}