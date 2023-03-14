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
    public class RepositorioReporteVentasDiarias : RepositorioBase<ReporteVentasDiario>, IRepositorioReporteVentasDiarias
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReporteVentasDiarias()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }


        #endregion

        public IEnumerable<ReporteVentasDiario> ObtenerReporteDiario(String fecha)
        {
            try
            {

                var lista = _cnn.Query<ReporteVentasDiario>("SP_ReporteVentasDiarias", param: new { DiaReporte = fecha}, commandType: System.Data.CommandType.StoredProcedure).ToList();
                return lista;

                //return listaVentas;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioReporteVentasDiarias.ObtenerReporteDiario");
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }

        public IEnumerable<ReporteInventario> ObtenerReporteInventario()
        {
            try
            {

                var lista = _cnn.Query<ReporteInventario>("ObtenerInventarioUbicacionPuntos", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
                return lista;

                //return listaVentas;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioReporteVentasDiarias.ObtenerReporteInventario");
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }

     
    }

}
