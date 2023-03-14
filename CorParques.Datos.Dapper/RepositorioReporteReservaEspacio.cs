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
    public class RepositorioReporteReservaEspacio : RepositorioBase<ReporteReservaEspacio>, IRepositorioReporteReservaEspacio
    {

        public IEnumerable<ReporteReservaEspacio> ObtenerTodos(ReporteReservaEspacio modelo)
        {
            return _cnn.Query<ReporteReservaEspacio>(
                "SP_ReporteReservaEspacios", 
                param: new {

                    @fechaInicial = modelo.fechaInicialGet,
                    @fechaFinal = modelo.fechaFinalGet,
                    @HoraInicial = modelo.horaIniGet,
                    @HoraFinal = modelo.horaFinGet,
                    @TipoEspacio = modelo.idEspGet,
                    @Espacio = modelo.idTipEpsGet,
                    @NumeroPedido = modelo.txtNPedidoGet
                }, 
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
