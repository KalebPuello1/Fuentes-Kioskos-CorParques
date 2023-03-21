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

	public class ServicioRegistroTorniquete : IServicioRegistroTorniquete
	{

		private readonly IRepositorioRegistroTorniquete _repositorio;

		#region Constructor
		public ServicioRegistroTorniquete (IRepositorioRegistroTorniquete repositorio)
		{

			_repositorio = repositorio;
		}
        #endregion

        #region Metodos

        /// <summary>
        /// RDSH:Actualiza el registro de torniquete y remueve a los auxiliares asociados al punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(RegistroTorniquete modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        /// <summary>
        /// RDSH:Actualiza el registro de torniquete y remueve a los auxiliares asociados al punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(RegistroTorniquete modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        /// <summary>
        /// RSDH: Retorna un objeto torniquete con la informacion de del dia o del ultimo registro.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <param name="blnDia"></param>
        /// <returns></returns>
        public RegistroTorniquete ObtenerRegistroTorniquete(int intIdPunto)
        {
            return _repositorio.ObtenerRegistroTorniquete(intIdPunto);
        }

        #endregion

        #region Metodos_NoImplementados

        public bool Actualizar(RegistroTorniquete modelo)
        {
            throw new NotImplementedException();
        }

        public RegistroTorniquete Crear(RegistroTorniquete modelo)
        {
            throw new NotImplementedException();
        }
        
        public RegistroTorniquete Obtener(int id)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<RegistroTorniquete> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
