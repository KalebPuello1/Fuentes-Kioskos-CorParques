using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioFidelizacion : IRepositorioBase<ClienteFideliacion>
    {
        ClienteFideliacion ObtenerClienteTarjeta(string doc);
        ClienteFideliacion ObtenerTarjetaSaldoPuntos(string consecutivo);
        IEnumerable<TipoGeneral> ObtenerProductosRedimibles(int puntos);
        bool BloquearTarjeta(string consecutivo, int usuario, int punto);
        bool RedimirProduto(string consecutivo,int producto, int usuario, int punto);
    }
}
