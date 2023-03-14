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

	public class ServicioTarjetaRecargable : IServicioTarjetaRecargable
    {

		private readonly IRepositorioTarjetaRecargable _repositorio;

		#region Constructor

		public ServicioTarjetaRecargable(IRepositorioTarjetaRecargable repositorio)
		{
			_repositorio = repositorio;
		}

        public string ValidarDocumento(string doc)
        {
            return _repositorio.ValidarDocumento(doc);
        }

        #endregion



    }
}
