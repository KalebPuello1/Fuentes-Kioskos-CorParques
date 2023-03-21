using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corparques.Datos.Contratos
{
    public interface IRepositorioReagendamiento
    {
        Boleteria ObtenerProducto(string consecutivo);
        Reagendamiento ObtenerFacturaReagendamiento(string CodBarra);
        string ModificarFecha(CambioFechaBoleta producto);
        string InsertarDetalleReagendamientoFecha(Reagendamiento producto);
        Boleteria ObtenerDetalleReagendamientoFecha(string consecutivo);
    }
}
