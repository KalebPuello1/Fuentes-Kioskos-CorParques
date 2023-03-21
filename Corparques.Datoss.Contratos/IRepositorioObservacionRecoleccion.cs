using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioObservacionRecoleccion : IRepositorioBase<ObservacionRecoleccion>
	{
        // RDSH: Retorna las observaciones realizadas por un usuario para una recoleccion.
        IEnumerable<ObservacionRecoleccion> ObtenerPorIdRecoleccionUsuario(int IdRecoleccion, int IdUsuario);
    }
}
