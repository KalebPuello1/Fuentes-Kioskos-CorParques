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
    public class RepositorioEstado : IRepositorioEstados
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioEstado()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Retorna la lista de estados por modulo.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerEstados(int IdModulo)
        {
            //var lista = _cnn.GetList<Estados>().Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
            var lista = _cnn.Query<Estados>("SP_ObtenerEstadosPorModulo", param: new {
                IdModulo = IdModulo
            }, commandType: System.Data.CommandType.StoredProcedure).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });            
            return lista;
        }
   

        public IEnumerable<Estados> ObtenerLista()
        {
            var lista = _cnn.GetList<Estados>();
            return lista;
        }

        #endregion

    }
}
