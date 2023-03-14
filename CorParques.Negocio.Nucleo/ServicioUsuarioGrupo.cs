using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioUsuarioGrupo : IServicioUsuarioGrupo
    {
        private readonly IRepositorioUsuarioGrupo _repositorio;


        public ServicioUsuarioGrupo(IRepositorioUsuarioGrupo gestion)
        {
            _repositorio = gestion;
        }

        public bool Actualizar(UsuarioGrupo modelo)
        {
            throw new NotImplementedException();
        }

        public UsuarioGrupo  Crear(UsuarioGrupo modelo)
        {

            if (_repositorio.Insertar(ref modelo)>0)
                return modelo;
            return null;
           
        }

        public UsuarioGrupo Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<UsuarioGrupo> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        

        public IEnumerable<UsuarioGrupo> ObtenerxGrupo(int id)
        {
            return _repositorio.ObtenerxGrupo(id);
        }


        //public IEnumerable<Parametro> ObtenerTodos()
        //{
        //    return _repositorio.ObtenerLista();
        //}
    }
}
