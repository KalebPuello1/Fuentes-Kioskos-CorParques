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
    public class RepositorioReportePasajerosAtracciones : IRepositorioReportePasajerosAtracciones
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReportePasajerosAtracciones()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<ReportePasajeros> ObtenerReporte(string FechaInicial, string FechaFinal, int? IdPunto, int? IdTipoProducto)
        {
            try
            {
                var objReportePasajeros = _cnn.Query<ReportePasajeros>("SP_ReportePasajerosAtracciones", param: new
                {
                    FechaInicial = FechaInicial,
                    FechaFinal = FechaFinal,
                    IdPunto = IdPunto == 0? null: IdPunto,
                    IdTipoProducto = IdTipoProducto == 0? null: IdTipoProducto
                }, commandType: CommandType.StoredProcedure).ToList();

                return objReportePasajeros;
            }
            catch (Exception ex)
            {

                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }
    }

}
