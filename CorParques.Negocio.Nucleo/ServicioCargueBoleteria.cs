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

	public class ServicioCargueBoleteria : IServicioCargueBoleteria
	{

		private readonly IRepositorioCargueBoleteria _repositorio;

        #region Constructor

        public ServicioCargueBoleteria (IRepositorioCargueBoleteria repositorio)
		{

			_repositorio = repositorio;
		}


        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza el estado del cargue de boleteria. 
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(CargueBoleteria modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        /// <summary>
        /// RDSH: Inserta un cargue masivo de boleteria.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(CargueBoleteria modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        /// <summary>
        /// RDSH Retorna un objeto CargueBoleteria por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CargueBoleteria Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        /// <summary>
        /// RDSH: Retorna la lista de cargues realizados en la tabla TB_CargueBoleteria.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CargueBoleteria> ObtenerListaCargueBoleteria()
        {
            return _repositorio.ObtenerListaCargueBoleteria();
        }

        /// <summary>
        /// RDSH: Retorna los productos de tipo boleteria.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerTipoBoleteria()
        {
            return _repositorio.ObtenerTipoBoleteria();
        }

        #endregion
        
        #region Metodos no implementados

        public bool Actualizar(CargueBoleteria modelo)
        {
            throw new NotImplementedException();
        }
        
        public CargueBoleteria Crear(CargueBoleteria modelo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CargueBoleteria> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
