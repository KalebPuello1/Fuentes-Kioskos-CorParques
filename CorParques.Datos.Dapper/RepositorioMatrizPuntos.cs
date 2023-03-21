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

    public class RepositorioMatrizPuntos : RepositorioBase<TipoGeneral>, IRepositorioMatrizPuntos
    {



        #region Metodos

        public string EliminarMatriz(int id)
        {
            try
            {

                var consulta = _cnn.Query<string>("SP_EliminarMatrizPuntos", param: new { id = id }, commandType: System.Data.CommandType.StoredProcedure);
                return "";
            }
            catch (Exception ex)
            {
                return string.Concat("Error al eliminar el producto: ", ex.Message);
            }
        }

        public string InsertarMatriz(TipoGeneral matriz)
        {
            try
            {

                var consulta = _cnn.Query<string>("SP_InsertarMatrizPuntos", param: new { id = matriz.Id, puntos = int.Parse(matriz.CodSAP) }, commandType: System.Data.CommandType.StoredProcedure);
                return "";
            }
            catch (Exception ex)
            {
                return string.Concat("Error al crear la relacion: ", ex.Message);
            }
        }

        public IEnumerable<TipoGeneral> ObtenerMatriz()
        {
            var consulta = _cnn.Query<TipoGeneral>("SP_ObtenerMatrizPuntos", null, commandType: System.Data.CommandType.StoredProcedure);
            return consulta;
        }

        #endregion


    }
}
