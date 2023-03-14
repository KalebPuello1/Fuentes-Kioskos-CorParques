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
    public class RepositorioBitacoraDia : RepositorioBase<BitacoraDia>, IRepositorioBitacoraDia
    {
        public BitacoraDia Asignar(BitacoraDia modelo)
        {
            return (_cnn.Query<BitacoraDia>("SP_insertarBitacoraDia", param: new {
                    IdClimaFK = modelo.IdClimaFK,
                    IdSegmentoDiaFK = modelo.IdSegmentoDiaFK,
                    Fecha = DateTime.Now,
                    Observacion = modelo.Observacion,
                    CantidadPersonas = modelo.CantidadPersonans
            }, commandType: System.Data.CommandType.StoredProcedure)).FirstOrDefault();
        }

        public IEnumerable<BitacoraDia> Obtener(DateTime fecha)
        {
            return _cnn.Query<BitacoraDia>("SP_obtenerBitacoraDia", param: new { fecha = fecha }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public int? ObtenerCantidadPersonas()
        {
            return _cnn.Query<int>("SP_ConsultaPersonasBitacoraDia", null, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
        }

        public IEnumerable<BitacoraDia> ObtenerTodos()
        {
            return _cnn.Query<BitacoraDia>("SP_obtenerBitacoraDia", commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
