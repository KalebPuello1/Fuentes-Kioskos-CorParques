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

    public class RepositorioOrden : RepositorioBase<Orden>, IRepositorioOrden
    {

        #region Metodos

        public IEnumerable<Orden> ObtenerListaOrdenes()
        {
            return _cnn.Query<Orden>("sp_obtnerOrdenFalla", null, commandType: System.Data.CommandType.StoredProcedure);
        }

        #endregion


    }
}
