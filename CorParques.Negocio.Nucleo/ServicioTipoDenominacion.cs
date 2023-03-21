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

	public class ServicioTipoDenominacion : IServicioTipoDenominacion
	{

		private readonly IRepositorioTipoDenominacion _repositorio;

		#region Constructor
		public ServicioTipoDenominacion (IRepositorioTipoDenominacion repositorio)
		{

			_repositorio = repositorio;
		}

        public bool Actualizar(TipoDenominacion modelo)
        {
            throw new NotImplementedException();
        }

        public TipoDenominacion Crear(TipoDenominacion modelo)
        {
            throw new NotImplementedException();
        }

        public TipoDenominacion Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoDenominacion> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoDenominacion> ObtenerTodosActivos()
        {
            return _repositorio.ObtenerTodosActivos();
        }
        #endregion
    }
}
