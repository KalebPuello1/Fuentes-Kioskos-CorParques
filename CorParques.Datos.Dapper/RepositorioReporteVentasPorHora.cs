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
    public class RepositorioReporteVentasPorHora : RepositorioBase<ReporteVentasPorHora>, IRepositorioReporteVentasPorHora
    {
        public IEnumerable<ReporteVentasPorHora> ObtenerReporte(string FechaInicial, string FechaFinal, string HoraInicial, string HoraFinal, string CodigoProducto, string NombreProducto, string CodigoPunto, string CB)
        {
            try
            {
                return _cnn.Query<ReporteVentasPorHora>(
                "SP_ReporteVentasPorHora",
                param: new
                {
                    FechaInicial = FechaInicial,
                    FechaFinal = FechaFinal,
                    HoraInicial = HoraInicial,
                    HoraFinal = HoraFinal,
                    CodigoProducto = CodigoProducto,
                    //NombreProducto = NombreProducto,
                    CodigoPunto = CodigoPunto,
                    centroBeneficio = CB
                },
                commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Transversales.Util.Utilidades.RegistrarError(ex, "RepositorioReporteVentasPorHora_ObtenerReporte");
                throw;
            }
        }
    }

}
