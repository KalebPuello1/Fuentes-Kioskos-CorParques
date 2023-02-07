using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using CorParques.Transversales.Util;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace CorParques.Datos.Dapper
{

    public class RepositorioDashBoard : RepositorioBase<DashBoard>, IRepositorioDashBoard
    {

        #region Metodos

        /// <summary>
        /// RDSH: Retorna valores para dashboard
        /// </summary>
        /// <param name="datFechaInicial"></param>
        /// <param name="datFechaFinal"></param>
        /// <returns></returns>
        public IEnumerable<DashBoard> ObtenerInformacionDashBoard(DateTime datFechaInicial, DateTime datFechaFinal)
        {
            try
            {

                var objRetorno = _cnn.Query<DashBoard>("SP_ObtenerInformacionDashBoard", param: new
                {
                    FechaInicial = datFechaInicial,
                    FechaFinal = datFechaFinal
                }, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 900000).ToList();

                return objRetorno;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioDashBoard_ObtenerInformacionDashBoard");
                return null;
            }

        }

        #endregion

    }
}
