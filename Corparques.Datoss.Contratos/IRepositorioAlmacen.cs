using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corparques.Datos.Contratos
{
    public interface IRepositorioAlmacen
    {
        IEnumerable<Almacen> getAllAlmacen();
    }
}
