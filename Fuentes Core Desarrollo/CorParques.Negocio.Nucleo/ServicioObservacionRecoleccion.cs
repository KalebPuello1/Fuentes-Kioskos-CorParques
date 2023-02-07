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

	public class ServicioObservacionRecoleccion : IServicioObservacionRecoleccion
	{

		private readonly IRepositorioObservacionRecoleccion _repositorio;

		#region Constructor
		public ServicioObservacionRecoleccion (IRepositorioObservacionRecoleccion repositorio)
		{

			_repositorio = repositorio;
		}

        public bool Actualizar(ObservacionRecoleccion modelo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RDSH: Inserta una observacion para una recoleccion.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public ObservacionRecoleccion Crear(ObservacionRecoleccion modelo)
        {
            modelo.IdObservacionRecoleccion = _repositorio.Insertar(ref modelo);
            if (modelo.IdObservacionRecoleccion > 0)
            {
                return modelo;
            }
            else
            {
                return null;
            }
            
        }

        public ObservacionRecoleccion Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ObservacionRecoleccion> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RDSH: Retorna las observaciones realizadas por un usuario para una recoleccion.
        /// </summary>
        /// <param name="IdRecoleccion"></param>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        public IEnumerable<ObservacionRecoleccion> ObtenerPorIdRecoleccionUsuario(int IdRecoleccion, int IdUsuario)
        {
            return _repositorio.ObtenerPorIdRecoleccionUsuario(IdRecoleccion, IdUsuario);
        }

        #endregion
    }
}
