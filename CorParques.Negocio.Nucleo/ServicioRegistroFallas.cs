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

	public class ServicioRegistroFallas : IServicioRegistroFallas
    {

		private readonly IRepositorioRegistroFallas _repositorio;

		#region Constructor

		public ServicioRegistroFallas(IRepositorioRegistroFallas repositorio)
		{
			_repositorio = repositorio;
		}

        public bool insertarRegistroFalla(RegistroFallas modelo)
        {
            return _repositorio.insertarRegistroFalla(modelo);
        }

        #endregion

        #region Metodos


        #endregion



    }
}
