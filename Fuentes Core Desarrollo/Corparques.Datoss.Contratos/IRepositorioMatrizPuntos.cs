using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioMatrizPuntos : IRepositorioBase<TipoGeneral>
    {       

        IEnumerable<TipoGeneral> ObtenerMatriz();

        string InsertarMatriz(TipoGeneral matriz);
        string EliminarMatriz(int id);

    }
}
