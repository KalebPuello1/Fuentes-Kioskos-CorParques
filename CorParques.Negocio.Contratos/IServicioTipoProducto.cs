using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioTipoProducto : IServicioBase<TipoProducto>
	{

        //RDSH: Retorna la lista de tipo de producto.
        IEnumerable<TipoGeneral> ObtenerListaTipoProduto();

    }
}
