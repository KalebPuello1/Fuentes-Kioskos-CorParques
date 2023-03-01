using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioFactura : IServicioBase<Factura>
	{
        IEnumerable<Factura> ObtenerFacturaContingencia();
        List<Factura> ProcesaFacturaContingencia(IEnumerable<Factura> _factura);
        string BorrarFacturaContingencia(List<Factura> _factura);
        DiccionarioContigencia ObtenerDiccionarioContigencia();
        Factura ObtenerUltimaFactura(string CodSapPunto);

        RespuestaTransaccionRedaban ObtenerIdFranquiciaRedeban(string CodFranquicia);
        
        string GenerarNumeroFactura(int IdPunto);
        bool FlujoRedebanXPunto(int IdPunto);
        
    }
}
