using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioCortesiaPunto
	{

        //RDSH: Inserta una CortesiaPunto.
        bool Insertar(CortesiaPunto modelo, out string error);

        //RDSH: Actualiza una CortesiaPunto.
        bool Actualizar(CortesiaPunto modelo, out string error);

        //RDSH: borrado logico CortesiaPunto.
        bool Eliminar(CortesiaPunto modelo, out string error);

        //RDSH: Retorna una lista de CortesiaPunto donde se puede filtrar por IdDestreza o por IdAtraccion o para traer todas enviar cero en ambos parametros.
        IEnumerable<CortesiaPunto> ObtenerPorDestrezaAtraccion(int IdDestreza, int IdAtraccion);

        //RDSH: Retorna un objeto CortesiaPunto para la edicion.
        CortesiaPunto ObtenerPorId(int Id);

        //Retorna los Productos con TipoProducto atracciones y destrezas.
        IEnumerable<TipoGeneral> ObtenerProductos(string CodTipoProducto);

    }
}
