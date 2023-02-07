//Cambioquitar: Este controlador usa el enumerador de perfiles.
using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Util;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioUsuario : IServicioUsuario
    {
        IRepositorioUsuario _repositorio;
        IRepositorioPerfil _repositorioPerfil;

        public ServicioUsuario(IRepositorioUsuario repositorio, IRepositorioPerfil repositorioPerfil)
        {
            _repositorio = repositorio;
            _repositorioPerfil = repositorioPerfil;
        }

        public bool Actualizar(ref Usuario usuario)
        {
            return _repositorio.ActualizarUsuario(usuario);
        }

        public bool ActualizarLogueado(ref Usuario usuario)
        {
            
            return _repositorio.Actualizar(ref usuario);
        }
        public bool ActualizarLogueadoExterno(ref UsuarioExterno usuario)
        {
           return _repositorio.ActualizarExterno(ref usuario);
        }
        public bool Eliminar(Usuario usuario)
        {
            return _repositorio.Eliminar(usuario);
        }

        public Usuario GetUserPassword2(string user, string pwd)
        {
            return _repositorio.GetUserPassword2(user, pwd);
        }
        public SaldoApp GetSaldoApp(string qr)
        {
            return _repositorio.GetSaldoApp(qr);
        }

        public bool Inactivar(Usuario usuario)
        {
            return _repositorio.Actualizar(ref usuario);
        }

        public bool Insertar(ref Usuario usuario)
        {
            return _repositorio.InsertarUsuarioPerfil(usuario)>0;
        }

        public Usuario Obtener(int id, int Punto)
        {
            var user =  _repositorio.Obtener(id);
            return _repositorio.GetUserPassword(user.NombreUsuario, user.Password, Punto);
        }

        public Usuario Obtener(string user, string pwd, int Punto)
        {
            var usuario = _repositorio.GetUserPassword(user, pwd, Punto);
            List<Menu> menuFinal = new List<Menu>();
            if (usuario != null)
            {
                foreach (var item in usuario.ListaPerfiles)
                {
                    try
                    {
                        var menu = _repositorioPerfil.Obtener(item.Id).Menus;
                        foreach (var item1 in menu)
                        {
                            if (menuFinal.FirstOrDefault(x => x.IdMenu.Equals(item1.IdMenu)) == null)
                                menuFinal.Add(item1);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
                usuario.ListaMenu = menuFinal.OrderBy(x => x.IdPadre).ThenBy(x => x.Orden).Where(x => x.IdEstado == 1).ToList();
            }
            return usuario;   
        }

        public IEnumerable<Usuario> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        public IEnumerable<Usuario> ObtenerTodosSinAdmin()
        {
            var _Usuarios = _repositorio.ObtenerLista();
            return _Usuarios.Where(x => x.IdTipoPuntoLogueado != (int)Enumerador.Perfiles.Administrador && x.NombreUsuario.StartsWith("40"));
        }

        public IEnumerable<Usuario> ObtenerTodosPorGrupo(int idGrupo)//TODO: Este metodo no corresponde a usuarios corresponde a grupos ya que este listado se obtiene de una relacion de usuarios con grupos: Se puede crear una extension de la interfaz y del servicio para estos escenarios
        {
            return _repositorio.StoreProcedure("SP_GetAllUserInGroup", new { idGroup = idGrupo });
        }
        public IEnumerable<Usuario> ObtenerUsuariosPorPerfil(string IdsUsuarios)
        {
            return _repositorio.ObtenerUsuariosPorPefil(IdsUsuarios);
        }

        public IEnumerable<Usuario> ObtenerUsuariosSinAperturaPorPefil(string IdsPerfil, int Tipo)
        {
            return _repositorio.ObtenerUsuariosSinAperturaPorPefil(IdsPerfil, Tipo);
        }

        public IEnumerable<TipoGeneral> ObtenerUsuarioSimple(string filtro)
        {
            return _repositorio.ObtenerUsuarioSimple(filtro);
        }

        public IEnumerable<Perfil> ObtenerPerfilxUsuario(int idUsuario)
        {
            return _repositorio.ObtenerPerfilxUsuario( idUsuario);
        }

        public bool ActualizarPerfilUsuario( ref Usuario usuario)
        {
            return _repositorio.ActualizarPerfilUsuario( usuario);
        }
        public string InsertarUsuarioExterno(Usuario usuario)
        {
            return _repositorio.InsertarUsuarioExterno(usuario);
        }
        public Usuario ObtenerExternos(string user, string pwd, int Punto)
        {
            var usuario = _repositorio.GetUserPasswordExterno(user, pwd, Punto);
            List<Menu> menuFinal = new List<Menu>();
            if (usuario != null)
            {
                foreach (var item in usuario.ListaPerfiles)
                {
                    try
                    {
                        var menu = _repositorioPerfil.Obtener(item.Id).Menus;
                        foreach (var item1 in menu)
                        {
                            if (menuFinal.FirstOrDefault(x => x.IdMenu.Equals(item1.IdMenu)) == null)
                                menuFinal.Add(item1);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
                usuario.ListaMenu = menuFinal.OrderBy(x => x.IdPadre).ThenBy(x => x.Orden).Where(x => x.IdEstado == 1).ToList();
            }
            return usuario;     
        }
        public string updatePedidoUsuarioExterno(string pedido, int idUsuario, string IdSap)
        {
            var dato = _repositorio.updatePedidoUsuarioExterno(pedido, idUsuario, IdSap);
            return dato;
        }
    }
}
