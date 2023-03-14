using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioBase<T> where T:class
    {
        IEnumerable<T> ObtenerTodos();

        T Obtener(int id);

        T Crear(T modelo);

        bool Actualizar(T modelo);
    }
}
