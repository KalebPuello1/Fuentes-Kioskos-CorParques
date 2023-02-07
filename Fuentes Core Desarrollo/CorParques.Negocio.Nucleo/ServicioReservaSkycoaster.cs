using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReservaSkycoaster : IServicioReservaSkycoaster
    {
        IRepositorioReservaSkycoaster _repositorio;

        public ServicioReservaSkycoaster(IRepositorioReservaSkycoaster repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReservaSkycoaster> ObtenerListaReservas()
        {
            return _repositorio.ObtenerListaReserva();
        }

        public bool Insertar(ReservaSkycoaster modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public int ObtenerReservaHora(string horaInicio)
        {
            return _repositorio.ObtenerReservaHora(horaInicio);
        }

        
        public bool LiberarReserva(ReservaSkycoaster modelo, out string error)
        {
            return _repositorio.LiberarReserva(modelo, out error);
        }


        public bool CerrarReserva(ReservaSkycoaster modelo, out string error)
        {
            return _repositorio.CerrarReserva(modelo, out error);
        }

    }
}
