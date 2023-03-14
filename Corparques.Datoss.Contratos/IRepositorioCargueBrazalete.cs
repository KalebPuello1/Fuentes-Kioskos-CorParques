using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioCargueBrazalete
    {

        //RDSH: Obtiene la lista de cargues de brazaletes realializados.
        IEnumerable<CargueBrazalete> ObtenerLista();

        //RDSH: Obtiene la lista de tipo de brazalete.
        IEnumerable<TipoGeneral> ObtenerTipoBrazalete();

        //RDSH: Ejecuta el cargue de brazaletes.
        bool Insertar(CargueBrazalete modelo,out string error);

        //RDSH: Actualiza la tabla cargue_brazalete y elimina los brazaletes inactivos.
        bool Actualizar(CargueBrazalete modelo, out string error);        
    }
}
