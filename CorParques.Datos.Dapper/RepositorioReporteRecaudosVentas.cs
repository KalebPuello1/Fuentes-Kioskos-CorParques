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
    public class RepositorioReporteRecaudosVentas : RepositorioBase<ReporteRecaudosVentas>, IRepositorioReporteRecaudosVentas
    {

        public IEnumerable<ReporteRecaudosVentas> ObtenerTodos(ReporteRecaudosVentas modelo)
        {
            return _cnn.Query<ReporteRecaudosVentas>(
                "SP_ReporteRecaudosVentas", 
                param: new {
                    @FechaInicial = modelo._FechaInicial,
                    @FechaFinal = modelo._FechaFinal,
                    @Consecutivo = modelo._Consecutivo,
                    @Cliente = modelo._Cliente,
                    @Entidad = modelo._Entidad,
                    @FormaPago = modelo._FormaPago
                }, 
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
