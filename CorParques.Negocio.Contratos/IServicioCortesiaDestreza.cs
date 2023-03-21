using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioCortesiaDestreza
	{

        //RDSH: Inserta una CortesiaDestreza.
        bool Insertar(CortesiaDestreza modelo, out string error, out string CodigoBarras);

        //RDSH: Retorna un objeto CortesiaDestreza para la edicion.
        CortesiaDestreza ObtenerPorId(int Id);

        //RDSH: Retorna un objeto CortesiaDestreza por su codigo de barras.
        CortesiaDestreza ObtenerPorCodigoBarras(string CodigoBarras);

        //RDSH: Actualiza una CortesiaDestreza.
        bool Actualizar(CortesiaDestreza modelo, out string error);

        //RDSH: Retorna la cantidad de cortesias entregadas por id destreza por atracción.
        IEnumerable<CortesiaDestreza> ObtenerPorDestrezaAtraccion(int IdDestreza, int IdAtraccion);
    }
}
