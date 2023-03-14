using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioCierrePunto : IRepositorioBase<CierreElementos>
	{

        IEnumerable<CierreElementos> ObtenerElementosCierre(int IdPunto);
        IEnumerable<CierreElementos> ObtenerElementosCierreSupervisor(int Usuario);
        IEnumerable<CierreElementos> ObtenerElementosCierreNido(int Usuario);

        // RDSH: Actualiza un cierre de elementos para supervisor o para nido.
        bool ActualizarMasivo(int intTipo, IEnumerable<CierreElementos> modelo, out string error);

    }
}
