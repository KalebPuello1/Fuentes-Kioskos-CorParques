using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioBitacoraDia : IServicioBase<BitacoraDia>
    {
        IEnumerable<BitacoraDia> Obtener(string fecha);
        BitacoraDiaLista Asignar(BitacoraDiaLista modelo);
        int? ObtenerCantidadPersonas();
    }
}
