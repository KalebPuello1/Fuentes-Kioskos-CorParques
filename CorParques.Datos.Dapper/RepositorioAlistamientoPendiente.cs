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

namespace CorParques.Datos.Dapper
{
    public class RepositorioAlistamientoPendiente : IRepositorioAlistamientoPendiente
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioAlistamientoPendiente()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion
        public IEnumerable<AlistamientoPendiente> ObtenerAlistamientosPendientes(int IdReporte)
        {
            var lista = _cnn.Query<AlistamientoPendiente>("SP_ObtenerAlistamientosPendientes", param: new {TipoReporte = IdReporte}, commandType: System.Data.CommandType.StoredProcedure).Select(x => new AlistamientoPendiente { Punto = x.Punto, Valor = x.Valor});
            return lista;
        }
    }
}
