using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioObservacionRecoleccion : IServicioBase<ObservacionRecoleccion>
	{
        // RDSH: Retorna las observaciones realizadas por un usuario para una recoleccion.
        IEnumerable<ObservacionRecoleccion> ObtenerPorIdRecoleccionUsuario(int IdRecoleccion, int IdUsuario);
    }
}
