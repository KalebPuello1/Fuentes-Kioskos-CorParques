using Corparques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReporteVentasPorConvenio : RepositorioBase<ReporteVentasPorConvenio>, IRepositorioReporteVentasPorConvenio
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion Declaraciones

        #region Constructor

        public RepositorioReporteVentasPorConvenio()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion Constructor

        public IEnumerable<ReporteVentasPorConvenio> ObtenerReporte(string FechaInicial, string FechaFinal)
        {
            try
            {
                var rta = _cnn.Query<ReporteVentasPorConvenio>("SP_ReporteVentasPorConvenio", param: new
                {
                    FechaInicial,
                    FechaFinal
                }, commandType: CommandType.StoredProcedure);

                return rta;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }
    }
}
