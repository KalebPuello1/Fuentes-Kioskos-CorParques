using CorParques.Negocio.Contratos;
using CorParques.Servicios.WebApi.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class UsuarioController : ApiController
    {
        private readonly IServicioUsuario _service;

        public UsuarioController(IServicioUsuario service)
        {
            _service = service;
        }

        [Route("api/Usuario/GetAll")]
        public HttpResponseMessage GetAll()
        {
            var list = _service.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

      
        [Route("api/Usuario/GetAllInGroup/{idGrupo}")]
        public HttpResponseMessage GetAll(int idGrupo)
        {
            var list = _service.ObtenerTodosPorGrupo(idGrupo);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [Route("api/Usuario/GetByUserPwd")]
        public HttpResponseMessage GetByUserPwd(string user,string pwd, int Punto)
        {
            var item = _service.Obtener(user,pwd, Punto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [Route("api/Usuario/GetByUserPwd2")]
        public HttpResponseMessage GetByUserPwd2(string user, string pwd)
        {
            var item = _service.GetUserPassword2(user, pwd);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [Route("api/Usuario/GetSaldoApp")]
        public HttpResponseMessage GetSaldoApp(string qr)
        {
            var item = _service.GetSaldoApp(qr);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [Route("api/Usuario/GetById")]
        public HttpResponseMessage GetById(int id, int Punto)
        {
            var item = _service.Obtener(id, Punto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet,Route("api/Usuario/ObtenerUsuariosPorPerfil")]
        public HttpResponseMessage ObtenerUsuariosPorPerfil(string idsPerfil)
        {
            var item = _service.ObtenerUsuariosPorPerfil(idsPerfil);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet,Route("api/Usuario/ObtenerPerfilxUsuario/{idUsuario}")]
        public HttpResponseMessage ObtenerPerfilxUsuario(int idUsuario)
        {
            var item = _service.ObtenerPerfilxUsuario(idUsuario);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet, Route("api/Usuario/ObtenerUsuariosSinAperturaPorPefil")]
        public HttpResponseMessage ObtenerUsuariosSinAperturaPorPefil(string idsPerfil, int Tipo)
        {
            var item = _service.ObtenerUsuariosSinAperturaPorPefil(idsPerfil, Tipo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost, Route("api/Usuario/Insert")]
        public HttpResponseMessage Insert(Negocio.Entidades.Usuario usuario)
        {
            var item = _service.Insertar(ref usuario);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                              : Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
        }

        [HttpPost, Route("api/Usuario/Delete")]
        public HttpResponseMessage Delete(Negocio.Entidades.Usuario usuario)
        {
            var item = _service.Inactivar(usuario);
            return Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpPut, Route("api/Usuario/Update")]
        public HttpResponseMessage Update(Negocio.Entidades.Usuario usuario)
        {
            var item = _service.Actualizar(ref usuario);
            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut, Route("api/Usuario/UpdatePerfilUsuario")]
        public HttpResponseMessage UpdatePerfilUsuario(Negocio.Entidades.Usuario usuario)
        {
            var item = _service.ActualizarPerfilUsuario(ref usuario);
            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut, Route("api/Usuario/ActualizarUsuario")]
        public HttpResponseMessage ActualizarUsuario(Negocio.Entidades.Usuario usuario)
        {
            var item = _service.ActualizarLogueado (ref usuario);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                             : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
        [HttpPut, Route("api/Usuario/ActualizarUsuarioExterno")]
        public HttpResponseMessage ActualizarUsuarioExterno(Negocio.Entidades.Usuario usuarioo)
        {
            Negocio.Entidades.UsuarioExterno usu = new Negocio.Entidades.UsuarioExterno();
            
            usu.Apellido = usuarioo.Apellido;
            usu.Nombre = usuarioo.Nombre;
            usu.NombreUsuario = usuarioo.NombreUsuario;
            usu.IdUsuarioCreacion= usuarioo.IdUsuarioCreacion;
            usu.ListaMenu = usuarioo.ListaMenu;
            usu.ListaPerfiles = usuarioo.ListaPerfiles;
            usu.ListaPuntos = usuarioo.ListaPuntos;
            usu.IdEstado = usuarioo.IdEstado;
            usu.CambioPassword = usuarioo.CambioPassword;
            usu.Password = usuarioo.Password;
            usu.Password2 = usuarioo.Password2;
            usu.Logueado = usuarioo.Logueado;
            usu.FechaCreacion = usuarioo.FechaCreacion;

            var item = _service.ActualizarLogueadoExterno(ref usu);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                             : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
        [Route("api/Usuario/ObtenerSinAdmin")]
        public HttpResponseMessage GetAllObtenerSinAdmin()
        {
            var list = _service.ObtenerTodosSinAdmin();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPut, Route("api/Usuario/DesbloquearUsuario")]
        public HttpResponseMessage DesbloquearUsuario(Negocio.Entidades.Usuario usuario)
        {
            var user = usuario.Id;
            var punto = usuario.IdPuntoLogueado;
            usuario.IdPuntoLogueado = 0;
            var item = _service.ActualizarLogueado(ref usuario);
            if (item)
            {
                StartHub.SetLogOut(user, punto);
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [Route("api/Usuario/GetAllSimple/{filtro}")]
        public HttpResponseMessage GetAllSimple(string filtro)
        {
            var list = _service.ObtenerUsuarioSimple(filtro);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpPost, Route("api/Usuario/InsertarUsuarioExterno")]
        public HttpResponseMessage InsertarUsuarioExterno(Negocio.Entidades.Usuario usuarioo)
        {
            var item = _service.InsertarUsuarioExterno(usuarioo);
            /*if (usuarioo != null)
            {

            }*/
            return item != "" ? Request.CreateResponse(HttpStatusCode.OK, item)
                              : Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
        }
        [Route("api/Usuario/GetByUserPwdExterno")]
        public HttpResponseMessage GetByUserPwdExterno(string user, string pwd, int Punto)
        {
            var item = _service.ObtenerExternos(user, pwd, Punto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpPost]
        [Route("api/Usuario/InsertarPedidoUsuarioExterno")]
        public HttpResponseMessage InsertarPedidoUsuarioExterno(Negocio.Entidades.PedidosPorCliente usu)
        {
            var dato = _service.updatePedidoUsuarioExterno(usu.Pedido , usu.IdUsuario, usu.IdCliente);
            return dato == "fallo" ? Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dato);
        }

    }
}
