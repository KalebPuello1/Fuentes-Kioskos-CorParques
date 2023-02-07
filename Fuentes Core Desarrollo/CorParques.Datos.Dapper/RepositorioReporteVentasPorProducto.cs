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
    public class RepositorioReporteVentasPorProducto : IRepositorioReporteVentasPorProducto
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReporteVentasPorProducto()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<ReporteVentasPorProducto> ObtenerReporte(string FechaInicial, string FechaFinal, string CodigoProducto, string CodigoPunto, string CentroBeneficio)
        {
            try
            {                
                
                return _cnn.Query<ReporteVentasPorProducto>(
                "SP_ReporteVentasPorProducto",
                param: new
                {
                    @FechaInicial = DateTime.Parse(FechaInicial),
                    @FechaFinal = DateTime.Parse(FechaFinal),
                    @CodigoProducto = string.IsNullOrEmpty(CodigoProducto) ? null : CodigoProducto,
                    @CodigoPunto = string.IsNullOrEmpty(CodigoPunto) ? null : CodigoPunto,
                    @CentroBeneficio = string.IsNullOrEmpty(CentroBeneficio) ? null : CentroBeneficio
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
