using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioDetalleRecoleccionMonetaria
	{

        //RDSH: Inserta el detalle de la recoleccion monetaria.
        bool Insertar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionMonetaria> modelo, out string error);

        //RDSH: Retorna un arreglo de DetalleRecoleccionMonetaria filtrando por el id de la recolección.
        IEnumerable<DetalleRecoleccionMonetaria> ObtenerPorIDRecoleccion(int IdRecoleccion);

        // RDSH: Actualiza el detalle de una recoleccion monetaria.
        bool Actualizar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioModificacion, IEnumerable<DetalleRecoleccionMonetaria> modelo, out string error);

        bool InsertarRecoleccionGeneral(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, float ValorRecoleccionBase, float ValorRecoleccionCorte, int idSupervisor, out string error);

    }
}
