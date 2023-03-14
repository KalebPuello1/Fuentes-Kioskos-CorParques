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

	public class ServicioRetorno : IServicioRetorno
	{

		private readonly IRepositorioRetorno _repositorio;

		#region Constructor
		public ServicioRetorno(IRepositorioRetorno repositorio)
		{

			_repositorio = repositorio;
		}

        public bool ActualizarOperacion(Operaciones modelo, out string error)
        {
            throw new NotImplementedException();
        }

        public bool Insertar(Retorno modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public bool InsertarDetalle(RetornoDetalle modelo, out string error)
        {
            return _repositorio.InsertarDetalle(modelo, out error);
        }

        #endregion

        #region Metodos



        #endregion

    }
}
