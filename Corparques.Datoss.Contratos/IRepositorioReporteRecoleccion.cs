using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReporteRecoleccion
    {
        IEnumerable<ReporteRecoleccion> ObtenerReporte(string Fecha, int? IdTaquillero, int? IdSupervisor);
    }
}
