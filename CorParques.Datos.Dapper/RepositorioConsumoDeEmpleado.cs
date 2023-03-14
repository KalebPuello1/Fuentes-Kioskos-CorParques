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
    public class RepositorioConsumoDeEmpleado : IRepositorioConsumoDeEmpleado
    {
        #region properties
        private readonly SqlConnection _cnn;
        public IEnumerable<ConsumoDeEmpleados>[] listaConsumoDeEmpleados = new IEnumerable<ConsumoDeEmpleados>[2];
        #endregion
        public RepositorioConsumoDeEmpleado()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        public IEnumerable<ConsumoDeEmpleados>[] buscar(string FInicial, string FFinal, string NDocumento, string Areaa)
        {
             
            
                var _list = _cnn.QueryMultiple("SP_ObtenerConsumoEmpleado",
              param: new
              {
                  FechaInicial = DateTime.ParseExact(FInicial, "yyyy-M-d", null),
                  FechaFinal = DateTime.ParseExact(FFinal, "yyyy-M-d", null),
                  Documento = NDocumento == "null" ? null : NDocumento,
                  Area = Areaa == "null" ? null : Areaa

              },
              commandType: System.Data.CommandType.StoredProcedure);
            try
            {
                listaConsumoDeEmpleados[0] = _list.Read<ConsumoDeEmpleados>().ToList();
                listaConsumoDeEmpleados[1] = _list.Read<ConsumoDeEmpleados>().ToList();
                }
            catch (Exception e)
                {
                    return listaConsumoDeEmpleados;
                }
          
                return listaConsumoDeEmpleados;
        }

        public IEnumerable<EstructuraEmpleado> buscarEmpresas()
        {
            var _list = _cnn.Query<EstructuraEmpleado>("SELECT DISTINCT Area From TB_EstructuraEmpleado WHERE IdEstado = 1").ToList();
            return _list;
        }
    }
}
