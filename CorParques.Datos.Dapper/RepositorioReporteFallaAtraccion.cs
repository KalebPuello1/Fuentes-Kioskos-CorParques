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
    public class RepositorioReporteFallaAtraccion : RepositorioBase<ReporteFallaAtraccion>, IRepositorioReporteFallaAtraccion
    {

        public IEnumerable<ReporteFallaAtraccion> ObtenerTodos(ReporteFallaAtraccion modelo)
        {
            try
            {
                return _cnn.Query<ReporteFallaAtraccion>(
                    "SP_ReporteFallaAtraccion",
                    param: new
                    {
                        @fIni = modelo.fechaInicial,
                        @fFin = modelo.fechaFinal,
                        @idAtraccion = modelo.idAtraccion,
                        @idArea = modelo.idArea
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
