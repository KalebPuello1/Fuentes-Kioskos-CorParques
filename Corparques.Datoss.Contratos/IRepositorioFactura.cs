using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioFactura 
    {
        string InsertarFactura(PagoFactura model);

        Factura ObtenerFactura(string codigoFactura);
        Factura ObtenerFactura(int idFactura);
        IEnumerable<Factura> ObtenerFacturaXConvenio(string consecutivo);
        DateTime? ObtenerFechaPago(int IdDetalleProducto);
        IEnumerable<AnulacionFactura> ObtenerFacturasAnular(int idPunto);

        IEnumerable<AnulacionFacturaRedeban> ObtenerFacturasRedebanAnular(int idPunto);
        int ObtenerVentasProductoCliente(string codigo, int producto);
        bool AnularFacturas(IEnumerable<AnulacionFactura> modelo);
        IEnumerable<MedioPagoFactura> ValidarTipoFactura(int idFactura );

        void ActualizarFacturaConvenio(string IdFactura, int IdConvenio, string ConsecutivoConvenio);
        IEnumerable<Factura> ObtenerFacturaContingencia();
        List<Factura> ProcesaFacturaContingencia(IEnumerable<Factura> _factura);
        string BorrarFacturaContingencia(List<Factura> _factura);
        DiccionarioContigencia ObtenerDiccionarioContigencia();

        RespuestaTransaccionRedaban ObtenerIdFranquiciaRedeban(string CodFranquicia);
        Factura ObtenerUltimaFactura(string CodSapPunto);
        string ValidarAlerta(int usuario, int producto);
        string GenerarNumeroFactura(int IdPunto);
        IEnumerable<FacturaValidaUsoRespuesta> ValidarUsoFactura(string codigoFactura);
        IEnumerable<DetalleFactura> ObtenerDetallesConsecutivoConvenioDia(string consecutivoConvenio);

    }
}
