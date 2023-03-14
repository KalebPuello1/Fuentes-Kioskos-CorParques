using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class UsuariosController : ControladorBase
    {
        //Ventana principal Crud de usuarios 
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<Usuario>>("Usuario/GetAll");
            return View(lista);
        }

        [HttpGet]
        public async Task<ActionResult> IndexPerfilUsuario()
        {
            var lista = await GetAsync<IEnumerable<Usuario>>("Usuario/GetAll");
            return View(lista);
        }
        /// <summary>
        /// Retorna vista partial para crear usuario 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetPartial()
        {
            var usuario = new Usuario();
            
            usuario.ListaPuntos = new List<Puntos>();
            
            ViewBag.lista = await GetAsync<IEnumerable<TipoGeneral>>("Perfil/ObtenerListaSimple");
            ViewBag.listaPunto = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Puntos}");
            ViewBag.listaTipoPunto = await GetAsync<IEnumerable<TipoGeneral>>("TipoPunto/ObtenerListaSimple");
            ViewBag.listaEmpleados = await GetAsync<IEnumerable<EstructuraEmpleado>>("ConvenioParqueadero/ObtenerListaEmpleados");

            return PartialView("_Create", usuario);
        }

        /// <summary>
        /// Inserta Usuario 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> Insert(Usuario modelo, string hdListaPuntos)
        {
            modelo.ListaPerfiles = new List<TipoGeneral>();
            modelo.ListaPuntos = new List<Puntos>();
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;

            
            if (!string.IsNullOrEmpty(hdListaPuntos))
                foreach (var item in hdListaPuntos.Split(','))
                    modelo.ListaPuntos.Add(new Puntos { Id = Convert.ToInt32(item) });

            modelo.IdEstado = (int)Enumerador.Estados.Activo;
            if (string.IsNullOrEmpty(modelo.Password))
                modelo.Password = ConfigurationManager.AppSettings["pwdGeneric"];


            //modelo.Password = Encripcion.Encriptar(modelo.Password, ConfigurationManager.AppSettings["llaveEncripcion"]);
            modelo.Password = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            modelo.Password2 = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            modelo.CambioPassword = true;
            var respuesta = await PostAsync<Usuario, string>("Usuario/Insert", modelo);
            if (string.IsNullOrEmpty(respuesta.Mensaje))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el usuario. Por favor intentelo de nuevo" });

        }

        /// <summary>
        /// Obtener lista de usuarios en vista parcial
        /// </summary>
        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<Usuario>>("Usuario/GetAll");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetListPerfilUsuario()
        {
            var lista = await GetAsync<IEnumerable<Usuario>>("Usuario/GetAll");
            return PartialView("_ListPerfilUsuario", lista);
        }

        //Eliminar Usuario 
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await GetAsync<Usuario>($"Usuario/GetById?id={id}&Punto={0}");
            usuario.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            usuario.FechaModificacion = DateTime.Now;
            usuario.IdEstado = (int)Enumerador.Estados.Inactivo;
            if (await PostAsync<Usuario, string>("Usuario/Delete", usuario) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al desactivar el usuario. Por favor intentelo de nuevo" });
        }

        /// <summary>
        /// Retorna objeto Usuario para actualizarlo
        /// </summary>
        public async Task<ActionResult> GetById(int id)
        {
            var usuario = await GetAsync<Usuario>($"Usuario/GetById?id={id}&Punto={0}");

            ViewBag.listaEmpleados = await GetAsync<IEnumerable<EstructuraEmpleado>>("ConvenioParqueadero/ObtenerListaEmpleados");
            ViewBag.lista = await GetAsync<IEnumerable<TipoGeneral>>("Perfil/ObtenerListaSimple");
            ViewBag.listaPunto = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Puntos}");
            ViewBag.listaTipoPunto = await GetAsync<IEnumerable<TipoGeneral>>("TipoPunto/ObtenerListaSimple");
            return PartialView("_Edit", usuario);
        }

        public async Task<ActionResult> GetByIdPerfilUsuario(int id)
        {
            var usuario = await GetAsync<Usuario>($"Usuario/GetById?id={id}&Punto={0}");

            ViewBag.listaEmpleados = await GetAsync<IEnumerable<EstructuraEmpleado>>("ConvenioParqueadero/ObtenerListaEmpleados");
            ViewBag.lista = await GetAsync<IEnumerable<TipoGeneral>>("Perfil/ObtenerListaSimple");
            ViewBag.listaPunto = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            ViewBag.Estados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Puntos}");
            ViewBag.listaTipoPunto = await GetAsync<IEnumerable<TipoGeneral>>("TipoPunto/ObtenerListaSimple");
            usuario.Perfiles = await GetAsync<IEnumerable<Perfil>>($"Usuario/ObtenerPerfilxUsuario/{id}");

            return PartialView("_EditPerfilUsuario", usuario);
        }

       

        public async Task<ActionResult> ConsultarSegregacion(int IdPerfil )
        {
            
   
            var resultado = await GetAsync<Perfil>($"Perfil/ConsultarSegregacion?IdPerfil={IdPerfil}");
            if (resultado == null)
                return Json(new RespuestaViewModel { Correcto = true, Elemento = resultado }, JsonRequestBehavior.AllowGet);
            else
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Este usuario tiene conflicto con el siguiente perfil " }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Metodo para actualizar usuario
        /// </summary>
        public async Task<ActionResult> Update(Usuario modelo, string hdListaPuntos)
        {
            try
            {

                modelo.ListaPerfiles = new List<TipoGeneral>();
                modelo.ListaPuntos = new List<Puntos>();


                if (!string.IsNullOrEmpty(hdListaPuntos))
                    foreach (var item in hdListaPuntos.Split(','))
                    {
                        if (Convert.ToInt32(item) != 0)
                        {
                            modelo.ListaPuntos.Add(new Puntos { Id = Convert.ToInt32(item) });
                        }                        
                    }                        
                modelo.FechaModificacion = DateTime.Now;
                modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
                //if (!string.IsNullOrEmpty(modelo.Password))
                //{
                //    modelo.Password = Encripcion.Encriptar(modelo.Password, ConfigurationManager.AppSettings["llaveEncripcion"]);
                //}
                if (await PutAsync<Usuario, string>("Usuario/Update", modelo) != null)
                    return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al actualizar el usuario. Por favor intentelo de nuevo" });

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "UsuariosController_Update");
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al actualizar el usuario. Por favor intentelo de nuevo" });
            }
        }

        public async Task<ActionResult> UpdatePerfilUsuario(Usuario modelo, string hdListaPerfiles)
        {
            try
            {

                modelo.ListaPerfiles = new List<TipoGeneral>();
               
                if (!string.IsNullOrEmpty(hdListaPerfiles))
                    foreach (var item in hdListaPerfiles.Split(','))
                    {
                        if (Convert.ToInt32(item) != 0)
                        {
                            modelo.ListaPerfiles.Add(new TipoGeneral { Id = Convert.ToInt32(item) });
                        }
                    }
                modelo.FechaModificacion = DateTime.Now;
                modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
                //if (!string.IsNullOrEmpty(modelo.Password))
                //{
                //    modelo.Password = Encripcion.Encriptar(modelo.Password, ConfigurationManager.AppSettings["llaveEncripcion"]);
                //}
                if (await PutAsync<Usuario, string>("Usuario/UpdatePerfilUsuario", modelo) != null)
                    return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al actualizar el usuario. Por favor intentelo de nuevo" });

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "UsuariosController_Update");
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al actualizar el usuario. Por favor intentelo de nuevo" });
            }
        }

        public async Task<ActionResult> Desbloqueo(int id)
        {
            var usuario = await GetAsync<Usuario>($"Usuario/GetById?id={id}&Punto={0}");
            usuario.Logueado = false;
            if (await PutAsync<Usuario, string>("Usuario/ActualizarUsuario", usuario) != null)
            {
                await PutAsync<Usuario, string>("Usuario/DesbloquearUsuario", usuario);
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al desbloquaer el usuario. Por favor intentelo de nuevo" });
        }

        public async Task<ActionResult> Reseteopwd2(int id)
        {
            var usuario = await GetAsync<Usuario>($"Usuario/GetById?id={id}&Punto={0}");
            usuario.Password = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            usuario.Password2 = Encripcion.Encriptar(ConfigurationManager.AppSettings["pwdGeneric"], ConfigurationManager.AppSettings["llaveEncripcion"]);
            usuario.CambioPassword = true;
            if (await PutAsync<Usuario, string>("Usuario/ActualizarUsuario", usuario) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al desbloquaer el usuario. Por favor intentelo de nuevo" });
        }

          public static bool Seleccionar(string IdPunto, string idsPuntosSeleccionados)
        {
            string[] ids = idsPuntosSeleccionados.Split(',');
            var idExistente = (from i in ids
                               where i == IdPunto
                               select i).FirstOrDefault();
            return idExistente != null;
        }

    }
}