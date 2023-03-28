using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

    public interface IServicioInventario : IServicioBase<TransladoInventario>
    {

        string ActualizarInventario(Inventario Inventario);
        string EntregaPedido(IEnumerable<TransladoInventario> Inventario);
        string ActualizarTransladoInventario(IEnumerable<TransladoInventario> TransladoInventario);
        string TrasladoPedido(IEnumerable<TransladoInventario> TransladoInventario);
        IEnumerable<SolicitudRetorno> ObtenerSolicitudesDevolucion();
        string InsertarAjusteInventario(IEnumerable<AjustesInventario> Ajustes);
        IEnumerable<TipoAjusteInventario> ObtenerTiposAjuste();
        IEnumerable<MotivosInventario> ObtenerMotivosAjuste(string CodSapAjuste);
        IEnumerable<MotivosInventario> BuscarMotivosInventario();
        string ActualizarAjustesInventario(IEnumerable<AjustesInventario> AjusteInventario);
        IEnumerable<ResumenCierre> ObtenerResumenCierre(DateTime? Fecha);
        IEnumerable<MotivosInventario> ObtenerTodosMotivos();
        string ObtenerCodSapAlmacen(int idPunto);
        IEnumerable<SolicitudRetorno> ConsultarPedidoRetorno(string pedido);
        IEnumerable<ProductosPedidos> ObtenerPedidosTraslado();
        IEnumerable<SolicitudRetorno> ObtenerPedidosEntregaAsesor(int idPunto);
        IEnumerable<TipoGeneral> ConsultarMotivosRetorno();
        string CrearSolicitudRetorno(SolicitudRetorno modelo);
        bool enviarMail(string to, string subject, string mensaje, MailPriority mpPriority, List<string> attachmentt);
    }
}
