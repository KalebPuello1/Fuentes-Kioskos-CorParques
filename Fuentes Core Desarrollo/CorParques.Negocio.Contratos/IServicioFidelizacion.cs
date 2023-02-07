using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioFidelizacion : IServicioBase<ClienteFideliacion>
	{
        ClienteFideliacion ObtenerClienteTarjeta(string doc);
        ClienteFideliacion ObtenerTarjetaSaldoPuntos(string consecutivo);
        IEnumerable<TipoGeneral> ObtenerProductosRedimibles(int puntos);
        bool BloquearTarjeta(string consecutivo,int usuario,int punto);
        ClienteFideliacion Obtener(string id);
        bool RedimirProduto(string consecutivo, int producto, int usuario, int punto);
    }
}
