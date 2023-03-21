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
    public class RepositorioReporteNotificaciones : RepositorioBase<ReporteNotificaciones>, IRepositorioReporteNotificaciones
    {
        #region Metodos

        /// <summary>
        /// RDSH: retorna la informacion de las notificaciones enviadas por rango de fecha.
        /// </summary>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <returns></returns>
        public IEnumerable<ReporteNotificaciones> ObtenerReporte(string FechaInicial, string FechaFinal)
        {
            try
            {
                return _cnn.Query<ReporteNotificaciones>(
                "SP_ReporteNotificaciones",
                param: new
                {
                    FechaInicial = FechaInicial,
                    FechaFinal = FechaFinal                   
                },
                commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Transversales.Util.Utilidades.RegistrarError(ex, "RepositorioReporteNotificaciones_ObtenerReporte");
                throw new ArgumentException("No fue posible generar el reporte de notificaciones.");
            }
        }

        #endregion



    }

}
