using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioDetalleRecoleccionDocumento
	{
        //RDSH: Inserta el detalle de la recoleccion de documentos.
        bool Insertar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionDocumento> modelo, out string error);

        //RDSH: Retorna un arreglo de DetalleRecoleccionDocumento filtrando por el id de la recolección.
        IEnumerable<DetalleRecoleccionDocumento> ObtenerPorIDRecoleccion(int IdRecoleccion);

        /// RDSH: Actualiza el detalle de una recoleccion de documentos.
        bool Actualizar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioModificacion, IEnumerable<DetalleRecoleccionDocumento> modelo, out string error);

        bool InsertarGeneral(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionDocumento> modelo, int intIdUsuarioSupervisor,
                             out string error);
    }
}
