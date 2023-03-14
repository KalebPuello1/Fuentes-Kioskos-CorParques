using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioAdicionPedidos : IServicioBase<AdicionPedidos>
	{

        //RDSH: Retorna el detalle de un pedido valido para adiciones.
        IEnumerable<AdicionPedidos> DetallePedido(string strCodigoSapPedido);

        // RDSH: Valida que un rango de brazaletes pueda ser usado en adicion de pedido.
        string ValidarRangoConsecutivos(string strConsecutivoInicial, string strConsecutivoFinal, int intCantidad, int intIdProducto, int idUsuario);

        //RDSH: Recibe un modelo para guardar y retorna un modelo con los consecutivos que debe imprimir.
        IEnumerable<AdicionPedidos> Guardar(IEnumerable<AdicionPedidos> modelo);
    }
}
