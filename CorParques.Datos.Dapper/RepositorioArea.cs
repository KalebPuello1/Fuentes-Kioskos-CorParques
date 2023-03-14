using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{

    public class RepositorioArea : RepositorioBase<Area>, IRepositorioArea
    {

        #region Metodos

        /// <summary>
        ///RDSH: Retorna lista de areas activas.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaAreas()
        {
            var consulta = _cnn.Query<Area>("SP_consultarAreas", null, commandType: System.Data.CommandType.StoredProcedure);
            return consulta.Select(x => new TipoGeneral { Id = x.IdArea, Nombre = x.descripcion });
        }

        #endregion


    }
}
