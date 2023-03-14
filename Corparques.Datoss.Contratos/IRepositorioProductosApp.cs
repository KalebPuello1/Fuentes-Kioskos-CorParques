using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioProductosApp :IRepositorioBase<Producto>
    {
        /// <summary>
        /// el filtro es como se va a usar en el SQL
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        
        bool Insertar(Producto producto);
        bool Crear(Producto producto);
        bool Inactivar(int IdProducto,int activo);
        IEnumerable<Producto> Obtener ();
        Producto ObtenerId(int IdProducto);

       
       
    }
}
