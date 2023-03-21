using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioGruposNotificacion
    {
        IEnumerable<Grupo> ObtenerTodos();
        Grupo Insertar(Grupo grupo);
        bool Actualizar(Grupo grupo);
        bool Eliminar(Grupo grupo);
        Grupo Obtener(int id);
        bool ActualizarGrupoUsuario(Grupo Modelo);
        Grupo ObtenerUsuarioxGrupo(int idGrupo);
        IEnumerable<Grupo> ObtenerTodosGrupo();
    }
}
