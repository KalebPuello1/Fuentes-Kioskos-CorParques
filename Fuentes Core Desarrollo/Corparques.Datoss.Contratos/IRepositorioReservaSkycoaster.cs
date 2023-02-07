using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReservaSkycoaster
    {
        IEnumerable<ReservaSkycoaster> ObtenerListaReserva();
        bool Insertar(ReservaSkycoaster modelo, out string error);
        int ObtenerReservaHora(string horaInicio);
        bool LiberarReserva(ReservaSkycoaster modelo, out string error);
        bool CerrarReserva(ReservaSkycoaster modelo, out string error);
    }
}
