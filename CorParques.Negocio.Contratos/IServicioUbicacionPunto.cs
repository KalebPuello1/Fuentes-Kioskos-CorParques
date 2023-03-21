using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioUbicacionPunto : IServicioBase<UbicacionPunto>
	{

        //RDSH: Retorna todas las ubicaciones activas para un punto especifico.
        IEnumerable<UbicacionPunto> ObtenerPorPunto(int intIdPunto);

        // RDSH: Retorna una lista de tipo general para cargar un dropdown list.
        IEnumerable<TipoGeneral> ObtenerListaSimplePorPunto(int intIdPunto);
        bool Insertar(UbicacionPunto modelo, out string error);
        bool ActualizarUbicacion(UbicacionPunto modelo, out string error);
        bool Eliminar(int id);
    }
}
