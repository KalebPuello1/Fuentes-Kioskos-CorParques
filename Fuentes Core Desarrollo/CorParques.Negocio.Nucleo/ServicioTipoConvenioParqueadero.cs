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

	public class ServicioTipoConvenioParqueadero : IServicioTipoConvenioParqueadero
	{

		private readonly IRepositorioTipoConvenioParqueadero _repositorio;

		#region Constructor

		public ServicioTipoConvenioParqueadero (IRepositorioTipoConvenioParqueadero repositorio)
		{
			_repositorio = repositorio;
		}
        #endregion

        #region Metodos

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple();
        }

        #endregion
    }
}
