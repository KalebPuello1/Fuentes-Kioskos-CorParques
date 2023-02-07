using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioInventario : IRepositorioBase<TransladoInventario>
	{

        string ActualizarInventario(Inventario Inventario);
        string ActualizarTransladoInventario(IEnumerable<TransladoInventario> TransladoInventario);

        int InsertarAjusteInventario(AjustesInventario Ajuste);
        IEnumerable<TipoAjusteInventario> ObtenerTiposAjuste();
        IEnumerable<MotivosInventario> ObtenerMotivosAjuste(string CodSapAjuste);
        IEnumerable<MotivosInventario> BuscarMotivosInventario();
        string ActualizarAjustesInventario(IEnumerable<AjustesInventario> AjusteInventario);
        string RegistrarSalidaPedido(int idusuario, string pedido, int idUsuarioRecibe);
        IEnumerable<ResumenCierre> ObtenerResumenCierre(DateTime? Fecha);
        IEnumerable<MotivosInventario> ObtenerTodosMotivos();
        string ObtenerCodSapAlmacen(int idPunto);
        IEnumerable<SolicitudRetorno> ObtenerSolicitudesDevolucion();
        IEnumerable<SolicitudRetorno> ConsultarPedidoRetorno(string pedido);
        string ActualizarPedidoTrasladado(int idPunto, string pedido);
        IEnumerable<TipoGeneral> ConsultarMotivosRetorno();
        string CrearSolicitudRetorno(SolicitudRetorno modelo);
      
    }
}
