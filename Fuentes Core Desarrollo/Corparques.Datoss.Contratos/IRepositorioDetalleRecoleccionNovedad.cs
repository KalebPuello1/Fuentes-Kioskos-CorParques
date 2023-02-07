using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioDetalleRecoleccionNovedad : IRepositorioBase<DetalleRecoleccionNovedad>
	{

        //RDSH: Inserta una recoleccion de novedad arqueo.
        bool Insertar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionNovedad> modelo, out string error);
        
        //RDSH: Retorna el detalle de una recoleccion de novedad por id recoleccion.
        IEnumerable<DetalleRecoleccionNovedad> ObtenerPorIDRecoleccion(int IdRecoleccion);

        //RDSH: Actualiza una recoleccion de novedad.
        bool Actualizar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioModificacion, IEnumerable<DetalleRecoleccionNovedad> modelo, out string error);

        bool InsertarGeneral(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionNovedad> modelo, int intIdUsuarioSupervisor, out string error);

    }
}
