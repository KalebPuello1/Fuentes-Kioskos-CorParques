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
    public class RepositorioReporteCuadreDiarioFlujoCajasTaq : RepositorioBase<ReporteCuadreDiarioFlujoCajasTaq>, IRepositorioReporteCuadreDiarioFlujoCajasTaq
    {

        public IEnumerable<ReporteCuadreDiarioFlujoCajasTaq> ObtenerTodos(ReporteCuadreDiarioFlujoCajasTaq modelo)
        {
            try
            {
                return _cnn.Query<ReporteCuadreDiarioFlujoCajasTaq>(
                "SP_ReporteCuadreDiarioFlujoCajasTaq",
                param: new
                {
                    @fIni = modelo.fechaInicial,
                    @fFin = modelo.fechaFinal,
                    @idTipIngreso = modelo.idTipIngreso,
                    @TipNovedad = modelo.TipNovedad,
                    @TipConsnumo = modelo.TipConsnumo
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
