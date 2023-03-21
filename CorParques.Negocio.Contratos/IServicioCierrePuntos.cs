using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioCierrePuntos : IServicioBase<CierreElementos>
	{

        IEnumerable<CierreElementos> ObtenerElementosCierre(int IdPunto);
        bool ActualizarCierre(IEnumerable<CierreElementos> modelo);
        IEnumerable<CierreElementos> ObtenerElementosCierreSupervisor(int Usuario);
        IEnumerable<CierreElementos> ObtenerElementosCierreNido(int Usuario);

        // RDSH: Actualiza un cierre de elementos para supervisor o para nido.
        bool ActualizarMasivo(int intTipo, IEnumerable<CierreElementos> modelo, out string error);
    }
}
