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
    public class ServicioFranquicia : IServicioFranquicia
    {
        protected IRepositorioFranquicia _repositorio;

        public ServicioFranquicia(IRepositorioFranquicia repositorio)
        {
            _repositorio = repositorio;
        }

        public bool Actualizar(Franquicia modelo)
        {
            Franquicia _franquicia = modelo;
            return _repositorio.Actualizar(ref _franquicia);

        }

        public Franquicia Crear(Franquicia modelo)
        {
            var _model = _repositorio.Insertar(ref modelo);
            if (_model > 0)
                return new Franquicia { Id = _model, Nombre = modelo.Nombre };
            else
                return new Franquicia();
        }

        public Franquicia Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<Franquicia> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }
    }
}
