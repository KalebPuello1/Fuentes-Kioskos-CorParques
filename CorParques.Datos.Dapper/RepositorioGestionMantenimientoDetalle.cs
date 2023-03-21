using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioGestionMantenimientoDetalle : RepositorioBase<GestionMantenimientoDetalle>, IRepositorioGestionMantenimientoDetalle
    {
        public IEnumerable<GestionMantenimientoDetalle> ObtenerListaSimple(int id)
        {
            return _cnn.Query<GestionMantenimientoDetalle>("Retornar_Mantenimiento_Detalle", param: new { IdMantenimiento_Control = id }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        }
    }
}
