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
    public class ServicioParametros : IServicioParametros
    {
        private readonly IRepositorioParametros _repositorio;

        public ServicioParametros(IRepositorioParametros repositorio)
        {
            _repositorio = repositorio;
        }
        public bool Actualizar(Parametro modelo)
        {
            var exist = _repositorio.Obtener(modelo.Id);
            exist.Nombre = modelo.Nombre;
            exist.Valor = modelo.Valor;
            exist.Modificado = modelo.Modificado;
            exist.FechModificacion = DateTime.Now;
            return _repositorio.Actualizar(ref exist);
        }

        public Parametro Crear(Parametro modelo)
        {
            modelo.FechaCreacion= DateTime.Now;
            if (_repositorio.Insertar(ref modelo)>0)
                return modelo;
            return null;
        }

        public bool Eliminar(int id)
        {
            return _repositorio.Eliminar(new Parametro { Id = id });
        }

        public Parametro Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public Parametro ObtenerParametroPorNombre(string strNombre)
        {
            return _repositorio.ObtenerParametroPorNombre(strNombre);
        }

        /// <summary>
        /// RDSH: Consulta todos los parametros de la aplicacion para almacenarlos en cache.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parametro> ObtenerParametrosGlobales()
        {
            return _repositorio.ObtenerParametrosGlobales();
        }

        public IEnumerable<Parametro> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }
    }
}
