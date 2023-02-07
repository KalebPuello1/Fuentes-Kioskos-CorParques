using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace CorParques.Datos.Dapper
{
    public class RepositorioGruposNotificacion : RepositorioBase<Grupo> , IRepositorioGrupoNotificacion
    {
        public bool ActualizarGrupoUsuario(Grupo Modelo)
        {

            try
            {
                _cnn.Query<bool>("SP_ActualizarGrupoUsuario",
                param: new { IdGrupo = Modelo.Id, Nombre = Modelo.Nombre,  Descripcion = Modelo.Descripcion, Estado = Modelo.EstadoId, UsuarioCreacion = Modelo.Creado, FechaCreacion = Modelo.FechaCreado, UsuarioModificacion = Modelo.Modificado, FechaModificacion = Modelo.FechaModificado, Puntos = Modelo.PuntosAsociados },
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public Grupo ObtenerUsuarioxGrupo(int idGrupo)
        {

            return _cnn.Query<Grupo>("Retornar_PuntoGrupo", param: new { IdGrupo = idGrupo }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

        }

        public IEnumerable<Grupo> ObtenerTodosGrupo()
        {
            return _cnn.Query<Grupo>("Retornar_Todos_Grupos", commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        public bool ObtenerxNombre(Grupo modelo)
        {
            var listaxId = ObtenerUsuarioxGrupo(modelo.Id);
            if (listaxId != null)
            {
                if (listaxId.Nombre != modelo.Nombre)
                {
                    var lista = _cnn.GetList<Grupo>().Where(x => x.Nombre == modelo.Nombre);
                    return (lista.Count() > 0) ? false : true;
                }
                return true;                
            }
            else
            {
                var lista = _cnn.GetList<Grupo>().Where(x => x.Nombre == modelo.Nombre);
                return (lista.Count() > 0) ? false : true;
            }
        }
    }
}
