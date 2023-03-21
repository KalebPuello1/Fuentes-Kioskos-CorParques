using CorParques.Negocio.Entidades;
using System;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using static Dapper.SqlMapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReporte_SI_Consumo_SF_FechaAbierta : IRepositorioReporte_SI_Consumo_SF_FechaAbierta
    {
        private readonly SqlConnection _cnn;
        public RepositorioReporte_SI_Consumo_SF_FechaAbierta() {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["cnn"]].ConnectionString);
        }

        public IEnumerable<Reporte_SI_Consumo_SF_FechaAbierta> getSI_SF(string fechaI, string fechaF, string Npedido, string redencion)
        {
            var data = _cnn.Query<Reporte_SI_Consumo_SF_FechaAbierta>("SP_ObtenerConsumoInicialFinal",
                commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    Fechainicial = DateTime.ParseExact(fechaI, "yyyy-M-d", null),
                    FechaFinal = DateTime.ParseExact(fechaF, "yyyy-M-d", null),
                    CodSapPedido = Npedido == "null" ? null : Npedido
                });
            return data;
        }
            public IEnumerable<Reporte_SI_Consumo_SF_FechaAbierta>[] getSI_SFF(string dato)
        {
            IEnumerable<Reporte_SI_Consumo_SF_FechaAbierta>[] reportSI_SF = new IEnumerable<Reporte_SI_Consumo_SF_FechaAbierta>[2];
            GridReader data = _cnn.QueryMultiple("SP_getReporte_SI_Consumo_SF_FechaAbierta",
                commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    NumPedido = dato
                });
            reportSI_SF[0] = data.Read<Reporte_SI_Consumo_SF_FechaAbierta>().ToList();
            reportSI_SF[1] = data.Read<Reporte_SI_Consumo_SF_FechaAbierta>().ToList();
            return reportSI_SF;
        }
    }
}
