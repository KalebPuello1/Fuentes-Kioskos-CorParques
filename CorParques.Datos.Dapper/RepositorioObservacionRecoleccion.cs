using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

	public class RepositorioObservacionRecoleccion : RepositorioBase<ObservacionRecoleccion>,  IRepositorioObservacionRecoleccion
	{

        #region Metodos

        /// <summary>
        /// RDSH: Retorna las observaciones realizadas por un usuario para una recoleccion.
        /// </summary>
        /// <param name="IdRecoleccion"></param>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        public IEnumerable<ObservacionRecoleccion> ObtenerPorIdRecoleccionUsuario(int IdRecoleccion, int IdUsuario)
        {
            var objListaObservacionRecoleccion = _cnn.Query<ObservacionRecoleccion>("SP_ObtenerObservacionesRecoleccion", param: new
            {
                IdRecoleccion = IdRecoleccion,
                IdUsuario = IdUsuario
            }, commandType: System.Data.CommandType.StoredProcedure);

            return objListaObservacionRecoleccion;
        }

        #endregion


    }
}
