using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioDashBoard : IServicioBase<DashBoard>
	{

        // RDSH: Retorna valores para dashboard
        IEnumerable<DashBoard> ObtenerInformacionDashBoard(string strFechaInicial, string strFechaFinal);

    }
}
