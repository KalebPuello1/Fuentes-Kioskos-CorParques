using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioCortesiaDestreza
	{

        //RDSH: Inserta una CortesiaDestreza.
        bool Insertar(CortesiaDestreza modelo, out string error, out string CodigoBarras);

        //RDSH: Retorna un objeto CortesiaDestreza para la edicion.
        CortesiaDestreza ObtenerPorId(int Id);

        // RDSH: Retorna un objeto cortesia destreza por el codigo de barras.
        CortesiaDestreza ObtenerPorCodigoBarras(string CodigoBarras);

        //RDSH: Actualiza una CortesiaDestreza.
        bool Actualizar(CortesiaDestreza modelo, out string error);

        //RDSH: Retorna la cantidad de cortesias entregadas por id destreza por atracción.
        IEnumerable<CortesiaDestreza> ObtenerPorDestrezaAtraccion(int IdDestreza, int IdAtraccion);
    }
}
