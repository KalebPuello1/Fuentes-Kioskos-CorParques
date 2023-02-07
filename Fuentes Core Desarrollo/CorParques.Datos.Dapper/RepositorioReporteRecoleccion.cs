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
    public class RepositorioReporteRecoleccion : RepositorioBase<ReporteVentas>, IRepositorioReporteRecoleccion
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReporteRecoleccion()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<ReporteRecoleccion> ObtenerReporte(string Fecha, int? IdTaquillero, int? IdSupervisor)
        {
         try
         {

                var respuesta = _cnn.Query<ReporteRecoleccion>(
                "SP_ReporteRecoleccionGeneral",
                param: new
                {
                    @Fecha = Fecha,
                    @IdTaquillero = IdTaquillero,
                    @IdSupervisor = IdSupervisor
                },
                commandType: System.Data.CommandType.StoredProcedure);

                return respuesta;


            }
          catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioReporteRecoleccion.ObtenerReporte");
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }
    }

}
