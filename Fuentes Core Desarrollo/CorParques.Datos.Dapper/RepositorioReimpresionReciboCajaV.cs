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
    public class RepositorioReimpresionReciboCajaV : IRepositorioReimpresionReciboCajaV
    {
        SqlConnection _cnn;
        public RepositorioReimpresionReciboCajaV()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        public IEnumerable<ReimpresionReciboCajaV> datosReimpresion(string datoI, string datoF)
        {
            IEnumerable<ReimpresionReciboCajaV> dato = _cnn.Query<ReimpresionReciboCajaV>("SP_ObtenerDatosReimpresionFactura", commandType: System.Data.CommandType.StoredProcedure,
                param: new {
                    @FechaI = DateTime.ParseExact(datoI,"yyyy-M-dd",null),
                    @FechaF = DateTime.ParseExact(datoF,"yyyy-M-dd",null)

                });
            return dato;
        }
        public ReimpresionReciboCajaV datoReimpresion()
        {
            ReimpresionReciboCajaV dato = _cnn.Query<ReimpresionReciboCajaV>("SP_ObtenerDatosReimpresionFactura", commandType: System.Data.CommandType.StoredProcedure).First();
            return dato;
        }
    }
}
