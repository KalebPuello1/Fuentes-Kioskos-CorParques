using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReservaSkycoaster
    {
        IEnumerable<ReservaSkycoaster> ObtenerListaReservas();
        bool Insertar(ReservaSkycoaster modelo, out string error);
        int ObtenerReservaHora(string horaInicio);
        bool LiberarReserva(ReservaSkycoaster modelo, out string error);
        bool CerrarReserva(ReservaSkycoaster modelo, out string error);
    }
}
