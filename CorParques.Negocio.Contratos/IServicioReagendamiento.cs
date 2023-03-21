using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReagendamiento
    {
        Boleteria ObtenerProducto(string consecutivo);
        Reagendamiento ObtenerFacturaReagendamiento(string CodBarra);
        string ModificarFecha(CambioFechaBoleta producto);
        string prueba(string f);
        string InsertarDetalleReagendamientoFecha(Reagendamiento producto);
        Boleteria ObtenerDetalleReagendamientoFecha(string consecutivo);
    }
}
