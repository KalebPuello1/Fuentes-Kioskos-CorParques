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
    public class ServicioGruposNotificacion : IServicioGruposNotificacion
    {
        IRepositorioGrupoNotificacion _repositorio;

        public ServicioGruposNotificacion(IRepositorioGrupoNotificacion repositorio)
        {
            _repositorio = repositorio;
        }

        public bool Actualizar(Grupo grupo)
        {
            var modelo = _repositorio.Obtener(grupo.Id);
            grupo.Nombre = modelo.Nombre;
            grupo.EstadoId = 2;
            grupo.Creado = modelo.Creado;
            grupo.FechaCreado = modelo.FechaCreado;
            grupo.Descripcion = modelo.Descripcion;
            return _repositorio.Actualizar(ref grupo);
        }

        public bool ActualizarGrupoUsuario(Grupo Modelo)
        {
            if (Modelo.ValidaNombre)
            { 
                if (_repositorio.ObtenerxNombre(Modelo))
                { 
                    return _repositorio.ActualizarGrupoUsuario(Modelo);
                }
                return false;
            }else {
                return _repositorio.ActualizarGrupoUsuario(Modelo);
            }
        }


        public bool Eliminar(Grupo grupo)
        {
            return _repositorio.Eliminar(grupo);
        }

        public Grupo Insertar(Grupo grupo)
        {
            if (_repositorio.Insertar(ref grupo)>0)
                return grupo;
            return null;
        }

        public Grupo Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<Grupo> ObtenerTodos()
        {
            return _repositorio.ObtenerLista().Where(x => x.EstadoId == (int)Enumerador.Estados.Activo).OrderBy(M => M.Descripcion).ToList();
        }

        public IEnumerable<Grupo> ObtenerTodosGrupo()
        {
            return _repositorio.ObtenerTodosGrupo();
        }

        public Grupo ObtenerUsuarioxGrupo(int idGrupo)
        {
            return _repositorio.ObtenerUsuarioxGrupo(idGrupo);
        }
    }
}
