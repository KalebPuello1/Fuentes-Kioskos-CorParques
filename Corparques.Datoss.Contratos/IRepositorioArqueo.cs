using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioArqueo : IRepositorioBase<NovedadArqueo>
    {

        IEnumerable<Arqueo> ObtenerArqueo(int IdUsuario);
        bool InsertarNovedadArqueo(NovedadArqueo modelo);

    }
}
