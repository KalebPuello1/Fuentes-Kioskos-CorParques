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
    public class RepositorioReporteCortesias : IRepositorioReporteCortesias
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReporteCortesias()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<ReporteCortesias> ObtenerReporte(string FechaInicial, string FechaFinal, string Documento, string TarjetaFan)
        {
            if (FechaInicial == "null")
                FechaInicial = "01-01-1900";
            if (FechaFinal == "null")
                FechaFinal = "01-01-9000";
            if (Documento == "null")
                Documento = null;
            if (TarjetaFan == "null")
                TarjetaFan = null;
            try
            {                
                return _cnn.Query<ReporteCortesias>(
                "SP_ReporteCortesias",
                param: new
                {
                    @FechaInicial = DateTime.Parse(FechaInicial),
                    @FechaFinal = DateTime.Parse(FechaFinal),
                    @Documento = Documento,
                    @TarjetaFan = TarjetaFan
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
