using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioRecoleccionSupervisor
    {
        Recoleccion ObtenerRecoleccionActiva(int IdUsuario, int IdPunto, int IdEstado);
        bool ActualizarRecoleccionMonetaria(DetalleRecoleccionMonetaria modelo, out string error);
        bool ActualizarRecoleccionDocumentos(DetalleRecoleccionDocumento modelo, out string error);
        bool ActualizarRecoleccion(Recoleccion modelo, out string error);
        bool InsertaObservacion(ObservacionRecoleccion modelo, out string error, out int IdRecoleccion);
        IEnumerable<MediosPagoFactura> ObtenerDocumentos(int intIdUsuario);
        bool ActualizarRecoleccionNovedades(DetalleRecoleccionNovedad modelo, out string error);
        IEnumerable<NovedadArqueo> ObtenerNovedadPorIdUsuario(int IdUsuario);
        IEnumerable<DetalleRecoleccion> ObtenerDetalleRecoleccion(int IdApertura, int TipoConsulta);
        string RegresarEstado(int idEstado, int idApertura);
    }
}
