using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioUsuario
    {
        IEnumerable<Usuario> ObtenerTodos();
        IEnumerable<TipoGeneral> ObtenerUsuarioSimple(string filtro);
        IEnumerable<Usuario> ObtenerTodosPorGrupo(int idGrupo);
        bool Insertar(ref Usuario usuario);
        bool Eliminar(Usuario usuario);
        bool Actualizar(ref Usuario usuario);
        Usuario Obtener(string user, string pwd, int Punto);
        Usuario Obtener(int id, int Punto);
        bool Inactivar(Usuario usuario);
        bool ActualizarLogueado(ref Usuario usuario);
        bool ActualizarLogueadoExterno(ref UsuarioExterno usuario);
        Usuario GetUserPassword2(string user, string pwd);
        SaldoApp GetSaldoApp(string qr);
        IEnumerable<Usuario> ObtenerUsuariosPorPerfil(string IdsUsuarios);
        IEnumerable<Usuario> ObtenerUsuariosSinAperturaPorPefil(string IdsPerfil, int Tipo);
        IEnumerable<Usuario> ObtenerTodosSinAdmin();
        IEnumerable<Perfil> ObtenerPerfilxUsuario(int idUsuario);
        bool ActualizarPerfilUsuario(ref Usuario usuario);
        string InsertarUsuarioExterno(Usuario usuario);
        Usuario ObtenerExternos(string user, string pwd, int Punto);
        string updatePedidoUsuarioExterno(string pedido, int idUsuario, string IdSap);
    }
    }
