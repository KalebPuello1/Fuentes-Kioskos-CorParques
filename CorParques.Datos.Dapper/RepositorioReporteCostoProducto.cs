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
    public class RepositorioReporteCostoProducto : RepositorioBase<ReporteCostoProducto>, IRepositorioReporteCostoProducto
    {
        public IEnumerable<TipoGeneral> obtenerProductos()
        {
            return _cnn.Query<TipoGeneral>(
                "sp_obtenerProductosReporte", null, commandType: System.Data.CommandType.StoredProcedure);
        }

        public IEnumerable<ReporteCostoProducto> Obtener(ReporteCostoProducto modelo)
        {
            return _cnn.Query<ReporteCostoProducto>(
                "sp_ObtenerCostoProducto",
                param: new
                {
                    @FechaInicio = modelo.fechaInicial,
                    @FechaFinal = modelo.fechaFinal,
                    @CodSap = string.IsNullOrEmpty(modelo.CodSap) ? null : modelo.CodSap
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}





