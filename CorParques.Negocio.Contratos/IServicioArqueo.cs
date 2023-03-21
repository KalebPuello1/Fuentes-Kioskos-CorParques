using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioArqueo : IServicioBase<Arqueo>, IServicioBase<NovedadArqueo>
	{

        IEnumerable<Arqueo> ObtenerArqueo(int IdUsuario, int IdPunto);
        bool ActualizarNovedadArqueo(List<NovedadArqueo> modelo);

    }
}
