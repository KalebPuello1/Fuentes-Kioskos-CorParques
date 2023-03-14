using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioConsumoDeEmpleados
    {
        IEnumerable<ConsumoDeEmpleados>[] buscar(string FInicial, string FFinal, string NDocumento, string Area);
        IEnumerable<EstructuraEmpleado> buscarEmpresas();
    }
}
