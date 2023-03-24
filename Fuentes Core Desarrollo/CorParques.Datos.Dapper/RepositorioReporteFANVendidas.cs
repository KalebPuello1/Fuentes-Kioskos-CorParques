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

namespace CorParques.Datos.Dapper
{
    public class RepositorioReporteFANVendidas : IRepositorioReporteFANVendidas
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReporteFANVendidas()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<ReporteFANVendidas> ObtenerReporte(string FechaInicial)
        {
            try
            {                
                return _cnn.Query<ReporteFANVendidas>(
                "SP_ReporteFANVendidas",
                param: new
                {
                    @Fecha = DateTime.Parse(FechaInicial)                    
                },
                commandType: System.Data.CommandType.StoredProcedure);

                
            }
            catch (Exception ex)
            {
                
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }
    }

}
