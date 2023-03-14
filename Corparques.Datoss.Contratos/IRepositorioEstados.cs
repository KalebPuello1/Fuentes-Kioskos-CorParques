using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioEstados
    {
        
        /// <summary>
        /// RDSH: Retorna la lista de estados por modulo.
        /// </summary>
        /// <param name="IdModulo"></param>
        /// <returns></returns>
        IEnumerable<TipoGeneral> ObtenerEstados(int IdModulo);


        
        IEnumerable<Estados> ObtenerLista();

    }
}
