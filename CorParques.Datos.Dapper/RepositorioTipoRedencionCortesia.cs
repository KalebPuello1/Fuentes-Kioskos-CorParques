using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

    public class RepositorioTipoRedencionCortesia : RepositorioBase<TipoRedencionCortesia>, IRepositorioTipoRedencionCortesia
    {
        #region Declaraciones

        private readonly SqlConnection _conn;


        #endregion

        #region Constructor

        public RepositorioTipoRedencionCortesia()
        {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion
    }
}