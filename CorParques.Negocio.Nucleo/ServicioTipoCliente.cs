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

	public class ServicioTipoCliente : IServicioTipoCliente
    {

		private readonly IRepositorioTipoCliente _repositorio;

		#region Constructor
		public ServicioTipoCliente (IRepositorioTipoCliente repositorio)
		{

			_repositorio = repositorio;
		}
        #endregion

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple();
        }
    }
}
