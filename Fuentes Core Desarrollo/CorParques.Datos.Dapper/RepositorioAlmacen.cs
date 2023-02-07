using Corparques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioAlmacen : IRepositorioAlmacen
    {
        private readonly SqlConnection _cnn;

        public RepositorioAlmacen()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ToString());
        }

        public IEnumerable<Almacen> getAllAlmacen()
        {
            IEnumerable<Almacen> dato = _cnn.Query<Almacen>("SP_ObtenerAlmacen", 
                commandType: System.Data.CommandType.StoredProcedure);
            return dato;
        }
    }
}
