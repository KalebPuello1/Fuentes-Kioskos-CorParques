using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioBitacoraDia : IRepositorioBase<BitacoraDia>
    {
        IEnumerable<BitacoraDia> ObtenerTodos();
        IEnumerable<BitacoraDia> Obtener(DateTime fecha);
        BitacoraDia Asignar(BitacoraDia modello);
        int? ObtenerCantidadPersonas();
    }
}
