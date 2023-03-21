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

	public class ServicioMatrizPuntos : IServicioMatrizPuntos
	{

		private readonly IRepositorioMatrizPuntos _repositorio;

		#region Constructor

		public ServicioMatrizPuntos(IRepositorioMatrizPuntos repositorio)
		{
			_repositorio = repositorio;
		}




        #endregion

        #region Metodos


        public bool Actualizar(TipoGeneral modelo)
        {
            throw new NotImplementedException();
        }

        public string InsertarMatriz(TipoGeneral modelo)
        {
            return _repositorio.InsertarMatriz(modelo);
        }

        public TipoGeneral Crear(TipoGeneral modelo)
        {
            throw new NotImplementedException();
        }

        public string Eliminar(int id)
        {
            return _repositorio.EliminarMatriz(id);
        }

        public TipoGeneral Obtener(int id)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<TipoGeneral> ObtenerTodos()
        {
            return _repositorio.ObtenerMatriz();
        }

        #endregion




    }
}
