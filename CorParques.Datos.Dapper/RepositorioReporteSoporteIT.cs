using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Data;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReporteSoporteIT : RepositorioBase<ReporteVentas>, IRepositorioReporteSoporteIT
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReporteSoporteIT()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<SoporteIT> ObtenerReporte(string Fecha)
        {
         try
         {

                var respuesta = _cnn.Query<SoporteIT>(
                "SP_ReporteSoporteIT",
                param: new
                {
                    @Fecha = Fecha
                },
                commandType: System.Data.CommandType.StoredProcedure);

                return respuesta;


            }
          catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioReporteSoporteIT.ObtenerReporte");
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }
    }

}
