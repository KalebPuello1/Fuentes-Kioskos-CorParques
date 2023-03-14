using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioReservaEspacios : IServicioBase<ReservaEspacios>
	{
        IEnumerable<TipoGeneral> ObtenerEspaciosxTipo(int IdTipoEsapcio);
        IEnumerable<TipoGeneral> ObtenerTipoEspacios();
        IEnumerable<TipoGeneral> ObtenerEspacios();
        

        /// RDSH: Retorna las reservas que se han realizado para el espacio y la fecha seleccionada.
        IEnumerable<ReservaEspacios> ObtenerReservaEspacios(int intIdReserva, string strFechaReserva);

        ///RDSH: Actualiza una reserva de espacios.
        bool Actualizar(ReservaEspacios modelo, out string error);

        ///RDSH: Inserta una reserva de espacios.
        bool Insertar(ReservaEspacios modelo, out string error);
        
        // RDSH: Retorna los tipos de reserva activos ordenados por nombre.        
        IEnumerable<TipoReserva> ObtenerTiposReserva();

        // RDSH: Retorna el detalle de un pedido sap para mostrarlo en la reserva de espacios.
        IEnumerable<ReservaEspaciosAuxiliar> ObtenerDetallePedido(string strCodigoSapPedido, string strFechaReserva);

        // RDSH: Elimina una reserva.
        bool Eliminar(int intIdReservaEspacio, int intIdUsuario, out string error);

    }
}
