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
    public class RepositorioReporteAprovechamientoFA : IRepositorioReporteAprovechamientoFA
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReporteAprovechamientoFA()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<ReporteAprovechamientoFA> ObtenerReporte(string FechaInicial, string FechaFinal, string cliente, string pedido, string factura)
        {
            try
            {                
                
                var rta =  _cnn.QueryMultiple(
                "SP_ReporteAprovechamientoFA",
                param: new
                {
                    @FechaInicial = DateTime.Parse(FechaInicial),
                    @FechaFinal = DateTime.Parse(FechaFinal),
                    @cliente = string.IsNullOrEmpty(cliente) ? null : cliente,
                    @pedido = string.IsNullOrEmpty(pedido) ? null : pedido,
                    @factura = string.IsNullOrEmpty(factura) ? null : factura
                },
                commandType: System.Data.CommandType.StoredProcedure);
                var datos = rta.Read<ReporteAprovechamientoFA>().ToList();
                if (datos.Count() > 0)
                    datos.Add(rta.Read<ReporteAprovechamientoFA>().FirstOrDefault());
                return datos;

                
            }
            catch (Exception ex)
            {
                
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }
        
    }

}
