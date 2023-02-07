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

	public class ServicioCliente : IServicioCliente
	{

		private readonly IRepositorioCliente _repositorio;

        #region Constructor

        public ServicioCliente (IRepositorioCliente repositorio)
		{

			_repositorio = repositorio;
		}

        #endregion

        #region Metodos
        
        public IEnumerable<Cliente> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        #endregion

        #region Metodos No Implementados

        public bool Actualizar(Cliente modelo)
        {
            throw new NotImplementedException();
        }

        public Cliente Crear(Cliente modelo)
        {
            throw new NotImplementedException();
        }

        public Cliente Obtener(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
