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
    public class RepositorioReporteDonaciones : IRepositorioReporteDonaciones
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReporteDonaciones()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion


        public IEnumerable<TipoGeneral> ObtenerProductos()
        {
            var rta = _cnn.Query<TipoGeneral>(
               "SP_ObtenerProductosDonaciones",
               commandType: System.Data.CommandType.StoredProcedure);
            return rta;
        }
        public IEnumerable<ReporteDonaciones> ObtenerReporte(string FechaInicial, string FechaFinal, string producto, int? punto)
        {
            try
            {                
                
                var rta =  _cnn.Query<ReporteDonaciones>(
                "SP_ReporteDonaciones",
                param: new
                {
                    @FechaInicial = DateTime.Parse(FechaInicial),
                    @FechaFinal = DateTime.Parse(FechaFinal),
                    @IdProducto = string.IsNullOrEmpty(producto) ? null : producto,
                    @IdPunto = punto
                },
                commandType: System.Data.CommandType.StoredProcedure);
                return rta;

                
            }
            catch (Exception ex)
            {
                
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }
        
    }

}
