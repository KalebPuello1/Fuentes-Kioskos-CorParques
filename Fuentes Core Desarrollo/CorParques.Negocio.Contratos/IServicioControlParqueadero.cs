using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioControlParqueadero
    {
        ControlParqueadero Insertar(ControlParqueadero ingresoPaqueadero, out string Mensaje);
        ControlParqueadero RegistrarSalida(ControlParqueadero ingreso, out string Mensaje);
        ControlParqueadero CalcularPago(ControlParqueadero ingreso, out string Mensaje);
        ControlParqueadero Obtener(int id);
        //ControlParqueadero ObtenerPorPlaca(string Placa);
        //bool Actualizar(ControlParqueadero salida);
        IEnumerable<TipoVehiculoPorParqueadero> ObtenerDisponibilidad();
        ControlParqueadero ObtenerPorPlaca(string Placa);
    }
}
