using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    /// <summary>
    /// NMSR CU006-DE-1CI-ARS-001-Configuración centro médico
    /// </summary>
	public interface IRepositorioCentroMedico
	{

        bool Insertar(CentroMedico modelo, out string error);

        bool Actualizar(CentroMedico modelo, out string error);

        IEnumerable<CentroMedico> ObtenerListaActivos();

        //IEnumerable<TipoGeneral> ObtenerListaSimple();

        CentroMedico Obtener(int idCentroMedico);

        IEnumerable<TipoGeneral> ObtenerListaCentroMedico();
        IEnumerable<TipoGeneral> ObtenerZonas();

        // RDSH: Retorna la lista de zona area o de ubicaciones para el reporte de centro medico.
        IEnumerable<TipoGeneral> ObtenerListaZonaAreaUbicacion(int intIdCentroMedico);

    }
}
