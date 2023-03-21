using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioParametros : IRepositorioBase<Parametro>
    {
        Parametro ObtenerParametroPorNombre(string strNombre);

        //RDSH: Consulta todos los parametros de la aplicacion para almacenarlos en cache.
        IEnumerable<Parametro> ObtenerParametrosGlobales();
    }   

}
