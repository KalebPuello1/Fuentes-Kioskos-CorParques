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

	public class ServicioTipoProducto : IServicioTipoProducto
	{

		private readonly IRepositorioTipoProducto _repositorio;

		#region Constructor

		public ServicioTipoProducto (IRepositorioTipoProducto repositorio)
		{

			_repositorio = repositorio;
		}



        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Retorna la lista de tipo de producto. para un combo.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaTipoProduto()
        {
            return _repositorio.ObtenerListaTipoProduto().Select(x => new TipoGeneral { Id = x.IdTipoProducto, Nombre = x.Nombre, CodSAP = x.CodigoSap }).ToList(); 
        }

        #endregion

        #region Metodos no implementados

        public bool Actualizar(TipoProducto modelo)
        {
            throw new NotImplementedException();
        }

        public TipoProducto Crear(TipoProducto modelo)
        {
            throw new NotImplementedException();
        }

        public TipoProducto Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoProducto> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
