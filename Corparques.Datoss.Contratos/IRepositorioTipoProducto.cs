using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioTipoProducto : IRepositorioBase<TipoProducto>
	{

        //RDSH: Retorna la lista de tipo de producto.
        IEnumerable<TipoProducto> ObtenerListaTipoProduto();

    }
}
