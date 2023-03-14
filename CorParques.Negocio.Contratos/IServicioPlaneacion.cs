using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioPlaneacion : IServicioBase<Planeacion>
	{

        // RDSH: Obtiene la lista de indicadores.
        IEnumerable<TipoGeneral> ObtenerListaIndicadores();

        bool Actualizar(Planeacion modelo, out string strError);

        Planeacion Crear(Planeacion modelo, out string strError);

        //RDSH: Retorna una planeacion filtrada por id indicador y fecha.
        IEnumerable<PlaneacionAuxiliar> ConsultarPlaneacion(int intIdIndicador, string strFecha);

        //RDSH: Inserta la planeacion mensual.
        bool Insertar(Planeacion modelo, out string error);
    }
}
