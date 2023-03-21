using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioBase<T> where T:class
    {
        IEnumerable<T> ObtenerListaPaginada(int pagina, int registrosPorPagina, string filtro, string columnaOrden);
        IEnumerable<T> ObtenerLista();
        IEnumerable<T> ObtenerLista(string filtro);
        T Obtener(int id);
        int Insertar(ref T modelo);
        bool Actualizar(ref T modelo);
        bool Eliminar(T modelo);
        IEnumerable<T> StoreProcedure(string nombre, object Parametros);
        IEnumerable<T> ExecuteQuery(string query, object Parametros);

    }
}
