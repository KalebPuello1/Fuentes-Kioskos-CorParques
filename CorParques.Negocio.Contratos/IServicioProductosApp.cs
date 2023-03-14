using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioProductosApp
    {
      
        bool Insertar (ref Producto producto);
        bool Crear (ref Producto producto);
        bool Inactivar(int IdProducto, int activo);
        IEnumerable<Producto> Obtener();
        Producto ObtenerId(int IdProducto);

        
    }
}
