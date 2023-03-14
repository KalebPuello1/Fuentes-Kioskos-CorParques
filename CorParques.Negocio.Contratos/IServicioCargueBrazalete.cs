using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioCargueBrazalete : IServicioBase<CargueBrazalete>
    {

        bool Actualizar(CargueBrazalete modelo, out string error);

        bool Insertar(CargueBrazalete modelo, out string error);

        IEnumerable<TipoGeneral> ObtenerTipoBrazalete();

    }
}
