using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioConvenioSAP : IRepositorioBase<Convenio>
    {
        IEnumerable<ConvenioDetalle> ObtenerDetalleConvenio(int IdConvenio);
        IEnumerable<ConvenioDetalle> ObtenerDetalleConvenioByConsec(string Consecutivo);
        int? ObtenerConvenioXConsecutivo(string Consecutivo);
        IEnumerable<ConvenioDetalle> ObtenerDetalleConvenioByApp(string Consecutivo, string productos, int Otroconvenio);
        string InsertarConvenioConsecutivos(List<ConvenioConsecutivos> lista);
        int ObtieneVentasConvenioProductoHoy(string CodSapConvenio, string CodigoSap, string CodSapTipoProducto);
    }
}
