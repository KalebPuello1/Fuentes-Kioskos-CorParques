using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioMediosPago: IServicioBase<MediosPago>
    {
        IEnumerable<TipoGeneral> ObtenerListaSimple();
    }
}
