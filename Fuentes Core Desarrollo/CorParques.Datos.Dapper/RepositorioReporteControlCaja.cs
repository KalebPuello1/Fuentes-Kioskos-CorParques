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
    public class RepositorioReporteControlCaja : RepositorioBase<ReporteControlCaja>, IRepositorioReporteControlCaja
    {

        public IEnumerable<ReporteControlCaja> ObtenerTodos(ReporteControlCaja modelo)
        {
            var rta =  _cnn.Query<ReporteControlCaja>(
                "SP_ReporteControlCaja", 
                param: new {
                    @fIni = modelo.fechaInicial,
                    @fFin = modelo.fechaFinal,
                    @idPerfil = modelo.idPerfil,
                    @idTaquillero = modelo.idTaquillero
                }, 
                commandType: System.Data.CommandType.StoredProcedure);
            return rta;
        }
    }
}
