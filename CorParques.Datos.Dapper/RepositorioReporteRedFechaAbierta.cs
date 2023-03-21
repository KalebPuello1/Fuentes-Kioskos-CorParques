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
    public class RepositorioReporteRedFechaAbierta : RepositorioBase<ReporteRedFechaAbierta>, IRepositorioReporteRedFechaAbierta
    {
        public IEnumerable<TipoGeneral> obtenerTiposProducto()
        {
            return _cnn.Query<TipoGeneral>(
                "sp_obtenerTiposProducto", null, commandType: System.Data.CommandType.StoredProcedure);
        }

        public IEnumerable<ReporteRedFechaAbierta> ObtenerTodos(ReporteRedFechaAbierta modelo)
        {
            IEnumerable<ReporteRedFechaAbierta> ListaDatos = new List<ReporteRedFechaAbierta>();
            try
            {
                var _list = _cnn.Query<ReporteRedFechaAbierta>(
                      "SP_InformeFechaAbierta",
                    param: new
                    {
                        @FechaInicio = modelo.fechaInicial,
                        @FechaFinal = modelo.fechaFinal,
                        @CodSapVendedor = modelo.SapAsesor,
                        @CodSapTipoProducto = modelo.SapTipoProducto
                    },
                    commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 7000);
                ListaDatos = _list.ToList();
                //ListaDatos[1] = _list.Read<ReporteRedFechaAbierta>().ToList();
                return ListaDatos;
            }
            catch (Exception ex)
            {
                Transversales.Util.Utilidades.RegistrarError(ex, "RepositorioReporteRedFechaAbierta_ObtenerTodos");
                return null;
            }
        }

        public IEnumerable<TipoGeneral> obtenerTodosVendedores()
        {
            return _cnn.Query<TipoGeneral>(
                "sp_obtenerTodosVendedores", null,commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}





