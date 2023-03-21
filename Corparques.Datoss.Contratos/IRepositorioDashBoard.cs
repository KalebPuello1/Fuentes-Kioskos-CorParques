using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioDashBoard : IRepositorioBase<DashBoard>
	{

        // RDSH: Retorna valores para dashboard
        IEnumerable<DashBoard> ObtenerInformacionDashBoard(DateTime datFechaInicial, DateTime datFechaFinal);

    }
}
