using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioGrupoNotificacion : IRepositorioBase<Grupo>
    {
        bool ActualizarGrupoUsuario(Grupo Modelo);
        Grupo ObtenerUsuarioxGrupo(int idGrupo);
        IEnumerable<Grupo> ObtenerTodosGrupo();
        bool ObtenerxNombre(Grupo modelo);
    }
}
