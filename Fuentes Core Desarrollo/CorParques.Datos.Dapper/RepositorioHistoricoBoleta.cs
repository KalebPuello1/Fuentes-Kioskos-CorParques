using CorParques.Datos.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioHistoricoBoleta : IRepositorioHistoricoBoleta
    {
        private SqlConnection _cnn;

        public RepositorioHistoricoBoleta()
        {
            this._cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        public DetalleBoleta ObtenerHistoricoBoleta(string consecutivo)
        {
            DetalleBoleta detalleBoleta = new DetalleBoleta();

            var _rta = _cnn.QueryMultiple(
                sql: "SP_ConsultarHistoricoBoleta",
                commandType: System.Data.CommandType.StoredProcedure,
                param: new { ConsecutivoBoleta = consecutivo }
               );
            var _detalle = _rta.Read<DetalleBoleta>();

            if(_detalle != null && _detalle.Count() > 0)
            {
                detalleBoleta = _detalle.Single();
                detalleBoleta.historicoUso = _rta.Read<HistoricoBoleta>();
            }

            return detalleBoleta;
        }
    }
}
