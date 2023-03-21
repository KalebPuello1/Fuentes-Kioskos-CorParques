using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioDetalleRecoleccionBrazalete
	{

        //RDSH: Inserta el detalle de la recoleccion de brazaletes.
        bool Insertar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionBrazalete> modelo, out string error);

        //RDSH: Retorna un arreglo de DetalleRecoleccionBrazalete filtrando por el id de la recolección.
        IEnumerable<DetalleRecoleccionBrazalete> ObtenerPorIDRecoleccion(int IdRecoleccion);

        //RDSH: Actualiza el detalle de una recoleccion de brazaletes (boleteria).
        bool Actualizar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioModificacion, IEnumerable<DetalleRecoleccionBrazalete> modelo, out string error);

        bool InsertarGeneral(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionBrazalete> modelo, int intIdUsuarioSupervisor, out string error);
    }
}
