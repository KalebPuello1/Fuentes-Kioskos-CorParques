//Cambioquitar: Este controlador usa el enumerador de perfiles.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class RecambioController : ControladorBase
    {

        public int IdUsuarioAuth
        {
            get { return (Session["UsuarioAutenticado"] as Usuario).Id; }
        }
        public async Task<ActionResult> Index()
        {
            string strPerfiles = "";
            var perfiles = (Session["UsuarioAutenticado"] as Usuario).ListaPerfiles;
            string strIdPerfilSupervisor = "";
            string strIdPerfilRecolector = "";
            string strIdPerfilNido = "";
            Parametro objParametro = null;

            try
            {
                objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilSupervisor");
                strIdPerfilSupervisor = objParametro.Valor;
                objParametro = null;
                objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilRecolector");
                strIdPerfilRecolector = objParametro.Valor;
                objParametro = null;
                objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilNido");
                strIdPerfilNido = objParametro.Valor;

                //foreach (var item in perfiles)
                //{
                //    if (item.Id == (int)Enumerador.Perfiles.Supervisor || item.Id == (int)Enumerador.Perfiles.Recolector)
                //    {
                //         strPerfiles = ((int)Enumerador.Perfiles.Nido).ToString();
                //    }
                //    else if (item.Id == (int)Enumerador.Perfiles.Nido)
                //    {
                //         strPerfiles = (int)Enumerador.Perfiles.Supervisor + "," + (int)Enumerador.Perfiles.Recolector;
                //    }                
                //}         
                foreach (var item in perfiles)
                {
                    if (item.Id.ToString().Equals(strIdPerfilSupervisor) || item.Id.ToString().Equals(strIdPerfilRecolector))
                    {
                        strPerfiles = strIdPerfilNido;
                    }
                    else if (item.Id.ToString().Equals(strIdPerfilNido))
                    {
                        strPerfiles = string.Concat(strIdPerfilSupervisor, ",", strIdPerfilRecolector);
                    }
                }
                var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
                ViewBag.usuarios = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RecambioController_Index");
                ViewBag.usuarios = new TipoGeneral();
            }
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Insertar(int idusuario, int cantidad, string observacionRec, string observacionAprov)
        {
            Recambio _recambio = new Recambio();
            _recambio.IdUsuarioCreacion = IdUsuarioAuth;
            _recambio.IdUsuarioAsignacion = idusuario;
            _recambio.ObservacionAprovado = observacionAprov;
            _recambio.ObservacionRecambio = observacionRec;
            _recambio.Valor = cantidad;
            _recambio.IdEstado = (int)Enumerador.Estados.Activo;
            var resultado = await PostAsync<Recambio, string>("Recambio/Insertar", _recambio);

            return Json(new RespuestaViewModel { Correcto = true, Mensaje = "OK" }, JsonRequestBehavior.AllowGet);
            
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ValidaClave(string usuario, string clave)
        {
            ViewBag.Validar = false;
            var IdUser = Convert.ToInt32(usuario);
            //var idPuntoConfigurado = IdPunto;
            var pwd = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            //var user = await GetAsync<Usuario>($"Usuario/GetByUserPwd?pwd={Server.UrlEncode(pwd)}&user={usuario}&Punto={idPuntoConfigurado}");

            var lista = await GetAsync<IEnumerable<Usuario>>("Usuario/GetAll");
            var user = lista.Where(x => x.Id == IdUser).ToList().SingleOrDefault();

            var pwd2 = Encripcion.Encriptar(clave, ConfigurationManager.AppSettings["llaveEncripcion"]);
            var user2 = (Usuario)Session["UsuarioAutenticadoTemp"];

            if (user != null && user.Password2 == pwd2)
            {
                return Json(new RespuestaViewModel { Correcto = true, Mensaje = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Usuario o contraseña incorrectos" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}