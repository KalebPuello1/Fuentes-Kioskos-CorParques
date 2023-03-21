using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    /// <summary>
    /// NMSR CU006-DE-1CI-ARS-001-Configuración centro médico
    /// </summary>
    public interface IServicioCentroMedico : IServicioBase<CentroMedico>
	{

        bool Insertar(CentroMedico modelo, out string error);

        bool Actualizar(CentroMedico modelo, out string error);

        IEnumerable<TipoGeneral> ObtenerListaCentroMedico();
        //IEnumerable<CentroMedico> ObtenerListaActivos();

        //IEnumerable<TipoGeneral> ObtenerListaSimple();

        //CentroMedico Obtener(int idCentroMedico);


        // RDSH: Retorna la lista de zona area o de ubicaciones para el reporte de centro medico.
        IEnumerable<TipoGeneral> ObtenerListaZonaAreaUbicacion(int intIdCentroMedico);

        IEnumerable<TipoGeneral> ObtenerZonas();
    }
}
