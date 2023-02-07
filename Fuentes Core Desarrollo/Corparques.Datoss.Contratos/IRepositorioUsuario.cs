using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioUsuario :IRepositorioBase<Usuario>
    {
        /// <summary>
        /// el filtro es como se va a usar en el SQL
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        IEnumerable<TipoGeneral> ObtenerUsuarioSimple(string filtro);
        Usuario GetUserPassword(string user, string pwd, int Punto);
        int InsertarUsuarioPerfil(Usuario usuario);
        bool ActualizarUsuario(Usuario usuario);
        Usuario GetUserPassword2(string user, string pwd);
        SaldoApp GetSaldoApp(string qr);
        IEnumerable<Usuario> ObtenerUsuariosPorPefil(string IdsPerfil);
        IEnumerable<Usuario> ObtenerUsuariosSinAperturaPorPefil(string IdsPerfil, int Tipo);
        IEnumerable<Perfil> ObtenerPerfilxUsuario(int idUsuario);
        bool ActualizarPerfilUsuario(Usuario usuario);
        string InsertarUsuarioExterno(Usuario usuario);
        Usuario GetUserPasswordExterno(string user, string pwd, int Punto);
        bool ActualizarExterno(ref UsuarioExterno usuario);
        string updatePedidoUsuarioExterno(string pedido, int idUsuario, string IdSap);
    }
}
